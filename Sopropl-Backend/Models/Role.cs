namespace Sopropl_Backend.Models
{
    public class Role
    {
        public string Id { get; set; }
        public string MemberId { get; set; }
        public string Name { get; set; }
        public Member Member { get; set; }
    }
}