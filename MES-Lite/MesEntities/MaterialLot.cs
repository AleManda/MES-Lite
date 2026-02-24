using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities
{

    public class MaterialLot
    {
        public int Id { get; set; }
        [Required]
        public string LotId { get; set; } = default!;   // LOT-20250219-0001
        [Required]
        public int MaterialDefinitionId { get; set; }   // Foreign key per MaterialDefinition
        [Required]
        public MaterialDefinition MaterialDefinition { get; set; } = default!;
        [Required]
        public decimal Quantity { get; set; } = 1;
        [Required]
        public string Status { get; set; } = "Received";   // Received, Validated, Blocked, Released

        public string? Supplier { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }
    }


}
