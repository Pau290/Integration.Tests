using System;

namespace Integration.Tests
{
    public class IntegrationTestException
    {
        public Exception Exception { get; }

        public TimeSpan Elapsed { get; set; }

        public IntegrationTestException(Exception exception, TimeSpan elapsed)
        {
            Exception = exception;

            Elapsed = elapsed;
        }       
    }
}
