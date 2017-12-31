using System.Management;
using System.Net;
using static MsftFsrm.Strings;

namespace MsftFsrm.Internal
{
    internal class Helpers
    {
        public static ManagementObject GetFsrmWmiObject(string className, string computerName = null, NetworkCredential credentials = null)
        {
            return GetFsrmWmiObjectCollection(className, computerName, credentials).Single();
        }
        public static ManagementObjectCollection GetFsrmWmiObjectCollection(string className, string computerName = null, NetworkCredential credentials = null)
        {
            var scope = computerName == null
                ? new ManagementScope()
                : new ManagementScope($"\\\\{computerName}\\{Constants.FsrmWmiNamespace}");

            if (credentials != null)
            {
                scope.Options = new ConnectionOptions
                {
                    Username = credentials.UserName,
                    SecurePassword = credentials.SecurePassword,
                    Authority = $"NTLMDOMAIN:{credentials.Domain}",
                    Impersonation = ImpersonationLevel.Impersonate
                };
            }

            var searcher = new ManagementObjectSearcher(scope, new ObjectQuery(string.Format(WmiQuerySelectStarFromClass, className)));
            return searcher.Get();
        }
    }
}
