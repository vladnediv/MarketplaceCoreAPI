using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

public class Controller1 : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}