using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting.Tolid;

public interface ITolidRepository : IEntityRepository<TolidBusiness, TolidReportBusiness>, IEditableDelivery, IDisposable
{
	Task<int> NextId2Async(CancellationToken token, string tranName);

	Task<IEnumerable<TolidReportBusiness>> GetAllReportAsync(TolidFilter filter, CancellationToken token, string tranName);

	Task<TolidBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName);
}
