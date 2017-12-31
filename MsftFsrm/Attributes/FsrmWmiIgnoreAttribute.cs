using System;
// ReSharper disable CheckNamespace

namespace MsftFsrm
{
    /// <inheritdoc />
    /// <summary>
    /// When attached to a property, no value will be assigned when binding to the WMI instance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class FsrmWmiIgnoreAttribute : Attribute
    {
    }
}