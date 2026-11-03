using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Upload;

public class BackupLogMapper : IItemAsyncMapper<BackupLogBusiness, WebBackupLog>, IItemAsyncMapper<BackupLogBusiness, BackupLogBusiness>, IItemAsyncMapper<WebBackupLog, BackupLogBusiness>
{
	public static BackupLogMapper Instance { get; private set; } = new BackupLogMapper();


	public Task<WebBackupLog> MapAsync(string contextGuid, BackupLogBusiness source, WebBackupLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebBackupLog();
			}
			destination.Id = source.Id;
			destination.DPId = source.DPId;
			destination.FileId = source.FileId;
			destination.LastBackup = source.LastBackup;
			destination.Status = source.Status;
			destination.Path = source.Path;
			destination.LastStateChange = source.LastStateChange;
			destination.Message = source.Message;
			destination.RetryCount = source.RetryCount;
			destination.TotalCrc = source.TotalCrc;
			destination.URL = source.URL;
			destination.FileName = source.FileName;
			destination.BackupType = source.BackupType;
			destination.IsDelete = source.IsDelete;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupLogMapper.cs", 44);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebBackupLog>> MapListAsync(string contextGuid, IEnumerable<BackupLogBusiness> source, List<WebBackupLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupLogBusiness> MapAsync(string contextGuid, BackupLogBusiness source, BackupLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Id = source.Id;
			destination.DPId = source.DPId;
			destination.FileId = source.FileId;
			destination.LastBackup = source.LastBackup;
			destination.Status = source.Status;
			destination.Path = source.Path;
			destination.LastStateChange = source.LastStateChange;
			destination.Message = source.Message;
			destination.RetryCount = source.RetryCount;
			destination.TotalCrc = source.TotalCrc;
			destination.URL = source.URL;
			destination.FileName = source.FileName;
			destination.BackupType = source.BackupType;
			destination.IsDelete = source.IsDelete;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupLogMapper.cs", 81);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupLogBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupLogBusiness> source, List<BackupLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupLogBusiness> MapAsync(string contextGuid, WebBackupLog source, BackupLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Id = source.Id;
			destination.DPId = source.DPId;
			destination.FileId = source.FileId;
			destination.LastBackup = source.LastBackup;
			destination.Status = source.Status;
			destination.Path = source.Path;
			destination.LastStateChange = source.LastStateChange;
			destination.Message = source.Message;
			destination.RetryCount = source.RetryCount;
			destination.TotalCrc = source.TotalCrc;
			destination.URL = source.URL;
			destination.FileName = source.FileName;
			destination.BackupType = source.BackupType;
			destination.IsDelete = source.IsDelete;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupLogMapper.cs", 118);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupLogBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBackupLog> source, List<BackupLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
