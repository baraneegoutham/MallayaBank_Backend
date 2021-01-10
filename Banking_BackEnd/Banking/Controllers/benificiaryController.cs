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
    public class benificiaryController : ApiController
    {

        public HttpResponseMessage GetUser(int id)
        {
            using (BankingDbEntities db = new BankingDbEntities())
            {
                var data = db.UsersAccounts.Where(a => a.Customer_Id == id).First();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
        }


        public HttpResponseMessage GetBenificiary(int id)
        {
            using (BankingDbEntities db = new BankingDbEntities())
            {
                var data = db.Beneficiaries.Where(a => a.Holder_Account_Number == id).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
        }

        public HttpResponseMessage GetBenificiary(int id, int acc)
        {
            using (BankingDbEntities db = new BankingDbEntities())
            {
                Beneficiary d = new Beneficiary();
                var data = db.Beneficiaries.Where(a => a.Holder_Account_Number == id).ToList();
                foreach (var i in data)
                {
                    if (i.Beneficiary_Account_Number == acc)
                    {
                        d = i;
                        break;
                    }
                }

                if (d != null)
                    return Request.CreateResponse(HttpStatusCode.OK, d);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Benificiary with Benificiary account number = " + id + " not found");
            }
        }

        public HttpResponseMessage GetBenificies(int id)
        {
            using (BankingDbEntities db = new BankingDbEntities())
            {
                var data = db.Beneficiaries.Where(a => a.Holder_Account_Number == id).ToList();
                if (data != null)
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Transaction with transactoin id= " + id + " not found");
            }
        }

        public HttpResponseMessage PostBenificiary(int id, [FromBody] Beneficiary d)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    d.Holder_Account_Number = id;
                    db.Beneficiaries.Add(d);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, d);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
