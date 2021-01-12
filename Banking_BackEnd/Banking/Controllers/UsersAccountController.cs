using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Banking.Models;
using System.Web.Http.Cors;
using System.Net.Mail;

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
                    var data = (from p in db.UsersAccounts join o in db.UserDetails on p.Reference_Id equals o.Reference_ID select new { p.Account_Number, p.Customer_Id, p.Reference_Id, o.Mobile_Number, o.Email_Id,p.Otp }).Where(a => a.Account_Number == id).FirstOrDefault();


                    if (data != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
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
                                select new { p.Account_Number, p.Customer_Id, p.Reference_Id, o.Mobile_Number, o.Email_Id,p.Otp}).Where(a => a.Customer_Id == id).FirstOrDefault();
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
        public HttpResponseMessage PutOtp(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Find(id);
                    var data2 = db.UserDetails.Where(a => a.Reference_ID == data.Reference_Id).FirstOrDefault();

                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Account number" + id + " not found");
                    }
                    else
                    {
                        Random r = new Random();
                        int randNum = r.Next(100000);
                        data.Otp = randNum;
                        db.SaveChanges();
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        SmtpServer.Host = "smtp.gmail.com";
                        var email = data2.Email_Id;
                        var mobilenumber = data2.Mobile_Number;
                        var cust_name = data.Customername;
                        var register = data2.Net_banking;


                        mail.From = new MailAddress("deepikasr.1rn16ec189@gmail.com");
                        mail.To.Add(email);
                        mail.Subject = "OTP Verification";
                        if (register != "NO")
                        {
                            mail.Body = "Hi " + cust_name + "\n" + "Your Otp to change credentails : " + randNum;
                        }
                        else
                        {
                            mail.Body = "Hi " + cust_name + "\n" + "Your have not opt for Net Banking. Kindly contact Admin to Unlock Netbanking ";
                        }


                        //System.Net.Mail.Attachment attachment;
                        //attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
                        //mail.Attachments.Add(attachment);

                        SmtpServer.Port = 587;

                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("deepikasr.1rn16ec189@gmail.com", "deepcoll@1234");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage PutOtppass(int id)
        {
            try
            {
                using (BankingDbEntities db = new BankingDbEntities())
                {
                    var data = db.UsersAccounts.Where(a => a.Customer_Id == id).FirstOrDefault();
                    var data2 = db.UserDetails.Where(a => a.Reference_ID == data.Reference_Id).FirstOrDefault();

                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Account number" + id + " not found");
                    }
                    else
                    {
                        Random r = new Random();
                        int randNum = r.Next(100000);
                        data.Otp = randNum;
                        db.SaveChanges();
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        SmtpServer.Host = "smtp.gmail.com";
                        var email = data2.Email_Id;
                        var mobilenumber = data2.Mobile_Number;
                        var cust_name = data.Customername;


                        mail.From = new MailAddress("deepikasr.1rn16ec189@gmail.com");
                        mail.To.Add(email);
                        mail.Subject = "OTP Verification";
                        mail.Body = "Hi " + cust_name + "\n" + "Your Otp to change credentails : " + randNum;


                        //System.Net.Mail.Attachment attachment;
                        //attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
                        //mail.Attachments.Add(attachment);

                        SmtpServer.Port = 587;

                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("deepikasr.1rn16ec189@gmail.com", "deepcoll@1234");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
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
