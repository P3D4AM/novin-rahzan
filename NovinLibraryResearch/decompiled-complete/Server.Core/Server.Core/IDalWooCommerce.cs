using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using Server.Core.Connection;

namespace Server.Core;

public interface IDalWooCommerce
{
	Task<IWooInstance> GetWooInstanceAsync(ClsConnectionData clsConnectionData, AggregateDal dal);

	Task StatWordPressSyncAsync(WebContextHolder context, short countOfReceiveDataFromWp);

	Task StopWordPressSyncAsync();

	Task WooSendPacketAsync(IWooInstance instance, Packet packet, CancellationToken cancellationToken);
}
