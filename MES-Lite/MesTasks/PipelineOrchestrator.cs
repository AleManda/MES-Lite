using MES_Lite.MesChannels;
using MES_Lite.MesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MES_Lite.MesTasks
{
    public class PipelineOrchestrator : IPipelineOrchestrator
    {
        private readonly IMaterialDefinitionGenerator _generator;
        private readonly IValidationStage _validation;
        private readonly IBatchAssignmentStage _batch;
        private readonly IFinalLoggingStage _final;

        private readonly Channel<MaterialDefinition> _input;
        private readonly Channel<MaterialDefinition> _validated;
        private readonly Channel<MaterialDefinition> _assigned;

        public PipelineOrchestrator(
            IMaterialDefinitionGenerator generator,
            IValidationStage validation,
            IBatchAssignmentStage batch,
            IFinalLoggingStage final,
            InputChannel input,
            ValidatedChannel validated,
            AssignedChannel assigned)
        {
            _generator = generator;
            _validation = validation;
            _batch = batch;
            _final = final;

            _input = input.ChannelInput;
            _validated = validated.ChannelValidated;
            _assigned = assigned.ChannelAssigned;
        }

        
        public async Task RunAsync()
        {
            var producerTask = _generator.ProduceAsync(_input.Writer);
            var validationTask = _validation.RunAsync(_input.Reader, _validated.Writer);
            var batchTask = _batch.RunAsync(_validated.Reader, _assigned.Writer);
            var finalTask = _final.RunAsync(_assigned.Reader);

            await Task.WhenAll(producerTask, validationTask, batchTask, finalTask);
            //await Task.WhenAll(producerTask, validationTask);

        }

    }

}
