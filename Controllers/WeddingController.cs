using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class WeddingController : Controller
{
    private MyContext _context;

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


    public WeddingController(MyContext context)
    {
        _context = context;
    }

    [HttpGet("/dashboard")]
    public IActionResult Dashboard()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        List<Wedding> AllWedding = _context.Weddings
        .Include(wed => wed.Attendees).ThenInclude(wed => wed.User).ToList();
        return View("Dashboard", AllWedding);
    }

    [HttpPost("/dashboard/{weddingId}")]
    public IActionResult Delete(int weddingId)
    {
        Wedding? weddingIdDelete = _context.Weddings.FirstOrDefault(wed => wed.WeddingId == weddingId);

        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        if (uid != weddingIdDelete.UserId)
        {
            return RedirectToAction("Dashboard");
        }

        if (weddingIdDelete != null)
        {
            _context.Weddings.Remove(weddingIdDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        return RedirectToAction("Dashboard");
    }

    [HttpGet("/create")]
    public IActionResult New()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        return View("NewWedding");
    }

    [HttpPost("/create/wedding")]
    public IActionResult Create(Wedding newWedding)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        newWedding.UserId = (int)uid;

        if (ModelState.IsValid == false)
        {
            return New();
        }
        _context.Add(newWedding);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpPost("/create/{weddingId}")]
    public IActionResult Attend(int weddingId)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "User");
        }
        Association? existingRSVP = _context.Associations.FirstOrDefault
        (rsvp => rsvp.WeddingId == weddingId && rsvp.UserId == uid);

        if (existingRSVP == null)
        {
            Association newRSVP = new Association()
            {
                WeddingId = weddingId,
                UserId = (int)uid
            };
            _context.Associations.Add(newRSVP);
        }
        else
        {
            _context.Remove(existingRSVP);
        }
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpGet("detail/{weddingId}")]
    public IActionResult Detail(int weddingId)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "User");
        }
        Wedding? wed = _context.Weddings
        .Include(wed => wed.Attendees)
        .ThenInclude(wed => wed.User)
        .FirstOrDefault(wed => wed.WeddingId == weddingId);
        return View("Detail", wed );
    }

}