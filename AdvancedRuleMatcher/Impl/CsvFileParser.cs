using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;

namespace AdvancedRuleMatcher.Impl
{
    public class CsvFileParser<T> where T: class, new()
    {
        public IEnumerable<T> Parse(FileInfo file)
        {
            using var reader = file.OpenText();
            using var parser = CreateTextFieldParser(reader);

            var readHeaders = parser.ReadFields();
            if (readHeaders == null)
                yield break;

            var properties = MapHeadersToProperties(readHeaders);

            while (!parser.EndOfData)
            {
                var obj = ParseLineToObject(parser, properties);
                if (obj == null) continue;

                yield return obj;
            }
        }

        private IReadOnlyList<PropertyInfo> MapHeadersToProperties(IReadOnlyList<string> readHeaders)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var propertyByName = properties.ToDictionary(p => p.Name);

            EnsureAllHeadersSupported(readHeaders, propertyByName);

            return readHeaders.Select(header => propertyByName[header]).ToArray();

            static void EnsureAllHeadersSupported(IReadOnlyList<string> readHeaders, IReadOnlyDictionary<string, PropertyInfo> propertyByName)
            {
                var unexpectedHeaders = readHeaders.Where(header => !propertyByName.ContainsKey(header)).ToArray();
                if (unexpectedHeaders.Length > 0)
                    throw new MissingFieldException($"The following headers are not supported: {string.Join(",", unexpectedHeaders)}");
            }
        }

        private static TextFieldParser CreateTextFieldParser(StreamReader reader)
        {
            var parser = new TextFieldParser(reader)
            {
                CommentTokens = new string[] { "#" },
                HasFieldsEnclosedInQuotes = true,
            };
            parser.SetDelimiters(new string[] { "," });
            return parser;
        }

        private T? ParseLineToObject(TextFieldParser parser, IReadOnlyList<PropertyInfo> properties)
        {
            var readFields = parser.ReadFields();
            if (readFields == null) return null;

            var obj = new T();

            for (int i = 0; i < readFields.Length; i++)
            {
                properties[i].SetMethod!.Invoke(obj, new[] { readFields[i] });
            }

            return obj;
        }
    }
}
