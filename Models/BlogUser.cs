using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RomanWrites.Models
{
    public class BlogUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string DisplayName { get; set; }

        public byte[] ImageData { get; set; }

        public string ContentType { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string LinkedIdUrl { get; set; }

        public string GithubUrl { get; set; }

        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
