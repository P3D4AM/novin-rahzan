using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IProductRepository : IDetailRepository<ProductBusiness>, IEditableDelivery, IDisposable
{
	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebProduct> items, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<Guid> productGuids, ServerStatusForClient status, CancellationToken token, string tranName);

	Task<ProductBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName);

	Task<long> GetNextCodeAsync(Guid defGroupGuid, EnSuggestionType suggestionMode, CancellationToken token, string tranName);

	Task<List<ProductBusiness>> GetAllAsync(CancellationToken token, string tranName);

	Task<ProductBusiness> GetByIdProductAsync(string idProduct, CancellationToken token, string tranName);

	void ResetCache(CancellationToken token, string tranName);

	Task<List<ProductBusiness>> GetVariationProductAsync(Guid prdGuid, CancellationToken token, string tranName);

	Task<ProductBusiness> GetByMappedIdAsync(string mappedId, CancellationToken token, string tranName);

	Task<ProductBusiness> GetBySkuAsync(string sku, CancellationToken token, string tranName);

	Task<ProductBusiness> GetBySlugAsync(string slug, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeTaxPercentAsync(IEnumerable<Guid> productGuidList, Guid? taxGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ReNewAllCodesAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ReNewEmptyCodesAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateRegularPriceAsync(Guid priceLevelGuid, Guid regularPriceGuid, CancellationToken token, string tranName);

	Task<HashSet<Guid>> GetAllForIndexingAsync(EnProductIndex mode, int count, CancellationToken token, string tranName);

	Task<IEnumerable<ProductForIndexingModel>> GetAllForIndexingAsync(HashSet<Guid> guids, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateIndexTypeAsync(HashSet<Guid> guids, EnProductIndex mode, CancellationToken token, string tranName);

	Task<IEnumerable<ProductDiscountModel>> GetAllDiscountModelAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateDiscountPercentAsync(List<Tuple<Guid, decimal>> changes, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateStatusAsync(Guid guid, bool newStatus, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateMasterProductAmountAsync(Guid? guid, CancellationToken token, string tranName);

	Task<List<ProductTolidReportModel>> GetAllTolidReportAsync(Guid productGuid, CancellationToken token, string tranName);

	Task<ProductBusiness> GetAsync(Guid guid, Guid? depotGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateProductsAsync(List<ProductUpdateBaseModel> originalList, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(Guid groupGuid, bool status, CancellationToken token, string tranName);

	Task<List<ProductAttributeAmountModel>> GetAllAttributeAmountAsync(Guid productGuid, Guid rowAttributeGuid, Guid columnAttributeGuid, CancellationToken token, string tranName);
}
