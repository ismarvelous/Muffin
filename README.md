#Muffin (An Umbraco Foundation)

Muffin is a simplefied templating API / facade on top of the
Umbraco framework to allow easier front-end development, quicker development. It also comes with some helpers and default implementations of frequently used functionality.
It tries to focus SOLID principles as much as possible to allow better testability. A summary of what we try to achieve:  
[Muffin an Umbraco Foundation](http://www.slideshare.net/JeroenWijdeven/muffin-an-umbraco-foundation)

###Some features:  

- Frontend developers friendly hybride "dynamic" or typesafe models for templates 
- Frontend developers friendly Partial view Macros
- T4 POCO model generation 
- Paged result views  
- Basic property editor converters 
- Focused on SOLID principles, to allow better unit-testability
- Mappers to convert objects to Json
- Mappers to convert objects to typed objects..
- Rss feeds for all document types http://--domain--/rss/{urlpath}
- Json Request converters. By calling http://--domain--/json/{urlpath} the IPublishedContent based on the urlpath is returned as Json.
- Umbraco.ImageCropper simplefied API, works for traditional mediatypes and grid images aswell.
- Standard Examine Search implementation  
- File based caching with MvcDonutCaching
- Grid manipulation (typed grid object and control value converters)

###Underconstruction:

The whole foundation is constantly under development. 

Like to know what we are working on right now, see our [Trello Board](https://trello.com/b/YMtpIdMT/muffin-development)

Currently build on top of Umbraco 7.2.4

##Installation
Umbraco is installed and updated via Nuget.

##Examples and documentation

- The example database username is "admin" and the password is "password".  
- The nuget package and the example project both use Autofac. See the implementation folder for more information and / or customization. 

##Credits
- [Macaw Umbraco Foundation](https://github.com/MacawNL/Macaw.Umbraco.Foundation/)