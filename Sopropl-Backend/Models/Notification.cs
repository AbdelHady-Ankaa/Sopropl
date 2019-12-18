using System;

namespace Sopropl_Backend.Models
{
    public class NotificationType
    {
        public const short NORMAL = 0;
        public const short INVITATION = 1;
    }
    public class Notification
    {
        public Notification()
        {
            sentDate = DateTime.Now;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime sentDate { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public string NormalizedUserName { get; set; }
        public string Data { get; set; }
        public short Type { get; set; }
        public User User { get; set; }
    }
}