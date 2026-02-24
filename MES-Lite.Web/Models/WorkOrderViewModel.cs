using MES_Lite.MesEntities;
using MES_Lite.Web.Common;

namespace MES_Lite.Web.Models
{
    public class WorkOrderViewModel
    {
        public PaginatedList<WorkOrder> WorkOrderList { get; set; }

        public string CurrentFilterWorkOrderId { get; set; } = string.Empty;
        public string CurrentFilterDescr { get; set; } = string.Empty;

        public string CurrentFilterSchedStart { get; set; } = string.Empty;

        public string CurrentFilterSchedEnd { get; set; } = string.Empty;
        public string CurrentFilterStatus { get; set; } = string.Empty;

    }
}
