using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;
using MassScheduler.Models;

namespace MassScheduler.Helpers
{
    class iCalResult : FileResult
    {
        public List<Meeting> Meetings { get; set; }

        public iCalResult(string filename)
            : base("text/calendar")
        {
            FileDownloadName = filename;
        }

        public iCalResult(List<Meeting> meetings, string filename)
            : this(filename)
        {
            Meetings = meetings;
        }

        public iCalResult(Meeting meeting, string filename)
            : this(filename)
        {
            Meetings = new List<Meeting> { meeting };
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            var iCal = new iCalendar();
            foreach (var meeting in Meetings)
            {
                try
                {
                    Event e = CalendarHelpers.MeetingToEvent(meeting, iCal);
                }
                catch (ArgumentOutOfRangeException)
                {
                    //Swallow folks that have Meetings in 9999. 
                }
            }

            var serializer = new iCalendarSerializer(iCal);
            var result = serializer.SerializeToString();
            response.ContentEncoding = Encoding.UTF8;
            response.Write(result);
        }

    }
}