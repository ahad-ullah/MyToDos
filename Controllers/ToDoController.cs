using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyToDos.Data;
using MyToDos.ExtenstionFunctions;
using MyToDos.Models;

namespace MyToDos.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private ApplicationDbContext _context;
        public ToDoController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            var user = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var myTodos = _context.toDos.Where(x => x.UserId == User.Identity.GetUserId()).ToList();
            Tuple<Microsoft.AspNetCore.Identity.IdentityUser, List<ToDos>> data = new Tuple<Microsoft.AspNetCore.Identity.IdentityUser, List<ToDos>>(user, myTodos);
            return View(data);
        }
        [HttpPost]
        public IActionResult Create(string todo)
        {
            try
            {
                var user = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                ToDos toDos = new ToDos() { ToDo = todo, UserId = User.Identity.GetUserId() };
                _context.toDos.Add(toDos);
                _context.SaveChanges();
                return Json("Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(HttpStatusCode.InternalServerError); 
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
        public IActionResult Delete(int Id, string UserId)
        {
            var user = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            ToDos toDos = _context.toDos.Find(Id);
            Tuple<Microsoft.AspNetCore.Identity.IdentityUser, ToDos> data = new Tuple<Microsoft.AspNetCore.Identity.IdentityUser, ToDos>(user, toDos);
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
