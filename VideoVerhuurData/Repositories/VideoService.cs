using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoVerhuurData.Models;

namespace VideoVerhuurData.Repositories;

public class VideoService
{
	private readonly IVideoVerhuurRepository VideoRepository;
	public VideoService(IVideoVerhuurRepository videoRepository)
	{
		this.VideoRepository = videoRepository;
	}

	public Genres? GetGenre(int id)
	{
		return VideoRepository.GetGenre(id);
	}

	public IEnumerable<Genres> GetAllGenres()
	{
		return VideoRepository.GetAllGenres();
	}

	public IEnumerable<Films> GetGenreFilms(int id)
	{
		return VideoRepository.GetGenreFilms(id);
	}

	public Klanten? FindKlant(string naam, int postcode)
	{
		return VideoRepository.FindKlant(naam, postcode);
	}

	public Klanten? FindKlant(int id)
	{
		return VideoRepository.FindKlant(id);
	}

	public Films? FindFilm(int id)
	{
		return VideoRepository.FindFilm(id);
	}

	public int Invoorraad(int id)
	{
		return VideoRepository.Invoorraad(id);
	}

	public void adjustFilmStock(int id)
	{
		VideoRepository.adjustFilmStock(id);
	}

	public void verwijderFilm(int id)
	{
		VideoRepository.verwijderFilm(id);
	}

	public void addVerhuuring(int klantID, List<Films> films)
	{
		VideoRepository.addVerhuuring(klantID,films);
	}




}
