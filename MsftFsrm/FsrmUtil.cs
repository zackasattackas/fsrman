using System;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using Microsoft.Dism;
using MsftFsrm.Internal;

namespace MsftFsrm
{
    public class FsrmUtil
    {
        #region Constants

        /// <summary>
        /// The name of the File Server Resource Manager Windows feature.
        /// </summary>
        internal const string FeatureName = "FS-Resource-Manager";

        #endregion

        #region Public Methods      

        /// <summary>
        /// Indicates whether the File Server Resource Manager service is installed and running.
        /// </summary>
        /// <returns></returns>
        public static bool IsServiceRunning()
        {
            return IsServiceRunning(computerName: Environment.MachineName);
        }
        /// <summary>
        /// Indicates whether the File Server Resource Manager service is installed and running.
        /// </summary>
        /// <returns></returns>
        public static bool IsServiceRunning(string computerName, NetworkCredential credentials = null)
        {
            ServiceController service;
            if (computerName != Environment.MachineName)
            {
                if (credentials == null)
                {
                    service = ServiceController.GetServices(computerName).FirstOrDefault(s => s.ServiceName == Constants.ServiceName);
                }
                else
                {
                    using (new Impersonation(credentials.Domain, credentials.UserName, credentials.Password))
                    {
                        service = ServiceController.GetServices(computerName).FirstOrDefault(s => s.ServiceName == Constants.ServiceName);
                    }
                }
            }
            else
            {
                service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == Constants.ServiceName);
            }

            return service != null && service.Status == ServiceControllerStatus.Running;
        }
        /// <summary>
        /// Attempts to enable the "FS-Resource-Manager" Windows feature via the Dism API.
        /// </summary>
        /// <param name="restartNeeded">Indicates whether a restart is needed to complete the installation.</param>
        /// <returns>True if the feature was installed successfully.</returns>
        public static bool EnableWindowsFeature(out bool restartNeeded)
        {
            DismApi.Initialize(DismLogLevel.LogErrorsWarningsInfo);
            using (var dismSession = DismApi.OpenOnlineSession())
            {
                var featureInfo = DismApi.GetFeatureInfo(dismSession, FeatureName);
                
                ThrowIfRestartNeeded(featureInfo.RestartRequired);
                restartNeeded = false;

                switch (featureInfo.FeatureState)
                {
                    case DismPackageFeatureState.NotPresent:
                        throw new Exception($"The Windows feature {FeatureName} is not availble on this system.");
                    case DismPackageFeatureState.UninstallPending:
                        throw new Exception($"The Windows feature {FeatureName} cannot be enabled because there is a pending uninstall.");
                    case DismPackageFeatureState.Staged:
                        break;
                    case DismPackageFeatureState.Resolved:
                        break;
                    case DismPackageFeatureState.Installed:
                        return true;
                    case DismPackageFeatureState.InstallPending:
                        throw new Exception($"The Windows feature {FeatureName} already has a pending installation.");
                    case DismPackageFeatureState.Superseded:
                        break;
                    case DismPackageFeatureState.PartiallyInstalled:
                        break;
                    default:
                        break;
                }

                
                DismApi.EnableFeatureByPackageName(dismSession, FeatureName, null, false, true);

                featureInfo = DismApi.GetFeatureInfo(dismSession, FeatureName);

                restartNeeded = featureInfo.RestartRequired == DismRestartType.Possible || featureInfo.RestartRequired == DismRestartType.Required;

                DismApi.Shutdown();
                return featureInfo.FeatureState == DismPackageFeatureState.Installed ||
                       featureInfo.FeatureState == DismPackageFeatureState.PartiallyInstalled;
            }            
        }
        /// <summary>
        /// Attempts to disable the "FS-Resource-Manager" Windows feature via the Dism API. 
        /// </summary>
        /// <param name="restartNeeded">Indicates whether a restart is needed to complete the uninstall.</param>
        /// <returns>True if the feature was uninstalled successfully.</returns>
        public static bool DisableWindowsFeature(out bool restartNeeded)
        {
            DismApi.Initialize(DismLogLevel.LogErrorsWarningsInfo);
            using (var dismSession = DismApi.OpenOnlineSession())
            {
                var featureInfo = DismApi.GetFeatureInfo(dismSession, FeatureName);

                ThrowIfRestartNeeded(featureInfo.RestartRequired);
                restartNeeded = false;

                switch (featureInfo.FeatureState)
                {
                    case DismPackageFeatureState.NotPresent:
                        throw new Exception($"The Windows feature {FeatureName} is not availble on this system.");
                    case DismPackageFeatureState.UninstallPending:
                        throw new Exception($"The Windows feature {FeatureName} already has a pending uninstall.");
                    case DismPackageFeatureState.Staged:
                        break;
                    case DismPackageFeatureState.Resolved:
                        break;
                    case DismPackageFeatureState.Installed:
                        return true;
                    case DismPackageFeatureState.InstallPending:
                        throw new Exception($"The Windows feature {FeatureName} cannot be disabled because it currently has a pending installation.");
                    case DismPackageFeatureState.Superseded:
                        break;
                    case DismPackageFeatureState.PartiallyInstalled:
                        break;
                    default:
                        break;
                }


                DismApi.DisableFeature(dismSession, FeatureName, null, false);

                featureInfo = DismApi.GetFeatureInfo(dismSession, FeatureName);

                restartNeeded = featureInfo.RestartRequired == DismRestartType.Possible || featureInfo.RestartRequired == DismRestartType.Required;

                DismApi.Shutdown();
                return featureInfo.FeatureState != DismPackageFeatureState.Installed &&
                       featureInfo.FeatureState != DismPackageFeatureState.PartiallyInstalled &&
                       featureInfo.FeatureState != DismPackageFeatureState.InstallPending;
            }
        }

        #endregion

        #region Helper Methods

        private static void ThrowIfRestartNeeded(DismRestartType restartType)
        {
            if (restartType == DismRestartType.Possible || restartType == DismRestartType.Required)
            {
                throw new RestartNeededException();
            }
        }

        #endregion
    }
}
