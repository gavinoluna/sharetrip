using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static SharedTrip.Infrastructure.Data.Constants.AdminConstants;

namespace SharedTrip.Areas.Admin.Controllers
{
    [Authorize(Roles = AdministratorRole)]
    [Area("Admin")]
    public class BaseController : Controller
    {
    }
}