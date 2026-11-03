using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository;

public interface IProductPriceRelationRepository : ISyncRepository<ProductPriceRelationBusiness>, IEditableDelivery, IRepository<ProductPriceRelationBusiness>, INoGuidRepository<ProductPriceRelationBusiness>, IDisposable
{
	Task<IEnumerable<ProductPriceRelationModel>> GetAllModelAsync(ProductPriceRelationFilter filter, CancellationToken token, string tranName);

	Task<IEnumerable<ProductPriceRelationBusiness>> GetAllAsync(ProductPriceRelationFilter filter, CancellationToken token, string tranName);

	Task<int> DbCountAsync(Guid? productGuid, Guid? priceLevelGuid, CancellationToken token, string tranName);
}
