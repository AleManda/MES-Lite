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
        [Required]
        public string MaterialId { get; set; } = default!; // Codice materiale
        [Required]
        public string Description { get; set; } = default!;
        [Required]
        public string Version { get; set; } = "1.0";
        [Required]
        public string UoM { get; set; } = "pcs";
        [Required]
        public int MaterialClassId { get; set; } 
        public string? Specification { get; set; } 
        public string? Supplier { get; set; }
        [Required]
        public bool Conformity { get; set; }

        // Logica MES-Lite
        [Required]
        public bool Critical { get; set; }
        [Required]
        public bool RequiresDoubleCheck { get; set; }

        // Relazione con MaterialLot
        public ICollection<MaterialLot> Lots { get; set; } = new List<MaterialLot>();
    }
}
