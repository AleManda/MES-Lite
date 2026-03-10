namespace MES_Lite.Web.Models
{
    public class WorkOrderModel
    {
        public int Id { get; set; }

        public string WorkOrderId { get; set; } = default!;   // Identificatore di dominio

        public string Description { get; set; } = default!;

        public DateTime ScheduledStart { get; set; }

        public DateTime ScheduledEnd { get; set; }

        public string Status { get; set; } = "Planned";       // Planned, Released, Running, Completed, Cancelled
    }
}
