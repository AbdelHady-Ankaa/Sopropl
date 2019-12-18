using System;

namespace Sopropl_Backend.DTOs
{
    public class GraphToReturnDTO
    {
        public ActivityToReturnDTO StartNode { get; set; }
        public string EarlyStart { get; set; }
    }
}