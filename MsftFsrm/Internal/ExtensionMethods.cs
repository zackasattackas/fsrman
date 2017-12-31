using System;
using System.Linq;
using System.Management;
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

        public static ManagementObject Single(this ManagementObjectCollection c)
        {
            if (c.Count == 0)
            {
                throw new Exception("The collection contained no elements."); // Should never happen.
            }
            if (c.Count > 1)
            {
                throw new Exception("The collection contained more the one element.");
            }

            ManagementObject ret = null;
            foreach (var obj in c)
            {
                ret = (ManagementObject) obj;
            }
            return ret;
        }
    }
}
