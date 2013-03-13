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

        [Authorize(Roles = @"hks-a\GreenweekAdmin")]
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

            ViewBag.Presenters = GetSpeakerList();

            return View(meeting);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Create(Meeting meeting, int[] presenters)
        {
            if (presenters != null)
            {
                var speakers = Db.Speakers.Where(s => presenters.Contains(s.Id)).ToList();
                meeting.UpdateSpeakers(speakers);
            }

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

            ViewBag.Presenters = GetSpeakerList();

            return View(meeting);
        }

        [Authorize(Roles = @"hks-a\GreenweekAdmin")]
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

            ViewBag.Presenters = GetSpeakerList(meeting.Speakers);

            return View(meeting);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Edit(int id, int[] presenters, FormCollection collection)
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

            meeting.Speakers.Clear();
            if (presenters != null)
            {
                var speakers = Db.Speakers.Where(s => presenters.Contains(s.Id)).ToList();
                meeting.UpdateSpeakers(speakers);
            }

            try
            {
                UpdateModel(meeting, null, null, new[] { "Speakers" });
                meeting.StartDate = TimeZone.CurrentTimeZone.ToUniversalTime(meeting.StartDate);
                meeting.EndDate = TimeZone.CurrentTimeZone.ToUniversalTime(meeting.EndDate);
                meeting.Modified = DateTime.UtcNow;

                Db.SaveChanges();

                return RedirectToAction("details", new {id = meeting.Id});
            }
            catch
            {
                ViewBag.Presenters = GetSpeakerList(meeting.Speakers);
                return View(meeting);
            }
        }

        [Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Delete(int id)
        {
            var meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            return View(meeting);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = @"hks-a\GreenweekAdmin")]
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


        /// <summary>
        /// Gets a list of speakers.
        /// </summary>
        /// <param name="selected">The selected speakers.</param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetSpeakerList(IEnumerable<Speaker> selected = null)
        {
            if (selected == null)
            {
                selected = new List<Speaker>();
            }

            return Db.Speakers
                      .ToList()
                      .OrderBy(x => x.Name)
                      .Select(x => new SelectListItem()
                      {
                          Selected = selected.Contains(x),
                          Text = x.Name,
                          Value = x.Id.ToString()
                      });
        }

        /// <summary>
        /// Gets the next future 15 minute increment
        /// <code>
        /// <example>
        ///     // Pseudo
        ///     var time = GetNextTimeIncrement(9:37pm)
        ///     // time should equal 9:45
        /// </example>
        /// </code>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static DateTime GetNextTimeIncrement(DateTime date)
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
