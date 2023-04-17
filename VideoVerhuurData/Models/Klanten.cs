using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoVerhuurData.Models;

public class Klanten
{
	[Key]

	public int KlantId { get; set; }
	public string Naam { get;set; }
	public string Voornaam { get; set; }
	public string Straat_Nr { get; set; }
	public int PostCode { get;set; }
	public string Gemeente { get; set; }	
	public string Klantstat { get; set; }
	public decimal HuurAantal { get; set; }
	public DateTime DatumLid { get; set; }
	public decimal LidGeld { get; set; }

	public ICollection<Verhuringen> Verhuringens { get; set; }



}
