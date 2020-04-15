using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Context;
using NetCore.Models;

namespace NetCore.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, MyContext>
    {
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
