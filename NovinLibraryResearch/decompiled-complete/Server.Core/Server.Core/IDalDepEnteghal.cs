using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalDepEnteghal
{
	Task<ReturnedSaveFuncInfo> SaveAsync(short depId, int depUserId, long hashId, ENSource _source, WebDepEnteghal item, int tryCount = 10);

	Task<WebDepEnteghal> LoadDataAsync(short DepId, Guid guid);

	Task<List<WebDepEnteghal>> GetAllAsync(short DepId, List<ReadStatus> ST, int top, CancellationToken cancellationToken);

	Task FixAsync(ConnectionData connection, WebDepEnteghal item);

	Task<WebDepEnteghal> GetAsync(Guid guid, short depId);
}
