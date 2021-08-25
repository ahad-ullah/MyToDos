using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyToDos.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [AllowAnonymous]
    public class ToDoController : Controller
    {
        private ApplicationDbContext _context;
        public ToDoController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            var myTodos = _context.toDos.Where(x => x.UserId== User.Identity.Name).ToList();
            return View(myTodos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string todo)
        {
            bool check = true;
            try
            {
                check = false;
                var user = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                ToDos toDos = new ToDos() { ToDo = todo, User = (Users)user, UserId = user.Id };
                _context.toDos.Add(toDos);
                _context.SaveChanges();

                return View(check);
            }
            catch
            {

                return View(check);
            }
        }
        public IActionResult Edit(int Id)
        {
            ToDos model = _context.toDos.Find(Id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ToDos toDos)
        {
            try
            {
                _context.toDos.Update(toDos);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return RedirectToAction(nameof(Edit));
            }
        }
        public IActionResult Delete(int Id, int UserId)
        {
            Users user = _context.users.Find(UserId);
            ToDos toDos = _context.toDos.Find(Id);
            Tuple<Users, ToDos> data = new Tuple<Users, ToDos>(user, toDos);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ConfirmDelete(int toDoId)
        {
            try
            {
                ToDos model = _context.toDos.Find(toDoId);
                _context.toDos.Remove(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return RedirectToAction(nameof(Delete));
            }
        }
    }
}
