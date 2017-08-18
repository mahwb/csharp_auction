using System;
using System.Collections.Generic;

namespace csharp_belt.Models
{
    public class User: BaseEntity
    {
        public int UserId {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Username {get; set;}
        public string Password {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UploadedAt {get; set;}
    }
}