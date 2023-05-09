using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VideoVerhuur.Filters;

public class AfrekeningFilter : ActionFilterAttribute
{
	public override void OnActionExecuted(ActionExecutedContext context)
	{


		context.HttpContext.Session.Remove("klantID");
		var sessionVariabeklantId = context.HttpContext.Session.GetInt32("klantID");

		/*if (sessionVariabeklantId == null)
		{

			var viewResult = new ViewResult() { ViewName = "~/Views/Shared/login.cshtml" };
			context.Result = viewResult;
			context.HttpContext.Session.Clear();

		}*/
	}
}
