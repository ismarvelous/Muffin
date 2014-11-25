#Muffin (An Umbraco Foundation)

Muffin is the reincarnation of the foundation formely known as the Macaw Umbraco Foundation. It's a simplefied API on top of the
Umbraco framework to allow easier front-end development. It tries to focus on the separation of concerns principle
to allow better testability. For more info you can als find the presentation sheets here: 
[A dynamic journey of concerns](http://www.slideshare.net/JeroenWijdeven)

Some features are:  

- Standard Examine Search implementation  
- Paged result views  
- Frontend developers friendly dynamic models for Templates and Macros 
- Basic property editor converters 
- Focused on the seperation of concers principle, to allow better unit-testability
- File based caching with MvcDonutCaching
- Several helpers like ToJson and mappers for typed objects
- Umbraco.ImageCropper support, for traditional mediatypes aswell.

new / underconstruction:

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