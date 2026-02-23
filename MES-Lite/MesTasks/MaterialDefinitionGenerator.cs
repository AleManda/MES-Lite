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
        public async Task ProduceAsync(ChannelWriter<MaterialDefinition> writer, CancellationToken token = default)
        {
            string sourcestring = "Supplier A";
            bool iscritical = false;


            for (int i = 0; i < 25; i++)
            {
                token.ThrowIfCancellationRequested();

                if (i % 10 == 0)
                { sourcestring = string.Empty; }

                if (i % 12 == 0)
                { iscritical = true; }

                var matdef = new MaterialDefinition
                {
                    //id(IDENTITY) autogenerato da EF Core, non lo setto
                    MaterialId = i.ToString(),
                    Description = $"Component {i}",
                    Version = "1.0",
                    UoM = "pcs",
                    MaterialClassId = 1,
                    Supplier = sourcestring,
                    Specification = "Spec A",
                    Conformity = true,
                    Critical = iscritical
                };

                sourcestring = "Supplier A";
                iscritical = false;
                await writer.WriteAsync(matdef);
                await Task.Delay(Random.Shared.Next(10, 50));
            }
            writer.Complete();
        }

    }
}
