using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Integration.Tests
{
    /// Arrezife Software ® 2019

    /// <summary>
    /// Integration Test
    /// </summary>
    public class IntegrationTest<T> : IIntegrationTest<T>
    {
        #region ctor

        public IntegrationTest(double timeout = 2000) : this(TestMode.Default)
        {
            Timeout = timeout;

            Elapsed.Begin();
        }

        private IntegrationTest(TestMode testMode)
        {
            TestMode = testMode;
        }

        /// <summary>
        /// Set test mode
        /// </summary>
        /// <param name="testMode"></param>
        public IIntegrationTest<T> SetTestMode(TestMode testMode)
        {
            TestMode = testMode;

            return this;
        }


        /// <summary>
        /// Set timeout
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public IIntegrationTest<T> SetTimeout(double milliseconds)
        {
            Timeout = milliseconds;

            return this;
        }

        #endregion

        #region tests

        /// <summary>
        /// Expects a result
        /// </summary>
        /// <param name="test"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ExpectTesteeEquals(T expected)
        {
            CheckCondition(new Condition<T> { Function = (t) => { return t.Equals(expected); }, Message = "Testee != expected" });

            return this;
        }


        /// <summary>
        /// Expect item equals
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="item"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ExpectItemEquals<S>(S item, S expected)
        {
            if (!item.Equals(expected))
            {
                Exception ex = new Exception($"expected:{expected}!=item:{item}");

                Errors.Add(new IntegrationTestException(ex, default(TimeSpan)));

                if (TestMode == TestMode.Stop) throw ex;
            }

            return this;
        }

        /// <summary>
        /// Checks a condition
        /// </summary>
        /// <param name="test"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ExpectCondition(Condition<T> condition)
        {
            CheckCondition(condition);

            return this;
        }

        /// <summary>
        /// Checks a serie of conditions
        /// </summary>
        /// <param name="test"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ExpectConditions(params Condition<T>[] conditions)
        {
            foreach (Condition<T> condition in conditions)
            {
                CheckCondition(condition);
            }

            return this;
        }

        private void CheckCondition(Condition<T> condition)
        {
            if (!condition.Function(Testee))
            {
                Exception ex = new Exception($"condition!={condition.Message}");

                Errors.Add(new IntegrationTestException(ex, default(TimeSpan)));

                if (TestMode == TestMode.Stop) throw ex;
            }
        }


        /// <summary>
        /// Try method n times
        /// </summary>
        /// <param name="method"></param>
        /// <param name="condition"></param>
        /// <param name="times"></param>
        private void Try(Action method, Condition<T> condition, int times)
        {
            Run(() =>
            {
                for (int i = 0; i < times; i++)
                {
                    try
                    {
                        method();

                        if (condition.Function(Testee))
                            break;
                        else
                            throw new Exception($"condition!:{condition.Message}");
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(new IntegrationTestException(ex, default(TimeSpan)));

                        if (TestMode == TestMode.Stop) throw;
                    }
                }
            });
        }


        private void Run(Action action)
        {
            System.Diagnostics.Stopwatch crono = new System.Diagnostics.Stopwatch();

            crono.Start();

            action();
            
            crono.Stop();

            Elapsed.AddElapsed(crono.Elapsed);

        }


        /// <summary>
        /// Try Function
        /// </summary>
        /// <param name="test"></param>
        /// <param name="function"></param>
        /// <param name="condition"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ThenTry(Func<T> function, Condition<T> condition, int times = 1)
        {
            Try(() => { Testee = function(); }, condition, times);

            return this;
        }

        /// <summary>
        /// Try Action
        /// </summary>
        /// <param name="test"></param>
        /// <param name="action"></param>
        /// <param name="condition"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ThenTryAction(Action<T> action, Condition<T> condition, int times = 1)
        {
            Try(() => { action(Testee); }, condition, times);

            return this;
        }

        /// <summary>
        /// Item
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="function"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ThenItem<S>(Func<T, S> function, ref S item)
        {
            System.Diagnostics.Stopwatch crono = new System.Diagnostics.Stopwatch();

            crono.Start();

            item = function(Testee);

            crono.Stop();

            Elapsed.AddElapsed(crono.Elapsed);

            return this;
        }

        /// <summary>
        /// Item
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="function"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ThenItem<S>(Func<T, S> function, S item) where S : class
        {
            System.Diagnostics.Stopwatch crono = new System.Diagnostics.Stopwatch();

            crono.Start();

            item = function(Testee);

            crono.Stop();

            Elapsed.AddElapsed(crono.Elapsed);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="function"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public IIntegrationTest<T> ThenItem<S>(Func<S> function, ref S item)
        {

            return this;
        }

        #endregion

        #region results

        /// <summary>
        /// Test mode
        /// </summary>
        public TestMode TestMode { get; set; }

        /// <summary>
        /// TestMode.TimesTest Timeout
        /// </summary>
        private double Timeout { get; set; }


        /// <summary>
        /// Elapsed information
        /// </summary>
        public Elapsed Elapsed = new Elapsed();

        /// <summary>
        /// Test OK
        /// </summary>
        public bool TestOK
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        private List<IntegrationTestException> errors;

        /// <summary>
        /// List of errors
        /// </summary>
        public List<IntegrationTestException> Errors { get { return (errors) ?? (errors = new List<IntegrationTestException>()); } }

        /// <summary>
        /// Testee
        /// </summary>
        private T Testee { get; set; }

        /// <summary>
        /// Get testee
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTestee()
        {
            return Testee;
        }

        /// <summary>
        /// Determine result
        /// </summary>
        /// <returns></returns>
        public void DetermineTest()
        {
            Elapsed.End();

            if (!TestOK && TestMode.HasFlag(TestMode.Stop))
            {
                StringBuilder sbuilder = new StringBuilder();

                foreach (IntegrationTestException error in Errors)
                {
                    sbuilder.AppendLine($"{error.Exception.ToString()}");

                    Exception inner = error.Exception.InnerException;

                    while (inner != null)
                    {
                        sbuilder.AppendLine($"{inner.ToString()}");

                        inner = inner.InnerException;
                    }
                }

                throw new Exception(sbuilder.ToString());
            }

            if (TestMode.HasFlag(TestMode.Skip) && Elapsed.TotalElapsed.TotalMilliseconds > Timeout)
            {
                IntegrationTestException itex = new IntegrationTestException(new Exception($"elapsed:{Elapsed.TotalElapsed}"), Elapsed.TotalElapsed);
                Errors.Add(itex);

#if DEBUG
                //throw new TimeoutException();
#else
                    throw itex.Exception;
#endif

            }
        }

        #endregion
    }
}
