using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MassScheduler.Helpers;
using MassScheduler.Models;

namespace MassScheduler.Controllers
{
    [AllowAnonymous]
    public class ServicesController : ControllerBase
    {
        //
        // GET: /Services/
        public ActionResult iCal(int id)
        {
            var meeting = db.Meetings.Find(id);

            if (meeting == null)
            {
                return HttpNotFound();
            }

            return new iCalResult(meeting, "Event.ics");
        }

    }
}
