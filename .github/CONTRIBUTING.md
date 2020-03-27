# For contributors

## Introduction

Thank you for your interest to contribute to **EduCATS Xamarin** project!

You can help us with the following:
- New features
- Bug fixes
- Refactoring
- Unit tests
- UI tests
- Documentation improvements
- Bug issues
- Feature requests

To start you can check [open issues](https://github.com/IlyaLehchylin/educats-xamarin/issues) and 
choose a preferred work. You can also start testing beta version of application 
(more details on [README](../README.md)) and then create your own 
[bug reports](https://github.com/IlyaLehchylin/educats-xamarin/issues/new?assignees=&labels=&template=bug-report.md&title=) 
or [feature requests](https://github.com/IlyaLehchylin/educats-xamarin/issues/new?assignees=&labels=&template=feature_request.md&title=).

If you're a designer you can add your own graphics which you can import inside the projects 
or [graphics](../graphics) directory.

Ask repository owner to add you to contributors list.

## Main code rules

- EduCATS project uses [MVVM](https://wikipedia.org/wiki/Model–view–viewmodel) as the base architectural pattern pattern.
- Don't forget to make [documentation comments](https://docs.microsoft.com/dotnet/csharp/language-reference/language-specification/documentation-comments) on your code.
- Private global variables should start with underscore (e.g. `const float _frameRadius = 10`).
- All constants should be placed at the top of the class (if they are used only inside that class), 
in the `Constants/GlobalConsts.cs` class (if they are used across the project), 
in `Themes` (if they represent colors or images, more details [here](../pages/articles/resources.md#themes) 
or in `Localization` (if they represent localized key, more details [here](../pages/articles/resources.md#localization).
- API services should be wrapped inside `DataAccess.cs` (more details [here](../pages/articles/services.md)).
- Data managers and other helpers should be placed inside `Helpers`.

## Repository

### Branches

Branches **must be** created from [develop](https://github.com/IlyaLehchylin/educats-xamarin/tree/develop) branch. 
Direct pushes to **develop** and **master** are not allowed.

Name conventions for branches:

```
[feature/bug/update]/[#issue_number-branch-name]
```

Examples:

```
bug/#123-app-settings-crash
update/docs-resources
feature/new-page
feature/#41-new-feature
```

### Commits

Name conventions for commits:

```
[ADD/DELETE/UPDATE/FIX/ETC] [#ISSUE_NUMBER] Description
```

Examples:

```
[FIX] [#123] App settings crash
[UPDATE] Docs resources
[ADD] New page
[ADD] [#41] New feature
[FEATURE] New app settings
```

#### Commit pattern for docs update:

`git commit -m "[ADD] Documentation source" --author="nocontribute <>"`

In this case GitHub will ignore `docs/` directory contribution (500,000+ lines) and won't spoil your statistics.

### Pull requests

Before submitting pull request you should choose the reviewer.  
The following checks should pass:

- Bitrise ci check (more details [here](../pages/articles/continuous-integration.md))
- Reviewer check
