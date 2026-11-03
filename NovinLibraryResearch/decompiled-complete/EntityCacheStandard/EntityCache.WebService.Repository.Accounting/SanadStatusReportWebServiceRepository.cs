using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Report;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace EntityCache.WebService.Repository.Accounting;

public class SanadStatusReportWebServiceRepository : WebServiceRepository<SanadStatusReportBusiness>, IDisposable
{
	public SanadStatusReportWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<SanadStatusReportBusiness>)cache.CacheRepository.SanadStatusReport, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.SanadStatusReport.OnReadResponse += Instance_OnReadResponse;
			Cache.Context.SanadStatusReport.OnSync += Instance_OnSync;
			Cache.Context.SanadStatusReport.OnSaveRequest += Instance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs", 26);
		}
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebSanadStatusReport> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs"))
		{
			CancellationToken? cancellationToken = e?.Connection?.cancellationToken;
			if (!cancellationToken.HasValue)
			{
				new CancellationToken(canceled: true);
			}
			else
			{
				cancellationToken.GetValueOrDefault();
			}
			try
			{
				if (e.ConnectionData?.Any() ?? false)
				{
					List<WebSanadStatusReport> source = e.ConnectionData.ToList();
					bool send2Server = Cache.Source == ENSource.Accounting;
					List<SanadStatusReportBusiness> lst = await SanadStatusReportMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await SanadStatusReportBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, lst, send2Server);
					if (!returnedSaveFuncInfo.HasError && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.SanadStatusReport, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs", 48);
			}
		}
	}

	private async Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebSanadStatusReport> e)
	{
		using (new JaegerMonitor("Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					Log.Warning("WebSanadStatusReport received but data is empty!");
					return;
				}
				List<WebSanadStatusReport> source = e.ConnectionData.ToList();
				List<SanadStatusReportBusiness> lst = await SanadStatusReportMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				bool send2Server = Cache.Source == ENSource.Accounting;
				await SanadStatusReportBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, lst, send2Server);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs", 65);
			}
		}
	}

	private async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebSanadStatusReport> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					throw new Exception("فراخوانی سیو رکوئست وضعیت اسناد در محصولی بغیر از حسابداری".GetTranslate());
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					Log.Warning("WebSanadStatusReport received but data is null or empty!");
					return;
				}
				List<SanadStatusReportBusiness> list = await SanadStatusReportMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || !list.Any())
				{
					throw new Exception("data was exists but mapped data is empty!");
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (SanadStatusReportBusiness data in list)
				{
					bool flag = Cache.Source == ENSource.Accounting;
					if (flag)
					{
						data.ServerStatus = ServerStatusForClient.None;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await data.SaveAsync(EnSystem.SignalR, flag, null, await4send: false, e.Connection.cancellationToken, 10);
					WebDelivery item = new WebDelivery
					{
						Guid = data.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.SanadStatusReport
					};
					deliveryList.Add(item);
				}
				if (deliveryList?.Any() ?? false)
				{
					List<Packet> lstPacket = WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId);
					await Cache.SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs", 106);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.SanadStatusReport.OnReadResponse -= Instance_OnReadResponse;
				Cache.Context.SanadStatusReport.OnSync -= Instance_OnSync;
				Cache.Context.SanadStatusReport.OnSaveRequest -= Instance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadStatusReportWebServiceRepository.cs", 118);
		}
	}
}
