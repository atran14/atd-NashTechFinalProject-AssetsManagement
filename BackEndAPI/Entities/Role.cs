using Microsoft.AspNetCore.Identity;

namespace BackEndAPI.Entities
{
    public class Role : IdentityRole<int>
    {
        public string Description {get; set;}
    }
}