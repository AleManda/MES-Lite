using MES_Lite.MesEntities.Enums;

namespace MES_Lite.Web.Services
{
    public interface IWorkOrderWorkflowService
    {
        bool CanTransition(WorkOrderStatus from, WorkOrderStatus to);
        Task<bool> TryChangeStatusAsync(int workOrderId, WorkOrderStatus newStatus);
    }

}
