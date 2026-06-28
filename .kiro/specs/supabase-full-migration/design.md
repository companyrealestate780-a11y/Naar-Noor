# Supabase Full Migration - Design Document

## 1. Architecture Overview

### 1.1 System Diagram
```
┌─────────────────────────────────────────────────────────────────┐
│                         Frontend (Angular)                       │
│  - Supabase JS SDK (Auth, Storage, Realtime)                    │
│  - API HTTP Client (REST calls)                                 │
└────────────┬──────────────────────────────────────────────────┬──┘
             │                                                    │
        HTTP │                                              WebSocket
             │                                                    │
     ┌───────▼────────────────────────────────────────────────────▼──────┐
     │                ASP.NET Core 8.0 Backend                           │
     │  ┌──────────────────────────────────────────────────────────────┐ │
     │  │ Controllers & Endpoints                                     │ │
     │  │ - Auth: Register, Login, Verify, GetUser                  │ │
     │  │ - Orders: Create, Get, Update, Delete                     │ │
     │  │ - Reservations: Book, Cancel, Get                         │ │
     │  │ - MenuItems: Browse, Search, Filter                       │ │
     │  │ - Storage: Upload image, Delete file                      │ │
     │  │ - Realtime: Subscribe, Unsubscribe, Broadcast             │ │
     │  └────────────┬────────────────┬────────────────┬────────────┘ │
     │  ┌────────────▼─────┐ ┌────────▼──────────┐ ┌──▼────────────┐ │
     │  │  Service Layer   │ │ Repository Layer │ │ Middleware    │ │
     │  │                  │ │                  │ │               │ │
     │  │ - SupabaseAuth   │ │ - IRepository<T> │ │ - JwtAuth     │ │
     │  │ - SupabaseStorage│ │ - IUnitOfWork    │ │ - ErrorHandler│ │
     │  │ - SupabaseRealtime│ │ - Specifications │ │ - Logging     │ │
     │  │ - BusinessLogic  │ │                  │ │ - CORS        │ │
     │  └────────────┬─────┘ └────────┬──────────┘ └──────────────┘ │
     │  ┌────────────▼───────────────────────────────────────────┐   │
     │  │ Entity Framework Core 8.0 + Npgsql                     │   │
     │  │ - DbContext (ApplicationDbContext)                      │   │
     │  │ - Entity Mappings (Fluent API)                          │   │
     │  │ - Migrations (20260628044513_Initial)                   │   │
     │  └────────────┬───────────────────────────────────────────┘   │
     └───────────────┼─────────────────────────────────────────────────┘
                     │
                     │ PostgreSQL Protocol (Port 5432)
                     │
     ┌───────────────▼──────────────────────────────────────────────┐
     │           Supabase PostgreSQL Database                       │
     │  ┌──────────────────────────────────────────────────────┐   │
     │  │ Tables:                                              │   │
     │  │ - Chefs (profiles, specialties)                      │   │
     │  │ - MenuItems (with dietary filters)                   │   │
     │  │ - Reservations (bookings with slots)                │   │
     │  │ - Orders + OrderItems (transactional)               │   │
     │  │ - Reviews (with approval workflow)                   │   │
     │  │ - ContactInquiries (form submissions)               │   │
     │  │                                                      │   │
     │  │ Features:                                            │   │
     │  │ - Row-Level Security (RLS) policies                 │   │
     │  │ - Realtime: pub/sub via LISTEN/NOTIFY              │   │
     │  │ - Storage: chef-images, menu-item-images buckets    │   │
     │  └──────────────────────────────────────────────────────┘   │
     │                                                               │
     │  ┌──────────────────────────────────────────────────────┐   │
     │  │ Auth System (Built-in Supabase Auth)                 │   │
     │  │ - User registration & email verification             │   │
     │  │ - JWT token generation                               │   │
     │  │ - Session management                                 │   │
     │  │ - Password reset flows                               │   │
     │  └──────────────────────────────────────────────────────┘   │
     │                                                               │
     │  ┌──────────────────────────────────────────────────────┐   │
     │  │ Storage System (Built-in Supabase Storage)           │   │
     │  │ - File upload with auto-resizing (images)            │   │
     │  │ - Public CDN URLs                                    │   │
     │  │ - CORS configured for frontend access                │   │
     │  │ - Automatic garbage collection                       │   │
     │  └──────────────────────────────────────────────────────┘   │
     │                                                               │
     │  ┌──────────────────────────────────────────────────────┐   │
     │  │ Realtime System (PostgreSQL LISTEN/NOTIFY)           │   │
     │  │ - Channel subscriptions (orders, reservations)       │   │
     │  │ - Event broadcasting                                 │   │
     │  │ - WebSocket connections                              │   │
     │  │ - Automatic reconnection                             │   │
     │  └──────────────────────────────────────────────────────┘   │
     └─────────────────────────────────────────────────────────────┘
```

