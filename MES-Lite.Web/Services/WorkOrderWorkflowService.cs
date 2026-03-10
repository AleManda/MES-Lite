using MES_Lite.MesEntities.Enums;
using System;
using MES_Lite.Data;    

namespace MES_Lite.Web.Services
{
    public class WorkOrderWorkflowService : IWorkOrderWorkflowService
    {
        private readonly MesLiteDbContext _db;

        public WorkOrderWorkflowService(MesLiteDbContext db)
        {
            _db = db;
        }

        public bool CanTransition(WorkOrderStatus from, WorkOrderStatus to)
        {
            return (from, to) switch
            {
                (WorkOrderStatus.Planned, WorkOrderStatus.Released) => true,
                (WorkOrderStatus.Planned, WorkOrderStatus.Cancelled) => true,

                (WorkOrderStatus.Released, WorkOrderStatus.Running) => true,
                (WorkOrderStatus.Released, WorkOrderStatus.Cancelled) => true,

                (WorkOrderStatus.Running, WorkOrderStatus.Completed) => true,

                _ => false
            };
        }

        public async Task<bool> TryChangeStatusAsync(int workOrderId, WorkOrderStatus newStatus)
        {
            var wo = await _db.WorkOrders.FindAsync(workOrderId);
            if (wo == null) return false;

            if (!CanTransition(wo.Status, newStatus))
                return false;

            wo.Status = newStatus;
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
