using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.ProductAttrebutes;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.ProductAttrebutes;

public class ProductAttributeValueMapper : IItemAsyncMapper<ProductAttributeValueBusiness, WebProductAttributeValue>, IItemAsyncMapper<ProductAttributeValueBusiness, ProductAttributeValueBusiness>, IItemAsyncMapper<WebProductAttributeValue, ProductAttributeValueBusiness>
{
	public static ProductAttributeValueMapper Instance { get; private set; } = new ProductAttributeValueMapper();


	public Task<WebProductAttributeValue> MapAsync(string contextGuid, ProductAttributeValueBusiness source, WebProductAttributeValue destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebProductAttributeValue();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Slug = source.Slug;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeValueMapper.cs", 37);
			throw;
		}
	}

	public Task<ProductAttributeValueBusiness> MapAsync(string contextGuid, ProductAttributeValueBusiness source, ProductAttributeValueBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductAttributeValueBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Slug = source.Slug;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeValueMapper.cs", 61);
			throw;
		}
	}

	public Task<ProductAttributeValueBusiness> MapAsync(string contextGuid, WebProductAttributeValue source, ProductAttributeValueBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ProductAttributeValueBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Slug = source.Slug;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ProductAttrebutes\\ProductAttributeValueMapper.cs", 85);
			throw;
		}
	}

	public Task<List<WebProductAttributeValue>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeValueBusiness> source, List<WebProductAttributeValue> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductAttributeValueBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeValueBusiness> source, List<ProductAttributeValueBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ProductAttributeValueBusiness>> MapListAsync(string contextGuid, IEnumerable<WebProductAttributeValue> source, List<ProductAttributeValueBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
