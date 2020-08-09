## GraphQL Gateway Implementation Examples


### Project Overview
The current examples within this repository uses the HotChocolate Framework to implement a GraphQL Gateway within an HttpTriggered Azure Function and an ASP.NET Core Web Application. These examples will include scenarios which will consume data from services such as third-party APIs, relational database systems like SQL Server utilizing Entity Frameworks, and non-relational document based systems like CosmoDb.

#### References
It's recommended that you read the following documentation and have a strong understanding of the listed references before jumping into the source code.  

- [Fundamentals on ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1)
- [Fundamentals on ASP.NET Cre Dependency Injectio](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)
- [Dependency Injection in Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
- [🌶 HotChocolate Docs](https://hotchocolate.io)
- [🌶 Framework Repository](https://github.com/ChilliCream/hotchocolate)
- [🌶 Demo by HotChocolate Auther (Michael Staib)](https://dev.to/michaelstaib/get-started-with-hot-chocolate-and-entity-framework-e9i)


### I. Azure Function Implementation [In-Progress]
Unlike a traditional ASP.NET Core Application where we can configure the host at Startup, we need to build a sort of Middleware that can handle the execution of incoming request within the Http Pipline. The current example deomstraits how to implement a custom middleware for executing queries that are processed from Http Pipline by parsing the request stream. 
![Implementation Mockup](https://github.com/chasec2018/GraphQL.Implementation.Examples/blob/features/initial_start/Assets/uml-query-middleware-diagram.png)
#### Deliverables
1. **Download Dependencies** ✓ Finished
    - HotChocolate
    - HotChocolate.AspNetCore
    - HotChocolate.Language
    - HotChocolate.Server
    - HotChocolate.Types
    - HotChocolate.Types.Filters
    - HotChocolate.Utilities
    - Microsoft.Azure.Functions.Extensions (This will allow us to enable dependency injection)
    - Microsoft.Extensions.Http (This will add Resiliance to our HttpClient so we don't run into Socket Exhaustion)
2. **Create a Startup Class which inherits from FunctionsStartup**
   - Make sure to have the following attribute above your startup class namespace or none of the service will be added to IoC Container : 
```
[assembly: FunctionsStartup(typeof(YourNamespace.Startup))]
namespace YourNameSpace
{
    public class Startup
    {
        
    }
}
```
2. ****

### II. ASP.NET Core Web App Implementation [Not Finished]
