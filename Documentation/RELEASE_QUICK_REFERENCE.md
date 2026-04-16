# GitHub Actions Release Workflow - Quick Reference

## 📌 One-Minute Overview

Push code → GitHub Actions runs → Creates release with automatic versioning

## 🎯 How to Use

### 1️⃣ Make Changes
```bash
git add .
```

### 2️⃣ Commit (Use Conventional Format)
```bash
# Bug fix (v0.0.X)
git commit -m "fix: correct validation"

# New feature (v0.X.0)
git commit -m "feat: add search endpoint"

# Breaking change (vX.0.0)
git commit -m "feat!: redesign API"
```

### 3️⃣ Push to Main
```bash
git push origin main
```

### ✨ Done!
GitHub Actions automatically creates a release with:
- ✅ Semantic version tag
- ✅ Release notes
- ✅ Compiled artifacts (ZIP)
- ✅ Build artifacts

---

## 📋 Commit Message Quick Ref

| Type | Format | Version Change |
|------|--------|-----------------|
| 🐛 Fix | `fix: description` | v0.0.**X** |
| ✨ Feature | `feat: description` | v0.**X**.0 |
| 💥 Breaking | `feat!: description` | v**X**.0.0 |

---

## 📊 See Your Release

1. Go to **Releases** tab on GitHub
2. Download artifacts (ZIP file)
3. View release notes

Or navigate to:
- Releases: `https://github.com/USERNAME/MyDemoApp/releases`
- Actions: `https://github.com/USERNAME/MyDemoApp/actions`

---

## ✋ Important Notes

- ✅ Only triggers on `main` branch
- ✅ Use conventional commit format
- ✅ One commit = one release
- ✅ Version bumps automatically
- ✅ No manual versioning needed

---

## 📚 Detailed Docs

For full documentation, see:
- **SEMANTIC_VERSIONING_GUIDE.md** - Complete guide with examples
- **README.md** - Project overview with versioning section

---

**Last Updated:** April 16, 2026  
**Framework:** ASP.NET Core 10  
**CI/CD:** GitHub Actions

