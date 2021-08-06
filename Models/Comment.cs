using Microsoft.AspNetCore.Identity;
using RomanWrites.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RomanWrites.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string AuthorId { get; set; }

        public string ModeratorId { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        [Display(Name = "Comment")]
        public string Body { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Updated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Moderated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Deleted { get; set; }

        [StringLength(250, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        [Display(Name = "Moderated")]
        public string ModeratedBody { get; set; }

        public ModerationType ModerationType { get; set; }

        //Navigation Properties
        public virtual Post Post { get; set; }

        public virtual IdentityUser Author { get; set; }

        public virtual IdentityUser Moderator { get; set; }


    }
}
