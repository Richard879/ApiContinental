namespace ApiContinental.Application.DTOs
{
    public class ImcRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal WeightKg { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int HeightCm { get; set; }
        public bool Persist { get; set; } = true; // opcional:
    }
}