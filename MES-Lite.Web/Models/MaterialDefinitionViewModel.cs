using MES_Lite.MesEntities;
using MES_Lite.Web.Common;

namespace MES_Lite.Web.Models
{
    public class MaterialDefinitionViewModel
    {
        public PaginatedList<MaterialDefinition> MaterialDefsList { get; set; }

        public string? CurrentFilterId { get; set; }
        public string? CurrentFilterDescr { get; set; }
        public string? CurrentFilterVersion { get; set; }
        public string? CurrentFilterUoM { get; set; }
        public int CurrentFilterMatClassId { get; set; }
        public string? CurrentFilterSpec { get; set; }
        public string? CurrentFilterSupplier { get; set; }

    }
}
