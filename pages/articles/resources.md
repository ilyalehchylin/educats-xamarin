# Resources

## Images

Images should be imported as described [here](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/images?tabs=windows#local-images):

- iOS images should be placed in `Asset` catalogs according to an application theme (`Light.xcassets` is used as default).
	
	- Before pushing:
	
		- Open changed `Asset` catalogs;
		- Fix all image sets with exclamation points (choose image set, select items with exclamation points and delete them);
		- Open `Attributes Inspector` (`View` - `Inspectors` - `Show Attributes Inspector`);
		- Select all new image sets;
		- Set `Appearances` to `None`;
		- Uncheck all `Devices` except `Universal` (if you are using only universal icons and images).
		
	- Push resources with "no contribute" commit in order not to spoil repository contribution statistics ([details here](https://github.com/ilyalehchylin/educats-xamarin/blob/master/.github/CONTRIBUTING.md#commit-pattern-for-docs-update)).
	
- Android image should be placed in `drawable` directory.

## Fonts

To add new font (`.ttf` supported only) please follow the next steps:

1. Add new font files (Font-Regular and Font-Bold) to `EduCATS/Fonts/Resources` directory and set build action to `EmbeddedResource`.

2. Add the following lines to the `EduCATS/Properties/AssemblyInfo.cs` file:

```csharp
[assembly: ExportFont("Font-Regular.ttf")]
[assembly: ExportFont("Font-Bold.ttf")]
```

3. If there is no bold version of font, please follow additional steps:

- Open `EduCATS/Fonts/FontExclude.cs` file.
- Add font name to the `_excludeList`:
  
```csharp
static readonly List<string> _excludeList = new List<string> {
	"Other fonts...",
  	"Font-Regular"
};
```

4. Usage in controls:

```csharp
var entry = new Entry {
	FontFamily = FontsController.GetCurrentFont(bold: true),
	FontSize = FontSizeController.GetSize(size, typeof(Entry))
};
```

5. If you use base control (like label, entry or button), you can use `AppStyles` in `EduCATS/Heplers/Styles/AppStyles.cs`:

```csharp
var entry = new Entry {
	Style = AppStyles.GetEntryStyle(size: NamedSize.Large, bold: true)
};
```

## Localization

To add a new language, please do the following:

1. Copy `EduCATS/Localization/**.json`.

2. Translate it to a new language and name it with two-letter language code (`ISO 639-1`). For example, `de.json`.

3. Add it to `EduCATS/Localization` and set build action to `EmbeddedResource`.

4. Open `EduCATS/Configuration/AppConfig.cs` file, find `setupLocalization()` method and before add there a new line before `CrossLocalization.SetDefaultLanguage(Languages.EN.LangCode);`:

```csharp
CrossLocalization.AddLanguageSupport(Languages.DE); // Or whatever language you want
```

This automatically will add new language support across the application (including Settings). 

More info about localization can be found [here](https://github.com/nyxbull/CrossLocalization).

## Themes

To add a new theme, please follow the next steps:

1. Create a new theme class in `EduCATS/Themes/Templates`;

2. Inherit from `DefaultTheme`;

3. Override properties declared in `DefaultTheme`:

```csharp
override public string MainSelectedTabColor => "#27AEE1";
```

To add a new theme color/image/etc. do the following:

1. Add a new property to `EduCATS/Themes/Interfaces/ITheme.cs`:

```csharp
string AppStatusBarBackgroundColor { get; }
```

2. Implement it in `DefaultTheme.cs`:

```csharp
virtual public string AppStatusBarBackgroundColor => "#27AEE1";
```

3. (optional) Override this property in other themes.
