## GraphQL Gateway Implementation Examples


### Project Overview
The current examples within this repository uses the HotChocolate Framework to implement a GraphQL
Gateway within Azure Functions and an ASP.NET Core Web Application. 

#### References
- [🌶 HotChocolate Docs](https://hotchocolate.io)
- [🌶 Framework Repository](https://github.com/ChilliCream/hotchocolate)


### I. Azure Function Implementation
Unlike a traditional ASP.NET Core Application where we can configure 
the host, the current example deomstraits how to implement a custom middleware for executing queries that are processed from Http Pipline. 

#### Deliverables
1. **Download Dependencies**
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
