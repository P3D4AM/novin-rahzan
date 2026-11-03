using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IPrdAnbargardaniDetRepository : IPersistenceMasterRepository<PrdAnbarGardaniDetBusiness>, IDisposable
{
	Task GetAllAsync(PrdAnbarGardaniBusiness master, CancellationToken token, string tranName);
}
