using Library;
using System.Collections.Generic;

namespace Integration.Tests.Cmd
{
    public class Program
    {
        static void Main(string[] args)
        {
            IIntegrationTest<DtoMain> i_test = new IntegrationTest<DtoMain>();

            DtoMain b = null;

            decimal derived_percentage = 0;

            Condition<DtoMain> condition1 = new Condition<DtoMain>
            {
                Function = (d) => { return (d) != null; },
                Message = "set dto main"
            };

            Condition<DtoMain> condition2 = new Condition<DtoMain> 
            { 
                Function = (d) => { return (d).Description.Equals("item"); }, 
                Message = "condition on description failed" 
            };

            Condition<DtoMain> condition3 = new Condition<DtoMain>
            {
                Function = (d) => { return (d).Description.Equals("description"); },
                Message = "condition 2 on description failed"
            };

            i_test
                .SetTestMode(TestMode.Skip)
                .SetTimeout(1000)
                .ThenTry(function: SetDtoMain, condition: condition1)
                .ExpectCondition(condition2)
                .ThenTryAction(action: (d) => { SetDescription((d)); }, condition: condition3 , times: 2)
                .ExpectTesteeEquals(b)
                .ThenItem(function: (d) => { return GetDerivedPercentage(d); }, item: ref derived_percentage)
                .ThenItem((d) => { return new DtoFourth(); }, null)
                .ExpectItemEquals(derived_percentage, 258.0M)
                .ExpectConditions(new Condition<DtoMain> { Function = (d) => { return (d).Fourth != null; }, Message = "condition on fourth failed" },
                                  new Condition<DtoMain> { Function = (d) => { return (d).Percent != 0; }, Message = "condition on percent failed" })
                .DetermineTest();

            DtoMain i_testee = i_test.GetTestee();

            List<IntegrationTestException> errors = i_test.Errors;
        }

        public static decimal GetDerivedPercentage(DtoMain dtoMain)
        {
            return (dtoMain.Percent * 0) + 258.0M;
        }

        public static DtoMain SetDtoMain()
        {
            DtoMain dto = new DtoMain
            {
                Id = 1,
                Description = "test",
                Percent = 2345.0M,
                Ones = new List<DtoOne> { new DtoOne { Information = "demo", OneId = 1, OneThird = new DtoThird { Average = 44, Declaration = "third", Id = 8 } } },
                Fourth = new DtoFourth
                {
                    Description = "fourth",
                    Second = new DtoSecond { Id = 74, Description = "second", Variance = 864.5M },
                    Third = new DtoThird { Id = 98, Average = 342, Declaration = "third data" }
                }
            };

            return dto;
        }

        public static void SetDescription(DtoMain dtoMain)
        {
            dtoMain.Description = "description";
        }


    }
}
