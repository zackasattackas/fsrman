namespace MsftFsrm
{
    /// <summary>
    /// The formats of the classification report being generated. The default value is {<see cref="DHtml"/>, <see cref="Xml"/>}.
    /// </summary>
    public enum ReportClassificationFormats : uint
    {
        /// <summary>
        /// The report is rendered in Dynamic HTML (DHTML).
        /// </summary>
        DHtml = 1,
        /// <summary>
        /// The report is rendered in HTML.
        /// </summary>
        Html = 2,
        /// <summary>
        /// The report is rendered as a text file.
        /// </summary>
        Text = 3,
        /// <summary>
        /// The report is rendered as a comma-separated value file.
        /// </summary>
        Csv = 4,
        /// <summary>
        /// The report is rendered in XML.
        /// </summary>
        Xml = 5
    }
}