using System;

// ReSharper disable CheckNamespace

namespace MsftFsrm
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class FsrmWmiPropertyAttribute : Attribute
    {
        public string Name { get; set; }

        public FsrmWmiPropertyAttribute(string name)
        {
            this.Name = name;
        }
    }
}