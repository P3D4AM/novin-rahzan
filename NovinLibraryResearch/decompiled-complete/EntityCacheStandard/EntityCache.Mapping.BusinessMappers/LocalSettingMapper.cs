using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Global.LocalSetting;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class LocalSettingMapper : IItemAsyncMapper<LocalSettingBusiness, LocalSettingBusiness>, IItemAsyncMapper<LocalSettingBusiness, WebLocalSetting>, IItemAsyncMapper<WebLocalSetting, LocalSettingBusiness>
{
	public static LocalSettingMapper Instance { get; private set; } = new LocalSettingMapper();


	public Task<LocalSettingBusiness> MapAsync(string contextGuid, LocalSettingBusiness source, LocalSettingBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LocalSettingBusiness(contextGuid);
			}
			destination.Key = source.Key;
			destination.Value = source.Value;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\LocalSettingMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<LocalSettingBusiness> MapAsync(string contextGuid, WebLocalSetting source, LocalSettingBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LocalSettingBusiness(contextGuid);
			}
			destination.Key = source.Key;
			destination.Value = source.Value;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\LocalSettingMapper.cs", 55);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebLocalSetting> MapAsync(string contextGuid, LocalSettingBusiness source, WebLocalSetting destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebLocalSetting();
			}
			destination.Key = source.Key;
			destination.Value = source.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\LocalSettingMapper.cs", 75);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<LocalSettingBusiness>> MapListAsync(string contextGuid, IEnumerable<LocalSettingBusiness> source, List<LocalSettingBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LocalSettingBusiness>> MapListAsync(string contextGuid, IEnumerable<WebLocalSetting> source, List<LocalSettingBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebLocalSetting>> MapListAsync(string contextGuid, IEnumerable<LocalSettingBusiness> source, List<WebLocalSetting> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
