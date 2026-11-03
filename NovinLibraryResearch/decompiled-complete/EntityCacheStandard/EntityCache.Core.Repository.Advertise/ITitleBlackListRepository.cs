using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface ITitleBlackListRepository : IRepository<TitleBlackListBusiness>, INoGuidRepository<TitleBlackListBusiness>, IDisposable
{
	Task<List<TitleBlackListBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);
}
