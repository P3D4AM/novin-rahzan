using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalDelivery
{
	Task<ReturnedSaveFuncInfo> SaveAsync(ConnectionData connection, IEnumerable<IDelivery> lst, short tryCount = 10);

	Task<WebDelivery> LoadDataAsync(ConnectionData connection, Guid Guid);

	Task<List<WebDelivery>> GetAllAsync(short depId, DateTime Modified, CancellationToken? token = null);

	Task<List<WebDelivery>> GetDeliveryByGuidSAsync(short depId, List<Guid> guids, CancellationToken? token = null);

	Task<ReturnedSaveFuncInfo> SaveDeliveryAsync(ConnectionEventArgs<WebDelivery> e, short tryCount = 10);
}
