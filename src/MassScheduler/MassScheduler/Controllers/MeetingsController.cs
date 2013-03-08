using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    public class MeetingsController : ControllerBase
    {

        //
        // GET: /Meetings/

        public ActionResult Index()
        {
            var meetings = db.Meetings
                             .Where(x => x.EndDate >= DateTime.UtcNow)
                             .OrderBy(x => x.StartDate);


            return View(meetings);
        }

        //
        // GET: /Meetings/Details/5

        public ActionResult Details(int id)
        {
            var meeting = db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            return View(meeting);
        }

        //
        // GET: /Meetings/Create

        public ActionResult Create()
        {
            var meeting = new Meeting()
            {
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2),
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Contact = CurrentUser.EmailAddress
            };

            return View(meeting);
        }

        //
        // POST: /Meetings/Create

        [HttpPost]
        public ActionResult Create(Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                meeting.Creator = CurrentUser.Username;
                meeting.Created = DateTime.UtcNow;
                meeting.Modified = DateTime.UtcNow;

                db.Meetings.Add(meeting);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(meeting);
        }

        //
        // GET: /Meetings/Edit/5

        public ActionResult Edit(int id)
        {
            var meeting = db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            if (meeting.Creator != CurrentUser.Username)
            {
                return View("InvalidOwner");
            }

            return View(meeting);
        }

        //
        // POST: /Meetings/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Meetings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Meetings/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
