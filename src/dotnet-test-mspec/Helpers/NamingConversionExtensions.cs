using System;
using System.Globalization;
using Machine.Specifications.Core.Runner.DotNet.Discovery;
using Machine.Specifications.Model;
using Machine.Specifications.Runner;
using Microsoft.Extensions.Testing.Abstractions;

namespace Machine.Specifications.Core.Runner.DotNet.Helpers
{
    public static class NamingConversionExtensions
    {
        public static DotNetTestIdentifier ToDotNetTestIdentifier(this SpecificationInfo specification, ContextInfo context)
        {
            return new DotNetTestIdentifier(String.Format(CultureInfo.InvariantCulture, "{0}::{1}", context?.TypeName ?? specification.ContainingType, specification.FieldName));
        }

        public static DotNetTestIdentifier ToDotNetTestIdentifier(this Test testCase)
        {
            return new DotNetTestIdentifier(testCase.FullyQualifiedName);
        }

        public static DotNetTestIdentifier ToDotNetTestIdentifier(this Specification specification, Context context)
        {
            return new DotNetTestIdentifier(String.Format(CultureInfo.InvariantCulture, "{0}::{1}", context.Type.FullName, specification.FieldInfo.Name));
        }
    }
}