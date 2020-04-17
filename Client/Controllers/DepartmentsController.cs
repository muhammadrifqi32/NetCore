using System;
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
    public class DepartmentsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44321/api/")
        };
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadDepartment()
        {
            DepartmentJson departmentVM = null;
            var responseTask = client.GetAsync("Departments");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return Json(departmentVM);
        }

        public JsonResult InsertOrUpdate(DepartmentVM departmentVM)
        {
            var myContent = JsonConvert.SerializeObject(departmentVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (departmentVM.Id == 0)
            {
                var result = client.PostAsync("Departments/", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Departments/" + departmentVM.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult GetById(int Id)
        {
            DepartmentVM departmentVM = null;
            var responseTask = client.GetAsync("Departments/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return Json(departmentVM);
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Departments/" + Id).Result;
            return Json(result);
        }
    }
}