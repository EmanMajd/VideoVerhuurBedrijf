using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoVerhuurData.Models;

public class Verhuringen
{
	[Key]

	public int VerhuurId { get; set; }
	[ForeignKey(nameof(Klant))]
	public int KlantId { get; set; }

	[ForeignKey(nameof(Film))]
	public int FilmId { get; set; }
	public DateTime VerhuurDatum { get; set; }

	public Klanten Klant { get; set; }

	public Films Film { get; set; }

	
}
