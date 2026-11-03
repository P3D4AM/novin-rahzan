using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace EntityCache.WebService.Repository;

public class TafsilWebServiceRepository : WebServiceRepository<TafsilBusiness>, IDisposable
{
	private static SemaphoreSlim _dbSemaphore = new SemaphoreSlim(1, 1);

	public event EventHandler<EventArgs> Changed;

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebTafsil.OnReadResponse -= TafsilInstance_OnReadResponse;
				Cache.Context.WebTafsil.OnSync -= TafsilInstance_OnSync;
				Cache.Context.WebTafsil.OnDeleteRequest -= TafsilInstance_OnDeleteRequest;
				Cache.Context.WebTafsil.OnSaveRequest -= TafsilInstance_OnSaveRequest;
				this.Changed = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 34);
		}
	}

	public TafsilWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<TafsilBusiness>)cache.CacheRepository.ITafsil, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebTafsil.OnReadResponse += TafsilInstance_OnReadResponse;
			Cache.Context.WebTafsil.OnSync += TafsilInstance_OnSync;
			Cache.Context.WebTafsil.OnDeleteRequest += TafsilInstance_OnDeleteRequest;
			Cache.Context.WebTafsil.OnSaveRequest += TafsilInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 47);
		}
	}

	private async Task TafsilInstance_OnDeleteRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("TafsilInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs"))
		{
			try
			{
				if (Cache.Source == ENSource.Accounting)
				{
					throw new InvalidOperationException("cache source is accounting");
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("TafsilInstance Ondelete received but data is null!", "", "TafsilInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 58);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("TafsilInstance Ondelete received but data is empty!", "", "TafsilInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 63);
				}
				else if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.Tafsil, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TafsilInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 76);
			}
		}
	}

	private async Task TafsilInstance_OnSync(object sender, ConnectionEventArgs<WebTafsil> e)
	{
		Stopwatch.StartNew();
		Stopwatch swTemp = Stopwatch.StartNew();
		StringBuilder sb = new StringBuilder(100000);
		try
		{
			if (Cache.Source == ENSource.Accounting)
			{
				throw new InvalidOperationException("cache source is accounting");
			}
			using (new JaegerMonitor("TafsilInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs"))
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("TafsilInstance sync received but data is null!", "", "TafsilInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 90);
					return;
				}
				List<WebTafsil> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("TafsilInstance sync received but data is empty!", "", "TafsilInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 95);
					return;
				}
				(await RemoveNotRelatedAccountsAsync(e.ConnectionData, e.Connection.cancellationToken)).throwExceptionIfError();
				List<WebTafsil> source = e.ConnectionData.ToList();
				bool updateAccount = Cache.Source != ENSource.Accounting;
				_ = Cache.Source;
				sb.AppendLine($"BeforeMap Time:{swTemp.ElapsedMilliseconds}(ms)");
				swTemp.Restart();
				List<TafsilBusiness> lstHesabsListTafsil = await TafsilMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				sb.AppendLine($"AfterMap and before SaveRange Time:{swTemp.ElapsedMilliseconds}(ms)");
				swTemp.Restart();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await TafsilBusiness.SaveRangeAsync(base.ContextGuid, EnSystem.SignalR, updateAccount, lstHesabsListTafsil, updateAccount, 10, e.Connection.cancellationToken, 20);
				sb.AppendLine($"AfterSaveRange Time:{swTemp.ElapsedMilliseconds}(ms)");
				swTemp.Restart();
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				returnedSaveFuncInfo.throwExceptionIfError();
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.Tafsil, e.PartitionId, returnedSaveFuncInfo);
					Cache.SendData(packetData, Priority.High);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TafsilInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 130);
		}
		finally
		{
			_ = 0;
		}
	}

	private async Task<ReturnedSaveFuncInfo> RemoveNotRelatedAccountsAsync(List<WebTafsil> connectionData, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		string tranName = Guid.NewGuid().ToString();
		bool transactionHasBeenCreated = false;
		bool semaphoreHasBeenTaken = false;
		try
		{
			_ = 2;
			try
			{
				await _dbSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				List<Tuple<Guid, List<Guid>>> data = connectionData.Select((WebTafsil p) => new Tuple<Guid, List<Guid>>(p.Guid, p.RelatedMoeinGuidList)).ToList();
				token.ThrowIfCancellationRequested();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 153), ReturnedState.None, "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 153);
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Cache.CacheRepository.Account.RemoveNotRelatedAccountsAsync(data, tranName, token), ReturnedState.None, "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 155);
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 157);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 160);
				ret.AddReturnedValue(ex2, "", "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 161);
			}
		}
		finally
		{
			if (transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 166), ReturnedState.None, "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 166);
			}
			if (semaphoreHasBeenTaken)
			{
				_dbSemaphore?.Release();
			}
		}
		return ret;
	}

	private async Task TafsilInstance_OnReadResponse(object sender, ConnectionEventArgs<WebTafsil> e)
	{
		using (new JaegerMonitor("TafsilInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (Cache.Source == ENSource.Accounting)
				{
					throw new InvalidOperationException("cache source is accounting");
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					return;
				}
				List<WebTafsil> source = e.ConnectionData.ToList();
				bool updateAccount = Cache.Source != ENSource.Accounting;
				_ = Cache.Source;
				string contextGuid = base.ContextGuid;
				bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate = updateAccount;
				await TafsilBusiness.SaveRangeAsync(contextGuid, EnSystem.SignalR, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, await TafsilMapper.Instance.MapListAsync(e.Connection.Context.Guid, source), updateAccount, 10, e.Connection.cancellationToken, 20);
				if (this.Changed != null)
				{
					Task.Run(delegate
					{
						this.Changed?.Invoke(this, new EventArgs());
					});
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TafsilInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 200);
			}
		}
	}

	private async Task TafsilInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebTafsil> e)
	{
		using (new JaegerMonitor("TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs"))
		{
			_ = 3;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					WebErrorLog.Instance.StartLog("فراخوانی سیو رکوئست حساب تفصیل در محصولی بغیر از حسابداری", "", "TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 209);
				}
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("TafsilInstance_OnSaveRequest received but data is null!", "", "TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 212);
					return;
				}
				List<WebTafsil> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("TafsilInstance_OnSaveRequest received but data is empty!", "", "TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 218);
				}
				else
				{
					if (e.ConnectionData == null || e.ConnectionData.Count <= 0)
					{
						return;
					}
					List<WebDelivery> deliveryList = new List<WebDelivery>();
					foreach (WebTafsil webTafsil in e.ConnectionData)
					{
						int existsTafsilVesionOnDatabase = (await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.SignalR, webTafsil.Guid, "", default(CancellationToken), "TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs"))?.Version ?? 0;
						TafsilBusiness tafsil = await TafsilMapper.Instance.MapAsync(e.Connection.Context.Guid, webTafsil);
						if (webTafsil.ClientVerion <= 0)
						{
							tafsil.Version = existsTafsilVesionOnDatabase;
						}
						else if (webTafsil.ClientVerion > 0 && webTafsil.Version < existsTafsilVesionOnDatabase)
						{
							deliveryList.Add(new WebDelivery
							{
								Guid = tafsil.Guid,
								Message = $"version({webTafsil.Version}) is less than database({existsTafsilVesionOnDatabase}) so update ignored!",
								DeliveryStatus = true,
								DeliveryDate = DateTime.Now,
								ShortMessage = $"version({webTafsil.Version}) is less than database({existsTafsilVesionOnDatabase}) so update ignored!",
								Part = EntityPart.Tafsil
							});
							continue;
						}
						bool flag = Cache.Source != ENSource.Accounting;
						bool send2Server = Cache.Source == ENSource.Accounting;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = await tafsil.SaveAsync(EnSystem.SignalR, flag, e.Connection.cancellationToken, flag, awaitOnSemaphore: true, 10, send2Server, await4sent: true, 10);
						WebDelivery item = new WebDelivery
						{
							Guid = tafsil.Guid,
							Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
							DeliveryStatus = !returnedSaveFuncInfo.HasError,
							DeliveryDate = DateTime.Now,
							ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
							Part = EntityPart.Tafsil
						};
						deliveryList.Add(item);
					}
					if (deliveryList?.Any() ?? false)
					{
						List<Packet> lstPacket = WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId);
						await Cache.SendData(lstPacket, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\TafsilWebServiceRepository.cs", 270);
			}
		}
	}
}
