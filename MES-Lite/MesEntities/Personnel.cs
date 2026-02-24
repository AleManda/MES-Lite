using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities
{
    // Membro del personale che può essere assegnato a compiti, turni o attrezzature.
    // Corrisponde al concetto di "Personnel" in ISA-95.

    public class Personnel
    {
        public int Id { get; set; }

        public string PersonId { get; set; } = default!;      // Identificatore di dominio
        public string Name { get; set; } = default!;
        public string Role { get; set; } = default!;          // Operatore, QA, Supervisor, ecc.
        public string? Qualification { get; set; }
    }

}
