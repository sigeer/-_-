using Autofac.Core;
using System.Reflection;

namespace DDDUtility.Autofac
{
    public class AutowiredPropertySelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.CustomAttributes.Any(it => it.AttributeType == typeof(AutowiredAttribute));
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AutowiredAttribute : Attribute
    {
    }

}
