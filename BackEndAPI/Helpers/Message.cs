namespace BackEndAPI.Helpers
{
    public class Message
    {
        public const string LoginFailed = "Username or password is incorrect. Please try again!";
        public const string ChangePasswordSucceed = "Your password has been changed successfully!";
        public const string OldPasswordIncorrect = "Your current password is incorrect!";
        public const string UserNotFound = "User not found!";
        public const string InternalError = "Internal server error!";
        public const string InvalidId = "Invalid Id!";
        public const string NullFirstName = "First Name can not be null!";
        public const string EmptyOrSpacesFirstName = "First Name can not be empty or contains only spaces!";
        public const string NullLastName = "Last Name can not be null!";
        public const string EmptyOrSpacesLastName = "Last Name can not be empty or contains only spaces!";
        public const string NullOrEmptyUsername = "Username can not be null or empty!";
        public const string NullUser = "User can not be null!";
        public const string RestrictedAge = "User is under 18. Please select a different date";
        public const string WeekendJoinedDate = "Joined date is Saturday or Sunday. Please select a different date";
        public const string JoinedBeforeBirth ="Joined date is not later than date of birth. Please select a different date";
    }
}