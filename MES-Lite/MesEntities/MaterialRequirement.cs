using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities
{

    // Rappresenta un requisito di materiale per un ordine di lavoro specifico. Indica quale materiale è necessario,
    // in quale quantità e per quale ordine di lavoro. Corrisponde al concetto di "Material Requirement" in ISA-95,

    public class MaterialRequirement
    {
        public int Id { get; set; }

        public int WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; } = default!;

        public int MaterialDefinitionId { get; set; }
        public MaterialDefinition MaterialDefinition { get; set; } = default!;

        public decimal RequiredQuantity { get; set; }
    }

}
