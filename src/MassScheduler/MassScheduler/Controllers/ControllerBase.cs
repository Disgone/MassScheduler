using System.Web.Mvc;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    public class ControllerBase : Controller
    {
        protected MeetingContext db = new MeetingContext();

        protected UserInformation CurrentUser
        {
            get { return Session["USER_INFORMATION"] as UserInformation; }
        }
    }
}