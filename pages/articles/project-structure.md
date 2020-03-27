# Project structure

[EduCATS](./source/EduCATS) project has the following structure:

- **Configuration**  
Contains configuration files like [AppConfig](https://ilyalehchylin.github.io/educats-xamarin/api/EduCATS.Configuration.AppConfig.html) (which is used to configure packages, helpers and tools on application start).

- **Constants**  
Contains static classes with global constants.

- **Controls**  
Contains views & controls used in the application.

- **Data**  
Retrieves, handles, caches and gives access to API data with the help of [DataAccess](https://ilyalehchylin.github.io/educats-xamarin/api/EduCATS.Data.DataAccess.html).

- **Fonts**  
Provides fonts and font controllers. More details [here](./pages/articles/resources.md#fonts).

- **Helpers**  
Provides helpers and tools for the application (for both UI and logic).

- **Localization**  
Contains `JSON` localization files. More details [here](./pages/articles/resources.md#localization).

- **Networking**  
Provides wrapper for the network services and contains API methods (used only by [DataAccess](https://ilyalehchylin.github.io/educats-xamarin/api/EduCATS.Data.DataAccess.html)).

- **Pages**  
Contains application pages with `Model-View-ViewModel (MVVM)` pattern.  
  - **Models**  
  Page models.
  - **ViewModels**  
  Page view models (logic).
  - **Views**  
  Page views (UI).

- **Properties**  
Project's `AssemblyInfo` (used basically for fonts, more details [here](./pages/articles/resources.md#fonts)).

- **Themes**  
Used to store application themes and provides manager for this purpose. More details here.

### Additional information

- Interfaces are stored in separate directories in places where they are implemented (e.g. `Themes/DependencyServices/Interfaces`, `Data/Interfaces`, etc.).
- API models are stored in `Data/Models`.
- Networking models (for `POST` requests) are stored in `Networking/Models`.
- `Data` also contains `User` directory used for managing user's account.
