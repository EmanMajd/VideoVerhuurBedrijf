using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoVerhuurData.Models;

public class Genres
{
	[Key]

	public int GenreId { get; set; }
	public string GenreNaam { get; set;}

	public ICollection<Films> Films { get; set; }

}
