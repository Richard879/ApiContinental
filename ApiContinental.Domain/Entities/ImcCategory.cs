namespace ApiContinental.Domain.Entities
{
    public class ImcCategory
    {
        public Guid Id { get; set; }
        public int MinAge { get; set; }    // inclusive
        public int MaxAge { get; set; }    // inclusive
        public decimal MinImc { get; set; } // inclusive
        public decimal MaxImc { get; set; } // exclusive
        public string Description { get; set; } = string.Empty;
    }
}
