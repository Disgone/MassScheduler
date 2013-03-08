using System.Data.Entity;

namespace MassScheduler.Models
{
    public class MeetingContext : DbContext
    {
        public IDbSet<Meeting> Meetings { get; set; }
        public IDbSet<RSVP> RSVPs { get; set; }
    }
}