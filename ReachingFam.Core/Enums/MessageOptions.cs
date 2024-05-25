using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Enums
{
    public enum MessageOptions
    {
        Voicemail = 1,
        [Display(Name = "Text Message")]
        TextMessage = 2,
        Neither = 3,
    }
}
