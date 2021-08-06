using System.ComponentModel.DataAnnotations;

namespace Api.Testing.Dtos
{
    public class LoginCommand
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}