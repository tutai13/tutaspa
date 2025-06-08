using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public bool FisrtLogin { get; set;  } = true ; 
    }
}
