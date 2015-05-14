using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Muffin.Core;
using Muffin.Core.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Muffin.Infrastructure
{
    public class CastleContentFactory : IPublishedContentModelFactory
    {
        private readonly Dictionary<string, Type> _types;

        public CastleContentFactory(IEnumerable<Type> types)
        {
            var ctorArgTypes = new[] { typeof(IPublishedContent) };
            var constructors = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var type in types)
            {
                var constructor = type.GetConstructor(ctorArgTypes);
                if (constructor == null)
                    throw new InvalidOperationException(string.Format("Type {0} is missing a public constructor with one argument of type IPublishedContent.", type.FullName));

                var attribute = type.GetCustomAttribute<PublishedContentModelAttribute>(false);
                var typeName = attribute == null ? type.Name : attribute.ContentTypeAlias;

                if (constructors.ContainsKey(typeName))
                    throw new InvalidOperationException(string.Format("More that one type want to be a model for content type {0}.", typeName));

                constructors[typeName] = type;
            }

            _types = constructors.Count > 0 ? constructors : null;
        }

        public IPublishedContent CreateModel(IPublishedContent content)
        {
            // fail fast
            if (_types == null)
                return content;

            //not generating objects twice..
            if (content is IModel)
                return content;

            // be case-insensitive
            var contentTypeAlias = content.DocumentTypeAlias;

            var generator = new ProxyGenerator();
            Type type;
            if (_types.TryGetValue(contentTypeAlias, out type))
            {
                return generator.CreateClassProxy(type, new object[] {content}, new ModelInterceptor(content)) as IPublishedContent;
            }
            
            return content;
        }
    }

    internal static class InterceptorExtensions
    {
        internal static PropertyInfo GetProperty(this MethodInfo method)
        {
            var takesArg = method.GetParameters().Length == 1;
            var hasReturn = method.ReturnType != typeof(void);
            if (takesArg == hasReturn) return null;
            if (takesArg)
            {
                if (method.DeclaringType != null)
                    return method.DeclaringType.GetProperties().FirstOrDefault(prop => prop.GetSetMethod() == method);
            }
            else
            {
                if (method.DeclaringType != null)
                    return method.DeclaringType.GetProperties().FirstOrDefault(prop => prop.GetGetMethod() == method);
            }

            return null;
        }
    }

    internal class ModelInterceptor : IInterceptor
    {
        public ISiteRepository Repository
        {
            get { return DependencyResolver.Current.GetService<ISiteRepository>(); }
        }

        public readonly IPublishedContent Source;
        public ModelInterceptor(IPublishedContent source)
        {
            Source = source;
        }

        public void Intercept(IInvocation invocation)
        {
            //do things before excecution

            if (invocation.Method.Name.StartsWith("get_"))
            {
                var propertyAlias = invocation.Method.Name.Remove(0, 4); //todo: this is quick and dirty remove get_ from method name

                if (Source.HasProperty(propertyAlias))
                {
                    var value = Source.GetPropertyValue(propertyAlias);

                    var converterAttribute = invocation.Method.GetProperty().GetCustomAttribute<TypeConverterAttribute>();
                    if (converterAttribute != null)
                    {
                        var converterType = Type.GetType(converterAttribute.ConverterTypeName);
                        if (converterType != null)
                        {
                            var converter = Activator.CreateInstance(converterType) as TypeConverter;
                            invocation.ReturnValue = converter != null ? converter.ConvertFrom(value) : value;
                        }
                    }
                    else
                    {
                        if (invocation.Method.ReturnType == typeof(string) && !(value is string))
                        {
                            invocation.ReturnValue = value.ToString();
                        }
                        else
                        {
                            invocation.ReturnValue = value;
                        }
                    }
                }
                else
                {
                    invocation.Proceed();
                }
            }
            else
            {
                invocation.Proceed();                
            }

            //do things after excecution
        }
    }
}
