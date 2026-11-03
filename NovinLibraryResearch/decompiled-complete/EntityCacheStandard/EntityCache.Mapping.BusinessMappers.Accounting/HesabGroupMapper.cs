using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class HesabGroupMapper : IItemAsyncMapper<HesabGroupBusiness, WebHesabGroup>, IItemAsyncMapper<HesabGroupBusiness, HesabGroupBusiness>, IItemAsyncMapper<WebHesabGroup, HesabGroupBusiness>
{
	public static HesabGroupMapper Instance { get; private set; } = new HesabGroupMapper();


	public Task<WebHesabGroup> MapAsync(string contextGuid, HesabGroupBusiness source, WebHesabGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebHesabGroup();
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Code = source.Code;
			destination.TypeGuid = source.TypeGuid;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\HesabGroupMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<HesabGroupBusiness> MapAsync(string contextGuid, HesabGroupBusiness source, HesabGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new HesabGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Code = source.Code;
			destination.TypeGuid = source.TypeGuid;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\HesabGroupMapper.cs", 62);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<HesabGroupBusiness> MapAsync(string contextGuid, WebHesabGroup source, HesabGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new HesabGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Code = source.Code;
			destination.TypeGuid = source.TypeGuid;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\HesabGroupMapper.cs", 88);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebHesabGroup>> MapListAsync(string contextGuid, IEnumerable<HesabGroupBusiness> source, List<WebHesabGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<HesabGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<HesabGroupBusiness> source, List<HesabGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<HesabGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<WebHesabGroup> source, List<HesabGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
