using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Models.Accounting.Prd;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository;

public interface IPrdAnbarRelationRepository : INoGuidRepository<PrdAnbarRelationBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> UpdateAccountAsync(ProductsChanges productsChanges, bool rollback, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAnbarRelationsAsync(IEnumerable<Guid> prdGuids, short tryCount, CancellationToken token, string tranName);

	Task<List<PrdAnbarRelationBusiness>> GetAnbarRelationsAsync(Guid? prdGuid, Guid? dptGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(PrdAnbarRelationBusiness item, bool updateAmount, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdAnbarRelationBusiness> items, bool updateAmount, short tryCount, CancellationToken token, string tranName);

	Task<IEnumerable<ProductOrderPointModel>> GetAllProductOrderpointsAsync(CancellationToken token, string tranName);

	Task<decimal> GetNegativeAmountAsync(CancellationToken token, string tranName);

	Task<decimal> GetPositiveAmountAsync(CancellationToken token, string tranName);

	Task<IEnumerable<PrdAnbarRelationBusiness>> GetAllAsync(List<ProductsChanges> list, CancellationToken token, string tranName);
}
