using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Indexes;

public abstract class IxBaseMapper : IItemAsyncMapper<IndexEntityBusiness, IndexEntityBusiness>
{
	public async Task<IndexEntityBusiness> MapAsync(string contextGuid, IndexEntityBusiness source, IndexEntityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public async Task<List<IndexEntityBusiness>> MapListAsync(string contextGuid, IEnumerable<IndexEntityBusiness> source, List<IndexEntityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
