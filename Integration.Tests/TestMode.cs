using System;

namespace Integration.Tests
{
    /// <summary>
    /// Test Mode
    /// </summary>
    [Flags]
    public enum TestMode
    {
        Default = SkipAndDetermine,

        /// <summary>
        /// Throw on error
        /// </summary>
        Stop = 1,

        /// <summary>
        /// Skip error
        /// </summary>
        Skip = 2,

        /// <summary>
        /// Skip error and throw when determine
        /// </summary>
        SkipAndDetermine = Skip | Stop,

        /// <summary>
        /// Skip errors and throw if times out max milliseconds
        /// </summary>
        TimesTest = 3 | Skip
    }
}
