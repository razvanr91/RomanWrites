using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RomanWrites.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string Text { get; set; }

        public virtual Post Post { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual IdentityUser Author { get; set; }
    }
}
