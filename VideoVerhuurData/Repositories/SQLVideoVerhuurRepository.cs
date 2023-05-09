using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoVerhuurData.Models;

namespace VideoVerhuurData.Repositories;

public class SQLVideoVerhuurRepository : IVideoVerhuurRepository
{

	private readonly VideoVerhuurDbContext context;
	public SQLVideoVerhuurRepository(VideoVerhuurDbContext context)
	{
		this.context = context;
	}

	public IEnumerable<Films> GetGenreFilms(int id)
	{
		return context.Films.Include(x => x.Genre)
				.Where(x => x.GenreId== id);
	}

	public IEnumerable<Genres> GetAllGenres() => context.Genres.AsNoTracking();


	public Genres? GetGenre(int id) => context.Genres.Find(id);

	public Klanten? FindKlant(string naam, int postcode)
	{
		return context.Klanten.Where(x => x.Naam == naam && x.PostCode == postcode).FirstOrDefault();
	}

	public Klanten? FindKlant(int id) => context.Klanten.Where(x => x.KlantId == id).FirstOrDefault();



	public Films? FindFilm(int id) => context.Films.Find(id);

	public int Invoorraad(int id) => FindFilm(id).InVoorraad;

	public void adjustFilmStock(int id)
	{
		var film = FindFilm(id);
		if (film != null)
		{
			film.TotaalVerhuurd += 1;
			film.InVoorraad -= 1;
			film.UitVoorraad += 1;
			context.SaveChanges();

		}



	}

	public void verwijderFilm(int id)
	{
		var film = FindFilm(id);
		if (film != null)
		{

			film.TotaalVerhuurd -= 1;
			film.InVoorraad += 1;
			film.UitVoorraad -= 1;
			context.SaveChanges();

		}

	}

	public void addVerhuuring(int klantID, List<Films> films)
	{

		if(films.Count() >0 && klantID >= 0)
		{
			foreach(var film in films)
			{
				var verhuurdFilm = new Verhuringen();
				verhuurdFilm.KlantId = klantID;
				verhuurdFilm.FilmId = film.FilmId;
				verhuurdFilm.VerhuurDatum = DateTime.Now;
				context.Verhuringen.Add(verhuurdFilm);
			}
			

		}
	}





}
