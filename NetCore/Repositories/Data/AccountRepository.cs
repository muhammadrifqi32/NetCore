using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using NetCore.Models;
using NetCore.ViewModels;

namespace NetCore.Repositories.Data
{
    public class AccountRepository
    {

        DynamicParameters parameters = new DynamicParameters();

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        IConfiguration _configuration { get; }

        public async Task<IEnumerable<EmployeeVM>> GetUserData()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_GetAllUserData"; //

                var employees = await connection.QueryAsync<EmployeeVM>(procName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        public int InsertUser(EmployeeVM employeeVM)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_InsertUser";
                parameters.Add("@Email", employeeVM.Email);
                parameters.Add("@Department_Id", employeeVM.Department_Id);
                var create = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return create;
            }
        }
    }
}
