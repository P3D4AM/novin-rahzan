using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.GoldAndCurrency;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel;

public interface IGoldAndCurrencySignalRClient : INpAsyncDisposable, IAsyncDisposable
{
	event Func<List<WebGoldAndCurrencyDetail>, Task> DataRecieved;

	event Func<List<WebGoldAndCurrencyReport>, Task> OnLogin;

	Task<ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>>> GetRankedDataAsync(CancellationToken token);
}
