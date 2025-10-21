namespace efcore.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public UserInputDto ToInputDto()
        {
            return new UserInputDto
            {
                Username = this.Username,
                Password = this.PasswordHash
            };
        }
        public UserOutputDto ToDto()
        {
            return new UserOutputDto
            {
                UserId = this.UserId,
                Username = this.Username,
                Roles = this.Roles
            };
        }
    }
}