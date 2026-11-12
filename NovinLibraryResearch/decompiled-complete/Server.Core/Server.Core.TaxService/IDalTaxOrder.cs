using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Accounting.TaxService;

namespace Server.Core.TaxService;

public interface IDalTaxOrder
{
	Task<List<TaxOrder>> GetNotDeliveredAsync(string taxClientId);

	Task<ReturnedSaveFuncInfoWithValue<int>> InsertAsync(short depId, int userId, TaxOrder item, CancellationToken token);

	Task<ReturnedSaveFuncInfo> UpdateAsync(short depId, int userId, TaxOrder item, CancellationToken token);
}
