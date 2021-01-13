using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Banking.Models
{
    public class UserDetailMetaData
    {
        public int Reference_ID { get; set; }
        public string Title { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Father_Name { get; set; }
        public long Mobile_Number { get; set; }
        public string Email_Id { get; set; }
        public long Aadhar_Number { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{yyyy-MM-dd}")]
        public System.DateTime Date_of_Birth { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string Lankmark { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string PermanentAddress_Line1 { get; set; }
        public string PermanentAddress_Line2 { get; set; }
        public string PermanentLankmark { get; set; }
        public string PermanentState { get; set; }
        public string PermanentCity { get; set; }
        public int PermanentPincode { get; set; }
        public string Occupation_type { get; set; }
        public string Source_of_Income { get; set; }
        public int Gross_Annual_Income { get; set; }
        public string Debit_Card { get; set; }
        public string Net_banking { get; set; }
        public string Account_type { get; set; }
        public string Approval_Status { get; set; }
        public ICollection<UsersAccount> UsersAccounts { get; set; }
    }
    [MetadataType(typeof(UserDetailMetaData))]
    public partial class UserDetail { }
}