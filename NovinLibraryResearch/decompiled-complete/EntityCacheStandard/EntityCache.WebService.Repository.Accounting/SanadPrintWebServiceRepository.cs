using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace EntityCache.WebService.Repository.Accounting;

public class SanadPrintWebServiceRepository : WebServiceRepository<SanadPrintBusiness>, IDisposable
{
	public SanadPrintWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<SanadPrintBusiness>)cache.CacheRepository.ISanaDPrint, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.SanadPrint.OnReadResponse += Instance_OnReadResponse;
			Cache.Context.SanadPrint.OnSync += Instance_OnSync;
			Cache.Context.SanadPrint.OnSaveRequest += Instance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs", 27);
		}
	}

	private async Task Instance_OnSync(object sender, ConnectionEventArgs<WebSanadPrint> e)
	{
		using (new JaegerMonitor("Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs"))
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
				if (e.ConnectionData != null && Cache.Source != ENSource.Accounting && (e.ConnectionData?.Any() ?? false))
				{
					List<WebSanadPrint> source = e.ConnectionData.ToList();
					List<SanadPrintBusiness> lst = await SanadPrintMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await SanadPrintBusiness.SaveRangeAsync(EnSystem.SignalR, sendToServer: false, e.Connection.cancellationToken, lst);
					returnedSaveFuncInfo.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.SanadPrint, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs", 48);
			}
		}
	}

	private async Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebSanadPrint> e)
	{
		using (new JaegerMonitor("Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData != null && Cache.Source != ENSource.Accounting)
				{
					List<WebSanadPrint> connectionData = e.ConnectionData;
					if (connectionData == null || connectionData.Any())
					{
						List<WebSanadPrint> source = e.ConnectionData.ToList();
						List<SanadPrintBusiness> lst = await SanadPrintMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
						await SanadPrintBusiness.SaveRangeAsync(EnSystem.SignalR, sendToServer: false, e.Connection.cancellationToken, lst);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs", 61);
			}
		}
	}

	private async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebSanadPrint> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs"))
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
					Log.Warning("WebSanadPrint received but data is null or empty!");
					return;
				}
				List<SanadPrintBusiness> list = await SanadPrintMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || !list.Any())
				{
					throw new Exception("data was exists but mapped data is empty!");
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (SanadPrintBusiness data in list)
				{
					bool flag = Cache.Source == ENSource.Accounting;
					if (flag)
					{
						data.ServerStatus = ServerStatusForClient.None;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await data.SaveAsync(EnSystem.SignalR, flag, Priority.Low, e.Connection.cancellationToken);
					WebDelivery item = new WebDelivery
					{
						Guid = data.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.SanadPrint
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
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs", 103);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.SanadPrint.OnReadResponse -= Instance_OnReadResponse;
				Cache.Context.SanadPrint.OnSync -= Instance_OnSync;
				Cache.Context.SanadPrint.OnSaveRequest -= Instance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\SanadPrintWebServiceRepository.cs", 115);
		}
	}
}
