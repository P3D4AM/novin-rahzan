using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Product;

public class ProductPriceRelationMapper : IItemAsyncMapper<WebProductPriceRelation, ProductPriceRelationBusiness>, IItemAsyncMapper<ProductPriceRelationBusiness, ProductPriceRelationBusiness>, IItemAsyncMapper<ProductPriceRelationBusiness, WebProductPriceRelation>, IItemAsyncMapper<ProductPriceRelationModel, ProductPriceRelationModel>
{
	public static ProductPriceRelationMapper Instance { get; private set; } = new ProductPriceRelationMapper();


	public Task<ProductPriceRelationBusiness> MapAsync(string contextGuid, WebProductPriceRelation source, ProductPriceRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductPriceRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Type = source.Type;
			destination.ProductGuid = source.ProductGuid;
			destination.Price = source.Price;
			destination.Quantity = source.Quantity;
			destination.Profit = source.Profit;
			destination.KasrRial = source.KasrRial;
			destination.PriceLevelGuid = source.PriceLevelGuid;
			destination.UnitGuid = source.UnitGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Product\\ProductPriceRelationMapper.cs", 51);
			throw;
		}
	}

	public Task<ProductPriceRelationBusiness> MapAsync(string contextGuid, ProductPriceRelationBusiness source, ProductPriceRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductPriceRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Type = source.Type;
			destination.ProductGuid = source.ProductGuid;
			destination.Price = source.Price;
			destination.Quantity = source.Quantity;
			destination.Profit = source.Profit;
			destination.KasrRial = source.KasrRial;
			destination.PriceLevelGuid = source.PriceLevelGuid;
			destination.UnitGuid = source.UnitGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Product\\ProductPriceRelationMapper.cs", 86);
			throw;
		}
	}

	public Task<WebProductPriceRelation> MapAsync(string contextGuid, ProductPriceRelationBusiness source, WebProductPriceRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebProductPriceRelation();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Type = source.Type;
			destination.ProductGuid = source.ProductGuid;
			destination.Price = source.Price;
			destination.Quantity = source.Quantity;
			destination.Profit = source.Profit;
			destination.KasrRial = source.KasrRial;
			destination.PriceLevelGuid = source.PriceLevelGuid;
			destination.UnitGuid = source.UnitGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Product\\ProductPriceRelationMapper.cs", 120);
			throw;
		}
	}

	public Task<ProductPriceRelationModel> MapAsync(string contextGuid, ProductPriceRelationModel source, ProductPriceRelationModel destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductPriceRelationModel();
			}
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.ProductGuid = source.ProductGuid;
			destination.Price = source.Price;
			destination.PriceLevelGuid = source.PriceLevelGuid;
			destination.PriceLevelName = source.PriceLevelName;
			destination.Type = source.Type;
			destination.Ordinal = source.Ordinal;
			destination.Profit = source.Profit;
			destination.KasrRial = source.KasrRial;
			destination.Quantity = source.Quantity;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Product\\ProductPriceRelationMapper.cs", 149);
			throw;
		}
	}

	public Task<List<ProductPriceRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebProductPriceRelation> source, List<ProductPriceRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductPriceRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductPriceRelationBusiness> source, List<ProductPriceRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebProductPriceRelation>> MapListAsync(string contextGuid, IEnumerable<ProductPriceRelationBusiness> source, List<WebProductPriceRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductPriceRelationModel>> MapListAsync(string contextGuid, IEnumerable<ProductPriceRelationModel> source, List<ProductPriceRelationModel> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
