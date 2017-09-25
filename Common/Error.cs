using System;

namespace Common
{
    public abstract class Error
    {
        public string Message { get; }

        public Error(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("Message must contain at least one character.");
            }

            Message = message;
        }

        public override string ToString() => Message;
    }
}
