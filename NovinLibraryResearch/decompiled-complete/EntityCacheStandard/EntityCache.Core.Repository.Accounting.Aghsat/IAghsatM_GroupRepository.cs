using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting.Aghsat;

public interface IAghsatM_GroupRepository : ISyncRepository<AghsatM_GroupBusiness>, IEditableDelivery, IRepository<AghsatM_GroupBusiness>, INoGuidRepository<AghsatM_GroupBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> UpdateSumAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName);
}
