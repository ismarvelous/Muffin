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
        protected readonly ProxyGenerator Generator;

        public CastleContentFactory(IEnumerable<Type> types)
        {
            Generator = new ProxyGenerator();
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

        /// <summary>
        /// Creates a Castle Dynamic (typed) proxy.
        /// </summary>
        /// <param name="content">Type which needs to be proxied, if already done the already proxied (IModel) is returned</param>
        /// <returns></returns>
        public IPublishedContent CreateModel(IPublishedContent content)
        {
            //not generating objects twice.. // fail fast.
            if (content is IModel || _types == null || content == null)
                return content;

            // be case-insensitive
            var contentTypeAlias = content.DocumentTypeAlias;

            //use DynamicProxy2 proxy generator to generate the proxies.

            Type type;
            if (_types.TryGetValue(contentTypeAlias, out type))
            {
                return Generator.CreateClassProxy(type, new object[] { content }, new ModelInterceptor(content)) as IPublishedContent;
            }

            return content;
        }
    }

    internal static class InterceptorExtensions
    {
        /// <summary>
        /// When the MethodInfo is a method of a property it returns the property otherwise it returns null.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
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
        public readonly IPublishedContent Source;
        public ModelInterceptor(IPublishedContent source)
        {
            Source = source;
        }

        public void Intercept(IInvocation invocation)
        {
            //do things before excecution

            var property = invocation.Method.GetProperty();
            var ignore = property != null && 
                (property.GetCustomAttribute<MuffinIgnoreAttribute>() != null ||
                property.GetSetMethod() == null); //skip everything that has no public setter

            var propertyAlias = property?.Name;
            if (Source.HasProperty(propertyAlias) && !ignore)
            {
                //DIRTY HACK: to support macro collections..
                var value = invocation.Method.ReturnType == typeof(IEnumerable<DynamicMacroModel>) ?
                    Source.GetProperty(propertyAlias) : //for macros
                    Source.GetPropertyValue(propertyAlias); //default

                var converterAttribute = property.GetCustomAttribute<TypeConverterAttribute>();
                if (converterAttribute != null) //TypeConverter support.
                {
                    var converterType = Type.GetType(converterAttribute.ConverterTypeName);
                    if (converterType == null) return;

                    var converter = Activator.CreateInstance(converterType) as TypeConverter;
                    invocation.ReturnValue = converter != null ?
                        converter.ConvertFrom(value) :
                        value;
                }
                else if (invocation.Method.ReturnType == typeof(string) && !(value is string))
                {
                    //when return type is string always return the to string value.
                    invocation.ReturnValue = value?.ToString();
                }
                else
                {
                    invocation.ReturnValue = value;
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

