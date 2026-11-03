using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tax;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting.Tax;

public interface ITaxRepository : ISyncRepository<TaxBusiness>, IEditableDelivery, IRepository<TaxBusiness>, INoGuidRepository<TaxBusiness>, IDisposable
{
	Task<IEnumerable<TaxReportBusiness>> GetAllReportAsync(bool? status, CancellationToken token, string tranName);

	Task<TaxReportBusiness> GetAsync(string name, CancellationToken token, string tranName);

	Task<TaxReportBusiness> GetBySlugAsync(string slug, CancellationToken token, string tranName);
}
