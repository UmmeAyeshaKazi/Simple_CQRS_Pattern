# Semantic Versioning & GitHub Actions Guide

This document explains how the automated release workflow works and how to use semantic versioning to trigger releases.

## Overview

The GitHub Actions workflow (`release.yml`) automatically:
- Runs when code is pushed to the `main` branch
- Determines the next semantic version based on commit messages
- Creates a Git tag and GitHub Release
- Publishes release artifacts
- Generates release notes

## Semantic Versioning

This project uses **Semantic Versioning 2.0.0** format: `MAJOR.MINOR.PATCH`

- **MAJOR** (v1.0.0): Breaking changes or incompatible API changes
- **MINOR** (v0.1.0): New features in a backward compatible manner
- **PATCH** (v0.0.1): Bug fixes and backward compatible changes

## Conventional Commits

The workflow automatically bumps versions based on **Conventional Commits** format:

### Commit Message Formats

#### 1. **Patch Release** (v0.0.X → v0.0.X+1)
```bash
fix: correct typo in Student model
fix: resolve database connection issue
```

**Example:**
```bash
git commit -m "fix: update Student email validation"
```

#### 2. **Minor Release** (v0.X.0 → v0.X+1.0)
```bash
feat: add student email validation
feat: implement GetStudentByEmail query
```

**Example:**
```bash
git commit -m "feat: add filter functionality to students endpoint"
```

#### 3. **Major Release** (vX.0.0 → vX+1.0.0)
```bash
feat!: redesign Student API response format
feat!: remove deprecated endpoints
```

Or include `BREAKING CHANGE:` in the commit body:
```bash
git commit -m "refactor: restructure Student model

BREAKING CHANGE: Student entity properties are renamed"
```

**Example:**
```bash
git commit -m "feat!: change Student age field to birthDate

This is a breaking change as existing clients expect age field."
```

## Workflow Behavior

### Trigger
- ✅ Automatically triggered on `git push origin main`
- ❌ Does NOT trigger on other branches
- ❌ Does NOT trigger on pull requests

### Steps
1. **Checkout**: Gets the latest code from main branch
2. **Setup .NET 10**: Installs the required runtime
3. **Build**: Compiles the project in Release mode
4. **Test**: Runs any tests (optional)
5. **Version Detection**: Reads latest tag and determines next version
6. **Tag Creation**: Creates and pushes the new version tag
7. **Release Creation**: Creates a GitHub Release with:
   - Semantic version tag
   - Commit information
   - Auto-generated release notes
   - Publish artifacts
8. **Asset Upload**: Zips and uploads the compiled application

## How to Use

### 1. Make Changes and Commit

```bash
# Make your changes
git add .

# Use conventional commit format
git commit -m "feat: add student search functionality"
```

### 2. Push to Main Branch

```bash
git push origin main
```

### 3. GitHub Actions Runs Automatically

- Navigate to your repository's **Actions** tab
- Watch the "Release with Semantic Versioning" workflow execute
- Once complete, check the **Releases** page

### 4. Access Your Release

Go to **Releases** tab on GitHub to see:
- New version tag (v0.1.0, v1.0.0, etc.)
- Changelog with commit details
- Compiled artifacts (ZIP file)

## Version History Example

Assuming we start from v0.0.0:

| Commit Message | Version Bump | New Version |
|---|---|---|
| `fix: correct validation` | PATCH | v0.0.1 |
| `feat: add new endpoint` | MINOR | v0.1.0 |
| `feat: add new endpoint` | MINOR | v0.2.0 |
| `feat!: breaking change` | MAJOR | v1.0.0 |
| `fix: small bug fix` | PATCH | v1.0.1 |

## Current Version

To check the current version:

```bash
# List all tags
git tag -l

# Show latest tag
git tag -l --sort=-version:refname | head -n 1
```

Or visit: `https://github.com/YOUR_USERNAME/MyDemoApp/releases`

## Release Notes Generation

The workflow automatically generates release notes including:
- Version number (MAJOR.MINOR.PATCH)
- Commit message
- Author information
- Commit SHA
- Technology stack
- Release date/time

Example:
```
## Release v0.1.0

**Version:** 0.1.0

### Changes
- feat: add student search functionality

### Commit Info
- Author: John Doe (john@example.com)
- Commit: abc1234

### Technology Stack
- ASP.NET Core 10
- Entity Framework Core 10
- SQLite
- CQRS Pattern

---

**Release Date:** 2026-04-16T10:30:00Z
```

## Build Artifacts

Each release includes:
- **ZIP file**: Contains the published application ready for deployment
- **Artifact storage**: Available for 90 days (GitHub default)

## Troubleshooting

### Release not triggering?
- ✅ Confirm push is to `main` branch (not other branches)
- ✅ Check GitHub Actions permissions in repository settings
- ✅ Verify commit message follows conventional commit format

### Same version released twice?
- The workflow checks if the tag already exists
- If it does, it skips release creation
- Change commit message to trigger new version

### Need to manually create a release?
```bash
# Create and push a tag manually
git tag -a v0.1.0 -m "Release v0.1.0"
git push origin v0.1.0
```

## Tips & Best Practices

1. **Use meaningful commit messages** - The better your commit message, the better the release notes

2. **One feature per commit** - Easier to understand changes and revert if needed

3. **Breaking changes are MAJOR** - Always use `feat!:` or `BREAKING CHANGE:` for incompatible changes

4. **Keep main branch stable** - Only push tested, working code to main

5. **Atomic commits** - Each commit should represent one logical change

## Example Workflow

```bash
# Feature branch work
git checkout -b feature/search-functionality
git commit -m "feat: implement student search by name"
git commit -m "feat: add search filters to API"

# Create pull request for review
# After approval and merge to main

# Automatic release created!
# Version: v0.1.0
# Released by: GitHub Actions
# Artifacts available for download
```

## GitHub Actions Status

Check the status in your repository:
- **Actions** tab → "Release with Semantic Versioning" workflow
- ✅ Green checkmark = Successful release
- ❌ Red X = Build or release failed (check logs for details)
- ⏳ Yellow circle = Currently running

## Environment Variables

The workflow uses GitHub's built-in environment variables:
- `GITHUB_TOKEN` - Automatically provided for GitHub API access
- `GITHUB_OUTPUT` - For passing data between steps

## Customization

To modify the workflow:
1. Edit `.github/workflows/release.yml`
2. Commit and push changes to main
3. New workflow behavior applies to next push

### Common Customizations:
- Change build configuration
- Add additional test steps
- Modify release notes format
- Add different artifact types
- Include deployment steps

## Questions?

For more information on:
- **Semantic Versioning**: https://semver.org/
- **Conventional Commits**: https://www.conventionalcommits.org/
- **GitHub Actions**: https://docs.github.com/en/actions
- **GitHub Releases**: https://docs.github.com/en/repositories/releasing-projects-on-github/about-releases

