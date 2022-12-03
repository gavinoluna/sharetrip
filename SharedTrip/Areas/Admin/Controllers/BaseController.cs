using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static SharedTrip.Infrastructure.Data.Constants.AdminConstants;

namespace SharedTrip.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdministratorRole)]
    public class BaseController : Controller
    {
    }
}