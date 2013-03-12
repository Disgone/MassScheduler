using System.Linq;
using System.Web.Mvc;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    [Authorize]
    public class RSVPController : BaseController
    {
        public ActionResult Register(int id)
        {
            var meeting = Db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            if (!meeting.IsUserAttending(CurrentUser.Username))
            {
                var reservation = new RSVP(CurrentUser.Username, CurrentUser.FullName, CurrentUser.EmailAddress);
                meeting.RSVP.Add(reservation);
                Db.SaveChanges();
            }

            return View(meeting);
        }

        public ActionResult Cancel(int id)
        {
            var meeting = Db.Meetings.Find(id);

            var rsvp = meeting.RSVP.SingleOrDefault(x => x.AttendeeUsername == CurrentUser.Username);
            if (rsvp != null)
            {
                Db.RSVPs.Remove(rsvp);
                Db.SaveChanges();
            }

            return View(meeting);
        }
    }
}
