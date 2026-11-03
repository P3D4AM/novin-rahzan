using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm.Calling;
using EntityCache.Business.Crm.Report;
using PacketParser.Entities;
using PacketParser.Entities.CRM.CallCenter;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Crm.Calling;

public class CallingLogDetailMapper : IItemAsyncMapper<CallingLogDetailBusiness, CallingLogDetailBusiness>, IItemAsyncMapper<CallingLogDetailBusiness, WebCallingLogDetail>, IItemAsyncMapper<WebCallingLogDetail, CallingLogDetailBusiness>, IItemAsyncMapper<WebCallingLogDetailReport, CallingLogDetailReportBusiness>, IItemAsyncMapper<CallingLogDetailReportBusiness, WebCallingLogDetailReport>
{
	public static CallingLogDetailMapper Instance { get; private set; } = new CallingLogDetailMapper();


	public Task<CallingLogDetailBusiness> MapAsync(string contextGuid, CallingLogDetailBusiness source, CallingLogDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CallingLogDetailBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.DateInsert = source.DateInsert;
			destination.HangUpDateM = source.HangUpDateM;
			destination.PickUpDateM = source.PickUpDateM;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.CallFrom_Number = source.CallFrom_Number;
			destination.CallFrom_Name = source.CallFrom_Name;
			destination.CallTo_Name = source.CallTo_Name;
			destination.CallTo_Number = source.CallTo_Number;
			destination.Description = source.Description;
			destination.Uuid = source.Uuid;
			destination.LinkedId = source.LinkedId;
			destination.Rings = source.Rings;
			destination.Version = source.Version;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogDetailMapper.cs", 52);
			throw;
		}
	}

	public Task<WebCallingLogDetail> MapAsync(string contextGuid, CallingLogDetailBusiness source, WebCallingLogDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCallingLogDetail();
			}
			destination.CashierGuid = source.CashierGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.DateInsert = source.DateInsert;
			destination.HangUpDateM = source.HangUpDateM;
			destination.PickUpDateM = source.PickUpDateM;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.CallFrom_Number = source.CallFrom_Number;
			destination.CallFrom_Name = source.CallFrom_Name;
			destination.CallTo_Name = source.CallTo_Name;
			destination.CallTo_Number = source.CallTo_Number;
			destination.Description = source.Description;
			destination.Uuid = source.Uuid;
			destination.LinkedId = source.LinkedId;
			destination.Rings = source.Rings;
			destination.Version = source.Version;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogDetailMapper.cs", 86);
			throw;
		}
	}

	public Task<CallingLogDetailBusiness> MapAsync(string contextGuid, WebCallingLogDetail source, CallingLogDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CallingLogDetailBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.DateInsert = source.DateInsert;
			destination.HangUpDateM = source.HangUpDateM;
			destination.PickUpDateM = source.PickUpDateM;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.CallFrom_Number = source.CallFrom_Number;
			destination.CallFrom_Name = source.CallFrom_Name;
			destination.CallTo_Name = source.CallTo_Name;
			destination.CallTo_Number = source.CallTo_Number;
			destination.Description = source.Description;
			destination.Uuid = source.Uuid;
			destination.LinkedId = source.LinkedId;
			destination.Rings = source.Rings;
			destination.Version = source.Version;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogDetailMapper.cs", 121);
			throw;
		}
	}

	public Task<CallingLogDetailReportBusiness> MapAsync(string contextGuid, WebCallingLogDetailReport source, CallingLogDetailReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CallingLogDetailReportBusiness();
			}
			destination.CashierGuid = source.CashierGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.DateInsert = source.DateInsert;
			destination.HangUpDateM = source.HangUpDateM;
			destination.PickUpDateM = source.PickUpDateM;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.CallFrom_Number = source.CallFrom_Number;
			destination.CallFrom_Name = source.CallFrom_Name;
			destination.CallTo_Name = source.CallTo_Name;
			destination.CallTo_Number = source.CallTo_Number;
			destination.Description = source.Description;
			destination.Uuid = source.Uuid;
			destination.LinkedId = source.LinkedId;
			destination.Rings = source.Rings;
			destination.Duration = source.Duration;
			destination.Status = source.Status;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogDetailMapper.cs", 159);
			throw;
		}
	}

	public Task<WebCallingLogDetailReport> MapAsync(string contextGuid, CallingLogDetailReportBusiness source, WebCallingLogDetailReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCallingLogDetailReport();
			}
			destination.CashierGuid = source.CashierGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.DateInsert = source.DateInsert;
			destination.HangUpDateM = source.HangUpDateM;
			destination.PickUpDateM = source.PickUpDateM;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.CallFrom_Number = source.CallFrom_Number;
			destination.CallFrom_Name = source.CallFrom_Name;
			destination.CallTo_Name = source.CallTo_Name;
			destination.CallTo_Number = source.CallTo_Number;
			destination.Description = source.Description;
			destination.Uuid = source.Uuid;
			destination.LinkedId = source.LinkedId;
			destination.Rings = source.Rings;
			destination.Duration = source.Duration;
			destination.Status = source.Status;
			destination.Message = source.Message;
			destination.StatusId = source.StatusId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLogDetailMapper.cs", 196);
			throw;
		}
	}

	public Task<List<CallingLogDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<CallingLogDetailBusiness> source, List<CallingLogDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCallingLogDetail>> MapListAsync(string contextGuid, IEnumerable<CallingLogDetailBusiness> source, List<WebCallingLogDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CallingLogDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCallingLogDetail> source, List<CallingLogDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CallingLogDetailReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCallingLogDetailReport> source, List<CallingLogDetailReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCallingLogDetailReport>> MapListAsync(string contextGuid, IEnumerable<CallingLogDetailReportBusiness> source, List<WebCallingLogDetailReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
