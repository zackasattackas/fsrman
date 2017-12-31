using System;

// ReSharper disable CheckNamespace

namespace MsftFsrm
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class FsrmWmiObjectAttribute : Attribute
    {
        public string ClassName { get; set; }
    }
}