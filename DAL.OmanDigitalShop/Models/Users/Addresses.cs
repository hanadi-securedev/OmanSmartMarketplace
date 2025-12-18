using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.OmanDigitalShop.Models.Users
{
    public class Addresses
    {
        public string Id { get; set; }

        [ForeignKey(nameof(appUser))]
        public string AppUserId { get; set; }
        public AppUser appUser { get; set; }
    }
}
