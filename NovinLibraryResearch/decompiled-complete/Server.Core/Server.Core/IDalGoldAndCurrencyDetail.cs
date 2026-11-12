using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.GoldAndCurrency;
using PacketParser.Filters.GoldAndCurrency;

namespace Server.Core;

public interface IDalGoldAndCurrencyDetail
{
	Task<List<WebGoldAndCurrencyDetail>> GetAllNotSendsAsync(CancellationToken token);

	Task<List<WebGoldAndCurrencyDetail>> GetDataByFilterAsync(GoldAndCurrencyDetailFilter filter, CancellationToken token);

	Task<ReturnedSaveFuncInfo> UpdateAsync(long ID, CancellationToken token);
}
