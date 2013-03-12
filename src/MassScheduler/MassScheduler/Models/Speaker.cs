using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MassScheduler.Models
{
    public class Speaker
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(125, ErrorMessage = "Speaker name may not be longer than 125 characters.")]
        public string Name { get; set; }

        [StringLength(125, ErrorMessage = "Title may not be longer than 125 characters.")]
        public string Title { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(125, ErrorMessage = "Organization may not be longer than 125 characters.")]
        public string Organization { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        public string Photo { get; set; }
    }
}