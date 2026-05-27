namespace DotnetApi.Models
{
    public enum Role
    {
        Guest,
        User,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public Role Role { get; set; }

        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

    }
}
