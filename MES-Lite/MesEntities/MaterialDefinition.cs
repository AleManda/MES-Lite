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
        public string? Description { get; set; }
        public string Version { get; set; }
        public string UoM { get; set; }
        public int MaterialClassId { get; set; }
        public string? Source { get; set; }
        public string MaterialTesSpecification { get; set; }
        public bool Conformity { get; set; }

        [Required]
        public string Type { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }

        public string? BatchId { get; set; }

        public bool RequiresDoubleCheck { get; set; } = false;

        //public bool Validated { get; set; } = false;

        public bool IsCritical { get; set; } = false;
    }
}
