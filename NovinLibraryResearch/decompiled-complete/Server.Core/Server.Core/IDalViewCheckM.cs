using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalViewCheckM
{
	Task<List<SaveResponse>> SaveAsync(short DepId, List<WebViewCheckM> lst, ENSource Source, CancellationToken token);

	Task<IEnumerable<WebViewCheckM>> GetAllAsync(short DepId, CancellationToken cancellationToken);

	Task<List<WebViewCheckM>> GetAllAsync(short DepId, long HashId, short Partition, CancellationToken cancellationToken);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);
}
