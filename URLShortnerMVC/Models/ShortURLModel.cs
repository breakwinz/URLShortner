using System.ComponentModel.DataAnnotations;
// ReSharper disable InconsistentNaming

namespace URLShortnerMVC.Models
{
    public class ShortUrlModel
    {
        [Display(Name = "Enter your long URL")]
        [Required(ErrorMessage = "You must enter a long URL!")]
        [RegularExpression("^[(http(s)?):\\/\\/(www\\.)?a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)$",ErrorMessage = "Not a valid URL!")]
        public string originalURL { get; set; }

        [Display(Name = "Customize your link (Optional)")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Custom shortlinks can only be between 4 and 10 characters")]
        [RegularExpression(@"^[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Use letters and numbers only")]
        public string shortURL { get; set; }
    }
}
