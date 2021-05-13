using System;

namespace BackEndAPI.Models
{
    public class EditModel
    {
        public DateTime DateOfBirth { get; set; }

        public DateTime JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public UserType Type { get; set; }
    }
}