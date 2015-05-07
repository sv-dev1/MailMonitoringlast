using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PinPayment.Models;

namespace PinPayment.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IsEmailExist(string Email)
            {
            CustomerModel cust = new CustomerModel();

            return Json(cust.IsEmailExist(Email), JsonRequestBehavior.AllowGet);
        }
    }
}