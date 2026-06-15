namespace Application.Users
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateOnly DateRegistration { get; set; }
    }
}
