using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Business;

public class PardakhtReportBusiness : BaseKhazaneReportBusiness, ISyncFromClient2Server<PardakhtReportBusiness>, IDeliverableEntity, IEditableDelivery, IDeliverable, IHasContext, IEntityAttachment
{
	private CancellationTokenSource _updateStatusToken;

	private string _lastServerStatus = "";

	public string AndroidIme { get; set; }

	public DateTime? TransferDate { get; set; }

	public string CpuSerial { get; set; }

	public DateTime DateSabt { get; set; }

	public string HddSerial { get; set; }

	public string Ip { get; set; }

	public bool IsRead { get; set; }

	public string LkSerial { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public string DeliveryMessage { get; set; }

	public decimal AfterAccount { get; set; }

	public decimal BeforeAccount { get; set; }

	public bool? DeliveryStatus { get; set; }

	public decimal TotalSum => base.Sum;

	[JsonIgnore]
	public string DeliveryShortMessageForAndroid
	{
		get
		{
			switch (base.ServerStatus)
			{
			case ServerStatusForClient.None:
				if (base.IsTemp)
				{
					return "موقت";
				}
				return "درحال ارسال...";
			case ServerStatusForClient.Sent:
				return ServerSt();
			case ServerStatusForClient.Delivered:
				return ServerSt();
			case ServerStatusForClient.SendError:
				return ServerSt() + "\r\n" + base.DeliveryShortMessage;
			case ServerStatusForClient.DeliveryError:
				return ServerSt() + "\r\n" + base.DeliveryShortMessage;
			case ServerStatusForClient.DirectDelivery:
				return ServerSt();
			default:
				return ServerSt();
			}
		}
	}

	[JsonIgnore]
	public int StatusForOrdering
	{
		get
		{
			switch (base.ServerStatus)
			{
			case ServerStatusForClient.Sent:
				return 5;
			case ServerStatusForClient.Delivered:
			case ServerStatusForClient.DirectDelivery:
				return 7;
			case ServerStatusForClient.SendError:
				return 1;
			case ServerStatusForClient.DeliveryError:
				return 2;
			case ServerStatusForClient.LinkToOrder:
				return 10;
			default:
				if (base.IsTemp)
				{
					return 3;
				}
				return 4;
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public string AutoTitle(ENAction act)
	{
		return "";
	}

	public PardakhtReportBusiness(string contextGuid)
		: base(contextGuid)
	{
	}

	public static async Task<PardakhtReportBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (guid == Guid.Empty)
			{
				return null;
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.GetReportAsync(guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 93);
			throw;
		}
	}

	public string ServerSt(string tranName = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(_lastServerStatus))
			{
				return _lastServerStatus;
			}
			_updateStatusToken?.Cancel();
			_updateStatusToken = new CancellationTokenSource();
			CancellationToken token = _updateStatusToken.Token;
			switch (base.ServerStatus)
			{
			case ServerStatusForClient.None:
				_lastServerStatus = (base.IsTemp ? "موقت" : "در حال ارسال..");
				break;
			case ServerStatusForClient.Sent:
				_lastServerStatus = base.ServerStatus.GetDisplay() + "(" + base.HostRefrenced + ")";
				break;
			case ServerStatusForClient.Delivered:
				_lastServerStatus = base.ServerStatus.GetDisplay() + "(" + base.HostRefrenced + ")";
				break;
			case ServerStatusForClient.SendError:
				_lastServerStatus = base.ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DeliveryError:
				_lastServerStatus = base.ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DirectDelivery:
				_lastServerStatus = $"{base.ServerStatus.GetDisplay()}({base.DeliveryId})";
				break;
			default:
				_lastServerStatus = $"خطا در تشخیص{base.ServerStatus}";
				break;
			}
			UpdateStatusAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ServerSt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 121);
		}
		return _lastServerStatus;
	}

	private async Task UpdateStatusAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(new Random().Next(200));
			string lastServerStatus = _lastServerStatus;
			switch (base.ServerStatus)
			{
			case ServerStatusForClient.None:
				if (base.IsTemp)
				{
					_lastServerStatus = "موقت";
				}
				else
				{
					_lastServerStatus = "در حال ارسال..";
				}
				break;
			case ServerStatusForClient.Sent:
				_lastServerStatus = base.ServerStatus.GetDisplay() + "(" + base.HostRefrenced + ")";
				break;
			case ServerStatusForClient.Delivered:
				_lastServerStatus = base.ServerStatus.GetDisplay() + "(" + base.HostRefrenced + ")";
				break;
			case ServerStatusForClient.SendError:
				_lastServerStatus = base.ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DeliveryError:
				_lastServerStatus = base.ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DirectDelivery:
				_lastServerStatus = $"{base.ServerStatus.GetDisplay()}({base.DeliveryId})";
				break;
			default:
				_lastServerStatus = $"خطا در تشخیص{base.ServerStatus}";
				break;
			}
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler == null || !(lastServerStatus != _lastServerStatus) || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					handler(this, new PropertyChangedEventArgs("SendToServerStatus"));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 152);
				}
			}, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 154);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 164), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 164);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).BusinessAggregator.ReceptionBusiness.SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 166);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 170);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 171);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 176), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 176);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			try
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 188), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 188);
				}
				transactionHasBeenCreated = true;
				ReceptionBusiness receptionBusiness = await ReceptionBusiness.GetAsync(base.ContextGuid, base.Guid, EnSystem.SignalR, null, fromDatabase: false, token);
				if (receptionBusiness != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 191);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 195);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 196);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 201), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 201);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			PardakhtBusiness obj = await PardakhtBusiness.GetAsync(base.ContextGuid, base.Guid, fromDatabase: true, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, sendToServer: false, priority, token, awaitOnSemaphore: true, 10), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 211);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 215);
			ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 216);
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			PardakhtBusiness obj = await PardakhtBusiness.GetAsync(base.ContextGuid, base.Guid, fromDatabase: true, token, tranName);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await obj.RemoveAsync(system, 10, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 228);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 232);
			ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtReportBusiness.cs", 233);
		}
		return ret;
	}
}
