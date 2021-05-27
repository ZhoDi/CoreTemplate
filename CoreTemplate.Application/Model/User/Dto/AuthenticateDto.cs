using System.ComponentModel.DataAnnotations;

namespace CoreTemplate.Application.Model.User.Dto
{
    public class AuthenticateDto
    {
        [Required]
        public string LoginId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
