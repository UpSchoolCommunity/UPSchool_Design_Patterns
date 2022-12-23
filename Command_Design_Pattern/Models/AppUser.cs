using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Command_Design_Pattern.Models
{
    [Table("AspNetUsers")]
    public class AppUser:IdentityUser
    {
     
    }
}
