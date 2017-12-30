using System;
using System.Security;

namespace fsrman
{
    internal static class ExtensionMethods
    {
        public static string Substring(this string s, Func<string, string> substringFunc)
        {
            return substringFunc?.Invoke(s);
        }

        public static SecureString ToSecureString(this string s)
        {
            var ss = new SecureString();
            foreach (var c in s)
            {
                ss.AppendChar(c);
            }
            return ss;
        }

        public static int GetLeadingWhitespace(this string s)
        {
            var idx = -1;
            while (char.IsWhiteSpace(s[++idx]))
                ;
            return idx;
        }
    }
}
