namespace UsersAPI.Controllers
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
    }
}