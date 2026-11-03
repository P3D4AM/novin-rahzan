using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ErrorLogMapper : IItemAsyncMapper<ErrorLogBusiness, WebErrorLog>, IItemAsyncMapper<ErrorLogBusiness, ErrorLogBusiness>, IItemAsyncMapper<WebErrorLog, ErrorLogBusiness>
{
	public static ErrorLogMapper Instance { get; private set; } = new ErrorLogMapper();


	public Task<WebErrorLog> MapAsync(string contextGuid, ErrorLogBusiness source, WebErrorLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebErrorLog();
			}
			destination.FuncName = source.FuncName;
			destination.Date = source.Date;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.HardSerial = source.HardSerial;
			destination.LKSerial = source.LKSerial;
			destination.CustInfo = source.CustInfo;
			destination.ClassName = source.ClassName;
			destination.Source = source.Source;
			destination.Version = source.Version;
			destination.ScreenShot = source.ScreenShot;
			destination.Fatal = source.Fatal;
			destination.GroupingID = source.GroupingID;
			destination.ExceptionType = source.ExceptionType;
			destination.ExceptionPlace = source.ExceptionPlace;
			destination.Ip = source.Ip;
			destination.HashAll = source.HashAll;
			destination.Path = source.Path;
			destination.LoggerVersion = source.LoggerVersion;
			destination.ExceptionMessage = source.ExceptionMessage;
			destination.RefrencedID = source.RefrencedID;
			destination.ExceptionError = source.ExceptionError;
			destination.StackTrace = source.StackTrace;
			destination.Description = source.Description;
			destination.FlowStackTrace = source.FlowStackTrace;
			destination.MaxCountPerTime = source.MaxCountPerTime;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ErrorLogMapper.cs", 59);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebErrorLog>> MapListAsync(string contextGuid, IEnumerable<ErrorLogBusiness> source, List<WebErrorLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ErrorLogBusiness> MapAsync(string contextGuid, ErrorLogBusiness source, ErrorLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ErrorLogBusiness(contextGuid);
			}
			destination.FuncName = source.FuncName;
			destination.Date = source.Date;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.HardSerial = source.HardSerial;
			destination.LKSerial = source.LKSerial;
			destination.CustInfo = source.CustInfo;
			destination.ClassName = source.ClassName;
			destination.Source = source.Source;
			destination.Version = source.Version;
			destination.ScreenShot = source.ScreenShot;
			destination.Fatal = source.Fatal;
			destination.GroupingID = source.GroupingID;
			destination.ExceptionType = source.ExceptionType;
			destination.ExceptionPlace = source.ExceptionPlace;
			destination.Ip = source.Ip;
			destination.HashAll = source.HashAll;
			destination.Path = source.Path;
			destination.LoggerVersion = source.LoggerVersion;
			destination.ExceptionMessage = source.ExceptionMessage;
			destination.RefrencedID = source.RefrencedID;
			destination.ExceptionError = source.ExceptionError;
			destination.StackTrace = source.StackTrace;
			destination.Description = source.Description;
			destination.FlowStackTrace = source.FlowStackTrace;
			destination.MaxCountPerTime = source.MaxCountPerTime;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ErrorLogMapper.cs", 110);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ErrorLogBusiness>> MapListAsync(string contextGuid, IEnumerable<ErrorLogBusiness> source, List<ErrorLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ErrorLogBusiness> MapAsync(string contextGuid, WebErrorLog source, ErrorLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ErrorLogBusiness(contextGuid);
			}
			destination.FuncName = source.FuncName;
			destination.Date = source.Date;
			destination.AndroidIme = source.AndroidIme;
			destination.CpuSerial = source.CpuSerial;
			destination.HardSerial = source.HardSerial;
			destination.LKSerial = source.LKSerial;
			destination.CustInfo = source.CustInfo;
			destination.ClassName = source.ClassName;
			destination.Source = source.Source;
			destination.Version = source.Version;
			destination.ScreenShot = source.ScreenShot;
			destination.Fatal = source.Fatal;
			destination.GroupingID = source.GroupingID;
			destination.ExceptionType = source.ExceptionType;
			destination.ExceptionPlace = source.ExceptionPlace;
			destination.Ip = source.Ip;
			destination.HashAll = source.HashAll;
			destination.Path = source.Path;
			destination.LoggerVersion = source.LoggerVersion;
			destination.ExceptionMessage = source.ExceptionMessage;
			destination.RefrencedID = source.RefrencedID;
			destination.ExceptionError = source.ExceptionError;
			destination.StackTrace = source.StackTrace;
			destination.Description = source.Description;
			destination.FlowStackTrace = source.FlowStackTrace;
			destination.MaxCountPerTime = source.MaxCountPerTime;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ErrorLogMapper.cs", 161);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ErrorLogBusiness>> MapListAsync(string contextGuid, IEnumerable<WebErrorLog> source, List<ErrorLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
