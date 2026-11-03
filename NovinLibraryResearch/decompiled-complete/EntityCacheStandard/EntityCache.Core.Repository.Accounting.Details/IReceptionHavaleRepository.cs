using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;

namespace EntityCache.Core.Repository.Accounting.Details;

public interface IReceptionHavaleRepository : IPersistenceMasterRepository<ReceptionHavaleBusiness>, IDisposable
{
	Task<bool> IsPeygiriNumberTekrariAsync(string number, Guid guid, Guid bankGuid, CancellationToken token, string tranName);

	Task<List<Guid>> GetAllMasterGuidsForFixWageAsync(CancellationToken token, string tranName);
}