---

## 2. Data Model & Database Schema

### 2.1 Entity Relationship Diagram
```
┌─────────────────┐       ┌──────────────────┐       ┌──────────────────┐
│     Chefs       │       │    MenuItems     │       │   Reservations   │
├─────────────────┤       ├──────────────────┤       ├──────────────────┤
│ id (PK)         │       │ id (PK)          │       │ id (PK)          │
│ name            │       │ name             │       │ customer_name    │
│ title           │       │ description      │       │ email            │
│ bio             │       │ price (10,2)     │       │ phone_number     │
│ image_url       │◄──┐   │ category (enum)  │       │ reservation_date │
│ specialty       │   │   │ is_vegetarian    │       │ reservation_time │
│ is_active       │   │   │ is_vegan         │       │ party_size       │
│ sort_order      │   └───│ is_gluten_free   │       │ status (enum)    │
│ created_at      │       │ is_available     │       │ special_requests │
│ updated_at      │       │ image_url        │       │ created_at       │
└─────────────────┘       │ sort_order       │       │ updated_at       │
                          │ created_at       │       └──────────────────┘
                          │ updated_at       │
                          └──────────────────┘
                                 ▲
                                 │ (1:N)
                          ┌──────┴────────┐
                          │               │
                    ┌─────▼──────┐  ┌────▼────────────┐
                    │   Orders   │  │  OrderItems     │
                    ├────────────┤  ├─────────────────┤
                    │ id (PK)    │  │ id (PK)         │
                    │ customer   │  │ order_id (FK)   │
                    │ email      │  │ menu_item_id(FK)│
                    │ phone      │  │ menu_item_name  │
                    │ notes      │  │ unit_price      │
                    │ type(enum) │  │ quantity        │
                    │ address    │  │ created_at      │
                    │ status     │  │ updated_at      │
                    │ total      │  └─────────────────┘
                    │ created_at │
                    │ updated_at │
                    └────────────┘

┌──────────────────┐       ┌────────────────────┐
│     Reviews      │       │ ContactInquiries   │
├──────────────────┤       ├────────────────────┤
│ id (PK)          │       │ id (PK)            │
│ customer_name    │       │ name               │
│ rating (1-5)     │       │ email              │
│ comment          │       │ phone_number       │
│ is_approved      │       │ subject            │
│ source           │       │ message            │
│ created_at       │       │ is_read            │
│ updated_at       │       │ created_at         │
└──────────────────┘       │ updated_at         │
                           └────────────────────┘

Legend:
  (PK) = Primary Key
  (FK) = Foreign Key
  (enum) = Enumeration / Integer constant
  (10,2) = Numeric(10,2) for currency
```

