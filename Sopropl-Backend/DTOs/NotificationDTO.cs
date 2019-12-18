using System;

namespace Sopropl_Backend.DTOs
{
    public class NotificationDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime sentDate { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public string UserId { get; set; }
        public string Data { get; set; }
        public short Type { get; set; }
    }
}