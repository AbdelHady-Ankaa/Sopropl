using System;

namespace Sopropl_Backend.DTOs
{
    public class UserToReturnDTO
    {
        public string UserName { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Bio { get; set; }
        public string Name { get; set; }

        public PhotoToReturnDTO Photo { get; set; }
    }
}