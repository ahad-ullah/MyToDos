using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDos.Models
{
    public class ToDos
    {
        public int Id { get; set; }
        public string ToDo { get; set; }
        public string UserId { get; set; }
        public virtual Users Users { get; set; }

    }
}
