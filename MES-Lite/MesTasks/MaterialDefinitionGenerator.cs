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
    internal class MaterialDefinitionGenerator : IMaterialDefinitionGenerator
    {
        //______________________________________________________________________________________
        // Simulates generation of material definitions and writes them to the output channel
        public async Task ProduceAsync(ChannelWriter<MaterialDefinition> writer)
        {
            for (int i = 0; i < 1; i++)
            {
                var matdef = new MaterialDefinition
                {
                    Id = i,
                    Description = $"Component {i}",
                    Version = "1.0",
                    UoM = "pcs",
                    MaterialClassId = 1,
                    Source = "Supplier A",
                    MaterialTesSpecification = "Spec A",
                    Conformity = true,
                    Type = "Type A",
                    MinQty = 1,
                    MaxQty = 100
                };
                await writer.WriteAsync(matdef);
                await Task.Delay(Random.Shared.Next(10, 50));
            }
            writer.Complete();
        }

    }
}
