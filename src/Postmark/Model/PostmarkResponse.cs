#region License

// Postmark
// http://postmarkapp.com
// (c) 2010 Wildbit
// 
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
// 
// RestSharp
// http://github.com/johnsheehan/RestSharp 
// 
// Copyright (c) 2010 John Sheehan
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License. 

#endregion

using System;

namespace PostmarkDotNet
{
    /// <summary>
    /// A response from the Postmark API after a request sent with <see cref="PostmarkClient" />.
    /// </summary>
    public class PostmarkResponse
    {
        /// <summary>
        /// The status outcome of the response.
        /// </summary>
        public PostmarkStatus Status { get; set; }

        /// <summary>
        /// The message from the API.  
        /// In the event of an error, this message may contain helpful text.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The time the request was received by Postmark.
        /// </summary>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// The recipient of the submitted request.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The error code returned from Postmark.
        /// This does not map to HTTP status codes.
        /// </summary>
        public int ErrorCode { get; set; }
    }
}