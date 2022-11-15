using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.CurrentUserId = User.Id();

            base.OnActionExecuting(context);
        }
    }
}