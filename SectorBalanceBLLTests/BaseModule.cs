using System;
using System.Collections.Generic;
using System.Text;

namespace SectorBalanceBLLTests
{
    public class BaseModule
    {
        public readonly string connString;

        public BaseModule()
        {
            connString = @" User ID=gbrooksman;
                            Password=Gollum17;
                            Host=sectormodeltest.cubwzjzweesa.us-east-1.rds.amazonaws.com;
                            Port=5432;
                            Database=sectormodeltest;
                            Pooling=true;
                            Connection Lifetime=0;";
        }

    }
}
