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
    public class transactionController : ApiController
    {
        
        public HttpResponseMessage GetUs(string id)
        {
            try
            {
                var data = id.Split(',');
                int acc = Convert.ToInt32(data[0]);
                DateTime start = Convert.ToDateTime(data[1]);
                DateTime finish = Convert.ToDateTime(data[2]);
                using (BankingDbEntities db = new BankingDbEntities())
                {

                    var accounts = db.Transactions.Where(a => a.From_Account_Number == acc).ToList();
                    List<Transaction> t = new List<Transaction>();
                    foreach (var i in accounts)
                    {
                        if (i.Transaction_Date >= start && i.Transaction_Date <= finish)
                            t.Add(i);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, t);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        public HttpResponseMessage GetUser(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Where(a => a.Customer_Id == id).First();
                    if (data == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer Id"+id+"not found");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        public HttpResponseMessage GetTransaction(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.Transactions.Where(a => a.From_Account_Number == id).ToList();
                    if (data == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account Number "+id+" Not found");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        public HttpResponseMessage GetLastTransaction(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.Transactions.Where(a => a.From_Account_Number == id).ToList();
                    int last = data.Count();
                    return Request.CreateResponse(HttpStatusCode.OK, data[last - 1]);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        public HttpResponseMessage GetTransaction(int id, int tr)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.Transactions.Where(a => a.From_Account_Number == id).ToList();
                    Transaction t = new Transaction();
                    foreach (var i in data)
                    {
                        if (i.Transaction_Id == tr)
                        {
                            t = i;
                            break;
                        }
                    }
                    if (t != null)
                        return Request.CreateResponse(HttpStatusCode.OK, t);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Transaction with transactoin id= " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        public HttpResponseMessage PostTransaction(int id, [FromBody] Transaction d)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    d.From_Account_Number = id;
                    d.Transaction_Date = DateTime.Now;
                    var data = db.UsersAccounts.Find(id);
                    d.balance = data.Balance - d.Amount;
                    if (d.Amount > data.Balance || data.Balance == null)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insufficient fund");
                    else
                    {
                        db.Transactions.Add(d);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.Created, d);
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
