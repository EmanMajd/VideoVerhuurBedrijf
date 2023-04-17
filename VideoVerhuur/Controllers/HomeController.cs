using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VideoVerhuur.Models;
using VideoVerhuurData.Models;

namespace VideoVerhuur.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
	

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult loginVerify(string naam,string postcode)
		{
			if(naam == null && postcode == null)
			{
				ViewBag.ErrorMessage("Er is geen aangemelde gebruiker");
			}
			else
			{
				ViewBag.Naam = naam;
				ViewBag.Postcode = postcode;	
			}

				return Redirect("Video/Genres"); 
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
}