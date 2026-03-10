namespace MES_Lite.Web.Models
{
    public class EquipmentModel
    {
        public int Id { get; set; }

        public string EquipmentId { get; set; } = default!;   // Identificatore di dominio
        public string Description { get; set; } = default!;
        public string EquipmentClassId { get; set; } = default!;

        public string? Location { get; set; }
        public string Status { get; set; } = "Available";     // Available, InUse, Maintenance, Down
    }
}
