using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;

namespace EntityCache.Core.Repository;

public interface IProductAttributeRelationRepository : INoGuidRepository<ProductAttributeRelationBusiness>, IDisposable
{
	Task<IEnumerable<ProductAttributeRelationBusiness>> GetAllAsync(Guid productGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid productGuid, CancellationToken token, string tranName);

	Task<IEnumerable<ProductAttributeRelationModel>> GetAllModelAsync(Guid productGuid, CancellationToken token, string tranName);

	Task<IEnumerable<ProductAttributeRelationBusiness>> GetAllBtAttributeValueAsync(Guid attributeValueGuid, CancellationToken token, string tranName);
}
