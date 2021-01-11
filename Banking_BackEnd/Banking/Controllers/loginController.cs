using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Banking.Models;

namespace Banking.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class loginController : ApiController
    {
        public string fetch(int id)
        {
            BankingDbEntities ad = new BankingDbEntities();
            List<UserDetail> d = new List<UserDetail>();
            var cd = ad.UsersAccounts.Where(a => a.Customer_Id == id).First();
            var refid = cd.Reference_Id;
            string yesno = "no";
            var data = ad.UserDetails.Where(a => a.Approval_Status == "yes");
            foreach (var i in data)
            {
                if (i.Reference_ID == refid)
                {
                    yesno = "yes";
                    break;
                }
            }
            return yesno;
        }

        public HttpResponseMessage PostLogin([FromBody] login log)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var iii = log.CustomerID;
                    string abcd = fetch(iii);
                    var cd = db.UsersAccounts.Where(a => a.Customer_Id == iii).First();
                    if (cd.Attemp == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account is been locked");
                    }
                    if (abcd=="no")
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account not yet approved");
                    var data = db.UsersAccounts.Where(a => a.Customer_Id == log.CustomerID).First();
                    var acc = db.AccountLockeds.ToList();
                    foreach (var i in acc)
                    {
                        if (i.Customer_Id == log.CustomerID)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account Locked for " + log.CustomerID+"use forgot password  and set your new password to unlock your account");
                        }
                    }
                    if (data.Login_Password == log.Password)
                        
                        return Request.CreateResponse(HttpStatusCode.OK, "User " + data.Customername + " has logged in");
                    else
                    {
                        
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid user id " + log.CustomerID);
                    }


                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage PostLocked([FromBody] login log)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    AccountLocked a = new AccountLocked();
                    a.Customer_Id = log.CustomerID;
                    db.AccountLockeds.Add(a);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, "Account locked");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Try fail");
            }
        }

        public HttpResponseMessage PutupdateAttempt(int id)
        {
            try
            {
                using(BankingDbEntities db=new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Where(a => a.Customer_Id == id).FirstOrDefault();
                    data.Attemp = 3;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Try fail");
            }
        }

        public HttpResponseMessage PutAttempt(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Where(a=>a.Customer_Id==id).FirstOrDefault();
                    var dec = data.Attemp;
                        
                        
                        if (dec == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account has been locked");
                        }
                    else
                    {
                        dec--;
                        data.Attemp = dec;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK,dec);
                    }

                        
                    
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
