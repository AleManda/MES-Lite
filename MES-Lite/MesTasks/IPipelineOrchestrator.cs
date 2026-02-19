using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    public interface IPipelineOrchestrator
    {
        Task RunAsync(CancellationToken token);
    }
}
