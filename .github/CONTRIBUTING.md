# Contributing Guidelines

## Introduction

Thanks for you interest in contributing to this project!

You can contribute in many ways:

- New features
- Bug fixes
- Refactoring
- Unit tests
- Documentation
- Bug reports
- Feature requests

> [!IMPORTANT]
> Start by reading the [README](../README.md) and browsing the project's [open issues](../../issues) to see where help is needed.

---

## 1. Getting started

1. Fork or clone (if you have direct access) the repository.
2. Create a branch from `develop` and follow naming conventions from the [Branch naming](#3-branch-naming).
3. Do your work - follow the [Code rules](#2-code-rules).
4. Use commit format as described in [Commit messages](#4-commit-messages).
5. Open a pull request back to `develop` (see [Pull requests](#5-pull-requests)).
6. Request at least one review and ensure the pipeline passes.

---

## 2. Code rules

1. Run the project linter before committing.  
2. Write or update tests to cover new or changed logic.  
3. Document any code using the project's documentation style.  
4. Keep scope focused - one feature or bug-fix per branch/pull request.  
5. Delete obsolete code instead of commenting it out.  
6. Any secret or sensitive content must be stored in separate file and added to [.gitignore](../.gitignore).
7. If the current version is released, a new one should be provided in [VERSION](../VERSION) file.

---

## 3. Branch naming

Branches should be created from `develop`.

> [!WARNING]
> Direct pushes to protected branches are **forbidden**.

Branch names example:

- bug/short-description
- feature/short-description
- update/short-description
- delete/short-description
- bug/#123-short-description
- feature/#98-short-description
- bug/JIRA-1234-description

> [!NOTE]
> Include the related issue number (or identifier for JIRA/other services) whenever one exists.

### Keyword guide

| Keyword   | Use when you                      |
|-----------|-----------------------------------|
| `bug`     | Fix a reported problem            |
| `feature` | Add new functionality             |
| `update`  | Improve or refactor existing code |
| `delete`  | Remove unused code or assets      |

Keep names short yet descriptive.

---

## 4. Commit messages

Examples:

- [FIX] Short description of the bug fix
- [ADD] Short description of the new feature
- [UPDATE] Short description of the update / refactor
- [DELETE] Short description of what was removed

> [!NOTE]
> If the work is related to an issue, include the number just like in the branch name:  
>  
> [FIX] [#123] Handle null user session
> [ADD] [JIRA-1234] New feature

Tag list:

| Tag       | When to use it                              |
|-----------|---------------------------------------------|
| `[FIX]`   | Fixing incorrect behavior                   |
| `[ADD]`   | Introducing new functionality               |
| `[UPDATE]`| Improving or refactoring existing code      |
| `[DELETE]`| Removing code, files, or dependencies       |

---

## 5. Pull requests

- **Title** begins with the same tag as your commits (`[ADD]`, `[FIX]`, `[UPDATE]`, `[DELETE]`).  
  Example: `[ADD] Dark mode toggle`
- **Description**: What changed, why, and how to test.
- **CI/CD** must pass before merging.

---

## 6. Tags and Releases

Both tags and releases must be named **without** `v` prefix, e.g. `1.2.3`.

---

## 7. Feature & bug requests

1. **Search first** to avoid duplicates.
2. **Use the issue templates** for clear reproduction steps or acceptance criteria.
3. **Label appropriately** (`feature`, `bug`, etc.) so maintainers can triage quickly.

---

Thanks again for contributing - your effort keeps this project thriving! 🎉
