using System;
using System.ComponentModel;

namespace RomanWrites.Enums
{
    public enum ModerationType
    {
        [Description("Political Propaganda")]
        Political,

        [Description("Offensive Language")]
        Language,

        [Description("Drug and/or Alcohol References")]
        Drugs,

        [Description("Threatening Speech")]
        Threatening,

        [Description("Sexual Content")]
        Sexual,

        [Description("Hate Speech")]
        HateSpeech,

        [Description("Targeted Shaming")]
        Shaming
    }
}
