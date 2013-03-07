using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MassScheduler.Models
{
    public class RSVP
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [ForeignKey("Meeting")]
        public int MeetingId { get; set; }

        [Required]
        public string AttendeeName { get; set; }

        [Required]
        public string AttendeeUsername { get; set; }

        [Required]
        public string AttendeeEmail { get; set; }

        public virtual Meeting Meeting { get; set; }

        public RSVP()
        {
        }

        public RSVP(string username, string name, string email)
        {
            AttendeeUsername = username;
            AttendeeName = name;
            AttendeeEmail = email;
        }
    }
}