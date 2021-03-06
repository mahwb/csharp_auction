using System;
using System.Collections.Generic;

namespace csharp_belt.Models
{
    public class Wallet: BaseEntity
    {
        public int WalletId {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public double Amount {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UploadedAt {get; set;}
    }
}