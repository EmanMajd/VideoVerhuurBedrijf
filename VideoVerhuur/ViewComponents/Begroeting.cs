using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace VideoVerhuur.ViewComponents;

public class Begroeting : ViewComponent
{
	public IViewComponentResult Invoke()
	{
		string imagePath = "videoVerhuur";
		return View((object)imagePath);
	}
}
