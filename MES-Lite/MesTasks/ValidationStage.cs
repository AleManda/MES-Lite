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
    internal class ValidationStage: IValidationStage
    {

        public async Task RunAsync(ChannelReader<MaterialDefinition> input, ChannelWriter<MaterialDefinition> output)
        {
            await foreach (var matdef in input.ReadAllAsync())
            {
                // Simulate validation logic
                if (matdef.Conformity)
                {
                    try
                    {
                        Console.WriteLine($"VALIDATE---Material {matdef.Id} passed validation.");
                        await output.WriteAsync(matdef);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error writing material {matdef.Id} to output: {ex.Message}");
                        //await Task.FromException(ex);
                    }
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
