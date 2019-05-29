using SectorBalanceShared;
using System;


namespace SectorBalanceBLLTests
{
    public class TestResult <T>:  ManagerResult <T>
    {
        public TestResult () 
        {
            
        }

        public string ModuleName { get; set; }

        public string MethodName { get; set; }

    }
}
