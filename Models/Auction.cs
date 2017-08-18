using System;
using System.Collections.Generic;

namespace csharp_belt.Models
{
    public class Auction: BaseEntity
    {
        public int AuctionId {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public double Bid {get; set;}
        public DateTime EndDate {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UploadedAt {get; set;}
    }
}