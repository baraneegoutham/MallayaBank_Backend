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
    public class AdminController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage PostAdLogin([FromBody] Admin1 ad)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var Obj = db.Admins.Where(a => a.Admin_Id == ad.Admin_Id).First();
                    var obj2 = Obj.Password;
                    if (obj2 != ad.Password)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Admin invalid");

                    else
                        return Request.CreateResponse(HttpStatusCode.OK, "Successful Login.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
