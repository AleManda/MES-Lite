using MES_Lite.MesEntities;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading;

namespace MES_Lite.MesChannels
{
    // Wrapper class for a Channel<T>,used by the pipeline
    public class InputChannel
    {
        public Channel<MaterialDefinition> ChannelInput { get; }

        public InputChannel()
        {
            ChannelInput = Channel.CreateBounded<MaterialDefinition>(new BoundedChannelOptions(50)
                {
                    FullMode = BoundedChannelFullMode.Wait
                });
        }
    }
}
