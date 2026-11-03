using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer.Models.Aghsat;

namespace EntityCache.Core.Repository.Accounting;

public interface IVamRepository : IRepository<VamBusiness>, INoGuidRepository<VamBusiness>, IDisposable
{
	Task<int> NextId2Async(CancellationToken token, string tranName);

	Task<IEnumerable<VamModel>> GetAllModelAsync(bool? status, CancellationToken token, string tranName);
}
