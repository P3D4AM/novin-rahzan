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

namespace EntityCache.Business;

public class ReceptionReportBusiness : BaseKhazaneReportBusiness, IDeliverable, IEntityAttachment, INotifyPropertyChanged
{
	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _updateStatusToken;

	private string _lastServerStatus = "";

	public string AndroidIme { get; set; }

	public DateTime? TransferDate { get; set; }

	public string CpuSerial { get; set; }

	public DateTime DateSabt { get; set; }

	public string HddSerial { get; set; }

	public short IdUser { get; set; }

	public string Ip { get; set; }

	public string LkSerial { get; set; }

	public decimal AfterAccount { get; set; }

	public decimal BeforeAccount { get; set; }

	public DependencyType DependencyType { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public string DeliveryMessage { get; set; }

	public bool? DeliveryStatus { get; set; }

	public decimal TotalSum => base.Sum;

	public PaymentMethod PaymentMethod { get; set; }

	[JsonIgnore]
	public string DeliveryShortMessageForAndroid
	{
		get
		{
			try
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
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeliveryShortMessageForAndroid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 59);
				return ex.Message;
			}
		}
	}

	[JsonIgnore]
	public int StatusForOrdering
	{
		get
		{
			try
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
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StatusForOrdering", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 87);
				return 0;
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public ReceptionReportBusiness(string contextGuid)
		: base(contextGuid)
	{
	}

	public static async Task<ReceptionReportBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.IReception.GetReportAsync(guid, token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
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
			WebErrorLog.Instance.StartLog(ex, "", "ServerSt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 134);
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
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 165);
				}
			}, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 167);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendAsync(EnSystem system, CancellationToken token = default(CancellationToken), Priority priority = Priority.High)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReceptionBusiness obj = await ReceptionBusiness.GetAsync(base.ContextGuid, base.Guid, system, null, fromDatabase: false, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, sendToServer: true, priority, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, "", token), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 175);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 179);
			ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionReportBusiness.cs", 180);
		}
		return ret;
	}
}
