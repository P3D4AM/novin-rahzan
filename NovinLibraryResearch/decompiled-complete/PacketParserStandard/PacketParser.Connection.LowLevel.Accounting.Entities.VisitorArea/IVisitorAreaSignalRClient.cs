using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.VisitorArea;

public interface IVisitorAreaSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebVisitorArea>>> GetAllAsync(Guid? parentGuid, bool status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebVisitorArea>> GetAsync(Guid guid, CancellationToken token, string tranName);
}
