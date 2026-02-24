using MES_Lite.MesEntities;
using MES_Lite.Web.Common;

namespace MES_Lite.Web.Models
{
    public class EquipmentViewModel
    {
        public PaginatedList<Equipment> EquipmentList { get; set; }

        public string? CurrentFilterId { get; set; } = string.Empty;
        public string? CurrentFilterDescr { get; set; } = string.Empty;
        public string? CurrentFilterClassId { get; set; } = string.Empty;
        public string? CurrentFilterLocation { get; set; } = string.Empty;
        public string? CurrentFilterStatus { get; set; } = string.Empty;
    }
}
