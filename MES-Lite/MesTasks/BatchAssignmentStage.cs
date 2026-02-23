using MES_Lite.MesEntities;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BatchAssignmentStage> _logger;

        public BatchAssignmentStage(ILogger<BatchAssignmentStage> logger)
        {
            _logger = logger;
        }

        //______________________________________________________________________________________
        // Assigns validated materials to new lots,add lots in material and pass materials
        // to the output channel
        public async Task RunAsync(ChannelReader<MaterialDefinition> input, ChannelWriter<MaterialDefinition> output, CancellationToken token = default)
        {           
            await foreach (var matdef in input.ReadAllAsync())
            {
                token.ThrowIfCancellationRequested();

                // Genera un lotto
                var lotId = $"LOT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}"; 
                matdef.Lots.Add(new MaterialLot 
                { 
                    LotId = lotId, 
                    MaterialDefinitionId = matdef.Id, 
                    Quantity = 1, 
                    Status = "Validated", 
                    Supplier = matdef.Supplier, 
                    CreatedAt = DateTime.UtcNow 
                });

                _logger.LogInformation($"BATCH---Material {matdef.Id} assigned to lot {lotId}.");

                await output.WriteAsync(matdef);
            }
            output.Complete();
        }
    }
}
