namespace Sopropl_Backend.Models
{
    public class Resource
    {
        public string Id { get; set; }
        public string ActivityName { get; set; }
        public string NormalizedTeamName { get; set; }
        public string NormalizedOrganizationName { get; set; }
        public string NormalizedProjectName { get; set; }
        public Activity Activity { get; set; }
        public Team Team { get; set; }
    }
}