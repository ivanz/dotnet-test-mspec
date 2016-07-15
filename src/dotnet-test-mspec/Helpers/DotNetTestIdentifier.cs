using System;
using System.Globalization;

namespace Machine.Specifications.Core.Runner.DotNet.Helpers
{
    public class DotNetTestIdentifier
    {
        public DotNetTestIdentifier()
        {
        }

        public DotNetTestIdentifier(string containerTypeFullName, string fieldName)
            : this(String.Format(CultureInfo.InvariantCulture, "{0}::{1}", containerTypeFullName, fieldName))
        {
        }

        public DotNetTestIdentifier(string fullyQualifiedName)
        {
            FullyQualifiedName = fullyQualifiedName;
        }

        public string FullyQualifiedName { get; private set; }

        public string FieldName {
            get {
                return FullyQualifiedName.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
        }

        public string ContainerTypeFullName {
            get {
                return FullyQualifiedName.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
        }

        public override bool Equals(object obj)
        {
            DotNetTestIdentifier test = obj as DotNetTestIdentifier;
            if (test != null)
                return FullyQualifiedName.Equals(test.FullyQualifiedName, StringComparison.Ordinal);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return FullyQualifiedName.GetHashCode();
        }

    }
}