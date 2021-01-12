using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Banking.Models;
using System.Web.Http.Cors; 

namespace Banking.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersAccountController : ApiController
    {
        public HttpResponseMessage GetCustId(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = (from p in db.UsersAccounts join o in db.UserDetails on p.Reference_Id equals o.Reference_ID select new { p.Account_Number, p.Customer_Id, p.Reference_Id, o.Mobile_Number, o.Email_Id }).Where(a => a.Account_Number == id).FirstOrDefault();
                    if (data != null)
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with Accountnumber= " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }

        public HttpResponseMessage Getstatement(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = (from p in db.UsersAccounts
                                join o in db.UserDetails on p.Reference_Id equals o.Reference_ID
                                select new { p.Customer_Id, p.Account_Number, p.Customername, o.Account_type, p.Balance }).Where(a => a.Customer_Id == id).ToList();
                    //var data = db.UsersAccounts.Where(a => a.Customer_Id == id).First();
                    if (data != null)
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with customerid= " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }
        public HttpResponseMessage GetSetnewPassword(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = (from p in db.UsersAccounts
                                join o in db.UserDetails on p.Reference_Id equals o.Reference_ID
                                select new { p.Account_Number, p.Customer_Id, p.Reference_Id, o.Mobile_Number, o.Email_Id}).Where(a => a.Customer_Id == id).FirstOrDefault();
                    if (data != null)
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with Accountnumber= " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }
        [HttpPut]
        public HttpResponseMessage PutRegister(int id, [FromBody] UsersAccount register)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Find(id);
                    var netbanking = db.UserDetails.Where(a => a.Reference_ID == data.Reference_Id).FirstOrDefault();

                    if (data == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account number" + id + " not found");
                    }
                    else
                    {
                        data.Login_Password = register.Login_Password;
                        data.Transaction_Password = register.Transaction_Password;
                        data.Otp = register.Otp;
                        data.Register_Internet_Banking = "yes";
                        data.Attemp = 3;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        public HttpResponseMessage PutNewPassword(int id, [FromBody] UsersAccount setpassword)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Find(id);

                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Account number" + id + " not found");
                    }
                    else
                    {
                        data.Login_Password = setpassword.Login_Password;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        public HttpResponseMessage PutChangePassword(int id, [FromBody] UsersAccount changepassword)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Where(a=>a.Customer_Id==id).FirstOrDefault();

                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Customer Id" + id + " not found");
                    }
                    else
                    {
                        data.Login_Password = changepassword.Login_Password;
                        data.Transaction_Password = changepassword.Transaction_Password;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Deletelocked(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Find(id);
                    var unlocked = db.AccountLockeds.Where(a => a.Customer_Id == data.Customer_Id).FirstOrDefault();
                    if (unlocked != null)
                    {
                        db.AccountLockeds.Remove(unlocked);
                        data.Attemp = 3;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Account unlocked");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK,"Your Account is Safe");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
