namespace Sopropl_Backend.DTOs
{
    public class AccessToReturnDTO
    {
        public short Permission { get; set; }
        public ProjectToReturnDTO Project { get; set; }
        public TeamToReturnDTO Team { get; set; }

    }
}