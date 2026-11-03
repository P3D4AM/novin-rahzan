using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IContentBlackListRepository : IRepository<ContentBlackListBusiness>, INoGuidRepository<ContentBlackListBusiness>, IDisposable
{
	Task<List<ContentBlackListBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);
}
