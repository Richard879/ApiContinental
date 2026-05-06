using System.ComponentModel.DataAnnotations;

namespace ApiContinental.Domain.Entities
{
    public class ImcRecord
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal WeightKg { get; set; }
        public int HeightCm { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public decimal ImcValue { get; set; }
        public string ImcDescription { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}