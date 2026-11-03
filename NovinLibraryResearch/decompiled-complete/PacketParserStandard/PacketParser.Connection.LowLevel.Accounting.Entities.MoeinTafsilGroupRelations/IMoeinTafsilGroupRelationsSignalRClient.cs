using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.MoeinTafsilGroupRelations;

public interface IMoeinTafsilGroupRelationsSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebMoeinTafsilGroupRelation>>> GetAllAsync(Guid? moeinGuid, Guid? tafsilLevelGuid, CancellationToken token, string tranName);
}
