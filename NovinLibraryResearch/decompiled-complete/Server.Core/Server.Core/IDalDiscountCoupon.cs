using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalDiscountCoupon
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebDiscountCoupon> lst, CancellationToken token);

	Task<List<WebDiscountCoupon>> GetAllAsync(short DepId, long HashId, int partitionId, CancellationToken cancellationToken);

	Task<WebDiscountCoupon> GetAsync(Guid guid, short depId);

	Task<List<short>> GetPartitionsAsync(short DepId, long hashId, CancellationToken cancellationToken);
}
