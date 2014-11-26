#Muffin (An Umbraco Foundation)

Muffin is the reincarnation of the foundation formely known as the Macaw Umbraco Foundation. It's a simplefied templating API / facade on top of the
Umbraco framework to allow easier front-end development, quicker development. It also comes with some helpers and default implementations of frequently used functionality.
It tries to focus SOLID principles to allow better testability. For more info you can als find a presentation given at the BUUG: 
[A dynamic journey of concerns](http://www.slideshare.net/JeroenWijdeven)

###Some features:  

- Frontend developers friendly dynamic models for Templates and 
- Frontend developers friendly Partial view Macros 
- Paged result views  
- Basic property editor converters 
- Focused on the seperation of concers principles / SOLID principles, to allow better unit-testability
- Helpers to convert objects to Json
- Mappers for using typed objects..
- Umbraco.ImageCropper support, works for traditional mediatypes aswell.
- Standard Examine Search implementation  

###Underconstruction:

- File based caching with MvcDonutCaching
- Support of the new Umbraco Grid datatype (out-of-the-box)
- Azure ready examine: https://github.com/Shazwazza/Examine/wiki/Examine-with-Azure-Websites

Currently build on top of Umbraco 7.2.0beta

##Installation
Umbraco is installed and updated via Nuget. Like this is supported since version 7.2.0

##Examples and documentation

- The example database username is "admin" and the password is "password".  
- The nuget package and the example project both use Autofac. See the implementation folder for more information and / or customization. 

##Credits
- [Macaw Umbraco Foundation](https://github.com/MacawNL/Macaw.Umbraco.Foundation/)