namespace Core.DTOs
{
    public class ContactDTO
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
