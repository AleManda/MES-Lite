using MES_Lite.MesEntities;

namespace MES_Lite.Web.Models
{
    public class MaterialRequirementModel
    {
        public int Id { get; set; }

        public int WorkOrderId { get; set; }

        public int MaterialDefinitionId { get; set; }

        public decimal RequiredQuantity { get; set; }

        public MaterialDefinitionModel MaterialDefinition { get; set; } = default!;

        public WorkOrderModel WorkOrder { get; set; } = default!;
    }
}
