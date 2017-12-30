using System;

namespace MsftFsrm
{
    public class RestartNeededException : Exception
    {
        public RestartNeededException()
            : base($"The Windows feature {FsrmUtil.FeatureName} is pending a system restart.")
        {            
        }
    }
}