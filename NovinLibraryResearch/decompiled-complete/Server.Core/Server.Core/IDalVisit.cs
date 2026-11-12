using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Connection;

namespace Server.Core;

public interface IDalVisit
{
	Task<WebVisit> LoadDataAsync(Guid guid);

	Task<List<SaveResponse>> Save(short depId, long hashId, List<WebVisit> lst, ENSource source, CancellationToken token);

	Task<List<short>> GetPartitionsAsync(short depId, CancellationToken cancellationToken);

	Task<List<WebVisit>> GetallFromClients_ListForServerAsync(IClsConnectionData connection);

	Task<List<WebVisit>> GetAllAsync(IClsConnectionData connection, short partition);
}
