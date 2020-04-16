using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.ConnectionString
{
    public class ConnectionStrings
    {
        public ConnectionStrings(string value) => Value = value; //to get value

        public string Value { get; }
    }
}
