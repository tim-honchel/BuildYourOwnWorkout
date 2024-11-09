namespace Workouts.Entities.Database
{
    public class User
    {
        public long Id { get; set; }
        public string NameIdentifierClaim { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime AccountCreated { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
