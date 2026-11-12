using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalViewCheckSh
{
	Task<List<SaveResponse>> SaveAsync(short DepId, List<WebViewCheckSh> lst, ENSource Source, CancellationToken token);

	Task<List<WebViewCheckSh>> GetAllAsync(short DepId, CancellationToken cancellationToken);

	Task<List<WebViewCheckSh>> GetAllAsync(short DepId, long HashId, short Partition, CancellationToken cancellationToken);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);
}
