using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting.Details;

public interface IReceptionCheckRepository : IPersistenceMasterRepository<ReceptionCheckBusiness>, IDisposable
{
	Task<string> NextPoshtNomreAsync(int checkCount, CancellationToken token, string tranName);

	Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(ReceptionChequeFilter filter, CancellationToken token, string tranName);

	Task<ReceptionCheckBusiness> GetAsync(Guid guid, CancellationToken token, string tranName);

	Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName);

	Task<ReceptionCheckBusiness> GetAsync(string number, CancellationToken token, ENUMCheck? status, string tranName);
}
