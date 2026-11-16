namespace UserNotesApp.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public string SenderId { get; set; } = string.Empty;
        public User? Sender { get; set; }

        public string ReceiverId { get; set; } = string.Empty;
        public User? Receiver { get; set; }

        public bool IsAccepted { get; set; } = false;   
    }
}