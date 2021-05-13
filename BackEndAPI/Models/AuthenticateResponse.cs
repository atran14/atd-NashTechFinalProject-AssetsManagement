using System;

namespace BackEndAPI.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public UserType Type { get; set; }

        public string StaffCode { get; set; }
        public Location Location { get; set; }

        public UserStatus Status { get; set; }

        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            DateOfBirth = user.DateOfBirth;
            JoinedDate = user.JoinedDate;
            Gender = user.Gender;
            Type = user.Type;
            StaffCode = user.StaffCode;
            Location = user.Location;
            Status = user.Status;
            Username = user.Username;
            Token = token;
        }
    }
}