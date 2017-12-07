﻿using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using SharpDocx;

namespace Inheritance
{
    public abstract class MyDocument : DocumentBase
    {
        protected MyDocument()
        {
            MyProperty = "very thorough";
        }

        public string MyProperty { get; set; }

        public new static List<string> GetUsingDirectives()
        {
            return new List<string>
            {
                "using Inheritance;"

                //"using static System.Math;" 
                // Requires support for C# 6.
                // See https://stackoverflow.com/questions/31639602/using-c-sharp-6-features-with-codedomprovider-rosyln
            };
        }

        public new static List<string> GetReferencedAssemblies()
        {
            return new List<string>
            {
                typeof(MyDocument).Assembly.Location
            };
        }

        protected void CreateHyperlink(string text, string url)
        {
            // This method will be called from Inheritance.cs.docx.
            var id = $"r{Guid.NewGuid().ToString("N")}";

            var hyperlink = new Hyperlink(
                new RunProperties(new RunStyle { Val = "Hyperlink" }),
                new Run(new Text(text)))
            {
                History = true,
                Id = id,
            };

            this.Package.MainDocumentPart.AddHyperlinkRelationship(
                new Uri(url, UriKind.Absolute), true, id);

            this.CurrentCodeBlock.Placeholder.Parent.InsertAfterSelf(hyperlink);
        }
    }
}