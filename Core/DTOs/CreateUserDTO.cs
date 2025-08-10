using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class CreateUserDTO
    {
        [Required, EmailAddress] 
        public string Email { get; set; } = "";
        [Required] 
        public string FirstName { get; set; } = "";
        [Required] 
        public string LastName { get; set; } = "";
        [Required, DataType(DataType.Password)] 
        public string Password { get; set; } = "";
        public required string Role { get; set; }
    }
}
