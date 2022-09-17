using System;
using System.Collections.Generic;

namespace Integration.Tests
{
    /// Arrezife Software 2019 ®   

    public interface IIntegrationTest<T>
    {
        List<IntegrationTestException> Errors { get; }
        TestMode TestMode { get; set; }
        bool TestOK { get; }
        void DetermineTest();
        T GetTestee();
        IIntegrationTest<T> ExpectCondition(Condition<T> condition);
        IIntegrationTest<T> ExpectConditions(params Condition<T>[] conditions);
        IIntegrationTest<T> ExpectTesteeEquals(T expected);
        IIntegrationTest<T> SetTimeout(double milliseconds);
        IIntegrationTest<T> SetTestMode(TestMode testMode);
        IIntegrationTest<T> ThenTry(Func<T> function, Condition<T> condition, int times = 1);
        IIntegrationTest<T> ThenTryAction(Action<T> action, Condition<T> condition, int times = 1);
        IIntegrationTest<T> ThenItem<S>(Func<T, S> function, ref S item);
        IIntegrationTest<T> ThenItem<S>(Func<T, S> function, S item) where S : class;
        IIntegrationTest<T> ThenItem<S>(Func<S> function, ref S item);
        IIntegrationTest<T> ExpectItemEquals<S>(S item, S expected);
    }
}