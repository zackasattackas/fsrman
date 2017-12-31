using System;

namespace MsftFsrm
{
    [Flags]
    public enum ReportClassificationLogTypes : uint
    {
        /// <summary>
        /// Put classification information in the XML log file.
        /// </summary>
        ClassificationsInLogFile = 1,
        /// <summary>
        /// Put classification errors in the XML log file.
        /// </summary>
        ErrorsInLogFile = 2,
        /// <summary>
        /// Put classification information in the system event log.
        /// </summary>
        ClassificationsInSystemLog = 4,
        /// <summary>
        /// Put classification errors in the system event log.
        /// </summary>
        ErrorsInSystemLog = 8
    }
}