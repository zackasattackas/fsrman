using System;
using System.Linq;
using System.Reflection;

namespace MsftFsrm.Internal
{
    internal static class ExtensionMethods
    {
        public static TAttribute GetCustomAttribute<TAttribute>(this Type t) where TAttribute : Attribute
        {
            return t.GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault();
        }
        public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo m) where TAttribute : Attribute
        {
            return m.GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault();
        }

        public static void Invoke(this MethodInfo m, object obj, params object[] parameters)
        {
            m.Invoke(obj, parameters);
        }
    }
}
