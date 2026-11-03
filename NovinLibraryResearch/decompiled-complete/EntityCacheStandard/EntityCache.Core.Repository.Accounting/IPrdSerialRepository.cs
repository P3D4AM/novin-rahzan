using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Products.ProductSerial;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface IPrdSerialRepository : ISyncRepository<PrdSerialBusiness>, IEditableDelivery, IRepository<PrdSerialBusiness>, INoGuidRepository<PrdSerialBusiness>, IDisposable
{
	Task<PrdSerialBusiness> GetAsync(string serialNumber, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveByPrdGuidAsync(Guid prdGuid, short tryCount, CancellationToken token, string tranName);

	Task<IEnumerable<PrdSerialBusiness>> GetAllAsync(ProductSerialFilter filter, CancellationToken token, string tranName);

	Task<IEnumerable<PrdSerialDetailReportBusiness>> GetAllAsync(string serial, CancellationToken token, string tranName);

	Task<IEnumerable<PrdSerialBusiness>> GetAllWithoutDetailAsync(CancellationToken token, string tranName);

	Task<IEnumerable<ProductSerialTechnicalInfoSuggestModel>> GetAllTechnicalSuggestionAsync(CancellationToken token, string tranName);

	Task<IEnumerable<PrdSerialReportBusiness>> GetAllModelAsync(ProductSerialFilter filter, CancellationToken token, string tranName);
}
