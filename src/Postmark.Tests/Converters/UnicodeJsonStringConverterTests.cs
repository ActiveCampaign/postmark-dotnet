using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using PostmarkDotNet.Converters;

namespace Postmark.Tests.Converters
{
    [TestFixture]
    public class UnicodeJsonStringConverterTests
    {
        [Test]
        public void Normal_string_is_not_modified()
        {
            string stringValue = SerializeString("test");
            AssertJsonString("test", stringValue);
        }

        [Test]
        public void Non_ascii_chars_represented_by_unicode_code_escapes()
        {
            var stringValue = SerializeString("Здравей");
            AssertJsonString("\\u0417\\u0434\\u0440\\u0430\\u0432\\u0435\\u0439", stringValue);
        }

        [Test]
        public void Danish_chars_represented_by_unicode_code_escapes()
        {
            string stringValue = SerializeString("æ ø å Æ Ø Å");
            AssertJsonString("\\u00e6 \\u00f8 \\u00e5 \\u00c6 \\u00d8 \\u00c5", stringValue);
        }

        [Test]
        public void Quotes_in_string_are_escaped()
        {
            var stringValue = SerializeString("\"Test\", he said.");
            AssertJsonString("\\\"Test\\\", he said.", stringValue);
        }

        [Test]
        public void Backslashes_in_string_are_escaped()
        {
            var stringValue = SerializeString("Find it at C:\\Test.");
            AssertJsonString("Find it at C:\\\\Test.", stringValue);
        }

        private static void AssertJsonString(string expected, string actual)
        {
            var expectedStringValue = string.Format("\"{0}\"", expected);
            Assert.AreEqual(expectedStringValue, actual);
        }

        private static string SerializeString(string value)
        {
            using(var textWriter = new StringWriter())
            {
                using(var jsonWriter = new JsonTextWriter(textWriter))
                {
                    var serializer = new JsonSerializer();
                    var converter = new UnicodeJsonStringConverter();

                    converter.WriteJson(jsonWriter, value, serializer);
                    return textWriter.GetStringBuilder().ToString();
                }
            }
        }
    }
}
