using System;

namespace ArgSharp
{
    public class SharpParseException : Exception
    {
        public SharpParseException(string message) : base(message) {}
    }
}
