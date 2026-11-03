using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface IDepEnteghalRepository : IRepository<DepEnteghalBusiness>, INoGuidRepository<DepEnteghalBusiness>, IDisposable
{
	Task<DepEnteghalBusiness> GetAsync(int no, CancellationToken token, string tranName);

	Task<int> SelectLastIdGAsync(CancellationToken token, string tranName);

	Task<int> CountQtByMasterGuidAsync(Guid masterGuid, CancellationToken token, string tranName);

	Task<IEnumerable<DepEnteghalReportBusiness>> GetAllAsync(DepEnteghalFilter filter, CancellationToken token, string tranName);

	Task<DepEnteghalBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName);
}
