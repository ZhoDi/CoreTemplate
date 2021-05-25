using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemplate.Domain.APIModel.User
{
    public class AuthenticateModel
    {
        [Required]
        public string LoginId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
