using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace Identity.Models
{

    public class Users : IdentityUser
    {
        public Users()
        {
            ToDos = new HashSet<ToDos>();
        }

        [PersonalData]
        public DateTime DOB { get; set; }
        public ICollection<ToDos> ToDos { get; set; }
    }
}
