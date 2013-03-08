using System;
using DDay.iCal;
using MassScheduler.Models;

namespace MassScheduler.Helpers
{
    public static class CalendarHelpers
    {
        public static Event MeetingToEvent(Meeting meeting, iCalendar iCal)
        {
            var eventLink = "http://nrddnr.com/" + meeting.Id;
            var evt = iCal.Create<Event>();
            evt.Start = new iCalDateTime(meeting.StartDate.ToString(@"yyyyMMdd\THHmmss\Z"));
            evt.End = new iCalDateTime(meeting.EndDate.ToString(@"yyyyMMdd\THHmmss\Z"));
            evt.Location = meeting.Location;
            evt.Summary = meeting.Title;
            evt.Url = new Uri(eventLink);
            evt.Description = String.Format("{0} Presented by: {1}", meeting.Descritpion, meeting.Speaker);
            return evt;
        }
    }
}