### 2.2 PostgreSQL DDL (Schema Definition)
```sql
-- Chefs Table
CREATE TABLE "Chefs" (
  "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  "Name" VARCHAR(100) NOT NULL,
  "Title" VARCHAR(100) NOT NULL,
  "Bio" VARCHAR(1000) NOT NULL,
  "ImageUrl" VARCHAR(500),
  "Specialty" VARCHAR(200) NOT NULL,
  "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
  "SortOrder" INTEGER NOT NULL DEFAULT 0,
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- MenuItems Table
CREATE TABLE "MenuItems" (
  "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  "Name" VARCHAR(150) NOT NULL,
  "Description" VARCHAR(500) NOT NULL,
  "Price" NUMERIC(10,2) NOT NULL,
  "Category" INTEGER NOT NULL, -- Enum: Appetizer=0, MainCourse=1, Dessert=2, Beverage=3
  "IsVegetarian" BOOLEAN NOT NULL DEFAULT FALSE,
  "IsVegan" BOOLEAN NOT NULL DEFAULT FALSE,
  "IsGlutenFree" BOOLEAN NOT NULL DEFAULT FALSE,
  "IsAvailable" BOOLEAN NOT NULL DEFAULT TRUE,
  "ImageUrl" VARCHAR(500),
  "SortOrder" INTEGER NOT NULL DEFAULT 0,
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Reservations Table
CREATE TABLE "Reservations" (
  "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  "CustomerName" VARCHAR(100) NOT NULL,
  "Email" VARCHAR(200) NOT NULL,
  "PhoneNumber" VARCHAR(20) NOT NULL,
  "ReservationDate" DATE NOT NULL,
  "ReservationTime" TIME WITHOUT TIME ZONE NOT NULL,
  "PartySize" INTEGER NOT NULL,
  "Status" INTEGER NOT NULL DEFAULT 0, -- Enum: Pending=0, Confirmed=1, Cancelled=2
  "SpecialRequests" VARCHAR(500),
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Orders Table
CREATE TABLE "Orders" (
  "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  "CustomerName" VARCHAR(100) NOT NULL,
  "Email" VARCHAR(200) NOT NULL,
  "PhoneNumber" VARCHAR(20) NOT NULL,
  "Notes" VARCHAR(500),
  "Type" INTEGER NOT NULL, -- Enum: DineIn=0, Delivery=1, Takeout=2
  "DeliveryAddress" VARCHAR(300),
  "TableReservationName" TEXT,
  "Status" INTEGER NOT NULL DEFAULT 0, -- Enum: Pending=0, Processing=1, Ready=2, Delivered=3
  "TotalAmount" NUMERIC(10,2) NOT NULL,
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- OrderItems Table
CREATE TABLE "OrderItems" (
  "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  "OrderId" UUID NOT NULL REFERENCES "Orders"("Id") ON DELETE CASCADE,
  "MenuItemId" UUID NOT NULL,
  "MenuItemName" VARCHAR(150) NOT NULL,
  "UnitPrice" NUMERIC(10,2) NOT NULL,
  "Quantity" INTEGER NOT NULL,
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Reviews Table
CREATE TABLE "Reviews" (
  "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  "CustomerName" VARCHAR(100) NOT NULL,
  "Rating" INTEGER NOT NULL CHECK("Rating" >= 1 AND "Rating" <= 5),
  "Comment" VARCHAR(1000) NOT NULL,
  "IsApproved" BOOLEAN NOT NULL DEFAULT FALSE,
  "Source" VARCHAR(50), -- e.g., 'Google', 'TripAdvisor', 'Website'
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- ContactInquiries Table
CREATE TABLE "ContactInquiries" (
  "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  "Name" VARCHAR(100) NOT NULL,
  "Email" VARCHAR(200) NOT NULL,
  "PhoneNumber" VARCHAR(20),
  "Subject" VARCHAR(200) NOT NULL,
  "Message" VARCHAR(2000) NOT NULL,
  "IsRead" BOOLEAN NOT NULL DEFAULT FALSE,
  "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
  "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Indexes for performance
CREATE INDEX "IX_Chefs_IsActive" ON "Chefs"("IsActive");
CREATE INDEX "IX_MenuItems_Category_IsAvailable" ON "MenuItems"("Category", "IsAvailable");
CREATE INDEX "IX_Reservations_ReservationDate" ON "Reservations"("ReservationDate");
CREATE INDEX "IX_Reservations_Email" ON "Reservations"("Email");
CREATE INDEX "IX_Orders_Email" ON "Orders"("Email");
CREATE INDEX "IX_Orders_Status" ON "Orders"("Status");
CREATE INDEX "IX_Reviews_IsApproved" ON "Reviews"("IsApproved");
```

---

## 3. Service Layer Design

