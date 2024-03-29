﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VideoVerhuur.Filters;
using VideoVerhuur.Models;
using VideoVerhuurData.Models;
using VideoVerhuurData.Repositories;

namespace VideoVerhuur.Controllers;
[LoginFilter]
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

	

	public IActionResult FilmVerhuren(int id)
	{

		Films? film = videoService.FindFilm(id);

		var sessionVariabeleVerhuurd = HttpContext.Session.GetString("VerhuurdFilms");

		List<Films> lijstVerhuurdFilms;
		if (string.IsNullOrEmpty(sessionVariabeleVerhuurd))
			lijstVerhuurdFilms = new List<Films>();

		else
			lijstVerhuurdFilms = JsonConvert.DeserializeObject<List<Films>>(sessionVariabeleVerhuurd);

		
			if(film!= null) {

			if (!lijstVerhuurdFilms.Exists(x => x.FilmId == film.FilmId))
			{
				lijstVerhuurdFilms?.Add(film);
				var geserializeerdeLijst = JsonConvert.SerializeObject(lijstVerhuurdFilms);
				HttpContext.Session.SetString("VerhuurdFilms", geserializeerdeLijst);
			}

			ViewBag.GenerId = film.GenreId;
			return View(lijstVerhuurdFilms);

			}
		
			return Redirect("Genres");

		
	}

	public IActionResult verwijder(int id)
	{
		Films? film = videoService.FindFilm(id);
		if(film != null )
		{
			ViewBag.Film = film;
		}

			return View(film);
	}

	public IActionResult VerwijderFilm(int id)
	{
		var sessionVariabeleVerhuurd = HttpContext.Session.GetString("VerhuurdFilms");
		List<Films>? lijstVerhuurdFilms = new List<Films>(); 
		if (string.IsNullOrEmpty(sessionVariabeleVerhuurd))
			ViewBag.ErrorMessage("Winkel mandje is empty");
		else
		{
			 
			lijstVerhuurdFilms = JsonConvert.DeserializeObject<List<Films>>(sessionVariabeleVerhuurd);
			Films? film = videoService.FindFilm(id);

			ViewBag.GenerId = film.GenreId;
			if (film != null && lijstVerhuurdFilms != null)
			{


				var index = lijstVerhuurdFilms.FindIndex(x => x.FilmId == film.FilmId);
				lijstVerhuurdFilms.RemoveAt(index);
				var geserializeerdeLijst = JsonConvert.SerializeObject(lijstVerhuurdFilms);

				HttpContext.Session.SetString("VerhuurdFilms", geserializeerdeLijst);


			}
			
			return View("FilmVerhuren", lijstVerhuurdFilms);

		}

		return View();
	}

	[AfrekeningFilter]
	public IActionResult Afrekenen()
	{

		var sessionVariabeklantId = HttpContext.Session.GetInt32("klantID");
		Klanten kalnt;
		if (sessionVariabeklantId != null)
		{
			kalnt = videoService.FindKlant((int)sessionVariabeklantId);
			if(kalnt != null)
			{
				ViewBag.klantID = sessionVariabeklantId;
				ViewBag.Naam = kalnt.Naam;
				ViewBag.Adres = kalnt.Straat_Nr;
				ViewBag.Gemeente = kalnt.Gemeente;
			}
				
		}

		var sessionVariabeleVerhuurd = HttpContext.Session.GetString("VerhuurdFilms");
		List<Films>? lijstVerhuurdFilms = new List<Films>();
		if (string.IsNullOrEmpty(sessionVariabeleVerhuurd))
			ViewBag.ErrorMessage("Je moet eerst een film verhuuren");
		else
			lijstVerhuurdFilms = JsonConvert.DeserializeObject<List<Films>>(sessionVariabeleVerhuurd);

		videoService.addVerhuuring((int)sessionVariabeklantId, lijstVerhuurdFilms);

		decimal totaal = 0;
		foreach(var film in lijstVerhuurdFilms)
		{
			videoService.adjustFilmStock(film.FilmId);
			totaal += film.Prijs;
		}
		videoService.addVerhuuring((int)sessionVariabeklantId, lijstVerhuurdFilms);

		ViewBag.totaal = totaal;


		// Adjust stock

		return View(lijstVerhuurdFilms);
		

	}



}	

