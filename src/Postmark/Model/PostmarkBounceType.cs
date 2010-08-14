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

namespace PostmarkDotNet
{
    /// <summary>
    ///   Represents the type of bounce for a <see cref = "PostmarkBounce" />.
    /// </summary>
    public enum PostmarkBounceType
    {
        /// <summary>
        ///   HardBounce
        /// </summary>
        HardBounce,
        /// <summary>
        ///   Transient
        /// </summary>
        Transient,
        /// <summary>
        ///   Unsubscribe
        /// </summary>
        Unsubscribe,
        /// <summary>
        ///   Subscribe
        /// </summary>
        Subscribe,
        /// <summary>
        ///   AutoResponder
        /// </summary>
        AutoResponder,
        /// <summary>
        ///   AddressChange
        /// </summary>
        AddressChange,
        /// <summary>
        ///   DnsError
        /// </summary>
        DnsError,
        /// <summary>
        ///   SpamNotification
        /// </summary>
        SpamNotification,
        /// <summary>
        ///   OpenRelayTest
        /// </summary>
        OpenRelayTest,
        /// <summary>
        ///   Unknown
        /// </summary>
        Unknown,
        /// <summary>
        ///   SoftBounce
        /// </summary>
        SoftBounce,
        /// <summary>
        ///   VirusNotification
        /// </summary>
        VirusNotification,
        /// <summary>
        ///   ChallengeVerification
        /// </summary>
        ChallengeVerification,
        /// <summary>
        ///   BadEmailAddress
        /// </summary>
        BadEmailAddress,
        /// <summary>
        ///   SpamComplaint
        /// </summary>
        SpamComplaint,
        /// <summary>
        ///   ManuallyDeactivated
        /// </summary>
        ManuallyDeactivated,
        /// <summary>
        ///   Unconfirmed
        /// </summary>
        Unconfirmed,
        /// <summary>
        ///   Blocked
        /// </summary>
        Blocked
    }
}