### 3.1 Authentication Service Architecture
```csharp
public interface ISupabaseAuthService
{
    // Registration & Login
    Task<(bool Success, string? UserId, string? Error)> RegisterUserAsync(string email, string password);
    Task<(bool Success, string? SessionToken, string? Error)> LoginUserAsync(string email, string password);
    
    // Token & Session Management
    Task<(bool Valid, string? UserId, string? Error)> VerifyTokenAsync(string token);
    Task<(bool Success, string? UserId, string? Email, string? Error)> GetCurrentUserAsync(string token);
    Task<(bool Success, string? Error)> LogoutUserAsync(string userId);
    
    // User Account Management
    Task<(bool Success, string? Error)> UpdateUserEmailAsync(string userId, string newEmail);
    Task<(bool Success, string? Error)> UpdateUserPasswordAsync(string userId, string newPassword);
    Task<(bool Success, string? Error)> ResetPasswordAsync(string email);
}

// Implementation uses Supabase Auth REST API:
// - Base URL: {Supabase_URL}/auth/v1/
// - Authorization: Bearer token or anonymous key
// - Response format: JSON with user objects and JWT tokens
```

### 3.2 Storage Service Architecture
```csharp
public interface ISupabaseStorageService
{
    // Chef Image Operations
    Task<(bool Success, string? PublicUrl, string? Error)> UploadChefImageAsync(
        string chefId, byte[] imageData, string contentType);
    Task<(bool Success, string? Error)> DeleteChefImageAsync(string chefId);
    
    // MenuItem Image Operations
    Task<(bool Success, string? PublicUrl, string? Error)> UploadMenuItemImageAsync(
        string menuItemId, byte[] imageData, string contentType);
    Task<(bool Success, string? Error)> DeleteMenuItemImageAsync(string menuItemId);
    
    // General File Operations
    Task<(bool Success, string? PublicUrl, string? Error)> UploadImageAsync(
        string bucket, string fileName, byte[] fileData, string contentType);
    Task<(bool Success, string? Error)> DeleteFileAsync(string bucket, string filePath);
    Task<(bool Success, List<string>? Files, string? Error)> ListFilesAsync(string bucket, string prefix = "");
    
    // Utility
    string GetPublicUrl(string bucket, string filePath);
}

// Storage Buckets:
// - chef-images: {chefId}/{filename}.jpg
// - menu-item-images: {menuItemId}/{filename}.jpg
// - CDN URLs: https://cdn.supabase.co/storage/v1/object/public/{bucket}/{path}
```

### 3.3 Realtime Service Architecture
```csharp
public interface ISupabaseRealtimeService
{
    // Subscriptions
    Task SubscribeToOrderUpdatesAsync(string orderId, 
        Func<dynamic, Task> onUpdate, Func<Exception, Task> onError);
    Task SubscribeToReservationUpdatesAsync(string reservationId,
        Func<dynamic, Task> onUpdate, Func<Exception, Task> onError);
    Task SubscribeToReviewUpdatesAsync(string menuItemId,
        Func<dynamic, Task> onUpdate, Func<Exception, Task> onError);
    Task SubscribeToTableAvailabilityAsync(
        Func<dynamic, Task> onUpdate, Func<Exception, Task> onError);
    
    // Management
    Task UnsubscribeAsync(string subscriptionId);
    Task BroadcastMessageAsync(string channel, string eventName, dynamic payload);
    Task ReconnectAsync();
    
    // Status
    bool IsConnected { get; }
}

// Channel Format: {entity}:{id}
// Examples:
// - orders:550e8400-e29b-41d4-a716-446655440000
// - reservations:550e8400-e29b-41d4-a716-446655440001
// - reviews:menu-item-id
// - table-availability
```

---

## 4. API Endpoint Design

### 4.1 Authentication Endpoints
```
POST /api/auth/register
  Request:  { email: string, password: string }
  Response: { success: bool, userId?: string, error?: string }

POST /api/auth/login
  Request:  { email: string, password: string }
  Response: { success: bool, token?: string, user?: { id, email }, error?: string }

POST /api/auth/logout
  Headers:  Authorization: Bearer {token}
  Response: { success: bool, error?: string }

GET /api/auth/me
  Headers:  Authorization: Bearer {token}
  Response: { userId: string, email: string, createdAt: datetime }

POST /api/auth/refresh-token
  Request:  { refreshToken: string }
  Response: { accessToken: string, refreshToken: string }

POST /api/auth/reset-password
  Request:  { email: string }
  Response: { success: bool, message: string }
```

