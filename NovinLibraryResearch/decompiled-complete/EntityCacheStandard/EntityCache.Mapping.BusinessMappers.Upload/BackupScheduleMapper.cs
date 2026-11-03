using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Upload;

public class BackupScheduleMapper : IItemAsyncMapper<BackupScheduleBusiness, WebBackupSchedule>, IItemAsyncMapper<BackupScheduleBusiness, BackupScheduleBusiness>, IItemAsyncMapper<WebBackupSchedule, BackupScheduleBusiness>
{
	public static BackupScheduleMapper Instance { get; private set; } = new BackupScheduleMapper();


	public Task<WebBackupSchedule> MapAsync(string contextGuid, BackupScheduleBusiness source, WebBackupSchedule destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebBackupSchedule();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.ConnectionString = source.ConnectionString;
			destination.Houre = source.Houre;
			destination.CreateDate = source.CreateDate;
			destination.BackupPath = source.BackupPath;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupScheduleMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebBackupSchedule>> MapListAsync(string contextGuid, IEnumerable<BackupScheduleBusiness> source, List<WebBackupSchedule> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupScheduleBusiness> MapAsync(string contextGuid, BackupScheduleBusiness source, BackupScheduleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupScheduleBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.ConnectionString = source.ConnectionString;
			destination.Houre = source.Houre;
			destination.CreateDate = source.CreateDate;
			destination.BackupPath = source.BackupPath;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupScheduleMapper.cs", 67);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupScheduleBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupScheduleBusiness> source, List<BackupScheduleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupScheduleBusiness> MapAsync(string contextGuid, WebBackupSchedule source, BackupScheduleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupScheduleBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.ConnectionString = source.ConnectionString;
			destination.Houre = source.Houre;
			destination.CreateDate = source.CreateDate;
			destination.BackupPath = source.BackupPath;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupScheduleMapper.cs", 97);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupScheduleBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBackupSchedule> source, List<BackupScheduleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
