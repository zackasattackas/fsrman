using System;
using System.Linq;
using System.Management;
using System.Net;

namespace MsftFsrm
{
    public abstract class FsrmManagementObject
    {
        #region Constants

        private const string Namespace = "root\\Microsoft\\Windows\\FSRM";

        #endregion

        #region Properties

        protected ManagementObject BaseWmiObject { get; set; }
        public string ComputerName { get; set; }
        public NetworkCredential Credentials { get; set; }

        #endregion

        #region Constructors

        protected FsrmManagementObject(string computerName, NetworkCredential credentials)
        {
            this.ComputerName = computerName;
            this.Credentials = credentials;
        }

        #endregion

        #region Methods

        protected void Bind(object o)
        {
            var attr = (FsrmWmiObjectAttribute[])o.GetType().GetCustomAttributes(typeof(FsrmWmiObjectAttribute), false);
            var className = attr.First()?.ClassName;

            this.BaseWmiObject = this.GetFsrmWmiObject(className);

            foreach (var property in o.GetType().GetProperties())
            {
                property.SetValue(o, this.BaseWmiObject[property.Name], null);
            }
        }

        protected void SaveChanges(object o)
        {
            foreach (var prop in o.GetType().GetProperties())
            {
                var value = prop.GetValue(o, null);
                if (this.BaseWmiObject[prop.Name] == value)
                {
                    continue;
                }
                this.BaseWmiObject[prop.Name] = value;
            }
            this.BaseWmiObject.Put();
        }

        #endregion

        #region Helper Methods

        private ManagementObject GetFsrmWmiObject(string className)
        {
            var scope = this.ComputerName == null
                ? new ManagementScope()
                : new ManagementScope($"\\\\{this.ComputerName ?? Environment.MachineName}\\{Namespace}");

            if (this.Credentials != null)
            {
                scope.Options = new ConnectionOptions
                {
                    Username = this.Credentials.UserName,
                    SecurePassword = this.Credentials.SecurePassword,
                    Authority = $"NTLMDOMAIN:{this.Credentials.Domain}",
                    Impersonation = ImpersonationLevel.Impersonate
                };
            }

            return new ManagementObject(scope, new ManagementPath(className), new ObjectGetOptions());
        }

        #endregion
    }
}
