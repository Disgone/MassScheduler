using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MassScheduler.Helpers;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    public class SpeakersController : BaseController
    {

        public ActionResult Index()
        {
            var speakers = Db.Speakers.OrderBy(x => x.Name).ToList();

            return View(speakers);
        }

        public ActionResult Details(int id)
        {
            var speaker = Db.Speakers.Find(id);

            if (speaker == null)
            {
                return HttpNotFound();
            }

            return View(speaker);
        }

        [Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Create()
        {
            var speaker = new Speaker();

            return View(speaker);
        }

        [HttpPost, Authorize(Roles = @"hks-a\GreenweekAdmin")]
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

        [Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Edit(int id)
        {
            var speaker = Db.Speakers.Find(id);

            if (speaker == null)
            {
                return HttpNotFound();
            }

            return View(speaker);
        }

        [HttpPost, Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Edit(int id, Speaker posted, HttpPostedFileBase photo)
        {
            Speaker speaker = Db.Speakers.Find(id);

            if (speaker == null)
            {
                return HttpNotFound();
            }

            try
            {
                UpdateModel(speaker, new[] {"Name", "Title", "Email", "Organization", "Bio"});
                if (photo != null)
                {
                    var image = UploadHelper.HandleUploadedImage(photo);
                    speaker.Photo = image;
                }

                Db.SaveChanges();

                return RedirectToAction("details", new { id = id });
            }
            catch
            {
                return View(posted);
            }
        }


        [Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Delete(int id)
        {
            var speaker = Db.Speakers.Find(id);

            if (speaker == null)
            {
                return HttpNotFound();
            }

            return View(speaker);
        }

        [HttpPost, Authorize(Roles = @"hks-a\GreenweekAdmin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var speaker = Db.Speakers.Find(id);

                if (speaker == null)
                {
                    return HttpNotFound();
                }

                Db.Speakers.Remove(speaker);
                Db.SaveChanges();

                return RedirectToAction("index");
            }
            catch
            {
                return RedirectToAction("index");
            }
        }

        [ChildActionOnly]
        public ActionResult SpeakerList(int? id)
        {
            var speakers = Db.Speakers.OrderBy(x => x.Name);
            ViewBag.CurrentSpeaker = id;

            return PartialView(speakers);
        }
    }
}
