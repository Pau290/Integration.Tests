using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using System.Collections.Generic;

namespace Integration.Tests.Cmd.Tests
{
    [TestClass]
    public class CmdTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IIntegrationTest<DtoMain> i_test = new IntegrationTest<DtoMain>();

            DtoMain b = null;

            decimal derived_percentage = 0;

            Condition<DtoMain> condition1 = new Condition<DtoMain>
            {
                Function = (d) => { return (d) != null; },
                Message = "set dto main"
            };

            i_test
                .SetTestMode(TestMode.Skip)
                .SetTimeout(1000)
                .ThenTry(function: Program.SetDtoMain, condition: condition1)
                .ExpectCondition(new Condition<DtoMain> { Function = (d) => { return (d).Description.Equals("item"); }, Message = "description = item" })
                .ThenTryAction(action: (d) => { Program.SetDescription((d)); },
                                condition: new Condition<DtoMain>
                                {
                                    Function = (d) => { return (d).Description.Equals("description"); },
                                    Message = "description"
                                },
                                times: 2)
                .ExpectTesteeEquals(b)
                .ThenItem(function: (d) => { return Program.GetDerivedPercentage(d); }, item: ref derived_percentage)
                .ExpectItemEquals(derived_percentage, 258.0M)
                .ExpectConditions(new Condition<DtoMain> { Function = (d) => { return (d).Fourth != null; }, Message = "condition on fourth failed" },
                                  new Condition<DtoMain> { Function = (d) => { return (d).Percent != 0; }, Message = "condition on percent failed" })
                .DetermineTest();

            DtoMain i_testee = i_test.GetTestee();

            List<IntegrationTestException> errors = i_test.Errors;

            //Assert.IsTrue(i_test.TestOK);
        }
    }
}
