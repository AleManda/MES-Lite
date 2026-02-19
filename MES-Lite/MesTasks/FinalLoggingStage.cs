using MES_Lite.MesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    internal class FinalLoggingStage: IFinalLoggingStage
    {
        public async Task RunAsync(ChannelReader<MaterialDefinition> input)
        {
            await foreach (var matdef in input.ReadAllAsync())
            {
                // Simulate final logging logic
                Console.WriteLine($"FINAL---Final Log: Material {matdef.Id} with Batch {matdef.BatchId} is ready for processing.");
            }
        }
    }
}
