using System;
using System.Collections.Generic;

namespace Integration.Tests
{
    /// <summary>
    /// Elapsed information
    /// </summary>
    public class Elapsed
    {
        public TimeSpan TotalElapsed;

        public List<TimeSpan> Elapseds = new List<TimeSpan>();

        public System.Diagnostics.Stopwatch Crono = new System.Diagnostics.Stopwatch();

        public void AddElapsed(TimeSpan timespan)
        {
            Elapseds.Add(timespan);
        }

        public void Begin()
        {
            Crono.Start();
        }

        public void End()
        {
            TotalElapsed = Crono.Elapsed;
            Crono.Stop();
        }
    }
}
