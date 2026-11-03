using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.ProductAttrebutes;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.ProductAttrebutes;

public class ProductAttributeRelationMapper : IItemAsyncMapper<ProductAttributeRelationBusiness, WebProductAttributeRelation>, IItemAsyncMapper<ProductAttributeRelationBusiness, ProductAttributeRelationBusiness>, IItemAsyncMapper<WebProductAttributeRelation, ProductAttributeRelationBusiness>
{
	public static ProductAttributeRelationMapper Instance { get; private set; } = new ProductAttributeRelationMapper();


	public async Task<WebProductAttributeRelation> MapAsync(string contextGuid, ProductAttributeRelationBusiness source, WebProductAttributeRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebProductAttributeRelation();
			}
			destination.ProductGuid = source.ProductGuid;
			destination.AttributeValueGuid = source.AttributeValueGuid;
			destination.AttributeGuid = source.AttributeGuid;
			ProductAttributeBusiness productAttributeBusiness = await ProductAttributeBusiness.GetAsync(contextGuid, source.AttributeGuid, token, tranName);
			destination.AttributeTitle = productAttributeBusiness?.Name ?? "";
			destination.AttributeSlug = productAttributeBusiness?.Slug ?? "";
			if (productAttributeBusiness != null)
			{
				destination.AttributeValue = productAttributeBusiness.DetailList.FirstOrDefault((ProductAttributeValueBusiness p) => p.Guid == source.AttributeValueGuid)?.Name ?? "";
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeRelationMapper.cs", 42);
			throw;
		}
	}

	public Task<ProductAttributeRelationBusiness> MapAsync(string contextGuid, ProductAttributeRelationBusiness source, ProductAttributeRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductAttributeRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.AttributeValueGuid = source.AttributeValueGuid;
			destination.AttributeGuid = source.AttributeGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeRelationMapper.cs", 64);
			throw;
		}
	}

	public Task<ProductAttributeRelationBusiness> MapAsync(string contextGuid, WebProductAttributeRelation source, ProductAttributeRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductAttributeRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.AttributeValueGuid = source.AttributeValueGuid;
			destination.AttributeGuid = source.AttributeGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeRelationMapper.cs", 86);
			throw;
		}
	}

	public Task<List<WebProductAttributeRelation>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeRelationBusiness> source, List<WebProductAttributeRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductAttributeRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeRelationBusiness> source, List<ProductAttributeRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductAttributeRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebProductAttributeRelation> source, List<ProductAttributeRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
