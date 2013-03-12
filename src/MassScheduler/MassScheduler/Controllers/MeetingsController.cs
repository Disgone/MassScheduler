using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    [Authorize]
    public class MeetingsController : BaseController
    {

        public ActionResult Index()
        {
            var meetings = Db.Meetings
                             .Where(x => x.EndDate >= DateTime.UtcNow)
                             .OrderBy(x => x.StartDate);

            ViewBag.CurrentUser = CurrentUser;
            return View(meetings);
        }

        public ActionResult Details(int id)
        {
            var meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            meeting.StartDate = TimeZone.CurrentTimeZone.ToLocalTime(meeting.StartDate);
            meeting.EndDate = TimeZone.CurrentTimeZone.ToLocalTime(meeting.EndDate);

            ViewBag.CurrentUser = CurrentUser;
            return View(meeting);
        }

        public ActionResult Create()
        {
            var date = GetNextTimeIncrement(DateTime.Now);

            var meeting = new Meeting()
            {
                StartDate = date,
                EndDate = date.AddHours(1),
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Contact = CurrentUser.EmailAddress,
                Speakers = new List<Speaker>()
            };

            ViewBag.Speakers =
                Db.Speakers
                  .ToList()
                  .OrderBy(x => x.Name)
                  .Select(x => new SelectListItem() {Selected = false, Text = x.Name, Value = x.Id.ToString()});

            return View(meeting);
        }

        [HttpPost]
        public ActionResult Create(Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                meeting.Creator = CurrentUser.Username;
                meeting.Created = DateTime.UtcNow;
                meeting.Modified = DateTime.UtcNow;

                meeting.StartDate = TimeZone.CurrentTimeZone.ToUniversalTime(meeting.StartDate);
                meeting.EndDate = TimeZone.CurrentTimeZone.ToUniversalTime(meeting.EndDate);

                Db.Meetings.Add(meeting);
                Db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(meeting);
        }

        public ActionResult Edit(int id)
        {
            var meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            if (meeting.Creator != CurrentUser.Username)
            {
                return View("InvalidOwner");
            }

            meeting.StartDate = TimeZone.CurrentTimeZone.ToLocalTime(meeting.StartDate);
            meeting.EndDate = TimeZone.CurrentTimeZone.ToLocalTime(meeting.EndDate);

            return View(meeting);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Meeting meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            if (meeting.Creator != CurrentUser.Username)
            {
                return View("InvalidOwner");
            }

            try
            {
                UpdateModel(meeting);
                meeting.StartDate = TimeZone.CurrentTimeZone.ToUniversalTime(meeting.StartDate);
                meeting.EndDate = TimeZone.CurrentTimeZone.ToUniversalTime(meeting.EndDate);
                meeting.Modified = DateTime.UtcNow;

                Db.SaveChanges();

                return RedirectToAction("details", new {id = meeting.Id});
            }
            catch
            {
                return View(meeting);
            }
        }

        public ActionResult Delete(int id)
        {
            var meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            return View(meeting);
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            var rsvps = meeting.RSVP.ToList();
            foreach (var rsvp in rsvps)
            {
                Db.RSVPs.Remove(rsvp);
            }
            Db.Meetings.Remove(meeting);

            Db.SaveChanges();

            return RedirectToAction("index");
        }

        private DateTime GetNextTimeIncrement(DateTime date)
        {
            if (date.Minute > 45)
            {
                return date.AddMinutes(60 - date.Minute);
            }

            if (date.Minute > 30)
            {
                return date.AddMinutes(45 - date.Minute);
            }

            if (date.Minute > 15)
            {
                return date.AddMinutes(30 - date.Minute);
            }

            return date.AddMinutes(15 - date.Minute);
        }
    }
}
