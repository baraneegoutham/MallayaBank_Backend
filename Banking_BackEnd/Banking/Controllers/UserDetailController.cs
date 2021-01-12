﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Banking.Models;
using System.Web.Http.Cors;

namespace Banking.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class UserDetailController : ApiController
    {
        public HttpResponseMessage GetFetch(int id)
        {
            try
            {
                using (BankingDbEntities ad = new BankingDbEntities())
                {
                    List<UserDetail> d = new List<UserDetail>();
                    var cd = ad.UsersAccounts.Where(a => a.Customer_Id == id).First();
                    var refid = cd.Reference_Id;
                    string yesno = "no";
                    var data = ad.UserDetails.Where(a => a.Net_banking == "YES");
                    foreach (var i in data)
                    {
                        if (i.Reference_ID == refid)
                        {
                            yesno = "yes";
                            break;
                        }
                    }
                    if (yesno == "yes")
                        return Request.CreateResponse(HttpStatusCode.OK, "YES");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, "NO");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Customer Id"+id);
            }

        }

        public HttpResponseMessage GetaDetails()
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UserDetails.Where(user => user.Approval_Status == "no").ToList();
                    if (data == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Accounts Pending to be Approved");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);

            }
 
        }

        public HttpResponseMessage GetallDetails()
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UserDetails.ToList();
                    if (data == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Accounts Available");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        public HttpResponseMessage GetDetails(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UserDetails.Find(id);
                    if (data != null)
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Userdetail with refid= " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        public HttpResponseMessage Getcredentials(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    //var data = (from p in db.UsersAccounts
                    //            join o in db.UserDetails on p.Reference_Id equals o.Reference_ID
                    //            select new { p.Customer_Id, p.Account_Number, p.Login_Password}).Where(a => a.Customer_Id == id).ToList();
                    var data = (from p in db.UsersAccounts select new { p.Reference_Id, p.Customer_Id, p.Account_Number, p.Login_Password }).Where(a => a.Reference_Id == id).FirstOrDefault();
                    if (data != null)
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User Credentials for reference id = " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }


        }

        public HttpResponseMessage Getrefid()
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {

                    var data = db.UserDetails.Max(a => a.Reference_ID);

                    var result = db.UserDetails.Where(a => a.Reference_ID == data).FirstOrDefault();
                    if (result != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, result);

                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Reference id not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage GetRefernceid(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UserDetails.Where(user => user.Aadhar_Number == id).FirstOrDefault();

                    if (data != null)
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Userdetail with Aadharnumber= " + id + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }
        public HttpResponseMessage PostDetails([FromBody] UserDetail detail)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    detail.Approval_Status = "no";
                    db.UserDetails.Add(detail);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.Created, detail);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage PutDetails(int id, [FromBody] UserDetail details)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UserDetails.Find(id);

                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Dept with id= " + id + " not found");
                    }
                    else
                    {
                        data.Mobile_Number = details.Mobile_Number;
                        data.Email_Id = details.Email_Id;
                        data.Occupation_type = details.Occupation_type;
                        data.Source_of_Income = details.Source_of_Income;
                        data.PermanentAddress_Line1 = details.PermanentAddress_Line1;
                        data.PermanentAddress_Line2 = details.PermanentAddress_Line2;
                        data.PermanentCity = details.PermanentCity;
                        data.PermanentLankmark = details.PermanentLankmark;
                        data.PermanentState = details.PermanentState;
                        data.PermanentPincode = details.PermanentPincode;
                        data.Address_Line1 = details.Address_Line1;
                        data.Address_Line2 = details.Address_Line2;
                        data.City = details.City;
                        data.Lankmark = details.Lankmark;
                        data.State = details.State;
                        data.Pincode = details.Pincode;

                        data.Gross_Annual_Income = details.Gross_Annual_Income;
                        if (data.Approval_Status == "no")
                        {
                            if (details.Approval_Status == "yes")
                            {
                                data.Approval_Status = details.Approval_Status;
                            }
                        }
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
        public HttpResponseMessage PutAdminEdit(int id, [FromBody] UserDetail details)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UserDetails.Find(id);

                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Dept with id= " + id + " not found");
                    }
                    else
                    {
                        data.Net_banking = details.Net_banking;
                        data.Debit_Card = details.Debit_Card;
                        data.Account_type = details.Account_type;
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
