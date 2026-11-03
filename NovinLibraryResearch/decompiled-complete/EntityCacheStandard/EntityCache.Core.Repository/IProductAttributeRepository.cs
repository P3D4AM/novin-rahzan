using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.ProductAttrebutes;

namespace EntityCache.Core.Repository;

public interface IProductAttributeRepository : IRepository<ProductAttributeBusiness>, INoGuidRepository<ProductAttributeBusiness>, IDisposable
{
	Task<IEnumerable<ProductAttributeReportBusiness>> GetAllModelAsync(bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<ProductAttributeValueReportBusiness>> GetAllValueAsync(CancellationToken token, string tranName);

	Task<short> GetNextOrdinalAsync(CancellationToken token, string tranName);

	Task<ProductAttributeBusiness> GetAsync(string name, CancellationToken token, string tranName);
}
