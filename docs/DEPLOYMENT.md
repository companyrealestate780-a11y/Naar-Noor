# Deployment Guide

Complete guide for deploying Naar & Noor to various platforms.

## Table of Contents

- [Quick Start - Vercel (Recommended)](#quick-start---vercel-recommended)
- [Build for Production](#build-for-production)
- [Docker Deployment](#docker-deployment)
- [Netlify Deployment](#netlify-deployment)
- [AWS S3 + CloudFront](#aws-s3--cloudfront)
- [GitHub Pages](#github-pages)
- [Traditional Hosting](#traditional-hosting)
- [CI/CD Automation](#cicd-automation)
- [PWA Features](#pwa-features)
- [SEO Optimization](#seo-optimization)

---

## Quick Start - Vercel (Recommended)

Your app is configured to deploy to: **https://naar222noor.vercel.app/**

### Quick Deploy Steps:

1. **Connect to Vercel:**
   - Go to [vercel.com](https://vercel.com)
   - Sign in with GitHub
   - Click "Add New Project"
   - Import your repository: `Mostafa-SAID7/Naar-Noor`

2. **Configure Build Settings:**
   - Framework Preset: **Angular**
   - Build Command: `npm run build:prod`
   - Output Directory: `dist/lost-yeti`
   - Install Command: `npm install`

3. **Environment Variables:**
   No environment variables needed for this project.

4. **Deploy:**
   - Click "Deploy"
   - Wait for build to complete (2-3 minutes)
   - Your site will be live at `https://naar222noor.vercel.app`

### Auto-Deploy:
Every push to the `main` branch will automatically trigger a new deployment.

### Features Included:
- ✅ PWA Support (Progressive Web App)
- ✅ SEO Optimized for Egyptian market
- ✅ Responsive design
- ✅ Performance optimized
- ✅ Image-based map with location marker
- ✅ Manifest.json for installable app
- ✅ Sitemap.xml configured
- ✅ Robots.txt configured

---

## Build for Production

### Create Production Build

```bash
npm run build:prod
```

This creates an optimized production build in `dist/lost-yeti/` with:
- Minified JavaScript and CSS
- Tree-shaking for smaller bundle sizes
- AOT (Ahead-of-Time) compilation
- Optimized images and assets
- Source maps disabled for security

### Build Output

```
dist/lost-yeti/
├── index.html
├── main.[hash].js
├── polyfills.[hash].js
├── runtime.[hash].js
├── styles.[hash].css
├── assets/
├── favicon.ico
├── favicon.svg
├── manifest.json
├── robots.txt
└── sitemap.xml
```

### Test Production Build Locally

```bash
npm install -g http-server
cd dist/lost-yeti
http-server -p 8080
```

Then open: http://localhost:8080

---

## Docker Deployment

### Build Docker Image

```bash
docker build -t naar-noor:latest .
```

### Run Container

```bash
docker run -d -p 80:80 naar-noor:latest
```

### Docker Compose

```yaml
version: '3.8'
services:
  web:
    build: .
    ports:
      - "80:80"
    restart: unless-stopped
```

Run with:
```bash
docker-compose up -d
```

---

## Netlify Deployment

### Option 1: Netlify CLI

```bash
npm install -g netlify-cli
npm run build:prod
netlify deploy --prod --dir=dist/lost-yeti
```

### Option 2: Netlify UI

1. Go to [netlify.com](https://netlify.com)
2. Click "Add new site" → "Import an existing project"
3. Connect to GitHub and select `Mostafa-SAID7/Naar-Noor`
4. Configure:
   - Build command: `npm run build:prod`
   - Publish directory: `dist/lost-yeti`
5. Click "Deploy site"

### netlify.toml Configuration

```toml
[build]
  command = "npm run build:prod"
  publish = "dist/lost-yeti"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200
```

---

## AWS S3 + CloudFront

### 1. Build the Application

```bash
npm run build:prod
```

### 2. Create S3 Bucket

```bash
aws s3 mb s3://naar-noor-website
aws s3 website s3://naar-noor-website --index-document index.html --error-document index.html
```

### 3. Upload Files

```bash
aws s3 sync dist/lost-yeti/ s3://naar-noor-website --delete
```

### 4. Set Bucket Policy

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "PublicReadGetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Action": "s3:GetObject",
      "Resource": "arn:aws:s3:::naar-noor-website/*"
    }
  ]
}
```

### 5. Create CloudFront Distribution

- Origin: S3 bucket endpoint
- Default Root Object: `index.html`
- Error Pages: 404 → `/index.html` (200)

---

## GitHub Pages

### 1. Install Angular CLI GitHub Pages

```bash
npm install -g angular-cli-ghpages
```

### 2. Build and Deploy

```bash
npm run build:prod
npx angular-cli-ghpages --dir=dist/lost-yeti
```

### 3. Configure Repository

- Go to repository Settings → Pages
- Source: `gh-pages` branch
- Your site will be at: `https://mostafa-said7.github.io/Naar-Noor/`

---

## Traditional Hosting

### Requirements

- Web server (Apache, Nginx, IIS)
- Support for Single Page Applications (SPA)

### Apache (.htaccess)

Already included in `src/.htaccess`:

```apache
<IfModule mod_rewrite.c>
  RewriteEngine On
  RewriteBase /
  RewriteRule ^index\.html$ - [L]
  RewriteCond %{REQUEST_FILENAME} !-f
  RewriteCond %{REQUEST_FILENAME} !-d
  RewriteRule . /index.html [L]
</IfModule>
```

### Nginx Configuration

```nginx
server {
    listen 80;
    server_name naar-noor.com;
    root /var/www/naar-noor;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    # Cache static assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|webp)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }
}
```

### Upload via FTP/SFTP

1. Build: `npm run build:prod`
2. Upload contents of `dist/lost-yeti/` to web root
3. Ensure `.htaccess` is uploaded (Apache)
4. Configure server for SPA routing

---

## CI/CD Automation

### GitHub Actions (Already Configured)

Workflows in `.github/workflows/`:

- **ci.yml**: Runs on every push/PR
  - Installs dependencies
  - Builds the app
  - Runs tests (if configured)

- **deploy.yml**: Deploys to production
  - Builds production bundle
  - Deploys to hosting platform
  - Runs smoke tests

- **code-quality.yml**: Code quality checks
  - Linting
  - Type checking
  - Security audits

### Manual Trigger

```bash
# Trigger deployment workflow
gh workflow run deploy.yml
```

---

## PWA Features

### Testing PWA

After deployment, test PWA features:

1. **Desktop (Chrome/Edge):**
   - Open site
   - Click install icon in address bar
   - App installs as standalone application

2. **Mobile (Android/iOS):**
   - Open site in browser
   - Tap "Add to Home Screen"
   - App installs with icon

### PWA Checklist

- ✅ manifest.json configured
- ✅ Service worker ready
- ✅ HTTPS enabled (required for PWA)
- ✅ Icons for all sizes (72x72 to 512x512)
- ✅ Offline fallback (optional)

### Lighthouse PWA Score

Run audit:
```bash
npm install -g lighthouse
lighthouse https://naar222noor.vercel.app --view
```

Target scores:
- Performance: 90+
- Accessibility: 90+
- Best Practices: 90+
- SEO: 95+
- PWA: 100

---

## SEO Optimization

### Keywords Optimized For:

**Arabic:**
- مطعم في مصر (Restaurant in Egypt)
- مطعم هيمالايا (Himalayan Restaurant)
- افضل مطعم في مصر (Best restaurant in Egypt)
- مطاعم القاهرة (Cairo restaurants)
- حجز مطعم (Restaurant reservation)

**English:**
- Himalayan restaurant Egypt
- Best restaurant Egypt
- Fine dining Egypt
- Restaurants in Cairo
- Mountain cuisine Egypt

### SEO Features Included:

- ✅ Meta tags (title, description, keywords)
- ✅ Open Graph tags (Facebook)
- ✅ Twitter Card tags
- ✅ Schema.org structured data (Restaurant, LocalBusiness)
- ✅ Sitemap.xml
- ✅ Robots.txt
- ✅ Canonical URLs
- ✅ Alt tags on images
- ✅ Semantic HTML
- ✅ Mobile-friendly
- ✅ Fast loading times

### Google Search Console

1. Go to [search.google.com/search-console](https://search.google.com/search-console)
2. Add property: `https://naar222noor.vercel.app`
3. Verify ownership
4. Submit sitemap: `https://naar222noor.vercel.app/sitemap.xml`

---

## Performance Optimization

### Current Optimizations:

- ✅ Lazy loading images
- ✅ WebP image format
- ✅ Preload critical assets
- ✅ DNS prefetch for external resources
- ✅ Font display: swap
- ✅ Minified CSS/JS
- ✅ Tree-shaking
- ✅ Code splitting
- ✅ Gzip/Brotli compression

### Performance Targets:

- First Contentful Paint: < 1.5s
- Largest Contentful Paint: < 2.5s
- Time to Interactive: < 3.5s
- Cumulative Layout Shift: < 0.1
- First Input Delay: < 100ms

---

## Troubleshooting

### Build Fails

```bash
# Clear cache and reinstall
rm -rf node_modules package-lock.json
npm install
npm run build:prod
```

### Routing Issues (404 on refresh)

Ensure server is configured for SPA:
- Apache: `.htaccess` with rewrite rules
- Nginx: `try_files` directive
- Vercel/Netlify: Automatic SPA handling

### PWA Not Installing

- Ensure HTTPS is enabled
- Check manifest.json is accessible
- Verify service worker registration
- Check browser console for errors

---

## Support & Resources

### Documentation
- [Angular Docs](https://angular.io/docs)
- [Vercel Docs](https://vercel.com/docs)
- [PWA Guide](https://web.dev/progressive-web-apps/)

### Deployment Issues
- Vercel Dashboard: https://vercel.com/dashboard
- Check build logs
- Review GitHub Actions runs

### Contact
- Repository: https://github.com/Mostafa-SAID7/Naar-Noor
- Issues: https://github.com/Mostafa-SAID7/Naar-Noor/issues

---

**Last Updated:** 2026-03-26
