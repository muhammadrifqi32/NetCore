using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Base;
using NetCore.Models;
using NetCore.Repositories.Data;
using NetCore.ViewModels;

namespace NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BasesController<Employee, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public Task<IEnumerable<EmployeeVM>> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet("GetById/{id}")]
        public async Task<IEnumerable<EmployeeVM>> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        //[HttpPost]
        //[Route("Post")]
        //public IActionResult Post(EmployeeVM employeeVM)
        //{
        //    var post = _repository.Post(employeeVM);
        //    if (post > 1)
        //    {

        //        return Ok("Employee Succesfully Added");
        //    }
        //    return BadRequest("Failed to Added New Data!");
        //}

        //[HttpPut]
        //[Route("Update/{id}")]
        //public IActionResult Update(int Id, EmployeeVM employeeVM)
        //{
        //    var put = _repository.Update(Id, employeeVM);
        //    if (put > 0)
        //    {
        //        return Ok("Data Succesfully Updated");
        //    }
        //    return BadRequest("Failed to Updated Data!");
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeVM>> Put(int id, EmployeeVM employeeVM)
        {
            var put = await _repository.Get(id);
            if (put == null)
            {
                return NotFound();
            }
            if (employeeVM.FirstName != null)
            {
                put.FirstName = employeeVM.FirstName;
            }
            if (employeeVM.LastName != null)
            {
                put.LastName = employeeVM.LastName;
            }
            if (employeeVM.Email != null)
            {
                put.Email = employeeVM.Email;
            }
            if (employeeVM.Address != null)
            {
                put.Address = employeeVM.Address;
            }
            if (employeeVM.BirthDate != default(DateTime))
            {
                put.BirthDate = employeeVM.BirthDate;
            }
            if (employeeVM.Address != null)
            {
                put.Address = employeeVM.Address;
            }
            if (employeeVM.Department_Id != 0)
            {
                put.Department_Id = employeeVM.Department_Id;
            }
            put.UpdateDate = DateTime.Now;
            await _repository.Put(put);
            return Ok("Succesfully Updated Data");
        }
    }
}