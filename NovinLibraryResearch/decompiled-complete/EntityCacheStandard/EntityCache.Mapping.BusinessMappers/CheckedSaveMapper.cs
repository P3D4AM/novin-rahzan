using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class CheckedSaveMapper : IItemAsyncMapper<CheckedSaveBusiness, CheckedSaveBusiness>, IItemAsyncMapper<WebCheckedSave, CheckedSaveBusiness>, IItemAsyncMapper<CheckedSaveBusiness, WebCheckedSave>
{
	public static CheckedSaveMapper Instance { get; private set; } = new CheckedSaveMapper();


	public Task<CheckedSaveBusiness> MapAsync(string contextGuid, CheckedSaveBusiness source, CheckedSaveBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CheckedSaveBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.EntityGuid = source.EntityGuid;
			destination.IsDebit = source.IsDebit;
			destination.ReportType = source.ReportType;
			destination.DateM = source.DateM;
			destination.DetailGuid = source.DetailGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\CheckedSaveMapper.cs", 38);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CheckedSaveBusiness>> MapListAsync(string contextGuid, IEnumerable<CheckedSaveBusiness> source, List<CheckedSaveBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CheckedSaveBusiness> MapAsync(string contextGuid, WebCheckedSave source, CheckedSaveBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CheckedSaveBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.EntityGuid = source.EntityGuid;
			destination.IsDebit = source.IsDebit;
			destination.ReportType = source.ReportType;
			destination.DateM = source.DateM;
			destination.DetailGuid = source.DetailGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\CheckedSaveMapper.cs", 68);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CheckedSaveBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCheckedSave> source, List<CheckedSaveBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebCheckedSave> MapAsync(string contextGuid, CheckedSaveBusiness source, WebCheckedSave destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCheckedSave();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.EntityGuid = source.EntityGuid;
			destination.IsDebit = source.IsDebit;
			destination.ReportType = source.ReportType;
			destination.DateM = source.DateM;
			destination.DetailGuid = source.DetailGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\CheckedSaveMapper.cs", 97);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebCheckedSave>> MapListAsync(string contextGuid, IEnumerable<CheckedSaveBusiness> source, List<WebCheckedSave> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
