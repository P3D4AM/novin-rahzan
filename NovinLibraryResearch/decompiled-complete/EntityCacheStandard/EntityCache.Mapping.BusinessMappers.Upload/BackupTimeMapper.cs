using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Upload;

public class BackupTimeMapper : IItemAsyncMapper<BackupTimeBusiness, WebBackupTime>, IItemAsyncMapper<BackupTimeBusiness, BackupTimeBusiness>, IItemAsyncMapper<WebBackupTime, BackupTimeBusiness>
{
	public static BackupTimeMapper Instance { get; private set; } = new BackupTimeMapper();


	public Task<WebBackupTime> MapAsync(string contextGuid, BackupTimeBusiness source, WebBackupTime destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebBackupTime();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParentId = source.ParentId;
			destination.DayOfWeek = source.DayOfWeek;
			destination.DateTime = source.DateTime;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupTimeMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebBackupTime>> MapListAsync(string contextGuid, IEnumerable<BackupTimeBusiness> source, List<WebBackupTime> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupTimeBusiness> MapAsync(string contextGuid, BackupTimeBusiness source, BackupTimeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupTimeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParentId = source.ParentId;
			destination.DayOfWeek = source.DayOfWeek;
			destination.DateTime = source.DateTime;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupTimeMapper.cs", 64);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupTimeBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupTimeBusiness> source, List<BackupTimeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupTimeBusiness> MapAsync(string contextGuid, WebBackupTime source, BackupTimeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupTimeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParentId = source.ParentId;
			destination.DayOfWeek = source.DayOfWeek;
			destination.DateTime = source.DateTime;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupTimeMapper.cs", 93);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupTimeBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBackupTime> source, List<BackupTimeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
