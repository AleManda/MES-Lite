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
    internal class FinalLoggingStage: IFinalLoggingStage
    {
        private readonly ILogger<FinalLoggingStage> _logger;

        public FinalLoggingStage(ILogger<FinalLoggingStage> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync(ChannelReader<MaterialDefinition> input, CancellationToken token = default)
        {
            int count = 0;
            int criticalcount = 0;
            await foreach (var matdef in input.ReadAllAsync())
            {
                token.ThrowIfCancellationRequested();
                Interlocked.Increment(ref count);

                if (matdef.IsCritical)
                {
                    _logger.LogWarning($"FINAL---Material {matdef.Id} is marked as critical and requires special attention.");
                    Interlocked.Increment(ref criticalcount);
                }
                // Simulate final logging logic
                _logger.LogInformation($"FINAL---Final Log: Material {matdef.Id} with Batch {matdef.BatchId} is ready for processing.");
            }

            _logger.LogInformation($"FINAL-FINAL--- {count} Materials have been processed,{criticalcount} Materials are marked as CRITICAL");


        }
    }
}
