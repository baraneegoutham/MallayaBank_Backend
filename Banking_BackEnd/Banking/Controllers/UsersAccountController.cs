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
            using (BankingDbEntities db = new BankingDbEntities())
            {
                var data = db.UsersAccounts.Find(id);
                if (data != null)
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with Accountnumber= " + id + " not found");
            }

        }
        public HttpResponseMessage GetSetnewPassword(int id)
        {
            using (BankingDbEntities db = new BankingDbEntities())
            {
                var data = db.UsersAccounts.Where(e=>e.Customer_Id==id).FirstOrDefault();
                if (data != null)
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with Accountnumber= " + id + " not found");
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

                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Account number" + id + " not found");
                    }
                    else
                    {
                        data.Login_Password = register.Login_Password;
                        data.Transaction_Password = register.Transaction_Password;
                        data.Otp = register.Otp;
                        data.Balance = register.Balance;
                       
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
    }

}
