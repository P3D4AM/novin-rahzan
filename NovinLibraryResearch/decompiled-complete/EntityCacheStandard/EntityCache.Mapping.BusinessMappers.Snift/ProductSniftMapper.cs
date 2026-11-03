using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Products;
using PacketParser.Entities;
using PacketParser.Entities.Snifts;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Snift;

public class ProductSniftMapper : IItemAsyncMapper<ProductBusiness, WebProductSnift>
{
	public static ProductSniftMapper Instance { get; private set; } = new ProductSniftMapper();


	public async Task<WebProductSnift> MapAsync(string contextGuid, ProductBusiness source, WebProductSnift destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebProductSnift();
			}
			destination.Guid = source.Guid;
			destination.Code = source.IdProduct;
			destination.Name = source.Name;
			WebProductSnift webProductSnift = destination;
			webProductSnift.GroupName = await source.GetGroupNameAsync(token, tranName);
			webProductSnift = destination;
			webProductSnift.VahedName = (await source.GetVahedAsync(token, tranName)).Name;
			webProductSnift = destination;
			webProductSnift.Price = (await ProductPriceRelationBusiness.GetFirstSellAsync(contextGuid, EnSystem.Mapper, source.Guid, token, tranName))?.Price ?? 0m;
			webProductSnift = destination;
			webProductSnift.TaxPercent = await source.GetTaxPercentAsync(token, tranName);
			destination.AvarezPercent = 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Snift\\ProductSniftMapper.cs", 37);
			throw;
		}
		return destination;
	}

	public Task<List<WebProductSnift>> MapListAsync(string contextGuid, IEnumerable<ProductBusiness> source, List<WebProductSnift> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
