---
name: angular-restaurant
description: "Expert skill for building and maintaining the Naar & Noor Angular restaurant website"
risk: safe
source: "local"
date_added: "2026-03-26"
---

# Naar & Noor Angular Restaurant Skill

You are an expert Angular developer specializing in restaurant websites with a focus on performance, SEO, and modern UI/UX.

## When to Use This Skill

Use this skill when:
- Working on the Naar & Noor restaurant website
- Building Angular 17 standalone components
- Implementing restaurant-specific features (reservations, menus, locations)
- Optimizing for SEO and performance
- Managing assets and images
- Implementing PWA features

## Project Overview

**Project:** Naar & Noor - Authentic Himalayan Restaurant in Egypt
**Tech Stack:** Angular 17 (Standalone Components), Tailwind CSS (CDN), TypeScript
**Deployment:** Vercel (https://naar222noor.vercel.app)
**Repository:** https://github.com/Mostafa-SAID7/Naar-Noor

## Design System

### Colors
- **Primary Orange:** `#C65A1E` - Used for CTAs, accents, highlights
- **Background Dark:** `#0a0a0a` - Main background
- **Card Background:** `#111` or `#0f0f0f` - Elevated surfaces
- **Text White:** `#ffffff` - Primary text
- **Text Neutral:** `#neutral-300` to `#neutral-500` - Secondary text
- **Borders:** `white/5` to `white/10` - Subtle borders

### Typography
- **Headings:** `font-['Forum']` - Elegant serif for titles
- **Body:** `font-['Open_Sans']` - Clean sans-serif for content
- **Font Weights:** 300 (light), 400 (regular), 500 (medium), 600 (semi-bold)

### Spacing
- **Section Padding:** `py-6` (24px) for consistent vertical spacing
- **Container Max Width:** `max-w-5xl` to `max-w-7xl` depending on section
- **Border Radius:** `rounded-xl` (12px) for cards, `rounded-2xl` (16px) for large containers

## Project Structure

```
src/
├── app/
│   ├── components/          # Reusable components
│   │   ├── header/
│   │   ├── footer/
│   │   ├── animated-background/
│   │   ├── custom-calendar/
│   │   └── custom-dropdown/
│   ├── sections/            # Page sections
│   │   ├── hero/
│   │   ├── about/
│   │   ├── category/
│   │   ├── menu/
│   │   ├── chefs/
│   │   ├── reviews/
│   │   ├── blog/
│   │   ├── locations/
│   │   └── cinematic-banner/
│   ├── pages/
│   │   └── home/
│   ├── app.component.ts
│   ├── app.config.ts
│   └── app.routes.ts
├── assets/                  # Images and static files
├── index.html
├── manifest.json           # PWA manifest
├── robots.txt
└── sitemap.xml
```

## Key Features

### 1. Hero Section with Embedded Reservation
- Full-screen hero with background image
- Reservation card positioned absolutely at bottom
- Custom calendar and dropdown components
- Form validation and submission

### 2. Responsive Navigation
- Fixed header with mobile menu toggle
- Smooth scroll to sections
- 2N logo badge with brand name

### 3. Custom Components
- **CustomCalendarComponent:** Interactive date picker
- **CustomDropdownComponent:** Styled select with icons
- **AnimatedBackgroundComponent:** CSS-based animated gradient

### 4. SEO Optimization
- Bilingual meta tags (English + Arabic)
- Schema.org structured data (Restaurant + LocalBusiness)
- Open Graph and Twitter Card tags
- Sitemap and robots.txt
- Optimized for Egyptian market keywords

### 5. Performance
- Lazy loading images
- WebP format for hero
- Preload critical assets
- DNS prefetch for external resources
- Optimized bundle size (~2.3MB assets)

## Development Guidelines

### Component Structure
```typescript
import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-section-name',
  standalone: true,
  imports: [CommonModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './section-name.component.html',
  styleUrls: ['./section-name.component.css']
})
export class SectionNameComponent {}
```

### Styling Conventions
- Use Tailwind utility classes
- Dark theme by default
- Hover states with transitions
- Focus states for accessibility
- Responsive breakpoints: sm, md, lg

### Image Management
- Use descriptive filenames (e.g., `chef-arjun.jpg`, `grilled-food.jpg`)
- Optimize images before adding
- Use `loading="lazy"` except for hero
- Use `loading="eager"` for above-the-fold images

### Form Handling
- Use FormsModule with ngModel
- Validate required fields
- Provide user feedback
- Use custom components for better UX

## Common Tasks

### Adding a New Section
1. Generate component: `ng generate component sections/section-name --standalone`
2. Add to home component imports and template
3. Follow spacing conventions (`py-6`)
4. Use consistent max-width containers

### Updating Assets
1. Add image to `src/assets/`
2. Use descriptive filename
3. Update references in components
4. Remove old unused files
5. Commit with clear message

### SEO Updates
1. Update `src/index.html` meta tags
2. Update `src/sitemap.xml` with new URLs
3. Update Schema.org structured data
4. Test with Lighthouse

### Deployment
1. Build: `npm run build:prod`
2. Test locally: `http-server dist/lost-yeti`
3. Push to GitHub
4. Vercel auto-deploys from main branch

## Best Practices

### Performance
- ✅ Minimize bundle size
- ✅ Lazy load below-the-fold images
- ✅ Use WebP format when possible
- ✅ Preload critical assets
- ✅ Remove unused code and assets

### SEO
- ✅ Descriptive alt tags on all images
- ✅ Semantic HTML structure
- ✅ Meta descriptions under 160 characters
- ✅ Structured data for rich snippets
- ✅ Mobile-friendly design

### Accessibility
- ✅ Keyboard navigation support
- ✅ Focus indicators on interactive elements
- ✅ Sufficient color contrast
- ✅ ARIA labels where needed
- ✅ Semantic HTML elements

### Code Quality
- ✅ TypeScript strict mode
- ✅ Component-based architecture
- ✅ Consistent naming conventions
- ✅ Clear commit messages
- ✅ Documentation for complex logic

## Troubleshooting

### Common Issues

**404 Errors for Assets:**
- Check file path is correct
- Verify file exists in `src/assets/`
- Check filename case sensitivity
- Update manifest.json if needed

**Calendar/Dropdown Not Working:**
- Ensure custom components are imported
- Check FormsModule is imported
- Verify z-index layering
- Check ngModel bindings

**Styling Issues:**
- Verify Tailwind CDN is loaded
- Check for conflicting styles
- Inspect element in DevTools
- Verify responsive breakpoints

**Build Errors:**
- Clear Angular cache: `rm -rf .angular/cache`
- Reinstall dependencies: `npm install`
- Check TypeScript errors
- Verify all imports are correct

## Resources

- **Angular Docs:** https://angular.io/docs
- **Tailwind CSS:** https://tailwindcss.com/docs
- **Iconify Icons:** https://icon-sets.iconify.design/solar/
- **Vercel Docs:** https://vercel.com/docs
- **Lighthouse:** https://developers.google.com/web/tools/lighthouse

## Project-Specific Notes

### Location Information
- **Address:** Town Centre, St Peter Port, Guernsey, GY1 2PN
- **Coordinates:** 49.4597, -2.5369
- **Hours:** 12:00 PM - 9:00 PM (Closed Mondays)
- **Phone:** +44 (0) 1481 123456

### Brand Identity
- **Name:** Naar & Noor (نار ونور)
- **Logo:** 2N badge in orange gradient
- **Tagline:** "Crafted by Fire & Flavor"
- **Cuisine:** Authentic Himalayan
- **Target Market:** Egypt (with English/Arabic content)

### Current Assets (10 files, ~2.3MB)
1. hero.webp (309KB)
2. chef-arjun.jpg (137KB)
3. chef-maya.jpg (82KB)
4. Starters.jpg (96KB)
5. Grill-BBQ.jpg (71KB)
6. Himalayan-Mains.jpg (83KB)
7. Cocktails.jpg (46KB)
8. cooking-fire.jpg (1.1MB)
9. 5 Must-Try Dishes at Naar & Noor.jpg (211KB)
10. The-Art-of-Fire-Grilled-Cooking.jpg (137KB)

## Quick Commands

```bash
# Development
npm start                    # Start dev server (localhost:4200)
npm run build:prod          # Production build
npm run docs:update         # Update all documentation

# Git
git add -A                  # Stage all changes
git commit -m "message"     # Commit with message
git push origin main        # Push to GitHub

# Deployment
# Automatic via Vercel on push to main
```

## Success Metrics

- ✅ Lighthouse Performance: 90+
- ✅ Lighthouse SEO: 95+
- ✅ Mobile-friendly: Yes
- ✅ PWA-ready: Yes
- ✅ Bundle size: < 3.5MB
- ✅ First Contentful Paint: < 1.5s
- ✅ Time to Interactive: < 3.5s
