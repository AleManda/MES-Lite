namespace MES_Lite.Web.Models
{
    public class MaterialDefinitionModel
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

        public bool Critical { get; set; }

        public bool RequiresDoubleCheck { get; set; }
    }
}
