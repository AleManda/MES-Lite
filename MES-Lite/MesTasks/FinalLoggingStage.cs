using MES_Lite.Data;
using MES_Lite.MesEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MES_Lite.MesTasks
{
    internal class FinalLoggingStage: IFinalLoggingStage
    {
        private readonly MesLiteDbContext _db;
        private readonly ILogger<FinalLoggingStage> _logger;

        public FinalLoggingStage(MesLiteDbContext db, ILogger<FinalLoggingStage> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task RunAsync(ChannelReader<MaterialDefinition> input, CancellationToken token = default)
        {
            int count = 0;
            int criticalcount = 0;

            //await foreach (var matdef in input.ReadAllAsync())
            //{
            //    token.ThrowIfCancellationRequested();
            //    Interlocked.Increment(ref count);

            //    if (matdef.Critical)
            //    {
            //        _logger.LogWarning($"FINAL---Material {matdef.Id} is marked as critical and requires special attention.");
            //        Interlocked.Increment(ref criticalcount);
            //    }
            //    // Simulate final logging logic
            //    _logger.LogInformation($"FINAL---Final Log: Material {matdef.Id} with Batch {matdef.Lots.FirstOrDefault().LotId} is ready for processing.");
            //}




            await foreach (var matdef in input.ReadAllAsync())
            {
                token.ThrowIfCancellationRequested();
                Interlocked.Increment(ref count);     // Incrementa il contatore totale dei materiali processati


                //materiale marcato critical, se è critico loggo warning, se non è critico loggo info, alla fine del ciclo loggo
                //un messaggio finale con il numero totale di materiali processati e il numero di materiali critici
                if (matdef.Critical)
                {
                    _logger.LogWarning($"FINAL---Material {matdef.Id} is marked as critical and requires special attention.");
                    Interlocked.Increment(ref criticalcount); // Incrementa il contatore dei materiali critici
                }

                try
                {
                    // Salvataggio MaterialDefinition (se non esiste già)
                    var existing = await _db.MaterialDefinitions.Include(m => m.Lots).FirstOrDefaultAsync(m => m.MaterialId == matdef.MaterialId);
                    if (existing == null)
                    {
                        _db.MaterialDefinitions.Add(matdef);
                        _logger.LogInformation("Inserted new MaterialDefinition {MaterialId}", matdef.MaterialId); }
                    else
                    {
                        // aggiorna campi modificabili
                        existing.Description = matdef.Description;
                        existing.Version = matdef.Version;
                        existing.UoM = matdef.UoM;
                        existing.Supplier = matdef.Supplier;
                        existing.Conformity = matdef.Conformity;
                        existing.Critical = matdef.Critical;
                        existing.RequiresDoubleCheck = matdef.RequiresDoubleCheck;
                        // aggiungi eventuali nuovi lotti
                        foreach (var lot in matdef.Lots)
                        {
                            if (!existing.Lots.Any(l => l.LotId == lot.LotId))
                            {
                                existing.Lots.Add(lot);
                                _logger.LogInformation("Added new lot {LotId} to material {MaterialId}", lot.LotId, matdef.MaterialId);
                            }
                        }
                    }
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving material {MaterialId}", matdef.MaterialId);
                }
            } 
            _logger.LogInformation("Final stage completed");
            _logger.LogInformation($"FINAL-FINAL--- {count} Materials have been processed,{criticalcount} Materials are marked as CRITICAL");
        }




        
    }
}
