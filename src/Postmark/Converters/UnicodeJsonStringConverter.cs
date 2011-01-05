#region License

// Postmark
// http://postmarkapp.com
// (c) 2010 Wildbit
// 
// Postmark.NET
// http://github.com/lunarbits/postmark-dotnet
// 
// The MIT License
// 
// Copyright (c) 2010 Daniel Crenna
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Json.NET 
// http://codeplex.com/json
//  
// Copyright (c) 2007 James Newton-King
// 
// The MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Hammock for REST
// http://hammock.codeplex.com
// 
// The MIT License
// 
// Copyright (c) 2010 Daniel Crenna and Jason Diller
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Text;
using Newtonsoft.Json;

namespace PostmarkDotNet.Converters
{
    internal class UnicodeJsonStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var buffer = new StringBuilder();
            buffer.Append("\"");
            var stringValue = (string) value;
            foreach (var c in stringValue)
            {
                var code = (int) c;
                switch (c)
                {
                    case '\"':
                        buffer.Append("\\\"");
                        break;
                    case '\\':
                        buffer.Append("\\\\");
                        break;
                    case '\r':
                        buffer.Append("\\r");
                        break;
                    case '\n':
                        buffer.Append("\\n");
                        break;
                    case '\t':
                        buffer.Append("\\t");
                        break;
                    case '\b':
                        buffer.Append("\\b");
                        break;
                    case '/':
                        buffer.Append("\\/");
                        break;
                    case '\f':
                        buffer.Append("\\f");
                        break;
                    default:
                        if (code > 127)
                        {
                            buffer.AppendFormat("\\u{0:x4}", code);
                        }
                        else
                        {
                            buffer.Append(c);
                        }
                        break;
                }
            }
            buffer.Append("\"");

            writer.WriteRawValue(buffer.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            // Automatic conversion to string from other types
            if (reader.ValueType != objectType && objectType == typeof(string))
            {
                return reader.Value.ToString();
            }

            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (string);
        }
    }
}