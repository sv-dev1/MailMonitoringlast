using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PinPayments.Models;
using PinPayments.Actions;
using PinPayments;
using System.Configuration;
using PinPayment.Models;
using System.Web.Mvc.Routing;
using PinPayment.Models.Utilities;

namespace PinPayment.Controllers
{
    
    public class PaymentController : Controller
    {
        // GET: Payment
     
         [Route("~/{id:int}")]
         
        public ActionResult Index(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            // initialise PIN by passing your API Key
            // See:  https://pin.net.au/docs/api#keys
            PinService ps = new PinService(ConfigurationManager.AppSettings["Secret_API"]);

            // https://pin.net.au/docs/api/test-cards
            // 5520000000000000 - Test Mastercard
            // 4200000000000000 - Test Visa

            var card = new Card();
            card.CardNumber = "5520000000000000";
            card.CVC = "111";
            card.ExpiryMonth = DateTime.Today.Month.ToString();  // Use the real Expiry
            card.ExpiryYear = (DateTime.Today.Year + 1).ToString(); // Not my defaults!
            card.Name = "Roland Robot";
            card.Address1 = "42 Sevenoaks St";
            card.Address2 = null;
            card.City = "Lathlain";
            card.Postcode = "6454";
            card.State = "WA";
            card.Country = "Australia";

            var response = ps.Charge(new PostCharge { Amount = 1500, Card = card, Currency = "AUD", Description = "Desc", Email = "email@test.com", IPAddress = "127.0.0.1" });
            System.Console.WriteLine(response.Charge.Success);
            return View();
        }


        public ActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pay(PaymentModel payment)
        {
            // initialise PIN by passing your API Key
            // See:  https://pin.net.au/docs/api#keys
            PinService ps = new PinService(ConfigurationManager.AppSettings["Secret_API"]);

            // https://pin.net.au/docs/api/test-cards
            // 5520000000000000 - Test Mastercard
            // 4200000000000000 - Test Visa

            var card = new Card();
            card.CardNumber = payment.CardNumber;
            card.CVC = payment.CVC;
            card.ExpiryMonth = payment.ExpiryMonth;  // Use the real Expiry
            card.ExpiryYear = payment.ExpiryYear; // Not my defaults!
            card.Name = payment.Name;
            card.Address1 = payment.Address;
            card.Address2 = null;
            card.City = payment.City;
            card.Postcode = payment.PostCode;
            card.State = payment.State;
            card.Country = payment.Country;

            //var response = ps.Charge(new PostCharge { Amount = payment.Amount, Card = card, Currency = payment.Currency, Description = "Desc", Email = "email@test.com", IPAddress = "127.0.0.1" });
            //System.Console.WriteLine(response.Charge.Success);

            return View();
        }




        [Route("{name}")]
        public ActionResult Customer(string name)
        {
            CustomerModel customerModel = new CustomerModel();
            customerModel=customerModel.GetCustomerModel(name);
            return View(customerModel);
        }

        [HttpPost]
        [Route("{name}")]
        public ActionResult Customer(CustomerModel customer)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    CustomerModel cm = new CustomerModel();
                    int id = cm.AddCustomer(customer);
                    if (id != 0)
                    {
                        PaymentModel payment = new PaymentModel();
                        payment.CustomerID = id;
                        payment.PlanID = customer.PlanID;
                        payment.ServiceId = customer.ServiceId;
                        payment.Price = customer.Price;

                        ViewBag.year = DBCommon.BindYear();
                        ViewBag.month = DBCommon.BindMonth();

                        return View("~/Views/Payment/Charges.cshtml", payment);
                    }
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 1)]
        public ActionResult Charges(PaymentModel paymentModel)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    PaymentModel payment = new PaymentModel();
                    ChargeResponse message = payment.Charge(paymentModel);

                    ViewBag.Message = message.Messages;
                    if (message.Charge != null)
                    {

                        return RedirectToAction("ThankYou");

                    }
                   
                }
               
            }
            catch
            {
               
            }

            ViewBag.year = DBCommon.BindYear();
            ViewBag.month = DBCommon.BindMonth();
            return View(paymentModel);
        }

        public ActionResult ThankYou()
        {
            return View();
        }

    }
}