using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.EventArguments;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class CustomerWebServiceRepository : WebServiceDetailRepository<CusBusiness>, IDisposable
{
	private static readonly SemaphoreSlim saveSemaphor = new SemaphoreSlim(1);

	protected List<WebCus> Entities { get; set; }

	public event EventHandler<CountChangedEventArgs> CountChanged;

	public static event EventHandler ClientSyncDoneResponse;

	public CustomerWebServiceRepository(ClsCache cache)
		: base((IDetailRepository<CusBusiness>)cache.CacheRepository.ICustomer, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebCus.OnReadResponse += Customer_OnReadResponse;
			Cache.Context.WebCus.OnSync += Customer_OnSync;
			Cache.Context.WebCus.OnDeleteRequest += Customer_OnDeleteRequest;
			if (cache.Source == ENSource.Accounting)
			{
				Cache.Context.WebCus.OnSaveRequest += Customer_OnSaveRequest;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 34);
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		return base.CacheRepository.DbCountAsync(status, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			Cache.Context.WebCus.OnReadResponse -= Customer_OnReadResponse;
			Cache.Context.WebCus.OnSync -= Customer_OnSync;
			Cache.Context.WebCus.OnDeleteRequest -= Customer_OnDeleteRequest;
			Cache.Context.WebCus.OnSaveRequest -= Customer_OnSaveRequest;
			this.CountChanged = null;
			Entities = null;
			CustomerWebServiceRepository.ClientSyncDoneResponse = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 53);
		}
	}

	private async Task Customer_OnSaveRequest(object sender, ConnectionEventArgs<WebCus> e)
	{
		try
		{
			bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate = Cache.Source != ENSource.Accounting;
			bool sendToServer = Cache.Source == ENSource.Accounting;
			ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue = await CusBusiness.SaveRangeAsync(base.ContextGuid, isFromSync: false, sendToServer, EnSystem.SignalR, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, e.ConnectionData, e.Connection.cancellationToken, Priority.High, awaitOnSemaphore: true, updateAccount: false, 10);
			Cache.SendData(WebCus.GenerateSaveResponse(returnedSaveFuncInfoWithValue.value), Priority.High);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				WebTelegramMessage.GetNp_UnseenErrors_bot().Sent2web("خطا در ثبت شخص در نرم افزار مالی".GetTranslate() + "\r\nCustomer_OnSaveRequest\r\ndata=>" + Json.ToStringJson(e), null, "Customer_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 63);
			}
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Customer_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 68);
		}
	}

	private async Task Customer_OnDeleteRequest(object sender, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("Customer_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source == ENSource.CrmPortal || Cache.Source == ENSource.Baje)
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.Customer, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
					return;
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("Customer Ondelete received but data is null!", "", "Customer_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 84);
					return;
				}
				if (Cache.Source == ENSource.Android_HesabYar || Cache.Source == ENSource.IOS_HesabYar || Cache.Source == ENSource.Android_SefareshGir)
				{
					(await CusBusiness.DeleteRangeAsync(e.Connection.ContextGuid, e.ConnectionData, awaitOnSemaphore: true, 10)).throwExceptionIfError();
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData2 = Sync.GenerateDeleteResponse(EntityPart.Customer, e.PartitionId);
					await Cache.SendData(packetData2, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Customer_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 100);
			}
		}
	}

	private async Task Customer_OnSync(object sender, ConnectionEventArgs<WebCus> e)
	{
		using (new JaegerMonitor("Customer_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs"))
		{
			int line = 0;
			bool semaphoreHasBeenTaken = false;
			try
			{
				if (Cache.Source == ENSource.Accounting)
				{
					throw new NotImplementedException("نباید سرور چنین داده ای ارسال کند!".GetTranslate());
				}
				line = 1;
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					line = 3;
					WebErrorLog.Instance.Warning("Customer sync received but data is null!", "", "Customer_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 115);
					return;
				}
				line = 6;
				List<WebCus> entityList = e.ConnectionData.ToList();
				line = 7;
				bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate = Cache.Source != ENSource.Accounting;
				await saveSemaphor.WaitAsync();
				semaphoreHasBeenTaken = true;
				ReturnedSaveFuncInfoWithValue<List<SaveResponse>> res1 = await CusBusiness.SaveRangeAsync(Cache.Context.Guid, isFromSync: true, sendToServer: false, EnSystem.SignalR, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, entityList, e.Connection.cancellationToken, Priority.High, awaitOnSemaphore: true, updateAccount: false, 10);
				line = 800;
				if ((e?.Connection?.cancellationToken.IsCancellationRequested).GetValueOrDefault(true))
				{
					return;
				}
				line = 801;
				res1.throwExceptionIfError();
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 9;
				if (this.CountChanged != null)
				{
					int count = await base.CacheRepository.DbCountAsync(true, default(CancellationToken), "");
					Task.Run(delegate
					{
						this.CountChanged?.Invoke(this, new CountChangedEventArgs(count));
					});
				}
				line = 10;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 11;
					Packet packetData = Sync.GenerateSyncDone(EntityPart.Customer, e.PartitionId, res1);
					line = 12;
					await Cache.SendData(packetData, Priority.High);
					line = 13;
					CustomerWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
					line = 15;
				}
				line = 16;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line = {line}", "Customer_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 151);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					saveSemaphor?.Release();
				}
			}
		}
	}

	private async Task Customer_OnReadResponse(object sender, ConnectionEventArgs<WebCus> e)
	{
		using (new JaegerMonitor("Customer_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs"))
		{
			bool semaphoreHasBeenTaken = false;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("Customer received but data is null!", "", "Customer_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 166);
					return;
				}
				bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate = Cache.Source != ENSource.Accounting;
				bool send2server = Cache.Source == ENSource.Accounting;
				await saveSemaphor.WaitAsync();
				semaphoreHasBeenTaken = true;
				await CusBusiness.SaveRangeAsync(Cache.Context.Guid, isFromSync: false, send2server, EnSystem.SignalR, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, e.ConnectionData, e.Connection.cancellationToken, Priority.High, awaitOnSemaphore: true, updateAccount: false, 10);
				if (this.CountChanged != null)
				{
					int count = await base.CacheRepository.DbCountAsync(true, e.Connection.cancellationToken, "");
					Task.Run(delegate
					{
						this.CountChanged?.Invoke(this, new CountChangedEventArgs(count));
					});
				}
				if (CustomerWebServiceRepository.ClientSyncDoneResponse != null)
				{
					Task.Run(delegate
					{
						CustomerWebServiceRepository.ClientSyncDoneResponse?.Invoke(this, EventArgs.Empty);
					});
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Customer_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerWebServiceRepository.cs", 185);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					saveSemaphor?.Release();
				}
			}
		}
	}
}
