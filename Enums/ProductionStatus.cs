using System;
using System.ComponentModel.DataAnnotations;

namespace RomanWrites.Enums
{
    public enum ProductionStatus
    {

        Incomplete,

        [Display(Name ="Production Ready")]
        ProductionReady,

        [Display(Name = "Preview Ready")]
        PreviewReady
    }
}
