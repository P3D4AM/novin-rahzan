using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;

namespace EntityCache.Core.Repository;

public interface INewsRepository : IRepository<NewsBusiness>, INoGuidRepository<NewsBusiness>, IDisposable
{
	Task<IEnumerable<NewsBusiness>> GetByCategoryAsync(string category, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<NewsBusiness>> GetAllByTypeCategoryAsync(NewsType type, string category, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<NewsBusiness>> GetAllByTypeAsync(NewsType type, bool? status, CancellationToken token, string tranName);
}
