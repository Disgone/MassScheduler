using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MassScheduler.Helpers;
using MassScheduler.Models;
using UrlHelper = MassScheduler.Helpers.UrlHelper;

namespace MassScheduler.Controllers
{
    public class ServicesController : BaseController
    {
        [AllowAnonymous]
        [OutputCache(VaryByParam = "none", Duration = 300)]
        public ActionResult RSS()
        {
            var meetings = Db.Meetings.Where(x => x.EndDate > DateTime.UtcNow);

            if (!meetings.Any())
            {
                return View("NoMeetings");
            }

            return new RssResult(meetings.ToList(), "Upcoming Events");
        }
        
        [AllowAnonymous]
        public ActionResult iCal(int id)
        {
            var meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            if (meeting.HasEnded())
            {
                return Content("Sorry, but that meeting has ended.");
            }

            var safeTitle = UrlHelper.ResolveTextToUrl(meeting.Title) + ".ics";

            return new iCalResult(meeting, safeTitle);
        }

        [AllowAnonymous]
        [OutputCache(VaryByParam = "none", Duration = 300)]
        public ActionResult iCalFeed()
        {
            var meetings = Db.Meetings.Where(x => x.EndDate > DateTime.UtcNow);

            if (!meetings.Any())
            {
                return View("NoMeetings");
            }

            return new iCalResult(meetings.ToList(), "Calendar.ics");
        }
    }
}
