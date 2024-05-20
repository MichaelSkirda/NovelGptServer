namespace DAL.Models
{
    public class MessageDto
    {
        public int Id { get; set; }
        public bool IsPlayerMessage { get; set; }
        public string Text { get; set; } = "";
    }
}
