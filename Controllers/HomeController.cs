using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActionKids.Models;
using Htmx;

namespace ActionKids.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
		string anActionKidIs = @"
			<div id=""target"" class=""text-center"">
			<h2>An Action Kid:</h2>
			<ul>
				<li>Loves God</li>
				<li>Is kind to others</li>
				<li>Respects leaders</li>
				<li>Does Hard things</li> 
			</ul>
			</div>";

        return Request.IsHtmx()
			? Content(anActionKidIs, "text/html")
			: View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
