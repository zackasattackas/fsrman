using System;
using System.Management;
using System.Net;

namespace fsrman
{
    public class FsrmSettings
    {
        private const string Namespace = "root\\Microsoft\\Windows\\FSRM";
        private const string ClassName = "MSFT_FSRMSettings.Server='Reserved'";
        private readonly ManagementObject _object;

        public string AdminEmailAddress { get; set; }
        public string FromEmailAddress { get; set; }
        public string SmtpServer { get; set; }


        public FsrmSettings(string computerName = null, NetworkCredential credentials = null)
        {
            this._object = GetWmiObject(computerName, credentials);
            this._object.Get();

            this.AdminEmailAddress = (string) this._object[nameof(AdminEmailAddress)];
            this.FromEmailAddress = (string) this._object[nameof(FromEmailAddress)];
            this.SmtpServer = (string) this._object[nameof(SmtpServer)];
        }

        public void SaveChanges()
        {
            this._object[nameof(AdminEmailAddress)] = this.AdminEmailAddress;
            this._object[nameof(FromEmailAddress)] = this.FromEmailAddress;
            this._object[nameof(SmtpServer)] = this.SmtpServer;

            this._object.Put();
        }

        public void SendTestEmail(string toEmailAddress)
        {
            var inParams = this._object.GetMethodParameters(nameof(SendTestEmail));

            inParams["ToEmailAddress"] = toEmailAddress;

            this._object.InvokeMethod(nameof(SendTestEmail), inParams, null);
        }

        private static ManagementObject GetWmiObject(string computerName = null, NetworkCredential credentials = null)
        {
            var scope = computerName == null ? new ManagementScope() : new ManagementScope($"\\\\{computerName ?? Environment.MachineName}\\{Namespace}");
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

            return new ManagementObject(scope, new ManagementPath(ClassName), new ObjectGetOptions());
        }
    }
}
