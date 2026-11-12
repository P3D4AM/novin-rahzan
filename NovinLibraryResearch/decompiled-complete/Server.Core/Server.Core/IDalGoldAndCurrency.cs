using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.GoldAndCurrency;

namespace Server.Core;

public interface IDalGoldAndCurrency
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebGoldAndCurrency master, decimal price, CancellationToken token);

	Task<List<WebGoldAndCurrencyReport>> GetAllRankedDataAsync(CancellationToken token);
}
