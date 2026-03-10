using MES_Lite.MesEntities;

namespace MES_Lite.Web.Models
{
    public class MaterialLotModel
    {
        public int Id { get; set; }

        public string LotId { get; set; } = default!;   // LOT-20250219-0001
        public int MaterialDefinitionId { get; set; }   // Foreign key per MaterialDefinition
 
        public decimal Quantity { get; set; } = 1;

        public string Status { get; set; } = "Received";   // Received, Validated, Blocked, Released

        public string? Supplier { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }

        public MaterialDefinitionModel MaterialDefinition { get; set; } = default!;
    }
}
