using Dashboard.DataService;
using Dashboard.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FellowshipOne.Dashboard.WebPortal.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Chapter01/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get all employee
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequestAll()
        {
            //Test Code for waiting.
            Thread.Sleep(3000);
            ViewBag.Employees = DBFacadeService.GetAllEmployees();
            return View("List");
        }

        [HttpGet]
        public ActionResult Detail()
        {
            ViewBag.Employee = new Employee();
            return View();
        }


        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            var entity = DBFacadeService.CreateEmployee(employee);
            return Json(JsonHelper.Serialize(entity));
        }

        [HttpPost]
        public ActionResult Update(Employee employee)
        {
            var entity = DBFacadeService.UpdateEmployee(employee);
            return Json(JsonHelper.Serialize(entity));
        }

        [HttpPost]
        public ActionResult Delete(int sysNo)
        {
            int count = DBFacadeService.DeleteEmployee(sysNo);
            return Content(count.ToString());
        }
	}
}