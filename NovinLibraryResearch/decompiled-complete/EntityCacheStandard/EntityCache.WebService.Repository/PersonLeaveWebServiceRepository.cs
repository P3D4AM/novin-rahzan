using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class PersonLeaveWebServiceRepository : WebServiceRepository<AttPersonLeaveBusiness>, IDisposable
{
	public event Func<object, AttPersonLeaveBusiness, Task> OnLeaveAccepted;

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.AttPersonLeave.OnReadResponse -= Instance_OnReadResponse;
				Cache.Context.AttPersonLeave.OnSync -= Instance_OnSync;
				Cache.Context.AttPersonLeave.OnSaveRequest -= Instance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs", 30);
		}
	}

	public PersonLeaveWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<AttPersonLeaveBusiness>)cache.CacheRepository.IAttPersonLeave, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.AttPersonLeave.OnReadResponse += Instance_OnReadResponse;
			Cache.Context.AttPersonLeave.OnSync += Instance_OnSync;
			Cache.Context.AttPersonLeave.OnSaveRequest += Instance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs", 42);
		}
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebAttPersonLeave> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs"))
		{
			new ReturnedSaveFuncInfo();
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs", 68);
			}
		}
	}

	private async Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebAttPersonLeave> e)
	{
		using (new JaegerMonitor("Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Error("AttPersonReport sync received but data is null!", "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs", 77);
					return;
				}
				List<WebAttPersonLeave> source = e.ConnectionData.ToList();
				await AttPersonLeaveBusiness.SaveRangeAsync(sendToServer: Cache.Source == ENSource.TimeAttendance, system: EnSystem.SignalR, lst: await AttPersonLeaveMapper.Instance.MapListAsync(e.Connection.Context.Guid, source, new List<AttPersonLeaveBusiness>()), priority: Priority.Medium, awaitOnSemaphore: true, token: e.Connection.cancellationToken, tryCount: 10);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs", 85);
			}
		}
	}

	private async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebAttPersonLeave> e)
	{
		_ = 4;
		try
		{
			if (Cache.Source != ENSource.TimeAttendance)
			{
				throw new NotImplementedException();
			}
			List<WebAttPersonLeave> source = e.ConnectionData.ToList();
			List<AttPersonLeaveBusiness> list = await AttPersonLeaveMapper.Instance.MapListAsync(e.Connection.Context.Guid, source, new List<AttPersonLeaveBusiness>(), "", e.Connection.cancellationToken);
			List<SaveResponse> listResponse = new List<SaveResponse>();
			foreach (AttPersonLeaveBusiness item in list)
			{
				if (e.Connection.cancellationToken.IsCancellationRequested || item == null)
				{
					return;
				}
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				AttPersonLeaveBusiness attPersonLeaveBusiness = await AttPersonLeaveBusiness.GetFromDatabaseAsync(base.ContextGuid, item.Guid, e.Connection.cancellationToken);
				if (attPersonLeaveBusiness == null || attPersonLeaveBusiness.LeaveStatus != item.LeaveStatus)
				{
					item.ServerStatus = ServerStatusForClient.Delivered;
					res = await item.SaveAsync(EnSystem.SignalR, sendToServer: false, Priority.Low, e.Connection.cancellationToken);
					if (!res.HasError && item.LeaveStatus == EnPersonLeaveStatus.Accepted)
					{
						PacketParser.Utilities.RaiseEvent(this.OnLeaveAccepted, this, item, e.Connection.cancellationToken, 60, "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs");
					}
				}
				listResponse.Add(new SaveResponse(0, item.Guid, DateTime.Now, !res.HasError, res.ErrorMessage, res.ErrorMessage));
			}
			if (!e.Connection.cancellationToken.IsCancellationRequested)
			{
				await Cache.SendData(WebAttPersonLeave.GenerateSaveResponse(listResponse, e.PacketReferenceId), Priority.High);
				await Cache.SendData(WebDelivery.GenerateReadResponse(listResponse.Select((SaveResponse p) => new WebDelivery
				{
					DeliveryDate = DateTime.Now,
					DeliveryId = 0,
					DeliveryStatus = p.SaveResultStatus,
					Guid = p.Guid,
					Message = p.Message,
					ShortMessage = p.Shortmessage,
					Status = true,
					Modified = DateTime.Now,
					Part = EntityPart.AttPersonLeave
				}), e.PacketReferenceId), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Attendance\\PersonLeaveWebServiceRepository.cs", 127);
		}
	}
}
