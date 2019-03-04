using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestNLog.MySystem
{
    public class TestThrows
    {

        public static void WhereIsMyFile()
        {
            throw new NullReferenceException(); 
        }
    }
}
