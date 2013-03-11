using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MassScheduler.Helpers;
using MassScheduler.Models;
using UrlHelper = MassScheduler.Helpers.UrlHelper;

namespace MassScheduler.Controllers
{
    public class ServicesController : ControllerBase
    {
        [AllowAnonymous]
        [OutputCache(VaryByParam = "none", Duration = 300)]
        public ActionResult RSS()
        {
            var meetings = db.Meetings.Where(x => x.EndDate > DateTime.UtcNow);

            if (!meetings.Any())
            {
                return View("NoMeetings");
            }

            return new RssResult(meetings.ToList(), "Upcoming Events");
        }
        
        [AllowAnonymous]
        public ActionResult iCal(int id)
        {
            var meeting = db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            var safeTitle = UrlHelper.ResolveTextToUrl(meeting.Title) + ".ics";

            return new iCalResult(meeting, safeTitle);
        }

        [AllowAnonymous]
        [OutputCache(VaryByParam = "none", Duration = 300)]
        public ActionResult iCalFeed()
        {
            var meetings = db.Meetings.Where(x => x.EndDate > DateTime.UtcNow);

            if (!meetings.Any())
            {
                return View("NoMeetings");
            }

            return new iCalResult(meetings.ToList(), "Calendar.ics");
        }

        public ActionResult MyEvents(string username)
        {
            var meetings = db.Meetings.Where(x => x.EndDate > DateTime.UtcNow).ToList();
            var myMeetings = meetings.Where(x => x.IsUserAttending(username)).ToList();

            if (!myMeetings.Any())
            {
                return View("NoMeetings");
            }

            return new iCalResult(myMeetings, "MyEvents.ics");
        }
    }
}
