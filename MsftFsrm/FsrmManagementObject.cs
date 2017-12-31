using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using MsftFsrm.Internal;

namespace MsftFsrm
{
    public abstract class FsrmManagementObject
    {
        #region Constants

        /// <summary>
        /// The FSRM WMI namespace.
        /// </summary>
        private const string Namespace = "root\\Microsoft\\Windows\\FSRM";

        #endregion

        #region Properties

        /// <summary>
        /// The WMI class wrapped by this instance.
        /// </summary>
        [FsrmWmiIgnore]
        protected ManagementObject BaseWmiObject { get; set; }

        /// <summary>
        /// The computer to connect to. Default is <see cref="Environment.MachineName"/>.
        /// </summary>
        [FsrmWmiIgnore]
        public string ComputerName { get; set; } = Environment.MachineName;

        /// <summary>
        /// Credentials to access a remote computer.
        /// </summary>
        [FsrmWmiIgnore]
        public NetworkCredential Credentials { get; set; }

        #endregion

        #region Constructors

        protected FsrmManagementObject()
        {            
        }

        protected FsrmManagementObject(string computerName, NetworkCredential credentials)
        {
            this.ComputerName = computerName;
            this.Credentials = credentials;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the current instance to the underlying WMI class.
        /// </summary>
        protected void Bind()
        {
            var attr = this.GetType().GetCustomAttribute<FsrmWmiObjectAttribute>();
            if (attr == null)
            {
                throw new Exception("The instance does not have the FsrmWmiObject attribute.");
            }

            this.BaseWmiObject = this.GetFsrmWmiObject(attr.ClassName);

            foreach (var property in this.GetAssigableProperties())
            {
                var propAttr = property.GetCustomAttribute<FsrmWmiPropertyAttribute>();
                //var typeConvAttr = property.GetCustomAttribute<FsrmWmiPropertyConversionAttribute>();

                property.SetValue(this, this.BaseWmiObject[propAttr?.Name ?? property.Name], null);
            }
        }

        /// <summary>
        /// Refreshes the instance to reflect any changes made by another process. If any changes made in the current process have not been persisted by calling <see cref="SaveChanges"/>, the changes will be overwritten.
        /// </summary>
        public virtual void Refresh()
        {
            this.Bind();
        }

        /// <summary>
        /// Calls <see cref="ManagementObject.Put()"/> to persist modified properties.
        /// </summary>
        public virtual void SaveChanges()
        {
            foreach (var prop in this.GetAssigableProperties())
            {
                var wmiPropAttr = prop.GetCustomAttribute<FsrmWmiPropertyAttribute>();
                var value = prop.GetValue(this, null);

                if (this.BaseWmiObject[wmiPropAttr?.Name ?? prop.Name] == value)
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

        private IEnumerable<PropertyInfo> GetAssigableProperties()
        {
            return this.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttribute<FsrmWmiIgnoreAttribute>() == null);
        }

        #endregion
    }
}
