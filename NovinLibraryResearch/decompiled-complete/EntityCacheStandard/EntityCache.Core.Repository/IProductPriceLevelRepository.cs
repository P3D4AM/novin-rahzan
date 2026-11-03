using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Records.Products;

namespace EntityCache.Core.Repository;

public interface IProductPriceLevelRepository : ISyncRepository<ProductPriceLevelBusiness>, IEditableDelivery, IRepository<ProductPriceLevelBusiness>, INoGuidRepository<ProductPriceLevelBusiness>, IDisposable
{
	Task<ProductPriceLevelBusiness> GetAsync(string name, EnPriceLevelType type, CancellationToken token, string tranName);

	Task<short> GetMaxOrdinalAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeOrdinalAsync(List<ChangeOrdinalRecord> list, CancellationToken token, string tranName);

	Task<IEnumerable<ProductPriceLevelBusiness>> GetAllAsync(ProductPriceLevelFilter filter, CancellationToken token, string tranName);
}
