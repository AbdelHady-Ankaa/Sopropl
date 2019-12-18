using System;

namespace Sopropl_Backend.Models
{
    public class Photo
    {
        public Photo()
        {
            this.DateAdded = DateTime.Now;
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }
        public Project Project { get; set; }
        public Organization Organization { get; set; }
    }
}