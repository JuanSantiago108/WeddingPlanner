using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models;

public class UserController : Controller
{
    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }

    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }
    

    private MyContext db;

        public UserController(MyContext context)
    {
        db = context;
    }


    [HttpGet("/")]
    public IActionResult Index()
    {

        if (loggedIn)
        {
            return RedirectToAction("All", "Wedding");
        }

        return View("Index");
    }



    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {


        if (db.Users.Any(user => user.Email == newUser.Email))
        {
            ModelState.AddModelError("Email", "is taken");
        }

        if (ModelState.IsValid == false)
        {
            return Index();
        }

        PasswordHasher<User> hashBrowns = new PasswordHasher<User>();
        newUser.Password = hashBrowns.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        return RedirectToAction("Dashboard", "Wedding");
    }




    
    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (ModelState.IsValid == false)
        {
            return Index();
        }
        User? dbUser = db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

        if (dbUser == null)
        {
            ModelState.AddModelError("LoginUser", "not found");
            return Index();
        }

        PasswordHasher<LoginUser> hashBrowns = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwCompareResult = hashBrowns.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LoginPassword", "is not correct");
            return Index();
        }

        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        return RedirectToAction("Dashboard", "Wedding");
    }


    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }




}