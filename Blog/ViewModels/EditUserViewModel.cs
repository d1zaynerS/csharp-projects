namespace Blog.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Phone
        {
            get => PhoneNumber;
            set => PhoneNumber = value;
        }
    }
}