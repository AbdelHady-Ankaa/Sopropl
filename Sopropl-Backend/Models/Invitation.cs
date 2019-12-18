namespace Sopropl_Backend.Models
{
    public class Invitation
    {
        public string Id { get; set; }
        public string NormalizedOrganizationName { get; set; }
        public string NormalizedUserName { get; set; }
        public Organization Organization { get; set; }
        public User User { get; set; }
    }
}