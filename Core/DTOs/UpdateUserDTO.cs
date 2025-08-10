using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class UpdateUserDTO
    {
        [Required] 
        public string Id { get; set; } = "";
        [Required, EmailAddress] 
        public string Email { get; set; } = "";
        [Required] 
        public string FirstName { get; set; } = "";
        [Required] 
        public string LastName { get; set; } = "";
        public List<string>? Roles { get; set; }
    }
}
