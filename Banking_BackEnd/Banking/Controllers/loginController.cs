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
        public HttpResponseMessage PostLogin([FromBody] login log)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Where(a => a.Customer_Id == log.CustomerID).First();
                    if (data.Login_Password == log.Password)
                        return Request.CreateResponse(HttpStatusCode.OK, "User " + data.Customername + " has logged in");
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid user id " + log.CustomerID);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
