using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalChequeMOperation
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebChequeMOperation> lst, CancellationToken token);

	Task<WebChequeMOperation> GetAsync(Guid guid, short depId);
}
