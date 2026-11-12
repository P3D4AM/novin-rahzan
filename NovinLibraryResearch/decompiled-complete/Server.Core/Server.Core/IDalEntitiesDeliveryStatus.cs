using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalEntitiesDeliveryStatus
{
	Task<List<Guid>> GetAllNotSentAsync(short depId, EntityPart part, CancellationToken cancellationToken);

	Task<List<WebDelivery>> GetAllAsync(short depId, long hashId, int partitionId, CancellationToken token);

	Task<WebDelivery> Get(short depId, EntityPart part, Guid guid);

	Task<ReturnedSaveFuncInfo> DeleteAsync(short depId, EntityPart part, Guid guid);

	Task<ReturnedSaveFuncInfo> DeleteAsync(short depId, EntityPart part, Guid guid, SqlConnection cn);

	Task<List<int>> GetAllPartitionsAsync(short depId, int hashId, CancellationToken token);
}
