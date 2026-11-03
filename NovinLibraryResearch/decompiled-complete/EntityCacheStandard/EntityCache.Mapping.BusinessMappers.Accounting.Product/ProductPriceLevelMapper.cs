using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Product;

public class ProductPriceLevelMapper : IItemAsyncMapper<ProductPriceLevelBusiness, WebProductPriceLevel>, IItemAsyncMapper<ProductPriceLevelBusiness, ProductPriceLevelBusiness>, IItemAsyncMapper<WebProductPriceLevel, ProductPriceLevelBusiness>
{
	public static ProductPriceLevelMapper Instance { get; private set; } = new ProductPriceLevelMapper();


	public Task<WebProductPriceLevel> MapAsync(string contextGuid, ProductPriceLevelBusiness source, WebProductPriceLevel destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebProductPriceLevel();
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Ordinal = source.Ordinal;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Type = source.Type;
			destination.DiscountCoefficient = source.DiscountCoefficient;
			destination.IsSystem = source.IsSystem;
			destination.IsDefault = source.IsDefault;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Product\\ProductPriceLevelMapper.cs", 46);
			throw;
		}
	}

	public Task<ProductPriceLevelBusiness> MapAsync(string contextGuid, ProductPriceLevelBusiness source, ProductPriceLevelBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new ProductPriceLevelBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Ordinal = source.Ordinal;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Type = source.Type;
			destination.DiscountCoefficient = source.DiscountCoefficient;
			destination.IsSystem = source.IsSystem;
			destination.IsDefault = source.IsDefault;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Product\\ProductPriceLevelMapper.cs", 81);
			throw;
		}
	}

	public Task<ProductPriceLevelBusiness> MapAsync(string contextGuid, WebProductPriceLevel source, ProductPriceLevelBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new ProductPriceLevelBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Ordinal = source.Ordinal;
			destination.Modified = source.Modified;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Type = source.Type;
			destination.DiscountCoefficient = source.DiscountCoefficient;
			destination.IsSystem = source.IsSystem;
			destination.IsDefault = source.IsDefault;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Product\\ProductPriceLevelMapper.cs", 116);
			throw;
		}
	}

	public Task<List<WebProductPriceLevel>> MapListAsync(string contextGuid, IEnumerable<ProductPriceLevelBusiness> source, List<WebProductPriceLevel> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductPriceLevelBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductPriceLevelBusiness> source, List<ProductPriceLevelBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductPriceLevelBusiness>> MapListAsync(string contextGuid, IEnumerable<WebProductPriceLevel> source, List<ProductPriceLevelBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
