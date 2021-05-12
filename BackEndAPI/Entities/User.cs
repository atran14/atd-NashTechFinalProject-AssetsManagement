using System;
using System.Collections.Generic;

namespace BackEndAPI.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public UserType Type { get; set; }

        public string StaffCode { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public Location Location { get; set; }

        public UserStatus Status { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<ReturnRequest> Requests { get; set; }
        
    }
}