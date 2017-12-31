using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using MsftFsrm.Internal;
using static MsftFsrm.Strings;

namespace MsftFsrm
{

    public class FsrmManagementObject
    {

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

        internal FsrmManagementObject(ManagementObject o)
        {            
            this.Bind(o);
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
        private void Bind(ManagementObject o)
        {
            var attr = this.GetType().GetCustomAttribute<FsrmWmiObjectAttribute>();
            if (attr == null)
            {
                throw new Exception("The instance does not have the FsrmWmiObject attribute.");
            }

            this.BaseWmiObject = Helpers.GetFsrmWmiObject(attr.ClassName);

            foreach (var property in this.GetAssigableProperties())
            {
                var propAttr = property.GetCustomAttribute<FsrmWmiPropertyAttribute>();
                //var typeConvAttr = property.GetCustomAttribute<FsrmWmiPropertyConversionAttribute>();

                property.SetValue(this, this.BaseWmiObject[propAttr?.Name ?? property.Name], null);
            }
        }

        ///// <summary>
        ///// Refreshes the instance to reflect any changes made by another process. If any changes made in the current process have not been persisted by calling <see cref="SaveChanges"/>, the changes will be overwritten.
        ///// </summary>
        //public virtual void Refresh()
        //{
        //    this.Bind();
        //}

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

        //private ManagementObjectCollection GetFsrmWmiObjectCollection(string className)
        //{
        //    var scope = this.ComputerName == null
        //        ? new ManagementScope()
        //        : new ManagementScope($"\\\\{this.ComputerName ?? Environment.MachineName}\\{Namespace}");

        //    if (this.Credentials != null)
        //    {
        //        scope.Options = new ConnectionOptions
        //        {
        //            Username = this.Credentials.UserName,
        //            SecurePassword = this.Credentials.SecurePassword,
        //            Authority = $"NTLMDOMAIN:{this.Credentials.Domain}",
        //            Impersonation = ImpersonationLevel.Impersonate
        //        };
        //    }

        //    var searcher = new ManagementObjectSearcher(scope, new ObjectQuery(string.Format(WmiQuerySelectStarFromClass, className)));
        //    return searcher.Get();
        //}
        //private ManagementObject GetFsrmWmiObject(string className)
        //{
        //    return GetFsrmWmiObjectCollection(className).Single();
        //}

        private IEnumerable<PropertyInfo> GetAssigableProperties()
        {
            return this.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttribute<FsrmWmiIgnoreAttribute>() == null);
        }

        #endregion
    }

    public abstract class FsrmManagementObjectCollection : List<FsrmManagementObject>
    {
        private List<FsrmManagementObject> _list;
        public ManagementObjectCollection BaseWmiObjectCollection { get; private set; }
        public string ComputerName { get; set; }
        public NetworkCredential Credentials { get; set; }

        protected void Bind(ManagementObjectCollection c)
        {
            this._list = new List<FsrmManagementObject>();
            foreach (var o in c)
            {
                this._list.Add(new FsrmManagementObject(o as ManagementObject));
            }
        }
    }
}
