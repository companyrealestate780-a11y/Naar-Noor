# Angular Restaurant Skill

Expert skill for building and maintaining the Naar & Noor Angular restaurant website.

## Overview

This skill provides comprehensive guidance for working on the Naar & Noor restaurant website, including:
- Angular 17 standalone components architecture
- Restaurant-specific features (reservations, menus, locations)
- SEO optimization for Egyptian market
- Performance optimization
- Asset management
- PWA implementation

## Quick Start

1. **Development Server:**
   ```bash
   npm start
   ```
   Opens at http://localhost:4200

2. **Production Build:**
   ```bash
   npm run build:prod
   ```

3. **Deploy:**
   Push to main branch - Vercel auto-deploys

## Key Features

- ✅ Full-screen hero with embedded reservation form
- ✅ Custom calendar and dropdown components
- ✅ Responsive navigation with mobile menu
- ✅ SEO optimized (English + Arabic)
- ✅ PWA-ready with manifest.json
- ✅ Optimized assets (~2.3MB total)
- ✅ Dark theme with orange accents

## Project Structure

```
src/
├── app/
│   ├── components/     # Reusable UI components
│   ├── sections/       # Page sections
│   └── pages/          # Full pages
├── assets/             # Images (10 files)
├── index.html          # SEO meta tags
├── manifest.json       # PWA config
└── sitemap.xml         # SEO sitemap
```

## Design System

- **Primary Color:** #C65A1E (Orange)
- **Background:** #0a0a0a (Dark)
- **Fonts:** Forum (headings), Open Sans (body)
- **Spacing:** py-6 (24px) for sections

## Common Tasks

### Add New Section
```bash
ng generate component sections/new-section --standalone
```

### Update Assets
1. Add to `src/assets/` with descriptive name
2. Update component references
3. Remove old unused files

### SEO Updates
- Edit `src/index.html` meta tags
- Update `src/sitemap.xml`
- Update Schema.org data

## Resources

- **Live Site:** https://naar222noor.vercel.app
- **Repository:** https://github.com/Mostafa-SAID7/Naar-Noor
- **Documentation:** See `docs/` folder

## Support

For issues or questions, refer to:
- `SKILL.md` for detailed guidelines
- `docs/DEPLOYMENT.md` for deployment help
- `OPTIMIZATION_REPORT.md` for performance info
