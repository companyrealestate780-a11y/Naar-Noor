# Root Directory Structure Guide

**Last Updated:** March 31, 2026

## 📁 Root Directory Organization

The root directory should contain only essential configuration and documentation files.

### ✅ Essential Files (Keep in Root)

#### Configuration Files
- `angular.json` - Angular CLI configuration
- `tsconfig.json` - TypeScript configuration (root)
- `tsconfig.app.json` - TypeScript configuration (app)
- `package.json` - npm dependencies and scripts
- `package-lock.json` - npm lock file
- `vercel.json` - Vercel deployment configuration

#### Build & Server Configuration
- `tailwind.config.js` - Tailwind CSS configuration
- `postcss.config.js` - PostCSS configuration
- `Dockerfile` - Docker container configuration
- `nginx.conf` - Nginx web server configuration

#### Git & Environment
- `.gitignore` - Git ignore rules
- `.dockerignore` - Docker ignore rules

#### Documentation
- `README.md` - Main project documentation (ONLY markdown file in root)

---

## 📊 Current Root Structure

```
Root/
├── Configuration Files (6)
│   ├── angular.json
│   ├── tsconfig.json
│   ├── tsconfig.app.json
│   ├── package.json
│   ├── package-lock.json
│   └── vercel.json
├── Build Configuration (3)
│   ├── tailwind.config.js
│   ├── postcss.config.js
│   └── Dockerfile
├── Server Configuration (1)
│   └── nginx.conf
├── Environment (2)
│   ├── .gitignore
│   └── .dockerignore
└── Documentation (1)
    └── README.md
```

**Total: 13 files** (Optimal)

---

## 📚 Documentation Organization

All documentation files should be in `/docs` folder:

### Current Documentation Structure
```
docs/
├── AUTOMATION.md          # GitHub workflows guide
├── CHANGELOG.md           # Version history
├── CODE_OF_CONDUCT.md     # Community guidelines
├── CONTRIBUTING.md        # Contribution guide
├── DEPLOYMENT.md          # Deployment instructions
├── FEATURES.md            # Feature documentation
├── PERFORMANCE.md         # Performance optimization
├── PROJECT_SETUP.md       # Setup instructions
├── PROJECT_STATUS.md      # Project status report
├── ROOT_STRUCTURE.md      # This file
├── SECURITY.md            # Security policies
├── STYLES.md              # Styling guidelines
└── TECHNOLOGIES.md        # Tech stack details
```

---

## 🎯 Best Practices

### Root Directory Rules
1. ✅ Keep only essential configuration files
2. ✅ Keep only main README.md
3. ✅ No temporary or summary files
4. ✅ No duplicate documentation
5. ✅ Clean and organized structure

### Documentation Rules
1. ✅ All docs in `/docs` folder
2. ✅ Clear, descriptive filenames
3. ✅ Organized by topic/purpose
4. ✅ Cross-referenced in README
5. ✅ No duplication

### File Naming Convention
- **Configuration:** `*.config.js`, `*.json`
- **Documentation:** `UPPERCASE.md` or `TOPIC.md`
- **Environment:** `.filename` (hidden files)
- **Build:** `Dockerfile`, `nginx.conf`

---

## 🔄 Migration Checklist

### Files to Keep in Root ✅
- [x] angular.json
- [x] tsconfig.json
- [x] tsconfig.app.json
- [x] package.json
- [x] package-lock.json
- [x] vercel.json
- [x] tailwind.config.js
- [x] postcss.config.js
- [x] Dockerfile
- [x] nginx.conf
- [x] .gitignore
- [x] .dockerignore
- [x] README.md

### Files to Remove from Root ✅
- [x] No extra markdown files
- [x] No summary files
- [x] No temporary files
- [x] No duplicate documentation

---

## 📖 README.md Content

The root `README.md` should contain:
1. Project overview
2. Quick start guide
3. Tech stack summary
4. Key features
5. Links to detailed documentation in `/docs`
6. Contact and support information
7. License information

**Size:** ~300-400 lines (concise and focused)

---

## 🚀 Deployment Files

### Vercel Configuration
- `vercel.json` - Deployment settings
- Auto-deploy from main branch
- Build command: `npm run build:prod`
- Output directory: `dist/lost-yeti`

### Docker Configuration
- `Dockerfile` - Container image
- `nginx.conf` - Web server config
- `.dockerignore` - Exclude files

### Build Configuration
- `angular.json` - Angular build settings
- `tailwind.config.js` - Tailwind CSS settings
- `postcss.config.js` - PostCSS settings

---

## 📋 TypeScript Configuration

### Root tsconfig.json
- Base TypeScript configuration
- Shared settings for all projects

### App tsconfig.app.json
- Application-specific settings
- Extends root tsconfig.json

---

## 🔐 Environment Files

### .gitignore
- Excludes node_modules
- Excludes dist folder
- Excludes environment files
- Excludes IDE files

### .dockerignore
- Excludes node_modules
- Excludes .git
- Excludes dist folder
- Excludes development files

---

## ✨ Summary

The root directory is now optimized with:
- **13 essential files** (configuration, build, environment)
- **1 main README.md** (documentation entry point)
- **13 detailed docs** in `/docs` folder
- **Clean, organized structure** for easy navigation
- **No duplication** or unnecessary files

This structure ensures:
- ✅ Easy project setup
- ✅ Clear file organization
- ✅ Scalable documentation
- ✅ Professional appearance
- ✅ Optimal performance

---

**Status:** ✅ **Optimized and Organized**
