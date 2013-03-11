using System.Linq;
using System.Web.Mvc;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    [Authorize]
    public class RSVPController : ControllerBase
    {
        public ActionResult Register(int id)
        {
            var meeting = db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            if (!meeting.IsUserAttending(CurrentUser.Username))
            {
                var reservation = new RSVP(CurrentUser.Username, CurrentUser.FullName, CurrentUser.EmailAddress);
                meeting.RSVP.Add(reservation);
                db.SaveChanges();
            }

            return View(meeting);
        }

        public ActionResult Cancel(int id)
        {
            var meeting = db.Meetings.Find(id);

            var rsvp = meeting.RSVP.SingleOrDefault(x => x.AttendeeUsername == CurrentUser.Username);
            if (rsvp != null)
            {
                db.RSVPs.Remove(rsvp);
                db.SaveChanges();
            }

            return View(meeting);
        }
    }
}
