using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Connection;

namespace Server.Core;

public interface IDalDesc
{
	Task<IEnumerable<WebDesc>> GetAllAsync(IClsConnectionData connection, short partition);

	Task<WebDesc> LoadDataAsync(Guid guid);

	Task<IEnumerable<SaveResponse>> SaveAsync(short depId, long hashId, IEnumerable<WebDesc> lst, ENSource source, CancellationToken token);

	Task<IEnumerable<short>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);
}
