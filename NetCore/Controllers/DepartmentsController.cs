using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Base;
using NetCore.Models;
using NetCore.Repositories.Data;

namespace NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BasesController<Department, DepartmentRepository>
    {
        private readonly DepartmentRepository _repository;
        public DepartmentsController(DepartmentRepository departmentRepository) : base(departmentRepository)
        {
            this._repository = departmentRepository ;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Department>> Put(int id, Department entity)
        {
            var put = await _repository.Get(id);
            if (put == null)
            {
                return NotFound();
            }
            put.Name = entity.Name;
            put.UpdateDate = DateTime.Now;
            await _repository.Put(put);
            return Ok("Succesfully Updated Data");
        }
    }
}