using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer;

namespace EntityCache.Business;

public class GPSBusiness : BaseBusiness<GPSBusiness>, IGPS, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<GPSBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _colorHex;

	private string _description;

	private string _icon;

	private static ConcurrentQueue<WebTelegramMessage> telegramMessages = new ConcurrentQueue<WebTelegramMessage>();

	private static UserInformationBusiness user = null;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public decimal Accuracy { get; set; }

	public EnGpsActivity Activity { get; set; }

	public decimal Altitude { get; set; }

	public decimal Bearing { get; set; }

	public string ColorHex
	{
		get
		{
			return _colorHex;
		}
		set
		{
			_colorHex = value?.FixString() ?? "";
		}
	}

	public DateTime DateM { get; set; }

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	public decimal Distance { get; set; }

	public decimal FromLastPointDistance { get; set; }

	public decimal FromStartPointDistance { get; set; }

	public DateTime GPSTime { get; set; }

	public string Icon
	{
		get
		{
			return _icon;
		}
		set
		{
			_icon = value?.FixString() ?? "";
		}
	}

	public decimal Latitude { get; set; }

	public decimal Longitude { get; set; }

	public decimal PointDurationSeconds { get; set; }

	public string PointDurationTime { get; }

	public int PreviewsID { get; set; }

	public decimal Speed { get; set; }

	public decimal Time { get; set; }

	public int UId { get; set; }

	public Guid UserAccGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public decimal MaxAccuracy { get; private set; } = 100m;


	public GPSBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool send2Server, CancellationToken token, bool awaitOnSemaphore = true, bool await4send = false, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "GpsBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 79);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 82), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 82);
						}
						transactionHasBeenCreated = true;
						line = 1;
						if (!(Accuracy > MaxAccuracy))
						{
							line = 2;
							if (Guid == Guid.Empty)
							{
								Guid = Guid.NewGuid();
							}
							line = 5;
							line = 10;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IGps.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 94);
							goto end_IL_018b;
						}
						result = ret.AddError(system, $"accuracy({Accuracy}) is more than max value({MaxAccuracy})", "Accuracy", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 86);
					}
					goto end_IL_00c7;
					end_IL_018b:;
				}
				goto end_IL_00b3;
				end_IL_00c7:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid) is null:{ClsCache.GetCache(ContextGuid) == null}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 98);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 99);
				goto end_IL_00b3;
			}
			return result;
			end_IL_00b3:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 104), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 104);
			}
			bool flag = !ret.HasError && send2Server;
			if (flag)
			{
				string contextGuid = ContextGuid;
				CancellationToken token2 = token;
				flag = await DbCountAsync(contextGuid, null, token2) == 0;
			}
			if (flag)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync("", "", token, 99999), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 108);
				}
				else
				{
					Task.Run(() => Send2ServerAsync("", "", token, 99999));
				}
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 123), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 123);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IGps.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 125);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 129);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 130);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 135), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 135);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 147), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 147);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IGps.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 149);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"cacheIsNull:{ClsCache.GetCache(ContextGuid) == null}//CacheIsDisposed:{ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true}//GuidIsDiposed:{base.IsDisposed}", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 153);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 154);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 159), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 159);
			}
		}
		return ret;
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IGps.DbCountAsync(status, token, tranName);
	}

	public static Task<IEnumerable<GPSBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IGps.GetAllAsync(status, token, tranName);
	}

	public static Task<GPSBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IGps.GetAsync(guid, token, tranName);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IGps;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 209), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 209);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IGps.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 211);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 215);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 216);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 221), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 221);
			}
		}
		return ret;
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IGps.GetAllNotSentAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<Guid>> ret = new ReturnedSaveFuncInfoWithValue<List<Guid>>();
			EnSystem system = EnSystem.SignalR;
			try
			{
				List<GPSBusiness> lst;
				do
				{
					if (token.IsCancellationRequested || (ClsCache.GetCache(contextGuid)?.BufferCancellationToken.IsCancellationRequested ?? true))
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 239);
					}
					lst = await ClsCache.GetCache(contextGuid).CacheRepository.IGps.GetAllNotSentAsync(10, token, tranName);
					if (lst != null && lst.Count > 0)
					{
						if (token.IsCancellationRequested || (ClsCache.GetCache(contextGuid)?.BufferCancellationToken.IsCancellationRequested ?? true))
						{
							return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 243);
						}
						ReturnedSaveFuncInfoWithValue<List<Guid>> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await SendRange(contextGuid, system, lst?.ToList(), "", "", token, 120000), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 245);
						if (token.IsCancellationRequested || (ClsCache.GetCache(contextGuid)?.BufferCancellationToken.IsCancellationRequested ?? true))
						{
							return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 247);
						}
						if (!ret.HasError && ret.value != null && ret.value.Any())
						{
							await RemoveAllAsync(contextGuid, ret?.value, 10, token);
						}
					}
				}
				while (lst != null && lst.Any());
				if (token.IsCancellationRequested || (ClsCache.GetCache(contextGuid)?.BufferCancellationToken.IsCancellationRequested ?? true))
				{
					ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 253);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 257);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 258);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		return Send2ServerAsync("", "", token, 300000);
	}

	private static async Task<ReturnedSaveFuncInfo> RemoveAllAsync(string contextGuid, IEnumerable<Guid> res, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 279), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 279);
				}
				transactionHasBeenCreated = true;
				line = 1;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IGps.RemoveAllAsync(res, tryCount, token, tranName), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 282);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}//CacheIsNuul Or Disposed:{ClsCache.GetCache(contextGuid)?.IsDisposed ?? true}", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 286);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 287);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 292), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 292);
			}
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<List<Guid>>> SendRange(string contextGuid, EnSystem system, IEnumerable<GPSBusiness> gpsBusinesses, string CustomerUserName, string CustomerPassword, CancellationToken token, int timeout)
	{
		using (new JaegerMonitor("SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<Guid>> ret = new ReturnedSaveFuncInfoWithValue<List<Guid>>();
			int line = 0;
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 304);
				}
				line = 1;
				List<WebGPS> list = await GpsMapper.Instance.MapListAsync(contextGuid, gpsBusinesses.ToList(), new List<WebGPS>(), "", token);
				line = 2;
				List<Packet> paks = WebGPS.GenerateSaveRequest(list);
				line = 3;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 310);
				}
				line = 4;
				GpsWaiter gpsWaiter = new GpsWaiter(timeout, token);
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 314);
				}
				line = 6;
				if ((ClsCache.GetCache(contextGuid)?.BufferLoginState?.Status).GetValueOrDefault() != EnLoginResult.Success)
				{
					return ret.AddError(system, "LoginFailed //(ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 317);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 318);
				}
				line = 7;
				if (!string.IsNullOrEmpty(CustomerUserName) && !string.IsNullOrEmpty(CustomerPassword))
				{
					Task<ReturnedSaveFuncInfo> sendTask = Task.Run(() => ClsCache.GetCache(contextGuid).SendOnCustomerBuffer(CustomerUserName, CustomerPassword, paks, token, Priority.High, timeout));
					ReturnedSaveFuncInfoWithValue<List<Guid>> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await gpsWaiter.WaitForSaveResponse(list.Select((WebGPS s) => s.Guid)?.ToList(), ClsCache.GetCache(contextGuid).Context);
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await sendTask, ReturnedState.None, "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 324);
				}
				else
				{
					Task.Run(() => ClsCache.GetCache(contextGuid).SendAndWaitForResponse(paks, token, 20, 2, 10, 10));
					line = 13;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 330);
					}
					line = 14;
					ReturnedSaveFuncInfoWithValue<List<Guid>> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await gpsWaiter.WaitForSaveResponse(list.Select((WebGPS s) => s.Guid)?.ToList(), ClsCache.GetCache(contextGuid).Context);
					line = 15;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 335);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Line Number Of Error Has :{line}", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 339);
				ret.AddReturnedValue(ex, "", "SendRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 340);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(string CustomerUserName, string CustomerPassword, CancellationToken token, int timeout)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 1;
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 351);
			}
			line = 2;
			WebGPS item = await GpsMapper.Instance.MapAsync(ContextGuid, this, new WebGPS(), "", token);
			List<Packet> paks = WebGPS.GenerateSaveRequest(new List<WebGPS> { item });
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 355);
			}
			line = 2;
			GpsWaiter gpsWaiter = new GpsWaiter(timeout, token);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 358);
			}
			if ((ClsCache.GetCache(ContextGuid).BufferLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
			{
				return ret.AddError(EnSystem.SignalR, "LoginFaield! //(ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 360);
			}
			line = 3;
			if (!string.IsNullOrEmpty(CustomerUserName) && !string.IsNullOrEmpty(CustomerPassword))
			{
				Task<ReturnedSaveFuncInfo> t = Task.Run(() => ClsCache.GetCache(ContextGuid).SendOnCustomerBuffer(CustomerUserName, CustomerPassword, paks, token, Priority.High, timeout));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await gpsWaiter.WaitForSaveResponse(Guid, ClsCache.GetCache(ContextGuid).Context);
				line = 5;
				ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 367);
				ReturnedSaveFuncInfo returnedSaveFuncInfo2 = ret;
				returnedSaveFuncInfo2.AddReturnedValue(await t, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 368);
				return ret;
			}
			Task.Run(() => ClsCache.GetCache(ContextGuid).SendAndWaitForResponse(paks, token, 20, 2, 10, 10));
			line = 6;
			return await gpsWaiter.WaitForSaveResponse(Guid, ClsCache.GetCache(ContextGuid).Context);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The Error Log IS : {line}", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 381);
			ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GPSBusiness.cs", 382);
		}
		return ret;
	}

	public static async Task SendTelegramLogAsync(string contextGuid, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (user == null)
		{
			user = await UserInformationBusiness.GetAsync(contextGuid);
		}
		msg = $"\r\n#User:{user.DepId}|{user.UserName}\r\n{msg}";
		ClsCache cache = ClsCache.GetCache(contextGuid);
		if (cache != null && cache.BufferLoginState?.Status == EnLoginResult.Success)
		{
			WebTelegramMessage.GetNp_GpsLog_bot().Sent2web(msg, null, memberName, sourceFilePath, sourceLineNumber);
		}
	}

	public static void SendTelegramLog(string contextGuid, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		Task.Run(() => SendTelegramLogAsync(contextGuid, msg, memberName, sourceFilePath, sourceLineNumber));
	}
}
