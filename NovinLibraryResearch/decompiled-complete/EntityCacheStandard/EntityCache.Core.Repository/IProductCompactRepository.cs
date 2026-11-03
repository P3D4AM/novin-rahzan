using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Products;
using EntityCache.Filters;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters.Configs;

namespace EntityCache.Core.Repository;

public interface IProductCompactRepository : ICompactRepository<ProductCompactBusiness>, IDisposable
{
	Task<ProductCompactBusiness> GetAsync(Guid? depotGuid, string Code, bool? status, CancellationToken token, string tranName);

	Task<List<ProductCompactBusiness>> GetAllAsync(Guid? depotGuid, Guid? guid, Document Type, CancellationToken token, ProductCompactFilter filter, string tranName);

	Task<List<ProductMicroBusiness>> GetAllAsync(ProductCompactFilter filter, Guid? guid, Guid? depotGuid, CancellationToken token, string tranName);

	Task<IEnumerable<ProductCompactBusiness>> GetLast20Async(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateGroupOfProductsAsync(List<ProductGroupRelationBusiness> originalList, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<bool>> HasPicImageAsync(string fileName, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<bool>> HasOtherPicImageAsync(string fileName, CancellationToken token, string tranName);

	void ResetCache(string tranName);

	Task<IEnumerable<ProductCompactBusiness>> GetVariationProductAsync(Guid prdGuid, CancellationToken token, string tranName);

	Task<ProductCompactBusiness> GetAsync(Guid? depotGuid, Guid guid, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<ProductPriceReportBusiness>> GetAllProductPriceModelAsync(ProductPriceReportConfig config, CancellationToken token, string tranName);

	Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySlugAsync(CancellationToken token, string tranName);

	Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuAsync(CancellationToken token, string tranName);

	Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuOrSlugAsync(CancellationToken token, string tranName);

	Task<ProductCompactBusiness> GetAsync(Guid guid, bool? status, CancellationToken token, string tranName);

	Task<IEnumerable<ProductCompactBusiness>> GetAllForSendToServerSniftsAsync(CancellationToken token, string tranName);

	Task<List<Guid>> CheckExistAsync(List<Guid> lst, CancellationToken token, string tranName);

	Task<ProductInfoModel> GetPrdInfoModelAsync(Guid guid, CancellationToken token, string tranName);

	Task<ProductMicroBusiness> GetMicroAsync(Guid guid, CancellationToken token, string tranName);
}
