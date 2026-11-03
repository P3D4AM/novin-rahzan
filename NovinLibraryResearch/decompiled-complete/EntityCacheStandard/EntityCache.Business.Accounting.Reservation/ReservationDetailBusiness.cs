using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Reservation;

namespace EntityCache.Business.Accounting.Reservation;

public class ReservationDetailBusiness : Serializable<ReservationDetailBusiness>, IReservationDetail, IHasJustGuid, IHasMasterGuid, IHasContext, INotifyPropertyChanged
{
	private PlacementBusiness _placement;

	private bool _isDisposed;

	private DateTime startDate;

	private DateTime endDate;

	private string description;

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid PlacementGuid { get; private set; }

	public string PlacementTitle => _placement?.Title ?? "";

	public int PeopleCount { get; set; }

	public DateTime StartDate
	{
		get
		{
			return startDate;
		}
		set
		{
			startDate = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartDateSh"));
				}, "StartDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs");
			}
		}
	}

	public string StartDateSh => $"{StartDate.Hour}:{StartDate.Minute} * {StartDate.MiladiToCalture()}";

	public DateTime EndDate
	{
		get
		{
			return endDate;
		}
		set
		{
			endDate = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EndDateSh"));
				}, "EndDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs");
			}
		}
	}

	public string EndDateSh => $"{EndDate.Hour}:{EndDate.Minute} * {EndDate.MiladiToCalture()}";

	public Guid? OrderGuid { get; set; }

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
				}, "Description", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs");
			}
		}
	}

	public string ContextGuid { get; set; }

	public ReservationBusiness MasterItem { get; private set; }

	public byte[] AddImage => ImageResourceManager.OpenInvoiceAdd;

	public byte[] EditImage => ImageResourceManager.OpenInvoiceEdit;

	public byte[] DeleteImage => ImageResourceManager.OpenInvoiceDelete;

	public event Func<object, EventArgs, Task> OnDisposed;

	public event PropertyChangedEventHandler PropertyChanged;

	private ReservationDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<ReservationDetailBusiness> GetFactoryAsync(string contextGuid, ReservationBusiness master)
	{
		try
		{
			return Task.FromResult(new ReservationDetailBusiness(contextGuid)
			{
				MasterItem = master
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs", 91);
			throw;
		}
	}

	public async Task SetPlacementGuidAsync(Guid placementGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			PlacementGuid = placementGuid;
			_placement = await PlacementBusiness.GetAsync(ContextGuid, PlacementGuid, token, tranName);
			if (system == EnSystem.UserInterface && this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlacementTitle"));
				}, "SetPlacementGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPlacementGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs", 110);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (PlacementGuid == Guid.Empty)
			{
				await res.AddErrorAsync("لطفا محل پذیرایی را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs", 119);
			}
			if (StartDate >= EndDate)
			{
				await res.AddErrorAsync("لطفا بازه زمانی را صحیح وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs", 120);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs", 124);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs", 125);
		}
		return res;
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
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationDetailBusiness.cs", 141);
		}
	}
}
