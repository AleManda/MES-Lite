using MES_Lite.MesEntities;
using System.Threading.Channels;

namespace MES_Lite.MesChannels
{
    public class ValidatedChannel
    {
        public Channel<MaterialDefinition> ChannelValidated { get; }

        public ValidatedChannel()
        {
            ChannelValidated = Channel.CreateBounded<MaterialDefinition>(new BoundedChannelOptions(50)
                {
                    FullMode = BoundedChannelFullMode.Wait
                });
        }
    }
}
