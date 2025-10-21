namespace efcore.Models
{
    public class UserOutputDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }
}
