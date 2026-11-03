using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;
using PacketParser.Entities.MetaData.Hotel;

namespace EntityCache.Business.Accounting.Hotel;

public class ReceiptDetailBusiness : IReceiptDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext, IDisposable
{
	private bool _isdisposed;

	private List<ProductCompactBusiness> _serviceList;

	private decimal _discount;

	private decimal _tax;

	private decimal _avarez;

	private decimal _pricePerNight;

	private decimal _extraPrice;

	private decimal _penalty;

	private decimal _sumServes;

	private int _extraCount;

	private Guid _roomGuid = Guid.Empty;

	private PlacementBusiness _room;

	public bool IsDisposed => _isdisposed;

	public Guid Guid { get; set; }

	public Guid RoomGuid => _roomGuid;

	[JsonIgnore]
	public int RoomNumber { get; private set; }

	[JsonIgnore]
	public int RoomBedCount => _room?.ItemCount ?? 0;

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid MasterGuid { get; set; }

	public DateTime StartDate { get; set; } = DateTime.Now;


	public DateTime EndDate { get; set; }

	public decimal TotalPrice
	{
		get
		{
			return CountStateNight * PricePerNight + CountStateNight * ExtraPrice * (decimal)ExtraCount + Avarez + Tax + SumServes - Discount;
		}
		set
		{
			try
			{
				_tax = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "TotalPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TotalPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 53);
			}
		}
	}

