using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoVerhuurData.Models;

namespace VideoVerhuurData.Repositories;

public interface IVideoVerhuurRepository
{

	Genres? GetGenre(int id);
	IEnumerable<Genres> GetAllGenres();

	IEnumerable<Films> GetGenreFilms(int id);

	Klanten? FindKlant(string naam, int postcode);

	Klanten? FindKlant(int id);

	Films? FindFilm(int id);

	int Invoorraad(int id);

	void adjustFilmStock(int id);

	void verwijderFilm(int id);

}
