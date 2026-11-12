using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalSyncEntityByHash
{
	Task<ReturnedSaveFuncInfo> SaveAsync(long hashid, WebEntitySyncByHash syncItem, CancellationToken token);

	Task<WebEntitySyncByHash> GetAsync(long hashid, Guid entityGuid, EntityPart part, CancellationToken token);
}
