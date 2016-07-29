﻿using System;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Reflection;

namespace Machine.Specifications.Runner.DotNet.Controller.Model
{
    internal static class XDocumentExtensions
    {
        public static T SafeGet<T>(this XDocument element, string xpath)
        {
            var selected = element.XPathSelectElement(xpath);
            if (selected != null && !selected.IsEmpty)
            {
                if (typeof(Enum).IsAssignableFrom(typeof(T)))
                {
                    return (T)Enum.Parse(typeof(T), selected.Value);
                }

                return (T)Convert.ChangeType(selected.Value, typeof(T));
            }

            return default(T);
        }
    }
}