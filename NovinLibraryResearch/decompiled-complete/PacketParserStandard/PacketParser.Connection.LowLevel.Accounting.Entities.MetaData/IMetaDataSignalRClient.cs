using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.MetaData;

public interface IMetaDataSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebMetaData>>> GetAllAsync(MetaDataFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid entityGuid, EntityPart part, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebMetaData item, short tryCount, CancellationToken token, string tranName);
}
