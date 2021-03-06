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
    
    public partial class UsersAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsersAccount()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int Account_Number { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public string Customername { get; set; }
        public string Login_Password { get; set; }
        public string Transaction_Password { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<int> Reference_Id { get; set; }
        public Nullable<int> Otp { get; set; }
        public Nullable<int> Attemp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<Transaction> Transactions { get; set; }
        public  UserDetail UserDetail { get; set; }
    }
}
