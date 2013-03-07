using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassScheduler.Controllers
{
    public class MeetingsController : Controller
    {
        //
        // GET: /Meetings/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Meetings/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Meetings/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Meetings/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Meetings/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
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
