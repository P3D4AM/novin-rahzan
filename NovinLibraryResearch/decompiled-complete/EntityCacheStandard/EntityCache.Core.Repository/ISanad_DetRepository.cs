using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Core.Repository;

public interface ISanad_DetRepository : IRepository<Sanad_DetBusiness>, INoGuidRepository<Sanad_DetBusiness>, IDisposable
{
	Task<List<Sanad_DetBusiness>> GetAllAsync(Guid SanadGuid, Document SanadType, Guid? MoeinGuid, Guid? TafsilGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveListToSanadDetAsync(int SanadGuid, Document SanadType, List<Sanad_DetBusiness> lst, Guid SanadMoeinGuid, Guid SanadTafsilGuid, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> DeleteBySanadNoAsync(int SanadNo, Guid SanadGuid, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<Sanad_DetBusiness> items, SanadBusiness master, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid asGuid, Document asType, CancellationToken token, string tranName);
}
