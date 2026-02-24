using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities
{
    // Rappresenta un'attrezzatura o macchina utilizzata nel processo produttivo. 
    //Corrisponde al concetto di "Equipment" in ISA-95.

    public class Equipment
    {
        public int Id { get; set; }

        public string EquipmentId { get; set; } = default!;   // Identificatore di dominio
        public string Description { get; set; } = default!;
        public string EquipmentClassId { get; set; } = default!;

        public string? Location { get; set; }
        public string Status { get; set; } = "Available";     // Available, InUse, Maintenance, Down
    }

}