	public decimal Tax
	{
		get
		{
			return _tax;
		}
		set
		{
			try
			{
				_tax = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Tax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Tax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 67);
			}
		}
	}

	public decimal TaxPercent { get; set; }

	public decimal Avarez
	{
		get
		{
			return _avarez;
		}
		set
		{
			try
			{
				_avarez = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Avarez", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Avarez", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 82);
			}
		}
	}

	public decimal AvarezPercent { get; set; }

	public int ExtraCount
	{
		get
		{
			return _extraCount;
		}
		set
		{
			try
			{
				_extraCount = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "ExtraCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ExtraCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 97);
			}
		}
	}

	public decimal ExtraPrice
	{
		get
		{
			return _extraPrice;
		}
		set
		{
			try
			{
				_extraPrice = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "ExtraPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ExtraPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 111);
			}
		}
	}

	public decimal Discount
	{
		get
		{
			return _discount;
		}
		set
		{
			try
			{
				_discount = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Discount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Discount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 125);
			}
		}
	}

	public decimal DiscountPercent { get; set; }

	public string ContextGuid { get; set; }

	public decimal PricePerNight
	{
		get
		{
			return _pricePerNight;
		}
		set
		{
			try
			{
				_pricePerNight = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "PricePerNight", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PricePerNight", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 141);
			}
		}
	}

	public decimal Penalty
	{
		get
		{
			return _penalty;
		}
		set
		{
			try
			{
				_penalty = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Penalty", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Penalty", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 155);
			}
		}
	}

	public ReceiptDetailMetaData MetaData { get; set; }

	public ReceiptBusiness Master { get; private set; }

	public decimal CountStateNight
	{
		get
		{
			try
			{
				if (EndDate < StartDate)
				{
					EndDate = StartDate.AddDays(1.0);
				}
				return (decimal)(EndDate.Date - StartDate.Date).TotalDays;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CountStateNight", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 171);
				throw;
			}
		}
	}

	public decimal SumServes
	{
		get
		{
			return 0m;
		}
		set
		{
			try
			{
				_sumServes = value;
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "SumServes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SumServes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 187);
			}
		}
	}

	public int CountCustomer => checked((MetaData?.AdultCount ?? 0) + (MetaData?.ChildCount ?? 0) + (MetaData?.BabyCount ?? 0));

	public byte[] EditImage => ImageResourceManager.EditImage;

	public byte[] DeleteImage => ImageResourceManager.OrderDeleteImage;

	public IReadOnlyCollection<ProductCompactBusiness> ServiceList
	{
		get
		{
			if (_serviceList == null)
			{
				_serviceList = new List<ProductCompactBusiness>();
			}
			return _serviceList;
		}
	}

	internal event Func<object, EventArgs, Task> OnServiceChanged;

	internal event Func<object, EventArgs, Task> OnRowSumChanged;

	public event Func<object, EventArgs, Task> OnDisposed;

	private ReceiptDetailBusiness()
	{
	}

	public static async Task<ReceiptDetailBusiness> ReciptDetalFactoryAsync(string contextGuid, ReceiptBusiness master, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("ReciptDetalFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs"))
		{
			ReceiptDetailBusiness res = new ReceiptDetailBusiness();
			if (master == null)
			{
				throw new NullReferenceException("master");
			}
			try
			{
				await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
				res.ContextGuid = master.ContextGuid;
				res.Master = master;
				res.MasterGuid = master.Guid;
				res.Status = master.Status;
				res.Modified = master.Modified;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ReciptDetalFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 219);
			}
			return res;
		}
	}

	public Task AddServesList(ProductCompactBusiness serviceRecipt)
	{
		try
		{
			if (serviceRecipt == null)
			{
				return Task.CompletedTask;
			}
			if (_serviceList == null)
			{
				_serviceList = new List<ProductCompactBusiness>();
			}
			_serviceList.Add(serviceRecipt);
			if (this.OnServiceChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnServiceChanged, this, new EventArgs(), default(CancellationToken), 60, "AddServesList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "AddServesList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddServesList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 238);
			throw;
		}
	}

	public Task AddRangeServesList(IEnumerable<ProductCompactBusiness> listserviceRecipt, bool clearlist = false, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (listserviceRecipt == null)
			{
				return Task.CompletedTask;
			}
			if (_serviceList == null)
			{
				_serviceList = new List<ProductCompactBusiness>();
			}
			if (clearlist)
			{
				_serviceList.Clear();
			}
			_serviceList.AddRange(listserviceRecipt);
			if (this.OnServiceChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnServiceChanged, this, new EventArgs(), token, 60, "AddRangeServesList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "AddRangeServesList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeServesList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 259);
			throw;
		}
	}

	public Task RemoveServes(Guid servesGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ProductCompactBusiness productCompactBusiness = ServiceList?.FirstOrDefault((ProductCompactBusiness q) => q.Guid == servesGuid);
			if (productCompactBusiness == null)
			{
				return Task.CompletedTask;
			}
			_serviceList?.Remove(productCompactBusiness);
			if (this.OnServiceChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnServiceChanged, this, new EventArgs(), token, 60, "RemoveServes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "RemoveServes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveServes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 278);
			throw;
		}
	}

	internal async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (StartDate > EndDate)
			{
				returnedSaveFuncInfo.AddError("تاریخ وارد شده صحیح نیست", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 287);
			}
			if (RoomGuid == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("لطفا یک اتاق انتخاب کنید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 288);
			}
			if (TotalPrice == 0m)
			{
				returnedSaveFuncInfo.AddError("قیمت های وارد شده صحیح نیست ", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 289);
			}
			if (TotalPrice < Discount)
			{
				returnedSaveFuncInfo.AddError("قیمت های وارد شده صحیح نیست", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 290);
			}
			if (StartDate < new DateTime(2010L) && EndDate > new DateTime(2099L))
			{
				returnedSaveFuncInfo.AddError("تاریخ های وارد شده صحیح نیست", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 291);
			}
			if (PricePerNight == 0m)
			{
				returnedSaveFuncInfo.AddError("قیمت های وارد شده صحیح نیست", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 292);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 295);
		}
		return returnedSaveFuncInfo;
	}

	public void SetCountStateNight(int value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			EndDate = StartDate.AddDays(value);
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetCountStateNight", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCountStateNight", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 308);
			throw;
		}
	}

	public async Task SetRoomGuidAsync(EnSystem system, Guid value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			_roomGuid = value;
			if (_roomGuid == Guid.Empty)
			{
				_room = null;
				return;
			}
			if ((_room?.Guid ?? Guid.Empty) != value)
			{
				_room = await PlacementBusiness.GetAsync(ContextGuid, _roomGuid, token, tranName);
			}
			if (_room != null)
			{
				RoomNumber = await _room.GetNumberAsync(system, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetRoomGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 327);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (!_isdisposed)
			{
				_isdisposed = true;
				Master = null;
				if (this.OnServiceChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnServiceChanged, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Hotel\\ReceiptDetailBusiness.cs", 344);
		}
	}
}
