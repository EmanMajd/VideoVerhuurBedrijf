using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VideoVerhuur.Models;

namespace VideoVerhuur.Filters;

public class LoginFilter : ActionFilterAttribute
{
	public override void OnActionExecuted(ActionExecutedContext context)
	{

		var sessionVariabeNaam = context.HttpContext.Session.GetString("Naam");
		var sessionVariablePostcode = context.HttpContext.Session.GetInt32("Postcode");
		if (sessionVariabeNaam == null || sessionVariablePostcode == null)
		{
		var viewResult = new ViewResult() { ViewName = "~/Views/Shared/login.cshtml" };
		context.Result = viewResult;
		}
	}
}
