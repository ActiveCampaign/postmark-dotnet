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

namespace PostmarkDotNet
{
    /// <summary>
    ///   Represents an email bounce logged by Postmark.
    /// </summary>
    public class PostmarkBounce
    {
        /// <summary>
        ///   The bounce ID.
        ///   This is used for other API calls that require the ID.
        /// </summary>
        /// <value>The ID</value>
        public string ID { get; set; }

        /// <summary>
        ///   The <see cref = "PostmarkBounceType" /> for this bounce.
        /// </summary>
        /// <value>The type</value>
        public PostmarkBounceType Type { get; set; }

        /// <summary>
        ///   The bounce details set by the server.
        /// </summary>
        /// <value>The details</value>
        public string Details { get; set; }

        /// <summary>
        ///   The email recipient that initiated the bounce.
        /// </summary>
        /// <value>The email</value>
        public string Email { get; set; }

        /// <summary>
        ///   The time the bounce occurred.
        /// </summary>
        /// <value>The time of the bounce</value>
        public DateTime BouncedAt { get; set; }

        /// <summary>
        ///   A value indicating whether a raw STMP dump is available.
        /// </summary>
        /// <value><c>true</c> if a dump is available; otherwise, <c>false</c></value>
        public bool DumpAvailable { get; set; }

        /// <summary>
        ///   A value indicating whether this <see cref = "PostmarkBounce" /> is inactive.
        /// </summary>
        /// <value><c>true</c> if inactive; otherwise, <c>false</c></value>
        public bool Inactive { get; set; }

        /// <summary>
        ///   A value indicating whether this bounce can be activated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can be activated; otherwise, <c>false</c>
        /// </value>
        public bool CanActivate { get; set; }
    }
}