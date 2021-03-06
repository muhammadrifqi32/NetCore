﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.Models;
using NetCore.ViewModels;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class EmployeesController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44321/api/")
        };
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult LoadEmployee()
        {
            IEnumerable<EmployeeVM> employeeVMs = null;
            var responseTask = client.GetAsync("Employees/GetAll");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<EmployeeVM>>();
                readTask.Wait();
                employeeVMs = readTask.Result;
            }
            else
            {
                employeeVMs = Enumerable.Empty<EmployeeVM>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(employeeVMs);
        }

        public JsonResult InsertOrUpdate(EmployeeVM employeeVM)
        {
            var myContent = JsonConvert.SerializeObject(employeeVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (employeeVM.Id == 0)
            {
                //var result = client.PostAsync("Employees/Post", byteContent).Result;
                var result = client.PostAsync("Employees", byteContent).Result;
                return Json(result);
            }
            else
            {
                //var result = client.PutAsync("Employees/Update/" + employeeVM.Id, byteContent).Result;
                var result = client.PutAsync("Employees/" + employeeVM.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult GetById(int Id)
        {
            Employee employee = null;
            var responseTask = client.GetAsync("Employees/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                employee = JsonConvert.DeserializeObject<Employee>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return Json(employee);
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Employees/" + Id).Result;
            return Json(result);
        }
    }
}