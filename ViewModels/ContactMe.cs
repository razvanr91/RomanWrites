using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RomanWrites.ViewModels
{
    public class ContactMe
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

    }
}
