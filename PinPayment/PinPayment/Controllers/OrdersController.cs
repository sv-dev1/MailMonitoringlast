using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PinPayment.Models;

namespace PinPayment.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            PaymentModel pm = new PaymentModel();
            return View(pm.GetCustomerPlan());
        }

        public ActionResult List()
        {
            PaymentModel pm = new PaymentModel();
            return View(pm.GetCustomerPlan());
        }
    }
}