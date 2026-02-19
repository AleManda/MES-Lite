using MES_Lite.MesEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    public interface IMaterialDefinitionGenerator
    {
        Task ProduceAsync(ChannelWriter<MaterialDefinition> writer, CancellationToken token = default);
    }

}
