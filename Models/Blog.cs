using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RomanWrites.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be between {2} and {1} characters long", MinimumLength = 2)]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Updated { get; set; }

        [Display(Name = "Blog Image")]
        public byte[] ImageData { get; set; }

        public string ContentType { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
