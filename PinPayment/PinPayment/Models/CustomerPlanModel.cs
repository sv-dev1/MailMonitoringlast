using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PinPayment.Models
{
    public class CustomerPlanModel
    {
       public int CustomerID { get; set; }
       public int PlanID { get; set; }
       public int ServiceLevelID { get; set; }
       public DateTime StartDate { get; set; }

       public DateTime EndDate { get; set; }
       public int price { get; set; } 
    }
}