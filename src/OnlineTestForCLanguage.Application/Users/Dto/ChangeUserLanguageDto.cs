using System.ComponentModel.DataAnnotations;

namespace OnlineTestForCLanguage.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}