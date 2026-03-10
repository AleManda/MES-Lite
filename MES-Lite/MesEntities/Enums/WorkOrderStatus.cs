using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesEntities.Enums
{
    public enum WorkOrderStatus
    {
        Planned = 0,
        Released = 1,
        Running = 2,
        Completed = 3,
        Cancelled = 4
    }

}
