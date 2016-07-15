using System;
using System.Security.Cryptography;
using System.Text;
using Machine.Specifications.Core.Runner.DotNet.Helpers;
using Machine.Specifications.Model;
using Microsoft.Extensions.Testing.Abstractions;

namespace Machine.Specifications.Core.Runner.DotNet.Discovery.DesignTime
{
    public class DesignTimeDiscoveryVisitor : IDiscoveryVisitor
    {
        private readonly ITestDiscoverySink _discoverySink;

        public DesignTimeDiscoveryVisitor(ITestDiscoverySink discoverySink)
        {
            if (discoverySink == null)
                throw new ArgumentNullException(nameof(discoverySink));

            _discoverySink = discoverySink;
        }

        public void Visit(Context context, Specification spec)
        {
            DotNetTestIdentifier dotNetTestIdentifier = spec.ToDotNetTestIdentifier(context);

            Test testCase = new Test() {
                Id = GuidFromString(dotNetTestIdentifier.FullyQualifiedName),
                FullyQualifiedName = dotNetTestIdentifier.FullyQualifiedName,
                DisplayName = $"{context.Type.Name.Replace("_", " ")} it {spec.Name}",
            };

            _discoverySink.SendTestFound(testCase);

            // TODO: .NET Core version
            //
            // SourceCodeLocationFinder locationFinder = new SourceCodeLocationFinder(assemblyPath);
            //
            //string fieldDeclaringType;
            //if (spec.FieldInfo.DeclaringType.GetTypeInfo().IsGenericType && !spec.FieldInfo.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
            //    fieldDeclaringType = spec.FieldInfo.DeclaringType.GetGenericTypeDefinition().FullName;
            //else
            //    fieldDeclaringType = spec.FieldInfo.DeclaringType.FullName;
            //
            // SourceCodeLocationInfo locationInfo = locationFinder.GetFieldLocation(fieldDeclaringType, spec.FieldInfo.Name);
            // if (locationInfo != null)
            // {
            //     testCase.CodeFilePath = locationInfo.CodeFilePath;
            //     testCase.LineNumber = locationInfo.LineNumber;
            // }

            //if (context.Tags != null)
            //    testCase.Tags = context.Tags.Select(tag => tag.Name).ToArray();

            //if (context.Subject != null)
            //    testCase.Subject = context.Subject.FullConcern;
        }

        public void OnEnd()
        {
            _discoverySink.SendTestCompleted();
        }

        private static readonly HashAlgorithm _hash = SHA1.Create();

        private static Guid GuidFromString(string data)
        {
            byte[] hash = _hash.ComputeHash(Encoding.Unicode.GetBytes(data));
            byte[] bytes = new byte[16];
            Array.Copy(hash, bytes, 16);
            return new Guid(bytes);
        }
    }
}
