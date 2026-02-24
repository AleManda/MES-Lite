using MES_Lite.MesEntities;
using MES_Lite.Web.Common;

namespace MES_Lite.Web.Models
{
    public class MaterialRequirementViewModel
    {
        public PaginatedList<MaterialRequirement> MaterialRequirementList { get; set; }

        public int CurrentFilterWorkOrderId { get; set; }
        public int CurrentFilterMatDefId { get; set; }
        public decimal CurrentFilterQuantity { get; set; }
    }
}
