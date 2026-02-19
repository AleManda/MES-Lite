using MES_Lite.MesEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    internal class ValidationStage: IValidationStage
    {
        private readonly ILogger<ValidationStage> _logger;


        public ValidationStage(ILogger<ValidationStage> logger) 
        {
            _logger = logger; 
        }

        //______________________________________________________________________________________
        // Validates material definitions and writes valid ones to the output channel
        public async Task RunAsync(ChannelReader<MaterialDefinition> input, ChannelWriter<MaterialDefinition> output)
        {
            await foreach (var matdef in input.ReadAllAsync())
            {
                // Simulate validation logic
                if (matdef.Conformity)
                {
                    _logger.LogInformation($"VALIDATE---Material {matdef.Id} passed validation.");
                    await output.WriteAsync(matdef);
                }
                else
                {
                    Console.WriteLine($"Material {matdef.Id} failed validation.");
                }
            }
            output.Complete();
        }





    }
}
