using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.ProductAttrebutes;
using EntityCache.Mapping.BusinessMappers.Accounting.ProductAttrebutes;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ProductAttributeMapper : IItemAsyncMapper<ProductAttributeBusiness, WebProductAttribute>, IItemAsyncMapper<ProductAttributeBusiness, ProductAttributeBusiness>, IItemAsyncMapper<WebProductAttribute, ProductAttributeBusiness>
{
	public static ProductAttributeMapper Instance { get; private set; } = new ProductAttributeMapper();


	public async Task<ProductAttributeBusiness> MapAsync(string contextGuid, WebProductAttribute source, ProductAttributeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ProductAttributeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Ordinal = source.Ordinal;
			destination.Slug = source.Slug;
			if ((source?.DetList?.Any()).GetValueOrDefault())
			{
				await destination.AddDetailListAsync(await ProductAttributeValueMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<ProductAttributeValueBusiness>(), tranName, token), token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeMapper.cs", 47);
			throw;
		}
	}

	public async Task<WebProductAttribute> MapAsync(string contextGuid, ProductAttributeBusiness source, WebProductAttribute destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebProductAttribute();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Ordinal = source.Ordinal;
			destination.Slug = source.Slug;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				destination.DetList = await ProductAttributeValueMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<WebProductAttributeValue>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeMapper.cs", 76);
			throw;
		}
	}

	public async Task<ProductAttributeBusiness> MapAsync(string contextGuid, ProductAttributeBusiness source, ProductAttributeBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ProductAttributeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Ordinal = source.Ordinal;
			destination.Slug = source.Slug;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				await destination.AddDetailListAsync(await ProductAttributeValueMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<ProductAttributeValueBusiness>(), tranName, token), token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeMapper.cs", 106);
			throw;
		}
	}

	public Task<List<ProductAttributeBusiness>> MapListAsync(string contextGuid, IEnumerable<WebProductAttribute> source, List<ProductAttributeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebProductAttribute>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeBusiness> source, List<WebProductAttribute> destination, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductAttributeBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeBusiness> source, List<ProductAttributeBusiness> destination, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
