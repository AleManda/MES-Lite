using MES_Lite.MesEntities;
using MES_Lite.Web.Common;

namespace MES_Lite.Web.Models
{
    public class PersonnelViewModel
    {
        public PaginatedList<Personnel> PersonnelList { get; set; }

        public string CurrentFilterPersonId { get; set; } = string.Empty;
        public string CurrentFilterName { get; set; } = string.Empty;
        public string CurrentFilterRole { get; set; } = string.Empty;
        public string CurrentFilterQualification { get; set; } = string.Empty;

    }
}
