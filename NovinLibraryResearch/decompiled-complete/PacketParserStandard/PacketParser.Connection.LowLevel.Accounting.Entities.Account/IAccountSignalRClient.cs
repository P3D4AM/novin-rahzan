using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Account;

public interface IAccountSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<decimal>> GetAccountSumAsync(List<Guid> moeinGuidList, List<Guid> tafsilGuidList, bool? status, CancellationToken token, string tranName);
}
