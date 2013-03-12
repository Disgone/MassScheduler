using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MassScheduler.Helpers;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    public class SpeakersController : BaseController
    {
        private readonly UploadHelper _uploadHelper = new UploadHelper();
        //
        // GET: /Speakers/

        public ActionResult Index()
        {
            var speakers = Db.Speakers.OrderBy(x => x.Name);

            return View(speakers);
        }

        //
        // GET: /Speakers/Details/5

        public ActionResult Details(int id)
        {
            var speaker = Db.Speakers.Find(id);

            if (speaker == null)
            {
                return HttpNotFound();
            }

            return View(speaker);
        }

        //
        // GET: /Speakers/Create

        public ActionResult Create()
        {
            var speaker = new Speaker();

            return View(speaker);
        }

        //
        // POST: /Speakers/Create

        [HttpPost]
        public ActionResult Create(Speaker speaker, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                var image = UploadHelper.HandleUploadedImage(Photo);
                speaker.Photo = image;

                Db.Speakers.Add(speaker);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(speaker);
        }

        //
        // GET: /Speakers/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Speakers/Edit/5

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
        // GET: /Speakers/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Speakers/Delete/5

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
