using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tickets;

namespace EntityCache.Business.Global.Tickets;

public class TicketPartDetailBusiness : Serializable<TicketPartDetailBusiness>, ITicketPartDetail, IHasMasterGuid, IHasContext, INotifyPropertyChanged
{
	private static SemaphoreSlim _itemSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _getTargetToken;

	private string _targetName = "";

	private TafsilBusiness _tafsilItem;

	private CashierBusiness _cashierItem;

	private bool _isDisposed;

	public Guid MasterGuid { get; set; }

	public Guid TargetGuid { get; set; }

	public EnTargetType TargetType { get; set; }

	public string TargetTypeForDisplay => TargetType.GetDisplay().GetTranslate();

	public string TargetName
	{
		get
		{
			if (!string.IsNullOrEmpty(_targetName))
			{
				return _targetName;
			}
			_getTargetToken?.Cancel();
			_getTargetToken = new CancellationTokenSource(60000);
			CancellationToken token = _getTargetToken.Token;
			Task.Run(() => GetTargetAsync(token));
			return _targetName;
		}
	}

	public string ContextGuid { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public event Func<object, EventArgs, Task> OnDisposed;

	public TicketPartDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private async Task GetTargetAsync(CancellationToken token)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _itemSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			if (token.IsCancellationRequested)
			{
				return;
			}
			switch (TargetType)
			{
			case EnTargetType.Cashier:
				if (_cashierItem == null)
				{
					_cashierItem = await CashierBusiness.GetAsync(ContextGuid, TargetGuid, token);
					_targetName = _cashierItem?.Name ?? "";
				}
				break;
			case EnTargetType.Tafsil:
				if (_tafsilItem == null)
				{
					_tafsilItem = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.UserInterface, TargetGuid, "", token, "GetTargetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartDetailBusiness.cs");
					_targetName = _tafsilItem?.Name ?? "";
				}
				break;
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetTargetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartDetailBusiness.cs", 72);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_itemSemaphore.Release();
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null && !base.IsDisposed && !string.IsNullOrEmpty(_targetName))
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					try
					{
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TargetName"));
					}
					catch (Exception ex4)
					{
						WebErrorLog.Instance.StartLog(ex4, "", "GetTargetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartDetailBusiness.cs", 80);
					}
				}, "GetTargetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartDetailBusiness.cs");
			}
		}
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartDetailBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tickets\\TicketPartDetailBusiness.cs", 96);
		}
	}
}
