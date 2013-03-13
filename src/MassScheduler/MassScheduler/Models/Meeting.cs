using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using MassScheduler.Validation;

namespace MassScheduler.Models
{
    public class Meeting
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(75, ErrorMessage = "Title may not be longer than 75 characters.")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descritpion { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "End date is required.")]
        [DateIsBefore("StartDate", "End date must be after start date.")]
        public DateTime EndDate { get; set; }

        [StringLength(125, ErrorMessage = "Sponsor name may not be longer than 125 characters.")]
        public string Sponsor { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(256, ErrorMessage = "Location may not be longer than 256 characters.")]
        public string Location { get; set; }

        [Display(Name = "Contact Information")]
        public string Contact { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Creator")]
        public string Creator { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime Created { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime Modified { get; set; }

        public virtual ICollection<RSVP> RSVP { get; set; }

        [Display(Name = "Speaker(s)")]
        public virtual ICollection<Speaker> Speakers { get; set; }

        [NotMapped]
        public DateTime LocalStartDate
        {
            get { return StartDate.ToLocalTime(); }
        }

        [NotMapped]
        public DateTime LocalEndDate
        {
            get { return EndDate.ToLocalTime(); }
        }

        public Meeting()
        {
            Created = DateTime.UtcNow;
            Modified = DateTime.UtcNow;
        }

        public bool IsUserAttending(string username)
        {
            return RSVP.Any(e => e.AttendeeUsername == username);
        }

        public void AddSpeaker(Speaker speaker)
        {
            if(!Speakers.Contains(speaker))
                Speakers.Add(speaker);
        }

        public void UpdateSpeakers(IEnumerable<Speaker> speakers)
        {
            if (Speakers == null)
            {
                Speakers = new Collection<Speaker>();
            }

            Speakers.Clear();
            Speakers = speakers.ToList();
        }

        public bool HasEnded()
        {
            return DateTime.UtcNow >= EndDate;
        }
    }
}