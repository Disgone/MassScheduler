using System.Web.Mvc;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    public class BaseController : Controller
    {
        protected MeetingContext Db = new MeetingContext();

        protected UserInformation CurrentUser
        {
            get { return Session["USER_INFORMATION"] as UserInformation; }
        }
    }
}