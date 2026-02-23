using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities
{

    public class MaterialLot
    {
        public int Id { get; set; }

        public string LotId { get; set; } = default!;   // LOT-20250219-0001
        public int MaterialDefinitionId { get; set; }

        public MaterialDefinition MaterialDefinition { get; set; } = default!;

        public decimal Quantity { get; set; } = 1;
        public string Status { get; set; } = "Received";   // Received, Validated, Blocked, Released

        public string? Supplier { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }
    }


}
