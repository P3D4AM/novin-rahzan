using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ResidStatusMapper : IItemAsyncMapper<ResidStatusBusiness, WebResidStatus>, IItemAsyncMapper<ResidStatusBusiness, ResidStatusBusiness>, IItemAsyncMapper<WebResidStatus, ResidStatusBusiness>
{
	public static ResidStatusMapper Instance { get; private set; } = new ResidStatusMapper();


	public Task<WebResidStatus> MapAsync(string contextGuid, ResidStatusBusiness source, WebResidStatus destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebResidStatus();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ResidStatusMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebResidStatus>> MapListAsync(string contextGuid, IEnumerable<ResidStatusBusiness> source, List<WebResidStatus> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ResidStatusBusiness> MapAsync(string contextGuid, ResidStatusBusiness source, ResidStatusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ResidStatusBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ResidStatusMapper.cs", 63);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ResidStatusBusiness>> MapListAsync(string contextGuid, IEnumerable<ResidStatusBusiness> source, List<ResidStatusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ResidStatusBusiness> MapAsync(string contextGuid, WebResidStatus source, ResidStatusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ResidStatusBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ResidStatusMapper.cs", 92);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ResidStatusBusiness>> MapListAsync(string contextGuid, IEnumerable<WebResidStatus> source, List<ResidStatusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
