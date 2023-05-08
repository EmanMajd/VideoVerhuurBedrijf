using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoVerhuurData.Models;

public class WinkelMandjeViewModel
{
	public string? Titel { get; set; }
	public decimal Prijs { get; set; }

	public List<Films>? WinkelFilmsVoorKlant { get; set; } = new List<Films>();
}
