using System;
using System.Collections.Generic;
using System.Text;

namespace OpneHackFunc2
{
    public class ConnectionString
    {
        public static string Value
        {
            get
            {
                return Environment.GetEnvironmentVariable("ConnectionString");
            }
        }
    }
}
