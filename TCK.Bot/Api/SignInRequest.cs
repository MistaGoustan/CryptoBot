using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCK.Bot.Api
{
    public class SignInRequest
    {
        [DefaultValue("TEST1234")]
        [Required(ErrorMessage = $"{nameof(Password)} is required")]
        public String Password { get; set; } = default!;

        [DefaultValue("admin")]
        [Required(ErrorMessage = $"{nameof(Username)} is required")]
        public String Username { get; set; } = default!;
    }
}
