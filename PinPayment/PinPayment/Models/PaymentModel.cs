using PinPayments;
using PinPayments.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PinPayment.Models
{
    public class PaymentModel
    {

        public int CustomerID { get; set; }

       
        [Display(Name = "Card Number")]
        [Required]
        [StringLength(16, MinimumLength = 16,ErrorMessage="Please enter 16 digit card number")]
       public string CardNumber { get; set; }
       [Required]
       public string CVC { get; set; }

       [Required]
       [Display(Name = "Expiry Month")]
       public string ExpiryMonth { get; set; }

        [Display(Name = "Expiry Year")]
        [Required]
       public string ExpiryYear { get; set; }

        [Display(Name = "Cardholder Name")]
        [Required]
       public string Name { get; set; }

        [Display(Name = "Phone Number")]
       public string PhoneNumber { get; set; }
        [Required]
       public string City { get; set; }

        [Display(Name = "Post Code")]
       public string PostCode { get; set; }
       public string State { get; set; }

        [Required]
       public string Country { get; set; }

       [Required]
       public long Amount { get; set; }
      

        [EmailAddress]
        [Required]
       public string Email { get; set; }

        [Required]
       public string Address { get; set; }

        public int PlanID { get; set; }

        public int ServiceId { get; set; }
        public int Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


       public ChargeResponse Charge(PaymentModel payment)
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

           string ipAddress =HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

           ChargeResponse response = ps.Charge(new PostCharge { Amount = payment.Price*100, Card = card, Currency = "USD", Description = "Desc", Email = payment.Email, IPAddress = ipAddress });
           if(response.Charge!=null)
           {
               CustomerPlanModel customerPlan = new CustomerPlanModel();
               customerPlan.CustomerID = payment.CustomerID;
               customerPlan.PlanID = payment.PlanID;
               customerPlan.price = payment.Price;
               customerPlan.ServiceLevelID = payment.ServiceId;
               customerPlan.StartDate = DateTime.Now;
               if (customerPlan.PlanID == 21303 || customerPlan.PlanID == 21304 || customerPlan.PlanID == 21305)
               {
                   
                   customerPlan.EndDate = DateTime.Now.AddMonths(1);
               }
               else
               {
                   customerPlan.EndDate = DateTime.Now.AddYears(1);
               }
               UpdateCustomer(customerPlan);
           }
           return response;

       }

       public int UpdateCustomer(CustomerPlanModel customerPlan)
       {
           int id = 0;
           string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
           using (SqlConnection con = new SqlConnection(connectionString))
           {
               using (SqlCommand cmd = new SqlCommand("sp_CustomerValidate", con))
               {
                   cmd.CommandType = CommandType.StoredProcedure;

                   cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerPlan.CustomerID;
                   cmd.Parameters.Add("@PlanID", SqlDbType.Int).Value = customerPlan.PlanID;
                   cmd.Parameters.Add("@ServiceLevelID", SqlDbType.Int).Value = customerPlan.ServiceLevelID;
                   cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value =customerPlan.StartDate;
                   cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value =customerPlan.EndDate;
                   cmd.Parameters.Add("@Price", SqlDbType.Int).Value = customerPlan.price;
                   

                   con.Open();
                   id = cmd.ExecuteNonQuery();
               }
           }
           return id;
       }


        public List<CustomerModel> Get()
       {
           string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
           DataTable table = new DataTable("allPrograms");
           using (SqlConnection conn = new SqlConnection(connectionString))
           {
              
               string command = "SELECT * FROM Customer";
               using (SqlCommand cmd = new SqlCommand(command, conn))
               {
                   SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                   conn.Open();
                   adapt.Fill(table);
                   conn.Close();
               }
           }
           List<CustomerModel> cust = new List<CustomerModel>();
           foreach (DataRow row in table.Rows)
           {

               cust.Add(new CustomerModel()
               {
                   FullName = row["PrimaryContact"].ToString(),
                   Company = row["CompanyName"].ToString(),
                   IsActive = Convert.ToBoolean(row["IsActive"]),
                   PromoCode = row["PromoCode"].ToString(),
                   ServiceLevel = row["ServiceLevel"].ToString(),
                   Email = row["PrimaryContactEmail"].ToString()

               });
           }

           return cust;
       }

        public List<CustomerModel> GetCustomerPlan()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
           
            List<CustomerModel> cust = new List<CustomerModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

             
                using (SqlCommand cmd = new SqlCommand("sp_GetCustomerPlanDetail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    conn.Open();
                   SqlDataReader   row = cmd.ExecuteReader();
                    while (row.Read())
                    {
                        cust.Add(new CustomerModel()
                        {
                            FullName = row["PrimaryContact"].ToString(),
                            Company = row["CompanyName"].ToString(),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                           
                            ServiceLevel = row["ServiceName"].ToString(),
                            Email = row["PrimaryContactEmail"].ToString(),
                            StartDate=row["StartDate"].ToString(),
                            EndDate=row["EndDate"].ToString(),
                            PlanName=row["PlanName"].ToString(),
                            Price=Convert.ToInt32(row["Price"])
                        });
                    }
                    conn.Close();
                }
            }
          
          

            return cust;
        }
    }


   
}