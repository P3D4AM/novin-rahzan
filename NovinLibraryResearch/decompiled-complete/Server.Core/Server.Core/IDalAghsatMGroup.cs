using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalAghsatMGroup
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashId, List<WebAghsatMGroup> lst, ENSource source, CancellationToken token);

	Task<List<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<List<WebAghsatMGroup>> GetAllAsync(short depId, long hashId, short partition, CancellationToken cancellationToken);

	Task<WebAghsatMGroup> GetAsync(Guid guid, short depId);
}
