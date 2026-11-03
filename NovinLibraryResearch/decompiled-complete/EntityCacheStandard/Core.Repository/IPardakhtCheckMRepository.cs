using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Core.Repository;

public interface IPardakhtCheckMRepository : IPersistenceMasterRepository<PardakhtCheckMBusiness>, IDisposable
{
	Task<IEnumerable<PardakhtCheckMBusiness>> GetAllWithChequeGuidAsync(Guid chequeGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckMBusiness> items, PardakhtBusiness masterItem, EnSystem system, short trycount, CancellationToken token, string tranName);

	Task<IEnumerable<PardakhtCheckMBusiness>> GetAllAsync(CancellationToken token, string tranName);
}
