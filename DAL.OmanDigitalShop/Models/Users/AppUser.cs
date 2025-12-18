

using Microsoft.AspNetCore.Identity;

namespace DAL.OmanDigitalShop.Models.Users
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public int NumberOfOrders { get; set; }

        public DateOnly DOB { get; set; }



    }
}
