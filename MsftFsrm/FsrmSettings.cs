using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MsftFsrm
{
    [FsrmWmiObject(ClassName = "MSFT_FSRMSettings.Server='Reserved'")]
    public class FsrmSettings : FsrmManagementObject
    {
        #region Properties

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// The default email address from which email messages are sent. Optional.
        /// </summary>
        public string FromEmailAddress { get; set; }
        /// <summary>
        /// The email address for the administrator. Optional.
        /// </summary>
        public string AdminEmailAddress { get; set; }
        /// <summary>
        /// If multiple email notifications are raised, they will only be performed if at least this many minutes have passed since the last such action was performed. Optional.
        /// </summary>
        public int EmailNotificationLimit { get; set; }
        /// <summary>
        /// If multiple event notifications are raised, they will only be performed if at least this many minutes have passed since the last such action was performed. Optional.
        /// </summary>
        public int EventNotificationLimit { get; set; }
        /// <summary>
        /// If multiple command notifications are raised, they will only be performed if at least this many minutes have passed since the last such action was performed. Optional.
        /// </summary>
        public int CommandNotificationLimit { get; set; }
        /// <summary>
        /// If multiple report notifications are raised, they will only be performed if at least this many minutes have passed since the last such action was performed. Optional.
        /// </summary>
        public int ReportNotificationLimit { get; set; }
        /// <summary>
        /// The maximum number of files to include in a storage report.
        /// </summary>
        public int ReportLimitMaxFile { get; set; }
        /// <summary>
        /// The maximum number of file groups to include in a files by file group report.
        /// </summary>
        public int ReportLimitMaxFileGroup { get; set; }
        /// <summary>
        /// In a files by owner report, the maximum number owners in the report.
        /// </summary>
        public int ReportLimitMaxOwner { get; set; }
        /// <summary>
        /// In a file by file group report, the maximum number of files in any file group.
        /// </summary>
        public int ReportLimitMaxFilesPerFileGroup { get; set; }
        /// <summary>
        /// In a files by owner report, the maximum number of files in for each owner.
        /// </summary>
        public int ReportLimitMaxFilesPerOwner { get; set; }
        /// <summary>
        /// In a duplicate files report, the maximum number of files in an individual duplicate group.
        /// </summary>
        public int ReportLimitMaxFilesPerDuplicateGroup { get; set; }
        /// <summary>
        /// In a duplicate files report, the maximum number of groups of duplicate files found.
        /// </summary>
        public int ReportLimitMaxDuplicateGroup { get; set; }
        /// <summary>
        /// The maximum number of quotas in a quota report.
        /// </summary>
        public int ReportLimitMaxQuota { get; set; }
        /// <summary>
        /// The maximum number of file screens events to include in a files by file screen audit report.
        /// </summary>
        public int ReportLimitMaxFileScreenEvent { get; set; }
        /// <summary>
        /// In a files by property report, the maximum number of property values per report.
        /// </summary>
        public int ReportLimitMaxPropertyValue { get; set; }
        /// <summary>
        /// In a files by property report, the maximum number of files for each property value.
        /// </summary>
        public int ReportLimitMaxFilesPerPropertyValue { get; set; }
        /// <summary>
        /// A local or remote path to a folder. Must not exceed the value of MAX_PATH. Permissions on the folder are assumed to be set so that FSRM can write data to it.
        /// </summary>
        public string ReportLocationIncident { get; set; }
        /// <summary>
        /// A local or remote path to a folder. Must not exceed the value of MAX_PATH. Permissions on the folder are assumed to be set so that FSRM can write data to it.
        /// </summary>
        public string ReportLocationScheduled { get; set; }
        /// <summary>
        /// A local or remote path to a folder. Must not exceed the value of MAX_PATH. Permissions on the folder are assumed to be set so that FSRM can write data to it.
        /// </summary>
        public string ReportLocationOnDemand { get; set; }
        /// <summary>
        /// The minimum number of days since the audit event to include in the report.
        /// </summary>
        public int ReportFileScreenAuditDaysSince { get; set; }
        /// <summary>
        /// A list of user email addresses to include audit events for. Each string must be less than 1KB in size. The default value is an empty list, which indicates all users.
        /// </summary>
        public string[] ReportFileScreenAuditUser { get; set; }
        /// <summary>
        /// A list of file groups to include in report. Each string must be the name of a valid file group and be less than 1KB in size. The default value is an empty list, which indicates all file groups.
        /// </summary>
        public string[] ReportFileGroupIncluded { get; set; }
        /// <summary>
        /// List of users, in "domain\user" format, to include files for in the file by owner report. Each string must be less than 1KB in size. The default value is an empty list, which indicates all users.
        /// </summary>
        public string[] ReportFileOwnerUser { get; set; }
        /// <summary>
        /// A file pattern string that indicates which files to include in the file by owner report. The string must be less than 1KB and allows the wildcard characters * and ?. The default value is an empty string.
        /// </summary>
        public string ReportFileOwnerFilePattern { get; set; } = "";
        /// <summary>
        /// The property name to report on for file by property report. The string must be a valid property name and must not exceed 1KB in size. The default value is an empty string. Optional.
        /// </summary>
        public string ReportPropertyName { get; set; } = "";
        /// <summary>
        /// A string of files to include in the file by property report. The string must be less than 1KB and allows the wildcard characters * and ?. The default value is an empty string.
        /// </summary>
        public string ReportPropertyFilePattern { get; set; } = "";
        /// <summary>
        /// The minimum file size to include in the large file report. The default valu is 0. Optional.
        /// </summary>
        public ulong ReportLargeFileMinimum { get; set; } = 0;
        /// <summary>
        /// A string of files to include in the file by property report. The string must be less than 1KB and allows the wildcard characters * and ?. The default value is an empty string.
        /// </summary>
        public string ReportLargeFilePattern { get; set; } = "";
        /// <summary>
        /// The minimum number of days since the report was last accessed, to include in the least frequently accessed report. The default value is 0. Optional.
        /// </summary>
        public int ReportLeastAccessedMinimum { get; set; } = 0;
        /// <summary>
        /// A string of files to include in the least frequently accessed report. The string must be less than 1KB and allows the wildcard characters * and ?. The default value is an empty string.
        /// </summary>
        public string ReportLeastAccessedFilePattern { get; set; } = "";
        /// <summary>
        /// The maximum number of days since the report was last accessed, to include in the least frequently accessed report. The default value is 0. Optional.
        /// </summary>
        public int ReportMostAccessedMaximum { get; set; } = 0;
        /// <summary>
        /// A string of files to include in the most frequently accessed report. The string must be less than 1KB and allows the wildcard characters * and ?. The default value is an empty string.
        /// </summary>
        public string ReportMostAccessedFilePattern { get; set; } = "";
        /// <summary>
        /// The minimum quota usage level to include in the quota usage report. The default value is 0. Optional.
        /// </summary>
        public int ReportQuotaMinimumUsage { get; set; } = 0;
        /// <summary>
        /// True if file screen auditing is enabled. The default value is False. Optional.
        /// </summary>
        public bool ReportFileScreenAuditEnable { get; set; } = false;
        /// <summary>
        /// The formats of the classification report being generated. The default value is {<see cref="ReportClassificationFormats.DHtml"/>, <see cref="ReportClassificationFormats.Xml"/>}.
        /// </summary>
        public uint[] ReportClassificationFormat { get; set; }
        /// <summary>
        /// A semicolon-separated list of email addresses. "[Admin Email]" is an acceptable email address. The default value is an empty string. Optional.
        /// </summary>
        public string ReportClassificationMailTo { get; set; } = "";
        /// <summary>
        /// A flags value indicating the type and content of logs generated for classification. Optional.
        /// </summary>
        public uint[] ReportClassificationLog { get; set; }
        /// <summary>
        /// The SMTP server that FSRM uses to send email.
        /// </summary>
        public string SmtpServer { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="FsrmSettings"/> type and binds it to the underlying WMI class.
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="credentials"></param>
        public FsrmSettings(string computerName = null, NetworkCredential credentials = null)
            : base(computerName, credentials)
        {
            this.Bind();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends a test email to the specified email address.
        /// </summary>
        /// <param name="toEmailAddress"></param>
        public void SendTestEmail(string toEmailAddress)
        {
            var inParams = this.BaseWmiObject.GetMethodParameters(nameof(this.SendTestEmail));

            inParams["ToEmailAddress"] = toEmailAddress;

            this.BaseWmiObject.InvokeMethod(nameof(this.SendTestEmail), inParams, null);
        }

        #endregion
    }
}
