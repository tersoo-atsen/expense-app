using System.ComponentModel.DataAnnotations;

namespace Expenses.Api.Models.Authenticate
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}