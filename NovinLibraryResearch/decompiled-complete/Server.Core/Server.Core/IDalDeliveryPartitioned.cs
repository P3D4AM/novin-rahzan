using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalDeliveryPartitioned
{
	Task<List<WebDelivery>> GetAllAsync(short DepId, long HashId, short Partition, CancellationToken cancellationToken);

	Task<List<WebDelivery>> GetDeliveryByGuidSAsync(short DepId, List<Guid> list, CancellationToken cancellationToken);

	Task<ReturnedSaveFuncInfo> SaveDeliveryAsync(ConnectionEventArgs<WebDelivery> e, CancellationToken token, short tryCount = 10);

	Task<ReturnedSaveFuncInfo> RemoveAsync(ConnectionData connection, Guid guid, CancellationToken token, short tryCount = 10);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);
}
