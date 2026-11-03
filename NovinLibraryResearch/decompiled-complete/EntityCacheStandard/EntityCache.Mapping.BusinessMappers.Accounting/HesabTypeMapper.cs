using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class HesabTypeMapper : IItemAsyncMapper<HesabTypeBusiness, HesabTypeBusiness>, IItemAsyncMapper<HesabTypeBusiness, WebHesabType>, IItemAsyncMapper<WebHesabType, HesabTypeBusiness>
{
	public static HesabTypeMapper Instance { get; private set; } = new HesabTypeMapper();


	public Task<HesabTypeBusiness> MapAsync(string contextGuid, HesabTypeBusiness source, HesabTypeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new HesabTypeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Code = source.Code;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\HesabTypeMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<HesabTypeBusiness>> MapListAsync(string contextGuid, IEnumerable<HesabTypeBusiness> source, List<HesabTypeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebHesabType> MapAsync(string contextGuid, HesabTypeBusiness source, WebHesabType destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebHesabType();
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Code = source.Code;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\HesabTypeMapper.cs", 59);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebHesabType>> MapListAsync(string contextGuid, IEnumerable<HesabTypeBusiness> source, List<WebHesabType> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<HesabTypeBusiness> MapAsync(string contextGuid, WebHesabType source, HesabTypeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new HesabTypeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Code = source.Code;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\HesabTypeMapper.cs", 85);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<HesabTypeBusiness>> MapListAsync(string contextGuid, IEnumerable<WebHesabType> source, List<HesabTypeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
