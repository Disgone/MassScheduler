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
            evt.Start = new iCalDateTime(meeting.StartDate);
            evt.Duration = meeting.EndDate.Subtract(meeting.StartDate);
            evt.Location = meeting.Location;
            evt.Summary = String.Format("{0} Presented by: {1}", meeting.Descritpion, meeting.Speaker);
            evt.Url = new Uri(eventLink);
            evt.Description = eventLink;
            return evt;
        }
    }
}