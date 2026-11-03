using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class BiometricDetailMapper : IItemAsyncMapper<BiometricDetailBusiness, BiometricDetailBusiness>, IItemAsyncMapper<WebBiometricDetail, BiometricDetailBusiness>, IItemAsyncMapper<BiometricDetailBusiness, WebBiometricDetail>
{
	public static BiometricDetailMapper Instance { get; private set; } = new BiometricDetailMapper();


	public Task<BiometricDetailBusiness> MapAsync(string contextGuid, BiometricDetailBusiness source, BiometricDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BiometricDetailBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.EnrolType = source.EnrolType;
			destination.Ordinal = source.Ordinal;
			destination.Data = source.Data;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BiometricDetailMapper.cs", 39);
			throw;
		}
	}

	public Task<BiometricDetailBusiness> MapAsync(string contextGuid, WebBiometricDetail source, BiometricDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BiometricDetailBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.EnrolType = source.EnrolType;
			destination.Ordinal = source.Ordinal;
			destination.Data = source.Data;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BiometricDetailMapper.cs", 64);
			throw;
		}
	}

	public Task<WebBiometricDetail> MapAsync(string contextGuid, BiometricDetailBusiness source, WebBiometricDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebBiometricDetail();
			}
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.EnrolType = source.EnrolType;
			destination.Ordinal = source.Ordinal;
			destination.Data = source.Data;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\Biometric\\BiometricDetailMapper.cs", 88);
			throw;
		}
	}

	public Task<List<BiometricDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<BiometricDetailBusiness> source, List<BiometricDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<BiometricDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBiometricDetail> source, List<BiometricDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebBiometricDetail>> MapListAsync(string contextGuid, IEnumerable<BiometricDetailBusiness> source, List<WebBiometricDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
