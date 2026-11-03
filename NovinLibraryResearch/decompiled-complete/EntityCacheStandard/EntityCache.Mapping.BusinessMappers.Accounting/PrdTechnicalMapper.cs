using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class PrdTechnicalMapper : IItemAsyncMapper<PrdTechnicalBusiness, WebPrdTechnical>, IItemAsyncMapper<PrdTechnicalBusiness, PrdTechnicalBusiness>, IItemAsyncMapper<WebPrdTechnical, PrdTechnicalBusiness>
{
	public static PrdTechnicalMapper Instance { get; private set; } = new PrdTechnicalMapper();


	public Task<WebPrdTechnical> MapAsync(string contextGuid, PrdTechnicalBusiness source, WebPrdTechnical destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdTechnical();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ProductGuid = source.ProductGuid;
			destination.Code = source.Code;
			destination.Name = source.Name;
			destination.TechnicalType = source.TechnicalType;
			destination.Key = source.Key;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdTechnicalMapper.cs", 38);
			throw;
		}
	}

	public Task<PrdTechnicalBusiness> MapAsync(string contextGuid, PrdTechnicalBusiness source, PrdTechnicalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdTechnicalBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ProductGuid = source.ProductGuid;
			destination.Code = source.Code;
			destination.Name = source.Name;
			destination.TechnicalType = source.TechnicalType;
			destination.Key = source.Key;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdTechnicalMapper.cs", 64);
			throw;
		}
	}

	public Task<PrdTechnicalBusiness> MapAsync(string contextGuid, WebPrdTechnical source, PrdTechnicalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdTechnicalBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ProductGuid = source.ProductGuid;
			destination.Code = source.Code;
			destination.Name = source.Name;
			destination.TechnicalType = source.TechnicalType;
			destination.Key = source.Key;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdTechnicalMapper.cs", 90);
			throw;
		}
	}

	public Task<List<WebPrdTechnical>> MapListAsync(string contextGuid, IEnumerable<PrdTechnicalBusiness> source, List<WebPrdTechnical> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdTechnicalBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdTechnicalBusiness> source, List<PrdTechnicalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdTechnicalBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdTechnical> source, List<PrdTechnicalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
