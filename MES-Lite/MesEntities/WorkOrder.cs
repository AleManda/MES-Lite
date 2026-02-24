using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities
{
    // Rappresenta un ordine di lavoro (Work Order) che specifica cosa deve essere prodotto,
    // quando e con quali materiali.Corrisponde al concetto di "Production Order"(o Job order nel Work Model) in ISA-95,
    // ma con un nome più generico che può essere adattato a diversi contesti produttivi.

    public class WorkOrder
    {
        public int Id { get; set; }

        [Required]
        public string WorkOrderId { get; set; } = default!;   // Identificatore di dominio
        [Required]
        public string Description { get; set; } = default!;
        [Required]
        public DateTime ScheduledStart { get; set; }
        [Required]
        public DateTime ScheduledEnd { get; set; }
        [Required]
        public string Status { get; set; } = "Planned";       // Planned, Released, Running, Completed, Cancelled

        // Relazioni ISA-95
        public ICollection<MaterialRequirement> MaterialRequirements { get; set; } = new List<MaterialRequirement>();
    }

}
