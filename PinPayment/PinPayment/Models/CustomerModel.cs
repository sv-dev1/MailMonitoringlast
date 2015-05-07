using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;

namespace PinPayment.Models
{
 
    public class CustomerModel
    {
        [Display(Name = "Full Name")]
        [Required]
        public  string FullName {get;set;}
       
        [EmailAddress]
        [Required]
        [Remote("IsEmailExist", "Customer", ErrorMessage = "Email already exist.")]
        public string Email { get; set; }
       
       
        public string Company { get; set; }
       
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be of length 6")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string c_pwd { get; set; }

        [Display(Name = "Promo Code")]
        public string PromoCode { get; set; }

        [Display(Name = "Service Level")]
        [Required]
        public string ServiceLevel { get; set; }

        public bool IsActive { get; set; }

        public int PlanID { get; set; }

        public int ServiceId { get; set; }

        public int Price { get; set; }

        [Display(Name = "Start Date")]
        public  string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Display(Name = "Plan Name")]
        public string PlanName { get; set; }


       public int AddCustomer(CustomerModel customer)
       {
            int id=0;
           string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
           try
           {
               using (SqlConnection con = new SqlConnection(connectionString))
               {
                   using (SqlCommand cmd = new SqlCommand("sp_CustomerInsert", con))
                   {
                       cmd.CommandType = CommandType.StoredProcedure;

                       cmd.Parameters.Add("@primaryContact", SqlDbType.VarChar).Value = customer.FullName;
                       cmd.Parameters.Add("@companyName", SqlDbType.VarChar).Value = customer.Company;
                       cmd.Parameters.Add("@primaryContactEmail", SqlDbType.VarChar).Value = customer.Email;
                       cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = customer.Password;
                       cmd.Parameters.Add("@serviceLevel", SqlDbType.VarChar).Value = customer.ServiceLevel;
                       cmd.Parameters.Add("@promoCode", SqlDbType.VarChar).Value = customer.PromoCode;


                       con.Open();
                       id = Convert.ToInt32(cmd.ExecuteScalar());
                   }
               }
           }
           catch { }
           return id;
       }

       public bool IsEmailExist(string Email)
       {
           string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
           DataTable table = new DataTable("allPrograms");
           using (SqlConnection conn = new SqlConnection(connectionString))
           {

               string command = "select * from Customer where PrimaryContactEmail=@Email" ;
               using (SqlCommand cmd = new SqlCommand(command, conn))
               {
                   cmd.Parameters.AddWithValue("@Email", Email);
                 
                    conn.Open();
                    var dd=  cmd.ExecuteScalar();
                    if(dd!=null)
                    {
                        return false;
                    }
                    conn.Close();
               }
           }

           return true;
       }

       public CustomerModel GetCustomerModel(string planName)
       {
           CustomerModel cm = new CustomerModel();
           // ... Switch on the string.
           switch (planName)
           {
               case "business-monthly":
                   cm.PlanID=21303;
                   cm.ServiceId=102;
                   cm.ServiceLevel = "Small Buisness";
                   cm.Price = 79;
                   break;
               case "business-annual":
                    cm.PlanID=23866;
                   cm.ServiceId=102;
                   cm.ServiceLevel = "Small Buisness";
                   cm.Price = 790;
                   break;
               case "performance-monthly":
                    cm.PlanID=21304;
                   cm.ServiceId=103;
                   cm.ServiceLevel = "Performance";
                   cm.Price = 149;
                   break;


               case "performance-annual":
                     cm.PlanID=23867;
                   cm.ServiceId=103;
                   cm.ServiceLevel = "Performance";
                   cm.Price = 1490;
                   break;


               case "enterprise-monthly":
                     cm.PlanID=21305;
                   cm.ServiceId=104;
                   cm.ServiceLevel = "Enterprise";
                   cm.Price = 499;
                   break;


               case "enterprise-annual":
                    cm.PlanID=23868;
                   cm.ServiceId=104;
                   cm.ServiceLevel = "Enterprise";
                   cm.Price = 4990;
                   break;
           }

           return cm;
       }

     
    }


 
}