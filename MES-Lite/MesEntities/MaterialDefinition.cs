using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities
{
    public record MaterialDefinition
    {
        public int Id { get; set; }
        public string MaterialId { get; set; } = default!; // Codice materiale
        public string Description { get; set; } = default!; 
        public string Version { get; set; } = "1.0"; 
        public string UoM { get; set; } = "pcs"; 
        public int MaterialClassId { get; set; } 
        public string? Specification { get; set; } 
        public string? Supplier { get; set; } 
        public bool Conformity { get; set; }

        // Logica MES-Lite
        public bool Critical { get; set; } 
        public bool RequiresDoubleCheck { get; set; }

        // Relazione con MaterialLot
        public ICollection<MaterialLot> Lots { get; set; } = new List<MaterialLot>();
    }
}