### 4.2 Business Endpoints
```
# Chefs
GET /api/chefs                    # Public
GET /api/chefs/{id}               # Public
POST /api/chefs                   # Protected (Admin)
PUT /api/chefs/{id}               # Protected (Admin)
DELETE /api/chefs/{id}            # Protected (Admin)

# Menu Items
GET /api/menu-items               # Public (with filters: category, dietary)
GET /api/menu-items/{id}          # Public
POST /api/menu-items              # Protected (Admin)
PUT /api/menu-items/{id}          # Protected (Admin)
DELETE /api/menu-items/{id}       # Protected (Admin)

# Orders
POST /api/orders                  # Protected
GET /api/orders/{id}              # Protected (Owner or Admin)
GET /api/orders                   # Protected (Admin - list all)
PUT /api/orders/{id}/status       # Protected (Admin)

# Reservations
POST /api/reservations            # Public or Protected
GET /api/reservations/{id}        # Protected
PUT /api/reservations/{id}        # Protected
DELETE /api/reservations/{id}     # Protected

# Storage
POST /api/storage/upload/chef-image      # Protected (Admin)
POST /api/storage/upload/menu-item-image # Protected (Admin)
DELETE /api/storage/{bucket}/{filename}  # Protected (Admin)

# Reviews
GET /api/reviews                  # Public
POST /api/reviews                 # Public (rate limiting)
```

### 4.3 Error Response Format
```json
{
  "success": false,
  "error": "Detailed error message",
  "code": "INVALID_REQUEST", // Error code for client handling
  "details": {
    "field": "email",
    "message": "Email already exists"
  }
}
```

---

## 5. Security Architecture

### 5.1 JWT Token Flow
```
1. User Registration/Login
   ↓
2. Supabase generates JWT (RS256 signed)
   ↓
3. Backend validates token signature with Supabase public key
   ↓
4. Extract claims: { sub (user_id), email, iat, exp }
   ↓
5. Attach to request context for downstream processing
   ↓
6. Token expires in 1 hour → Client uses refresh token
   ↓
7. Backend issues new token before expiry
```

### 5.2 Authorization Middleware
```csharp
// [Authorize] attribute enforces:
// 1. JWT present in Authorization header: "Bearer {token}"
// 2. Token signature valid
// 3. Token not expired
// 4. User ID extracted to HttpContext.User.FindFirst("sub")

// [Authorize(Roles = "admin")] enforces:
// 1. All above checks
// 2. User has "admin" role in Supabase metadata

// Public endpoints have [AllowAnonymous] or no [Authorize]
```

### 5.3 CORS Policy
```csharp
services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", builder =>
    {
        builder
            .WithOrigins(corsOrigins.Split(','))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Production CORS Origins:
// - https://naarnooreats.com
// - https://www.naarnooreats.com
```

### 5.4 Storage Security
```
Bucket Policies (Supabase):
- chef-images: 
  - SELECT: authenticated users (public URLs available)
  - INSERT: Service role only (backend)
  - DELETE: Service role only (backend)

- menu-item-images:
  - SELECT: authenticated users (public URLs available)
  - INSERT: Service role only (backend)
  - DELETE: Service role only (backend)
```

---

## 6. Error Handling & Resilience

### 6.1 Exception Handling Strategy
```
Global Exception Handler Middleware:
├── DatabaseException (EF Core)
│   └── → 500 Internal Server Error
├── OperationCanceledException (Timeouts)
│   └── → 408 Request Timeout
├── AuthenticationException
│   └── → 401 Unauthorized
├── AuthorizationException
│   └── → 403 Forbidden
├── ValidationException
│   └── → 400 Bad Request (with details)
└── Exception (Generic)
    └── → 500 Internal Server Error (log details)
```

### 6.2 Retry Strategies
```csharp
// Database operations: Exponential backoff (max 3 retries, 1s-5s delay)
// HTTP calls to Supabase: 2 retries with jitter
// Realtime connections: Immediate reconnect with exponential backoff
```

---

## 7. Deployment Architecture

