using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace VideoVerhuurData.Models;

public class Films
{
	[Key]
	public int FilmId { get; set; }
	public string Titel { get; set; }
	[ForeignKey(nameof(Genre))]
	public int GenreId { get; set; }
	public int InVoorraad { get; set; }
	public int UitVoorraad { get; set; }

	public decimal Prijs { get; set; }
	public int TotaalVerhuurd { get; set; }

	public Genres Genre { get; set; }
	public ICollection<Verhuringen> Verhuringen { get; set; }


}
