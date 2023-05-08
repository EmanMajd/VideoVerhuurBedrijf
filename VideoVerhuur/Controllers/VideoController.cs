using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VideoVerhuur.Models;
using VideoVerhuurData.Models;
using VideoVerhuurData.Repositories;

namespace VideoVerhuur.Controllers;

public class VideoController : Controller
{

	private readonly VideoService videoService;
	public VideoController(VideoService videoService)
	{
		this.videoService = videoService;
	}
	public IActionResult Index()
	{

		var sessionVariabeNaam = HttpContext.Session.GetString("Naam");
		var sessionVariablePostcode = HttpContext.Session.GetInt32("Postcode");
		if (sessionVariabeNaam == null || sessionVariablePostcode == null)
			return Redirect("Login");


			return Redirect("Genres");
		

	}
	[HttpGet]
	public IActionResult Login()
	{
		var sessionVariabeNaam = HttpContext.Session.GetString("Naam");
		var sessionVariablePostcode = HttpContext.Session.GetInt32("Postcode");
		List<LoginViewModel> list;
		if (sessionVariabeNaam == null || sessionVariablePostcode == null)
		{
			list = new List<LoginViewModel>();

			LoginViewModel loginViewModel = new LoginViewModel();
			return View("Login", loginViewModel);

		}
		else
		{
			var sessionVariabeklantId = HttpContext.Session.GetInt32("klantID");

			if (sessionVariabeklantId <= 0)
				return Redirect("Login");
				
			return Redirect("Genres");

		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult LoginVerify(string naam, int postcode)
	{

		
		if (naam == null || postcode == 0)
		{
			ViewBag.ErrorMessage = "Er is geen aangemelde gebruiker";
			return View("Login");
		}

		else
		{
			HttpContext.Session.SetString("Naam", naam);
			HttpContext.Session.SetInt32("Postcode", postcode);
			Klanten? klant = CheckGebruiker(naam, postcode);
			if(klant == null)
			{
				ViewBag.ErrorMessage = "Onbekende klant, probeer opnieuw";

				return View("Login");
			}
			else
			{
				HttpContext.Session.SetInt32("klantID" , klant.KlantId);
			}

			return Redirect("Genres");

		}

	}

	[NonAction]
	public Klanten? CheckGebruiker(string naam,int postcode)
	{
		Klanten? klant = videoService.FindKlant(naam,postcode);
		
		return klant;
	}

	public IActionResult Genres()
	{

		var klantid = HttpContext.Session.GetInt32("klantID");
		if (klantid == null)
		{
			return View("Login");
		}
		else
		{
			ViewBag.Klant = videoService.FindKlant((int)klantid).Voornaam;
			ViewBag.Naam = HttpContext.Session.GetString("Naam");
			return View(videoService.GetAllGenres());
		}
	}

	public IActionResult GenreFilms(int id)
	{
		Genres? genre = videoService.GetGenre(id);
		ViewBag.Naam = HttpContext.Session.GetString("Naam");

		if (genre == null)
		{
			ViewBag.ErrorMessage = $"Geen genre was found met dit id : {id}";
			Redirect("Genres");

		}
		

		return View(videoService.GetGenreFilms(id));

	}

	public IActionResult FilmVerhuren(Films film)
	{
		WinkelMandjeViewModel winkel = new WinkelMandjeViewModel();
		List<Films>? verhuurdFilms = winkel.WinkelFilmsVoorKlant;
		if( verhuurdFilms?.Count == 0 ) 
			 verhuurdFilms = new List<Films>();
		
		verhuurdFilms.Add(film);

		if (!verhuurdFilms.Contains(film))
		{
			verhuurdFilms.Remove(film);
			winkel.Titel = film.Titel;
			winkel.Prijs = film.Prijs;

		}
		return View(winkel);
	}
	/*
	public IActionResult FilmVerhuren(int id) {


		Films? film = videoService.FindFilm(id);

		var sessionVariabeleVerhuurd = HttpContext.Session.GetString("VerhuurdFilms");
		List<Films>? lijstVerhuurdFilms;
		if (string.IsNullOrEmpty(sessionVariabeleVerhuurd))
				lijstVerhuurdFilms = new List<Films>(); 
		else
				lijstVerhuurdFilms = JsonConvert.DeserializeObject<List<Films>>(sessionVariabeleVerhuurd);

		List<Films> ff = new List<Films>();
			if (film != null && lijstVerhuurdFilms != null)
			{
				//videoService.adjustFilmStock(id);
			bool x = lijstVerhuurdFilms.Contains(film);
			ViewBag.x = x;
				if (!lijstVerhuurdFilms.Contains((object)film))
				{
					lijstVerhuurdFilms?.Add(film);
					var geserializeerdeLijst = JsonConvert.SerializeObject(lijstVerhuurdFilms);
					HttpContext.Session.SetString("VerhuurdFilms", geserializeerdeLijst);

				}
				
			return View(lijstVerhuurdFilms);

			}

		return Redirect("Genres");

	}
	*/
	public IActionResult VerwijderFilm(int id)
	{
		var sessionVariabeleVerhuurd = HttpContext.Session.GetString("VerhuurdFilms");
		List<Films>? lijstVerhuurdFilms = new List<Films>(); 
		if (string.IsNullOrEmpty(sessionVariabeleVerhuurd))
			ViewBag.ErrorMessage("Winkel mandje is empty");
		else
		{
			 
			lijstVerhuurdFilms = JsonConvert.DeserializeObject<List<Films>>(sessionVariabeleVerhuurd);
			lijstVerhuurdFilms.ToList();
			Films? film = videoService.FindFilm(id);
			if(film != null && lijstVerhuurdFilms != null)
			{
				//videoService.verwijderFilm(id);

				lijstVerhuurdFilms.Remove(film);
				var geserializeerdeLijst = JsonConvert.SerializeObject(lijstVerhuurdFilms);
				HttpContext.Session.Remove("VerhuurdFilms");

				HttpContext.Session.SetString("VerhuurdFilms", geserializeerdeLijst);


			}
			
			return View("FilmVerhuren", lijstVerhuurdFilms);

		}

		return View();
	}

	public IActionResult Afrekenen()
	{

		var sessionVariabeklantId = HttpContext.Session.GetInt32("klantID");

		if (sessionVariabeklantId != null)
		{
			var klant = videoService.FindKlant((int)sessionVariabeklantId);
			if(klant != null)
			{
				ViewBag.klantID = sessionVariabeklantId;
				ViewBag.Naam = klant.Naam;
				ViewBag.Adres = klant.Straat_Nr;
				ViewBag.Gemeente = klant.Gemeente;
			}
			

			
		}

			var sessionVariabeleVerhuurd = HttpContext.Session.GetString("VerhuurdFilms");
		List<Films>? lijstVerhuurdFilms = new List<Films>();
		if (string.IsNullOrEmpty(sessionVariabeleVerhuurd))
			ViewBag.ErrorMessage("Fout gebeurd met afrekening");
		else
			lijstVerhuurdFilms = JsonConvert.DeserializeObject<List<Films>>(sessionVariabeleVerhuurd);

		decimal totaal = 0;
		foreach(var film in lijstVerhuurdFilms)
		{
			totaal += film.Prijs;
		}

		ViewBag.totaal = totaal;


		// Adjust stock

		return View(lijstVerhuurdFilms);
		

	}



}	

