using MES_Lite.MesEntities;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading;

namespace MES_Lite.MesChannels
{
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