### 7.1 Container Orchestration
```
Production Environment:
┌─────────────────────────────────────┐
│  Docker Container (ASP.NET Core)    │
│  - Base: mcr.microsoft.com/dotnet:8 │
│  - Port: 80 (internal)              │
│  - Health Check: /health endpoint    │
│  - Logging: stdout (container logs)  │
└────────────────┬────────────────────┘
                 │
         ┌───────▼────────┐
         │ Reverse Proxy  │
         │ (nginx/traefik)│
         │ Port: 443 TLS  │
         └────────────────┘
                 │
    ┌────────────┴────────────┐
    │                         │
    ▼                         ▼
[Supabase PostgreSQL]    [Supabase Auth/Storage]
(db.*.supabase.co)       (*.supabase.co)
```

### 7.2 CI/CD Pipeline
```
Trigger: Push to main
  ↓
1. Unit Tests (.NET: xUnit/NUnit)
  ↓
2. Integration Tests (Database + API)
  ↓
3. Code Quality Analysis (SonarQube)
  ↓
4. Security Scan (OWASP, dependencies)
  ↓
5. Build Docker Image
  ↓
6. Push to Registry
  ↓
7. Deploy to Staging (auto)
  ↓
8. Smoke Tests
  ↓
9. Manual Approval (Production)
  ↓
10. Deploy to Production (rolling deployment)
```

---

## 8. Monitoring & Logging

### 8.1 Structured Logging Format
```json
{
  "timestamp": "2026-06-28T10:30:45.123Z",
  "level": "Information",
  "logger": "NaarNoor.API.Controllers.OrdersController",
  "message": "Order created successfully",
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "orderId": "550e8400-e29b-41d4-a716-446655440001",
  "duration_ms": 125,
  "traceId": "0HN1GI7D41H5E:00000001"
}
```

### 8.2 Key Metrics
```
API Metrics:
  - Request rate (req/sec)
  - Response time (p50, p95, p99)
  - Error rate (4xx, 5xx)
  - Endpoint-specific metrics

Database Metrics:
  - Query execution time
  - Connection pool usage
  - Slow queries (> 1 second)
  - Transaction rollback rate

Storage Metrics:
  - Upload success rate
  - Upload latency
  - Storage quota usage
  - Bandwidth usage

Realtime Metrics:
  - Active connections
  - Message throughput
  - Connection failures
  - Subscription errors
```

---

## 9. Performance Optimization Strategy

### 9.1 Database Optimization
- Composite indexes on frequently filtered columns
- Lazy loading disabled (explicit eager loading with Include())
- Query projection (Select dto instead of full entity)
- Pagination (max 100 items per page)
- Caching layer (Redis for high-read operations)

### 9.2 API Optimization
- Gzip compression
- HTTP caching headers (ETag, Last-Modified)
- Response DTOs (avoid circular references)
- Async/await throughout

### 9.3 Storage Optimization
- Image resizing/compression
- CDN caching (30 days)
- Automatic cleanup of orphaned files (cron job)

---

## 10. Data Consistency & Integrity

### 10.1 Transaction Management
```csharp
// Orders + OrderItems should be atomic
using (var transaction = await context.Database.BeginTransactionAsync())
{
    try
    {
        await context.Orders.AddAsync(order);
        await context.OrderItems.AddRangeAsync(orderItems);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

### 10.2 Referential Integrity
- Foreign keys with CASCADE DELETE for OrderItems
- Soft deletes (IsDeleted flag) for audit trails
- CreatedAt/UpdatedAt timestamps automatic

---

## 11. Future Enhancements

1. **Caching Layer**: Redis for frequently accessed data
2. **GraphQL API**: Alternative to REST for complex queries
3. **Message Queue**: Async job processing (order notifications)
4. **Search Engine**: Elasticsearch for menu item search
5. **Analytics**: Supabase Analytics for usage metrics
6. **Mobile App**: Native iOS/Android with Supabase SDK
7. **Admin Dashboard**: Real-time metrics and management
8. **Multi-language Support**: i18n for menu descriptions

---

## Sign-Off

**Design Reviewed By**: Architecture Team
**Status**: Ready for Implementation
**Last Updated**: June 28, 2026
