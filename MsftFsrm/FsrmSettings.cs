using System;
using System.Management;
using System.Net;

namespace MsftFsrm
{
    [FsrmWmiObject(ClassName = "MSFT_FSRMSettings.Server='Reserved'")]
    public class FsrmSettings : FsrmManagementObject
    {
        //private const string Namespace = "root\\Microsoft\\Windows\\FSRM";
        //private const string ClassName = "MSFT_FSRMSettings.Server='Reserved'";
        //private readonly ManagementObject _object;

        public string AdminEmailAddress { get; set; }
        public string FromEmailAddress { get; set; }
        public string SmtpServer { get; set; }


        public FsrmSettings(string computerName = null, NetworkCredential credentials = null)
            :base(computerName, credentials)
        {
            //this._object = GetWmiObject(computerName, credentials);
            //this._object.Get();

            //this.AdminEmailAddress = (string) this._object[nameof(this.AdminEmailAddress)];
            //this.FromEmailAddress = (string) this._object[nameof(this.FromEmailAddress)];
            //this.SmtpServer = (string) this._object[nameof(this.SmtpServer)];
            base.Bind(this);
        }

        public void SaveChanges()
        {
            //this._object[nameof(this.AdminEmailAddress)] = this.AdminEmailAddress;
            //this._object[nameof(this.FromEmailAddress)] = this.FromEmailAddress;
            //this._object[nameof(this.SmtpServer)] = this.SmtpServer;

            //this._object.Put();
            this.BaseWmiObject.Put();
        }

        public void SendTestEmail(string toEmailAddress)
        {
            //var inParams = this._object.GetMethodParameters(nameof(this.SendTestEmail));

            //inParams["ToEmailAddress"] = toEmailAddress;

            //this._object.InvokeMethod(nameof(this.SendTestEmail), inParams, null);
        }

        //private static ManagementObject GetWmiObject(string computerName = null, NetworkCredential credentials = null)
        //{
        //    var scope = computerName == null ? new ManagementScope() : new ManagementScope($"\\\\{computerName ?? Environment.MachineName}\\{Namespace}");
        //    if (credentials != null)
        //    {
        //        scope.Options = new ConnectionOptions
        //        {
        //            Username = credentials.UserName,
        //            SecurePassword = credentials.SecurePassword,
        //            Authority = $"NTLMDOMAIN:{credentials.Domain}",
        //            Impersonation = ImpersonationLevel.Impersonate
        //        };
        //    }

        //    return new ManagementObject(scope, new ManagementPath(ClassName), new ObjectGetOptions());
        //}
    }
}
