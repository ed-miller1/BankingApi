using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BankingApi.Utilities.AutoMapping
{
    public static class AutoMapper
    {
        public static T MapTo<TS, T>(TS source)
        {
            var target = (T)Activator.CreateInstance(typeof(T));
            var sType = source.GetType();
            var sourceProperties = source.GetType().GetProperties().ToList();
            var targetProperties = typeof(T).GetProperties().ToList();

            sourceProperties.ForEach(x =>
            {
                var tProp = targetProperties.FirstOrDefault(
                    tp => tp.Name == x.Name && tp.PropertyType == x.PropertyType);
                if (tProp != default(PropertyInfo))
                {
                    TrySetProperty(target, tProp.Name, x.GetValue(source));
                }

            });
            return target;
        }

        private static bool TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(obj, value, null);
                return true;
            }
            return false;
        }
    }
}
