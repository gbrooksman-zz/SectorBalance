using System;


namespace SectorBalanceShared
{
    public class ManagerResult <T> 
    {
        public ManagerResult ()
        {
            
        }

       public bool Success { get; set; } = true;

       public string Message { get; set; }

       public Exception Exception
        {
            get
            {
                return this.Exception;
            }

            set
            {
                this.Exception = value;
                this.Message = value.Message;
                this.Success = false;
            }
        }

        public T Entity { get; set; } = default;    

    }
}
