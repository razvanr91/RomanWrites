using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RomanWrites.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RomanWrites.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int BlogId { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Created On")]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated On")]
        public DateTime? Updated { get; set; }

        [Display(Name = "Post Image")]
        public byte[] ImageData { get; set; }

        public string ContentType { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string Slug { get; set; }

        [Display(Name = "Production Status")]
        public ProductionStatus ProductionStatus { get; set; }

        //Navigation Properties
        public virtual Blog Blog { get; set; }

        public virtual BlogUser Author { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
