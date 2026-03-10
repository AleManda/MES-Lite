namespace MES_Lite.Web.Models
{
    public class PersonnelModel
    {
        public int Id { get; set; }

        public string PersonId { get; set; } = default!;      // Identificatore di dominio
        public string Name { get; set; } = default!;
        public string Role { get; set; } = default!;          // Operatore, QA, Supervisor, ecc.
        public string? Qualification { get; set; }
    }
}
