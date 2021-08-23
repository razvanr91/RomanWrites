using RomanWrites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomanWrites.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}
