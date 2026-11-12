using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Connection;

namespace Server.Core;

public interface IDalSyncEntity
{
	Task<IEnumerable<WebEntitySync>> GetAllAsync(IClsConnectionData connection, short partition);

	Task<WebEntitySync> LoadDataAsync(Guid guid);

	Task<IEnumerable<SaveResponse>> Save(short depId, long hashId, IEnumerable<WebEntitySync> lst, ENSource source, CancellationToken token);

	Task<IEnumerable<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);
}
