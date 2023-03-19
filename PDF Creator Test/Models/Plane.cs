using System.ComponentModel.DataAnnotations;

namespace PDF_Creator_Test.Models
{
    public class Plane
    {
        [Key]
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int MaxSpeed { get; set; }
        public int MaxAltitude { get; set; }
        public int MaxRange { get; set; }
        public int Wingspan { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public string Picture { get; set; }
    }
}
