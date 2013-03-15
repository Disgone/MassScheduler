using System;
using DDay.iCal;
using MassScheduler.Models;

namespace MassScheduler.Helpers
{
    public static class CalendarHelpers
    {
        public static Event MeetingToEvent(Meeting meeting, iCalendar iCal)
        {
            var eventLink = "http://greenweek/meetings/details/" + meeting.Id;
            var evt = iCal.Create<Event>();
            evt.Start = new iCalDateTime(meeting.StartDate.ToString(@"yyyyMMdd\THHmmss\Z"));
            evt.End = new iCalDateTime(meeting.EndDate.ToString(@"yyyyMMdd\THHmmss\Z"));
            evt.Location = meeting.Location;
            evt.Summary = meeting.Title;
            evt.Url = new Uri(eventLink);
            evt.Description = String.Format("{0}\nSponsored by: {1}\nLync Meeting Url: \n{2}", meeting.Descritpion, meeting.Sponsor, meeting.MeetingUrl);
            return evt;
        }
    }
}