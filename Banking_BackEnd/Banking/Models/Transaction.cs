//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Banking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int Transaction_Id { get; set; }
        public Nullable<int> From_Account_Number { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<long> To_Account_Number { get; set; }
        public string Mode { get; set; }
        public string Maturity_Instructions { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
        public Nullable<decimal> balance { get; set; }
    
        public  UsersAccount UsersAccount { get; set; }
    }
}
