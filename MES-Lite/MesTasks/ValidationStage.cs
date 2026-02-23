using MES_Lite.MesEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    internal class ValidationStage: IValidationStage
    {
        private int _discarded = 0; 
        public int DiscardedCount => _discarded;

        private readonly ILogger<ValidationStage> _logger;


        public ValidationStage(ILogger<ValidationStage> logger) 
        {
            _logger = logger; 
        }

        //______________________________________________________________________________________
        // Validates material definitions and writes valid ones to the output channel
        public async Task RunAsync(ChannelReader<MaterialDefinition> input, ChannelWriter<MaterialDefinition> output, CancellationToken token = default)
        {
            await foreach (var matdef in input.ReadAllAsync())
            {
                token.ThrowIfCancellationRequested();

                if (ValidateMaterialDefinition(matdef))
                {
                    _logger.LogInformation("VALIDATE---Material {Id} passed validation", matdef.Id);
                    await output.WriteAsync(matdef);
                }
                else 
                {
                    _logger.LogWarning("VALIDATE---Material {Id} failed validation", matdef.Id);
                    Interlocked.Increment(ref _discarded); 
                }

            }
            output.Complete();
            _logger.LogInformation("Validation completed. Discarded: {Count}", _discarded);
        }


        private bool ValidateMaterialDefinition(MaterialDefinition matdef)
        {
            // Implement actual validation logic here
            matdef.RequiresDoubleCheck = matdef switch
            {
                { Critical: true } => true,
                _ => false
            };

            return string.IsNullOrEmpty(matdef.Supplier) ?  false : true;
        }





    }
}
