using System.Net;
using System.Net.NetworkInformation;
using MsftFsrm.Internal;

namespace MsftFsrm
{
    [FsrmWmiObject(ClassName = "MSFT_FSRMFileGroup")]
    public class FsrmFileGroup : FsrmManagementObject
    {
        private const string ClassName = "MSFT_FSRMFileGroup";
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] IncludePattern { get; set; }
        public string[] ExcludePattern { get; set; }

        public FsrmFileGroup(string computerName = null, NetworkCredential credentials = null)
            : base(computerName, credentials)
        {
            this.Bind(Helpers.GetFsrmWmiObjectCollection(ClassName,));
        }
    }
}