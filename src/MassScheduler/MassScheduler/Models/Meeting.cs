using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MassScheduler.Models
{
    public class Meeting
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(75, ErrorMessage = "Title may not be longer than 75 characters.")]
        public string Title { get; set; }

        [StringLength(256, ErrorMessage = "Description cannot be more than 256 characters.")]
        public string Descritpion { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Speaker(s)")]
        public string Speaker { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(256, ErrorMessage = "Location may not be longer than 256 characters.")]
        public string Location { get; set; }

        [Display(Name = "Contact Information")]
        public string Contact { get; set; }

        [Display(Name = "Creator")]
        public string Creator { get; set; }

        public virtual ICollection<RSVP> RSVP { get; set; } 

        public bool IsUserAttending(string username)
        {
            return RSVP.Any(e => e.AttendeeUsername == username);
        }
    }
}