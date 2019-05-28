using System;


namespace SectorBalanceBLLTests
{
    public class TestResult <T> 
    {
        public TestResult ()
        {
            
        }

       public bool Success { get; set; } = true;

       public string Message { get; set; }

       public Exception Exception {get; set;}

      public T Entity {get; set;}      

    }
}
