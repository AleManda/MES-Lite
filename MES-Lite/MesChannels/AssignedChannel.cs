using MES_Lite.MesEntities;
using System.Threading.Channels;

namespace MES_Lite.MesChannels
{
    public class AssignedChannel
    {
        public Channel<MaterialDefinition> ChannelAssigned { get; }

        public AssignedChannel()
        {
            ChannelAssigned = Channel.CreateBounded<MaterialDefinition>(new BoundedChannelOptions(50)
                {
                    FullMode = BoundedChannelFullMode.Wait
                });
        }
    }
}
