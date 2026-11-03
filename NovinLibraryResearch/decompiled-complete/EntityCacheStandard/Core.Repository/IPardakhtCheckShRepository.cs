using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.BusinessLayer.Models;
using PacketParser.Filters;

namespace Core.Repository;

public interface IPardakhtCheckShRepository : IPersistenceMasterRepository<PardakhtCheckShBusiness>, IDisposable
{
	Task<IEnumerable<PardakhtCheckShReportBusiness>> GetAllAsync(PardakhtChequeFilter filter, CancellationToken token, string tranName);

	Task<PardakhtCheckShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName);
}
