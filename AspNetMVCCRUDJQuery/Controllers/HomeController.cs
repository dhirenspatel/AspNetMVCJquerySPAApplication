using AspNetMVCCRUDJQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetMVCCRUDJQuery.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (EmployeeDBContext dc = new EmployeeDBContext())
            {
                var employees = dc.EmployeeDB.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (EmployeeDBContext dc = new EmployeeDBContext())
            {
                var v = dc.EmployeeDB.Where(a => a.EmployeeID == id).FirstOrDefault();
                return View(v);
            }
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (EmployeeDBContext dc = new EmployeeDBContext())
                {
                    if (emp.EmployeeID > 0)
                    {
                        //Edit 
                        var v = dc.EmployeeDB.Where(a => a.EmployeeID == emp.EmployeeID).FirstOrDefault();
                        if (v != null)
                        {
                            v.FirstName = emp.FirstName;
                            v.LastName = emp.LastName;
                            v.EmailID = emp.EmailID;
                            v.City = emp.City;
                            v.Country = emp.Country;
                        }
                    }
                    else
                    {
                        //Save
                        dc.EmployeeDB.Add(emp);
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (EmployeeDBContext dc = new EmployeeDBContext())
            {
                var v = dc.EmployeeDB.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (EmployeeDBContext dc = new EmployeeDBContext())
            {
                var v = dc.EmployeeDB.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    dc.EmployeeDB.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}