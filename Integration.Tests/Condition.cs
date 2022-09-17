using System;

namespace Integration.Tests
{
    /// <summary>
    /// Condition 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Condition<T>
    {
        /// <summary>
        /// Function
        /// </summary>
        public Func<T, bool> Function { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
