using MES_Lite.MesEntities;
using MES_Lite.Web.Common;

namespace MES_Lite.Web.Models
{
    public class MaterialLotViewModel
    {
        public PaginatedList<MaterialLot> MaterialLotsList { get; set; }

        public string? CurrentFilterId { get; set; } = string.Empty;
        public int CurrentFilterMatdefId { get; set; }
        public int CurrentFilterQuantity { get; set; }
        public string? CurrentFilterStatus { get; set; } = string.Empty;
        public string? CurrentFilterSupplier { get; set; } = string.Empty;
        public string? CurrentFilterCreatedAt { get; set; } = string.Empty;
        public string? CurrentFilterExpiration { get; set; } = string.Empty;
    }
}
