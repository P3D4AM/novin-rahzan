using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm;
using EntityCache.Business.Crm.Calling;
using EntityCache.Business.Crm.Report;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Filters.Crm;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Crm.Calling;

public class CallingLogMapper : IItemAsyncMapper<CallingLogBusiness, WebCallingLog>, IItemAsyncMapper<CallingLogBusiness, CallingLogBusiness>, IItemAsyncMapper<WebCallingLog, CallingLogBusiness>, IItemAsyncMapper<CallingLogReportBusiness, WebCallingLogReport>, IItemAsyncMapper<WebCallingLogReport, CallingLogReportBusiness>, IItemAsyncMapper<CallingLogReportBusiness, WebCallingLog>
{
	public static CallingLogMapper Instance { get; private set; } = new CallingLogMapper();


	public async Task<WebCallingLog> MapAsync(string contextGuid, CallingLogBusiness source, WebCallingLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 6;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebCallingLog();
			}
			destination.Modified = source.Modified;
			destination.MasterUuid = source.MasterUuid;
			destination.CallerIdNumber = source.CallerIdNumber;
			destination.Rings = source.Rings;
			destination.DialedNumber = source.DialedNumber;
			destination.PickUpDateM = source.PickUpDateM;
			destination.HangUpDateM = source.HangUpDateM;
			destination.DateM = source.DateM;
			destination.CoGuid = source.CoGuid;
			destination.ExGuid = source.ExGuid;
			destination.Status = source.Status;
			destination.IsIncoming = source.IsIncoming;
			destination.IsCompleteCall = source.IsCompleteCall;
			destination.Description = source.Description;
			destination.DateSh = source.DateSh;
			destination.Version = source.Version;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.CalcedNumber = source.CalcedNumber;
			destination.Uuid = source.Uuid;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			WebCallingLog webCallingLog = destination;
			webCallingLog.FullName = await source.GetTafsilNameAsync(EnSystem.Mapper, tranName, token);
			Guid? exGuid = source.ExGuid;
			if (!exGuid.HasValue)
			{
				_ = Guid.Empty;
			}
			else
			{
				exGuid.GetValueOrDefault();
			}
			webCallingLog = destination;
			CallingLineMapper instance = CallingLineMapper.Instance;
			string contextGuid2 = contextGuid;
			webCallingLog.Extension = await instance.MapAsync(contextGuid2, await source.GetExtentionAsync(token), destination.Extension, tranName, token);
			if ((source.CoGuid ?? Guid.Empty) == Guid.Empty)
			{
				destination.Co = new WebCallingLine();
			}
			else
			{
				webCallingLog = destination;
				instance = CallingLineMapper.Instance;
				contextGuid2 = contextGuid;
				webCallingLog.Co = await instance.MapAsync(contextGuid2, await source.GetCoAsync(tranName, token), destination.Co, tranName, token);
			}
			IEnumerable<CallingLogDetailBusiness> source2 = await CallingLogDetailBusiness.GetAllAsync(contextGuid, new CallingLogDetailFilter
			{
				LinkedId = source.Uuid
			}, token, tranName);
			webCallingLog = destination;
			webCallingLog.ListCallingLogDetail = await CallingLogDetailMapper.Instance.MapListAsync(contextGuid, source2, destination.ListCallingLogDetail, tranName, token);
			destination.DateUtc = source.DateUtc;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogMapper.cs", 79);
			throw;
		}
	}

	public Task<CallingLogBusiness> MapAsync(string contextGuid, CallingLogBusiness source, CallingLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CallingLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.MasterUuid = source.MasterUuid;
			destination.Modified = source.Modified;
			destination.CallerIdNumber = source.CallerIdNumber;
			destination.Rings = source.Rings;
			destination.DialedNumber = source.DialedNumber;
			destination.PickUpDateM = source.PickUpDateM;
			destination.HangUpDateM = source.HangUpDateM;
			destination.DateM = source.DateM;
			destination.CoGuid = source.CoGuid;
			destination.ExGuid = source.ExGuid;
			destination.Status = source.Status;
			destination.IsIncoming = source.IsIncoming;
			destination.IsCompleteCall = source.IsCompleteCall;
			destination.Description = source.Description;
			destination.Version = source.Version;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Uuid = source.Uuid;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			if (source.Rings < destination.Rings)
			{
				WebErrorLog.Instance.StartLog("لاگ جدید رینگ \r\n شیء Business: \r\n " + Json.ToStringJson(destination) + "\r\n شیء Web: \r\n" + Json.ToStringJson(source) + "\r\nاین داده از سرور اومده", "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogMapper.cs", 120);
			}
			destination.DateUtc = source.DateUtc;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogMapper.cs", 128);
			throw;
		}
	}

	public Task<CallingLogBusiness> MapAsync(string contextGuid, WebCallingLog source, CallingLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CallingLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Modified = source.Modified;
			destination.MasterUuid = source.MasterUuid;
			destination.CallerIdNumber = source.CallerIdNumber;
			destination.Rings = source.Rings;
			destination.DialedNumber = source.DialedNumber;
			destination.PickUpDateM = source.PickUpDateM;
			destination.HangUpDateM = source.HangUpDateM;
			destination.DateM = source.DateM;
			destination.CoGuid = source.CoGuid;
			destination.ExGuid = source.ExGuid;
			destination.Status = source.Status;
			destination.IsIncoming = source.IsIncoming;
			destination.IsCompleteCall = source.IsCompleteCall;
			destination.Description = source.Description;
			destination.Version = source.Version;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Uuid = source.Uuid;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			destination.DateUtc = source.DateUtc;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogMapper.cs", 172);
			throw;
		}
	}

	public Task<CallingLogReportBusiness> MapAsync(string contextGuid, WebCallingLogReport source, CallingLogReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CallingLogReportBusiness();
			}
			destination.TicketGuid = source.TicketGuid;
			destination.MasterUuid = source.MasterUuid;
			destination.CallerIdNumber = source.CallerIdNumber;
			destination.DialedNumber = source.DialedNumber;
			destination.Status = source.Status;
			destination.IsIncoming = source.IsIncoming;
			destination.IsCompleteCall = source.IsCompleteCall;
			destination.CalcedNumber = source.CalcedNumber;
			destination.Uuid = source.Uuid;
			destination.Status = source.Status;
			destination.NumberForDisplay = source.NumberForDisplay;
			destination.CashierList = source.CashierList;
			destination.DateM = source.DateM;
			destination.HangUpTime = source.HangUpTime;
			destination.PickUpTime = source.PickUpTime;
			destination.CallToName = source.CallToName;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogMapper.cs", 208);
			throw;
		}
	}

	public Task<WebCallingLogReport> MapAsync(string contextGuid, CallingLogReportBusiness source, WebCallingLogReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCallingLogReport();
			}
			destination.TicketGuid = source.TicketGuid;
			destination.CallerIdNumber = source.CallerIdNumber;
			destination.DialedNumber = source.DialedNumber;
			destination.Status = source.Status;
			destination.IsIncoming = source.IsIncoming;
			destination.IsCompleteCall = source.IsCompleteCall;
			destination.CalcedNumber = source.CalcedNumber;
			destination.Uuid = source.Uuid;
			destination.MasterUuid = source.MasterUuid;
			destination.Status = source.Status;
			destination.NumberForDisplay = source.NumberForDisplay;
			destination.StatusConversation = source.StatusConversation;
			destination.CashierList = source.CashierList;
			destination.DateM = source.DateM;
			destination.HangUpTime = source.HangUpTime;
			destination.PickUpTime = source.PickUpTime;
			destination.CallToName = source.CallToName;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogMapper.cs", 245);
			throw;
		}
	}

	public async Task<WebCallingLog> MapAsync(string contextGuid, CallingLogReportBusiness source, WebCallingLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebCallingLog();
			}
			destination.MasterUuid = source.MasterUuid;
			destination.CallerIdNumber = source.CallerIdNumber;
			destination.Rings = source.Rings;
			destination.DialedNumber = source.DialedNumber;
			destination.PickUpDateM = source.PickUpDateM;
			destination.HangUpDateM = source.HangUpDateM;
			destination.DateM = source.DateM;
			destination.Status = source.Status;
			destination.IsIncoming = source.IsIncoming;
			destination.IsCompleteCall = source.IsCompleteCall;
			destination.DateSh = source.DateSh;
			destination.Version = source.Version;
			destination.CalcedNumber = source.CalcedNumber;
			destination.Uuid = source.Uuid;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			IEnumerable<CallingLogDetailBusiness> source2 = await CallingLogDetailBusiness.GetAllAsync(contextGuid, new CallingLogDetailFilter
			{
				LinkedId = source.Uuid
			}, token, tranName);
			WebCallingLog webCallingLog = destination;
			webCallingLog.ListCallingLogDetail = await CallingLogDetailMapper.Instance.MapListAsync(contextGuid, source2, destination.ListCallingLogDetail, tranName, token);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogMapper.cs", 282);
			throw;
		}
	}

	public Task<List<WebCallingLog>> MapListAsync(string contextGuid, IEnumerable<CallingLogBusiness> source, List<WebCallingLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CallingLogBusiness>> MapListAsync(string contextGuid, IEnumerable<CallingLogBusiness> source, List<CallingLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCallingLogReport>> MapListAsync(string contextGuid, IEnumerable<CallingLogReportBusiness> source, List<WebCallingLogReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CallingLogBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCallingLog> source, List<CallingLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CallingLogReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCallingLogReport> source, List<CallingLogReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCallingLog>> MapListAsync(string contextGuid, IEnumerable<CallingLogReportBusiness> source, List<WebCallingLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
