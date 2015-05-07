using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PinPayment.Models.Utilities
{
    public static class DBCommon
    {
        public static SelectList BindYear()
        {
            List<Year> userServerModel = new List<Year>();
            //DDAcType.Add(new AccountType { ID = 0, NameEnglish = "Please select" });


            SelectList slSound;

            Year[] m = new Year[16] {new Year(){ExpiryYear ="2015"},new Year(){ExpiryYear ="2016"},
            new Year(){ExpiryYear ="2017"},new Year(){ExpiryYear ="2018"},new Year(){ExpiryYear ="2019"},
            new Year(){ExpiryYear ="2020"},new Year(){ExpiryYear ="2021"},new Year(){ExpiryYear ="2022"},
            new Year(){ExpiryYear ="2023"},new Year(){ExpiryYear ="2024"},new Year(){ExpiryYear ="2025"},
            new Year(){ExpiryYear ="2026"},new Year(){ExpiryYear ="2027"},new Year(){ExpiryYear ="2028"},
            new Year(){ExpiryYear ="2029"},new Year(){ExpiryYear ="2030"}
            };

            userServerModel.AddRange(m);


            //slDialStatus = new SelectList(dialStatusModel, "ID", "DialStatus");
            slSound = new SelectList(userServerModel, "ExpiryYear", "ExpiryYear");


            return slSound;
        }

        public static SelectList BindMonth()
        {
            List<Month> userServerModel = new List<Month>();
            //DDAcType.Add(new AccountType { ID = 0, NameEnglish = "Please select" });


            SelectList slSound;

            Month[] m = new Month[12]{new Month(){ExpiryMonth="1"},
           new Month(){ExpiryMonth="2"},new Month(){ExpiryMonth="3"},new Month(){ExpiryMonth="4"},
           new Month(){ExpiryMonth="5"},new Month(){ExpiryMonth="6"},new Month(){ExpiryMonth="7"},new Month(){ExpiryMonth="8"},
           new Month(){ExpiryMonth="9"},new Month(){ExpiryMonth="10"},new Month(){ExpiryMonth="11"},new Month(){ExpiryMonth="12"}};



            userServerModel.AddRange(m);


            //slDialStatus = new SelectList(dialStatusModel, "ID", "DialStatus");
            slSound = new SelectList(userServerModel, "ExpiryMonth", "ExpiryMonth");


            return slSound;
        }

    }

    public class Month
    {
       public string ExpiryMonth { get; set; }
    }

    public class Year
    {
        public string ExpiryYear { get; set; }
    }
}