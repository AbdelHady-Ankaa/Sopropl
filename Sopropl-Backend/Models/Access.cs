namespace Sopropl_Backend.Models
{
    public class Access
    {
        public string Id { get; set; }
        public string NormalizedOrganizationName { get; set; }
        public string NormalizedTeamName { get; set; }
        public string NormalizedProjectName { get; set; }
        public short Permission { get; set; }
        public Project Project { get; set; }
        public Team Team { get; set; }
    }
}