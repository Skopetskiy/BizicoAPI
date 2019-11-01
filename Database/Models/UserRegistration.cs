using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class UserRegistration
    {
        public string Login { get; set; }
        public string Password { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
    }
}
