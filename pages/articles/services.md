# Services

To add a new service follow the next steps:

1. Add the link to API service to `EduCATS/Networking/Links.cs` file:

```csharp
public static string GetRecomendations => $"{Servers.Current}/Tests/GetRecomendations";
```

2. Add the app service method to `EduCATS/Networking/AppServices/AppServices.cs`:

```csharp
public static async Task<object> GetRecommendations(int subjectId, int userId)
{
  return await AppServicesController.Request($"{Links.GetRecomendations}?subjectId={subjectId}&studentId={userId}");
}
```

2.1. If you need `POST` request you should create the model for body in `EduCATS/Networking/Models` and use it like:

```csharp
static string postRequest(string username)
{
  var usernameBody = new UsernameModel {
    Username = username
  };
  
  var body = JsonController.ConvertObjectToJson(userLogin);
  return await AppServicesController.Request(Links.PostRequest, body);
}
```

3. Add the service callback to `EduCATS/Data/DataAccessCallbacks.cs`:

```csharp
static async Task<object> getRecommendationsCallback(int subjectId, int userId) => await AppServices.GetRecommendations(subjectId, userId);
```

4. Add the model for API result in `EduCATS/Data/Models/{API_name}.cs`:

```csharp
public class RecommendationModel
{
  [JsonProperty("IsTest")]
  public bool IsTest { get; set; }

  [JsonProperty("Id")]
  public int Id { get; set; }

  [JsonProperty("Text")]
  public string Text { get; set; }
}
```

5. Specify caching key in `EduCATS/Constants/GlobalConsts.cs`:

```csharp
public const string DataGetRecommendationsKey = "GET_RECOMMENDATIONS_KEY";
```

6. Add the method to `EduCATS/Data/DataAccess.cs`:

```csharp
public async static Task<List<RecommendationModel>> GetRecommendations(int subjectId, int userId)
{
  var dataAccess = new DataAccess<RecommendationModel>(
    "recommendations_fetch_error", // localized error key
    getRecommendationsCallback(subjectId, userId), // callback specified in DataAccessCallbacks.cs
    $"{GlobalConsts.DataGetRecommendationsKey}/{subjectId}/{userId}"); // specified caching key; this parameter is optional - no caching will be provided if null
  return getDataObject(dataAccess, await dataAccess.GetList()) as List<RecommendationModel>;
}
```

7. Use it inside your ViewModel:

```csharp
var recommendations = await DataAccess.GetRecommendations(CurrentSubject.Id, AppUserData.UserId);
```
