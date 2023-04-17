using System.ComponentModel.DataAnnotations;
using System.IO;

namespace VideoVerhuur.Models;

public class LoginViewModel
{
	[Display(Name = "Naam")]
	[Required(ErrorMessage = "Naam is een verplicht veld")]
	public string Naam { get; set; }

	[Display(Name = "Postcode")]
	[Required(ErrorMessage = "Postcode is een verplicht veld")]
	public int Postcode { get; set; }


}
