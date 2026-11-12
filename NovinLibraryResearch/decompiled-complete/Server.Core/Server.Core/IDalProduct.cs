using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalProduct
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashId, IEnumerable<WebProduct> lst, ENSource source, CancellationToken token);

	Task<List<WebProduct>> GetAllAsync(int depId, CancellationToken cancellationToken = default(CancellationToken));

	Task<List<WebProduct>> GetAllAsync(int depId, Guid masterProductGuid, CancellationToken cancellationToken);

	Task<int> Count(short depId, CancellationToken cancellationToken);

	Task<List<WebProduct>> GetAllAsync(short depId, long hashId, int partition, CancellationToken cancellationToken);

	Task<List<int>> GetPartitionsAsync(short depId, long hashId, CancellationToken cancellationToken);

	Task<List<WebProduct>> GetAllAsync(ProductReadEventArgs request, short? depId, Guid? token);

	Task<List<WebProduct>> GetVarationsAsync(short depid, Guid guid);

	Task<IProduct> GetAsync(short depId, Guid guid);

	Task<List<WebProduct>> GetAllByCodeAsync(short depId, string code);

	Task<WebProduct> GetBySkuAsync(short depId, string sku);

	Task<WebProduct> GetAsync(short depId, int id);

	Task<List<WebProduct>> SearchAsync(int depId, CancellationToken cancellationToken, Dictionary<string, string> filters, Dictionary<string, string> orderBy);

	Task<List<WebProduct>> GetAllAsync(short depId, string name);

	Task UpdateSkuAsync(short depId, Guid prdGuid, string prdSku, bool force);

	Task UpdateBarcodeRelationsAsync();
}
