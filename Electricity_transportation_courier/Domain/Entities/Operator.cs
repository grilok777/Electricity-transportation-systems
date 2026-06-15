
namespace Domain.Entities
{
    public class Operator
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AccessLevel { get; set; } = string.Empty;
    }
}
