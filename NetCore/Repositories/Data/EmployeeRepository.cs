using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using NetCore.Context;
using NetCore.Models;
using NetCore.ViewModels;

namespace NetCore.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, MyContext>
    {
        public EmployeeRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
        }

        DynamicParameters parameters = new DynamicParameters();

        IConfiguration _configuration { get; }

        public async Task<IEnumerable<EmployeeVM>> GetAll()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_GetAllEmployee"; //

                var employees = await connection.QueryAsync<EmployeeVM>(procName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        public async Task<IEnumerable<EmployeeVM>> GetById(int Id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_GetEmployeeById";
                parameters.Add("@ID", Id);

                var get = await connection.QueryAsync<EmployeeVM>(procName, parameters, commandType: CommandType.StoredProcedure);
                return get;
            }
        }

        public int Post(EmployeeVM employeeVM)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_InsertEmployee";
                parameters.Add("@FirstName", employeeVM.FirstName);
                parameters.Add("@LastName", employeeVM.LastName);
                parameters.Add("@Email", employeeVM.Email);
                parameters.Add("@Birthdate", employeeVM.BirthDate);
                parameters.Add("@PhoneNumber", employeeVM.PhoneNumber);
                parameters.Add("@Address", employeeVM.Address);
                parameters.Add("@DeptId", employeeVM.DeptId);

                var create = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return create;
            }
        }

        public int Update(int Id, EmployeeVM employeeVM)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_UpdateEmployee";
                parameters.Add("@id", Id);
                parameters.Add("@FirstName", employeeVM.FirstName);
                parameters.Add("@LastName", employeeVM.LastName);
                parameters.Add("@Email", employeeVM.Email);
                parameters.Add("@Birthdate", employeeVM.BirthDate);
                parameters.Add("@PhoneNumber", employeeVM.PhoneNumber);
                parameters.Add("@Address", employeeVM.Address);
                parameters.Add("@DeptId", employeeVM.DeptId);

                var create = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return create;
            }
        }
    }
}
