using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Upload;

public class BackupSettingMapper : IItemAsyncMapper<BackupSettingsBusiness, WebBackupSetting>, IItemAsyncMapper<BackupSettingsBusiness, BackupSettingsBusiness>, IItemAsyncMapper<WebBackupSetting, BackupSettingsBusiness>
{
	public static BackupSettingMapper Instance { get; private set; } = new BackupSettingMapper();


	public Task<WebBackupSetting> MapAsync(string contextGuid, BackupSettingsBusiness source, WebBackupSetting destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebBackupSetting();
			}
			destination.Id = source.Id;
			destination.Username = source.Username;
			destination.Password = source.Password;
			destination.Startup = source.Startup;
			destination.Zip = source.Zip;
			destination.RememberMe = source.RememberMe;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupSettingMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebBackupSetting>> MapListAsync(string contextGuid, IEnumerable<BackupSettingsBusiness> source, List<WebBackupSetting> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupSettingsBusiness> MapAsync(string contextGuid, BackupSettingsBusiness source, BackupSettingsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupSettingsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Id = source.Id;
			destination.Username = source.Username;
			destination.Password = source.Password;
			destination.Startup = source.Startup;
			destination.Zip = source.Zip;
			destination.RememberMe = source.RememberMe;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupSettingMapper.cs", 67);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupSettingsBusiness>> MapListAsync(string contextGuid, IEnumerable<BackupSettingsBusiness> source, List<BackupSettingsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BackupSettingsBusiness> MapAsync(string contextGuid, WebBackupSetting source, BackupSettingsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BackupSettingsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Id = source.Id;
			destination.Username = source.Username;
			destination.Password = source.Password;
			destination.Startup = source.Startup;
			destination.Zip = source.Zip;
			destination.RememberMe = source.RememberMe;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\BackupSettingMapper.cs", 97);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BackupSettingsBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBackupSetting> source, List<BackupSettingsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
