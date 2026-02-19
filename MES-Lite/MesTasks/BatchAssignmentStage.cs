using MES_Lite.MesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    internal class BatchAssignmentStage:IBatchAssignmentStage
    {
        public async Task RunAsync(ChannelReader<MaterialDefinition> input, ChannelWriter<MaterialDefinition> output)
        {

            await foreach (var matdef in input.ReadAllAsync())
            {

                // Simulate batch assignment logic
                matdef.BatchId = $"Batch-{matdef.Id % 10}"; // Simple batch assignment based on ID
                Console.WriteLine($"BATCH---Material {matdef.Id} assigned to {matdef.BatchId}.");
                await output.WriteAsync(matdef);
            }
            output.Complete();
        }
    }
}
