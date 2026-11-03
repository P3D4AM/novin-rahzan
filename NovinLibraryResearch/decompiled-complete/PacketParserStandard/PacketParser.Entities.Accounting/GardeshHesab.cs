using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces.Accounting;
using Serilog;

namespace PacketParser.Entities.Accounting;

public class GardeshHesab : IGardeshHesab, INotifyPropertyChanged
{
	private static SemaphoreSlim _dbSemaphore = new SemaphoreSlim(1, 50);

	private CancellationTokenSource _prdInfoToken;

	private CancellationTokenSource _userToken;

	private int _sanadNo;

	private int _sanadSt;

	private readonly decimal _remFromLastRecord;

	private DateTime _Date = DateTime.Now;

	private CancellationTokenSource _loadSanadPrintToken;

	private string _sanadPrintDesc = "";

	private string _tafsilName = "";

	private string userName;

	private long _tafsilCode;

	private IProduct _PrdInfo;

	private ITafsil tafsil;

	private IUserAcc _user;

	private Guid? _tafsilGuid;

	private IPrdVahed _prdVahed;

	private int sanadNO;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public DateTime Date
	{
		get
		{
			if (_Date < new DateTime(1900, 1, 1))
			{
				_Date = new DateTime(2000, 1, 1);
			}
			else if (_Date > new DateTime(2100, 1, 1))
			{
				_Date = DateTime.Now;
			}
			return _Date;
		}
		set
		{
			_Date = value;
		}
	}

	public string DateSh
	{
		get
		{
			if (AsType == Document.PishFactor || AsType == Document.AmaniKhoroj || AsType == Document.AmaniVorod || AsType == Document.OrderPreBuy || AsType == Document.Note || AsType == Document.AghsatSh || AsType == Document.AghsatM)
			{
				return Date.MiladiToCalture();
			}
			if (SanadNO == 0)
			{
				return "";
			}
			return Date.MiladiToCalture();
		}
		set
		{
		}
	}

	public Guid DetailGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public string Description { get; set; }

	public Guid? AsGuid { get; set; }

	public Document AsType { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public IDalTurnover DalTernover { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public List<GardeshTafsil> LstTafsiliItems { get; set; } = new List<GardeshTafsil>();


	public string TafsiliItems
	{
		get
		{
			return Json.ToStringJson(LstTafsiliItems);
		}
		set
		{
		}
	}

	public bool IsFactor
	{
		get
		{
			if ((AsType != Document.OrderBackSell || !(Credit > 0m)) && AsType != Document.OrderBackBuy && (AsType != Document.OrderSell || !(Debit > 0m)))
			{
				return AsType == Document.OrderBuy;
			}
			return true;
		}
		set
		{
		}
	}

	public string AutoDescriptionShort
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(_sanadPrintDesc) && DalTernover.Source != ENSource.Android_HesabYar)
				{
					_loadSanadPrintToken?.Cancel();
					_loadSanadPrintToken = new CancellationTokenSource(60000);
					CancellationToken token = _loadSanadPrintToken.Token;
					LoadSanadPrintAsync(token);
				}
				string text = "";
				if (IsEnteghalHesab)
				{
					return "انتقال حساب: " + RowDesc;
				}
				switch (AsType)
				{
				case Document.EftetaheHesab:
					text = "انتقال اول دوره";
					break;
				case Document.BankBankEnteghal:
					text = "انتقال بین بانکی";
					break;
				case Document.None:
				case Document.Note:
					return SanadDesc;
				case Document.CheckSanad:
					text = string.Format("{0}({1}) {2}", AsType.GetDisplay(), SanadNO, SanadDesc ?? "");
					break;
				case Document.Sanad:
					text = string.Format("{0}({1}) {2}", AsType.GetDisplay(), Id2, SanadDesc ?? "");
					break;
				case Document.AghsatM:
				case Document.AghsatSh:
				case Document.AghsatMProfit:
					if (Id2 > 0)
					{
						return string.Format("{0} ({1}) {2} ({3:N0})", AsType.GetDisplay(), Id2, RowDesc ?? "", Price);
					}
					return AsType.GetDisplay() + " - " + RowDesc;
				case Document.Reception:
					text = string.Format("{0}({1}) {2}", AsType.GetDisplay(), Id2, SanadDesc ?? "");
					break;
				case Document.Pardakht:
					text = string.Format("{0}({1}) {2}", AsType.GetDisplay(), Id2, SanadDesc ?? "");
					break;
				default:
					text = string.Format("{0}({1}) {2}", AsType.GetDisplay(), Id2, SanadDesc ?? "");
					break;
				}
				if (!string.IsNullOrEmpty(_sanadPrintDesc))
				{
					text += _sanadPrintDesc;
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AutoDescriptionShort", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 140);
				return null;
			}
		}
		set
		{
		}
	}

	public DocumentDet AsTypeEnum { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNO
	{
		get
		{
			if (AsType == Document.PishFactor || AsType == Document.OrderPreBuy || AsType == Document.Note)
			{
				sanadNO = 0;
			}
			return sanadNO;
		}
		set
		{
			sanadNO = value;
		}
	}

	public int SanadSt { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal Sood { get; set; }

	public bool Ischecked { get; set; }

	public decimal DiscountPercent
	{
		get
		{
			if (QT * Price <= 0m)
			{
				return 0m;
			}
			return Discount / (QT * Price);
		}
		set
		{
		}
	}

	public int Id2 { get; set; }

	public decimal QT { get; set; }

	public decimal QT2 { get; set; }

	public decimal Bazaryab { get; set; }

	public Guid VGuid { get; set; }

	public Guid PrdGuid { get; set; }

	public string SanadDesc { get; set; }

	public short depId { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public IProduct PrdInfo
	{
		get
		{
			if (_PrdInfo == null && PrdGuid != Guid.Empty)
			{
				_prdInfoToken?.Cancel();
				_prdInfoToken = new CancellationTokenSource();
				CancellationToken token = _prdInfoToken.Token;
				UpdatePrdInfoAsync(token);
			}
			return _PrdInfo;
		}
		set
		{
			_PrdInfo = value;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public IPrdVahed PrdVahed
	{
		get
		{
			if (_prdVahed == null && PrdInfo != null)
			{
				_prdVahed = DalTernover.GetPrdVahed(depId, PrdInfo.GuidVahed);
			}
			return _prdVahed;
		}
		set
		{
			_prdVahed = value;
		}
	}

	public decimal Rem { get; set; }

	public decimal RemWithQest { get; set; }

	public Guid DecimalGuid { get; set; }

	public string RowDesc { get; set; }

	public int RowIndex { get; set; }

	public Guid? TafsilGuid
	{
		get
		{
			if (_tafsilGuid.HasValue && !(_tafsilGuid.Value == Guid.Empty))
			{
				return _tafsilGuid.Value;
			}
			return null;
		}
		set
		{
			_tafsilGuid = value;
		}
	}

	public Guid MoeinGuid { get; set; }

	public Guid KolGuid { get; set; }

	public Guid UGuid { get; set; }

	public string UserName
	{
		get
		{
			if (!string.IsNullOrEmpty(userName))
			{
				return userName;
			}
			_userToken?.Cancel();
			_userToken = new CancellationTokenSource(60000);
			CancellationToken token = _userToken.Token;
			UpdateUserAccAsync(token);
			return userName;
		}
		set
		{
		}
	}

	public string RemForDisplay
	{
		get
		{
			decimal rem = Rem;
			if (rem < 0m)
			{
				rem *= -1m;
			}
			if (rem == 0m)
			{
				return "تسویه";
			}
			return rem.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string RemWithQestForDisplay
	{
		get
		{
			decimal remWithQest = RemWithQest;
			if (remWithQest < 0m)
			{
				remWithQest *= -1m;
			}
			if (remWithQest == 0m)
			{
				return "تسویه";
			}
			return remWithQest.NpToString("N0", 0);
		}
	}

	public string RemTashkhisForDisplay
	{
		get
		{
			decimal rem = Rem;
			if (rem < 0m)
			{
				return "بد";
			}
			if (rem == 0m)
			{
				return "";
			}
			return "بس";
		}
		set
		{
		}
	}

	public string CreditForDisplay
	{
		get
		{
			return Credit.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string DebitForDisplay
	{
		get
		{
			return Debit.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string DiscountForDisplay
	{
		get
		{
			return Discount.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string PriceForDisplay
	{
		get
		{
			return Price.NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string DescForDisplay
	{
		get
		{
			using (new JaegerMonitor("DescForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs"))
			{
				try
				{
					if (PrdInfo != null)
					{
						string text = PrdInfo?.Name ?? "";
						string text2 = PrdInfo?.Code.ToString() ?? "";
						return AsType.GetDisplay() + " ( " + Id2 + " )  ( " + text + " - " + text2 + " ) " + RowDesc;
					}
					if (AsType == Document.None)
					{
						return "مانده از قبل (-)";
					}
					if (Id2 <= 0 && SanadNO != 0)
					{
						Id2 = SanadNO;
					}
					if (AsType == Document.AghsatM || AsType == Document.AghsatSh)
					{
						return string.Format("{0} ({1}) {2} ({3:N0})", AsType.GetDisplay(), Id2, RowDesc ?? "", Price);
					}
					if (AsType == Document.Sanad)
					{
						return string.Format("{0} ({1}) {2}", AsType.GetDisplay(), Id2, SanadDesc ?? "");
					}
					if (IsEnteghalHesab)
					{
						return RowDesc ?? "";
					}
					return string.Format("{0} ({1}) {2}", AsType.GetDisplay(), Id2, RowDesc ?? "");
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "DescForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 356);
					return null;
				}
			}
		}
		set
		{
		}
	}

	public string vahedForDisplay
	{
		get
		{
			return PrdVahed?.Name ?? "";
		}
		set
		{
		}
	}

	public string RowSumForDisplay
	{
		get
		{
			return Math.Abs(Credit - Debit).NpToString("N0", 0);
		}
		set
		{
		}
	}

	public string Tashkhis
	{
		get
		{
			if (Credit - Debit > 0m)
			{
				return "بس";
			}
			if (Credit - Debit < 0m)
			{
				return "بد";
			}
			return "";
		}
		set
		{
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public ITafsil Tafsil
	{
		set
		{
			tafsil = value;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public string TafsilItemName
	{
		get
		{
			object obj;
			if (string.IsNullOrEmpty(_tafsilName))
			{
				obj = tafsil?.Name;
				if (obj == null)
				{
					return "";
				}
			}
			else
			{
				obj = _tafsilName;
			}
			return (string)obj;
		}
		set
		{
			_tafsilName = value;
		}
	}

	public long TafsilItemCode
	{
		get
		{
			if (_tafsilCode <= 0)
			{
				return tafsil?.Code ?? 0;
			}
			return _tafsilCode;
		}
		set
		{
			_tafsilCode = value;
		}
	}

	public bool IsEnteghalHesab { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public GardeshHesab()
	{
	}

	public GardeshHesab(IDalTurnover iDalTurnover, int rowIndex)
	{
		DalTernover = iDalTurnover;
		RowIndex = rowIndex;
	}

	private GardeshHesab(IDalTurnover iDalTurnover, decimal remaining, int rowIndex)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs"))
		{
			try
			{
				DalTernover = iDalTurnover;
				_remFromLastRecord = remaining;
				RowIndex = rowIndex;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 48);
			}
		}
	}

	private async Task UpdatePrdInfoAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (!token.IsCancellationRequested)
			{
				_PrdInfo = await DalTernover.GetProductAsync(depId, PrdGuid);
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged != null)
				{
					propertyChanged(this, new PropertyChangedEventArgs("DescForDisplay"));
					propertyChanged(this, new PropertyChangedEventArgs("PrdInfo"));
					propertyChanged(this, new PropertyChangedEventArgs("PrdVahed"));
					propertyChanged(this, new PropertyChangedEventArgs("vahedForDisplay"));
				}
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
			WebErrorLog.Instance.StartLog(ex3, "", "UpdatePrdInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 219);
		}
	}

	private async Task UpdateUserAccAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (!token.IsCancellationRequested)
			{
				_user = await DalTernover.GetUserAccAsync(depId, UGuid, token);
				userName = _user?.Name ?? "";
				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("userName"));
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
			WebErrorLog.Instance.StartLog(ex3, "", "UpdateUserAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 276);
		}
	}

	private async Task LoadSanadPrintAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			if (!string.IsNullOrEmpty(_sanadPrintDesc))
			{
				return;
			}
			await _dbSemaphore.WaitAsync();
			try
			{
				await Task.Delay(1000);
				if (token.IsCancellationRequested || (AsGuid ?? Guid.Empty) == Guid.Empty || DalTernover == null)
				{
					return;
				}
				ISanadPrint sanadPrint = await DalTernover.GetSanadPrintAsync(AsGuid.Value, AsType);
				if (sanadPrint == null)
				{
					return;
				}
				if (!string.IsNullOrEmpty(sanadPrint.Name))
				{
					_sanadPrintDesc = "(طرف متفرقه: " + sanadPrint.Name + " " + sanadPrint.Mob + " " + sanadPrint.Tel + ")";
				}
				if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
				{
					return;
				}
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					try
					{
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DescForDisplay"));
					}
					catch (Exception ex3)
					{
						WebErrorLog.Instance.StartLog(ex3, "", "LoadSanadPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 377);
					}
				}, "LoadSanadPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs");
			}
			finally
			{
				_dbSemaphore.Release();
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "LoadSanadPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 382);
		}
	}

	public async Task<ITafsil> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!TafsilGuid.HasValue || TafsilGuid.Value == Guid.Empty)
			{
				return null;
			}
			if (tafsil == null && TafsilGuid != Guid.Empty)
			{
				tafsil = await DalTernover.GetTafsilAsync(system, depId, TafsilGuid.Value, token);
			}
			return tafsil;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 418);
			throw;
		}
	}

	public static async Task<IEnumerable<GardeshHesab>> GetGardeshTajmieiAsync(string contextGuid, IDalTurnover iDalTurnover, GardeshTafsiliFilter filter, EnSystem system)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsil>> returnedSaveFuncInfoWithValue = await iDalTurnover.GetAllAsync(contextGuid, filter, system);
			return GetFromSanadDetail(filter.TafsilGuid, iDalTurnover, returnedSaveFuncInfoWithValue.value);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGardeshTajmieiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 431);
			return null;
		}
	}

	public static IEnumerable<GardeshHesab> GetFromSanadDetail(Guid? tafsilGuid, IDalTurnover iDalTurnover, IEnumerable<GardeshTafsil> detList)
	{
		checked
		{
			using (new JaegerMonitor("GetFromSanadDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs"))
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				try
				{
					if (detList == null || detList.Count() <= 0)
					{
						return null;
					}
					decimal remaining = default(decimal);
					List<GardeshHesab> list = new List<GardeshHesab>();
					int num = 0;
					GardeshHesab gardeshHesab = new GardeshHesab(iDalTurnover, ++num);
					foreach (GardeshTafsil det in detList)
					{
						if ((det.AsType != 0 && !(det.AsGuid == Guid.Empty)) || !(det.Credit - det.Debit == 0m))
						{
							if ((gardeshHesab.AsType == Document.None || gardeshHesab.AsGuid == Guid.Empty) && gardeshHesab.Credit - gardeshHesab.Debit == 0m)
							{
								gardeshHesab = new GardeshHesab(iDalTurnover, remaining, ++num);
							}
							gardeshHesab.Ischecked = det.IsChecked;
							if (!gardeshHesab.AddItem(det, tafsilGuid))
							{
								remaining += gardeshHesab.Credit - gardeshHesab.Debit;
								list.Add(gardeshHesab);
								gardeshHesab = new GardeshHesab(iDalTurnover, remaining, ++num);
								gardeshHesab.AddItem(det, tafsilGuid);
							}
						}
					}
					if (gardeshHesab.AsGuid != Guid.Empty)
					{
						list.Add(gardeshHesab);
					}
					CalcAghsat(list);
					return list;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetFromSanadDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 470);
					return null;
				}
				finally
				{
					ILogger obj = LogCache.Instance.Value.Get("UiLogger");
					if (obj != null)
					{
						obj.Debug($"GardeshHesab.GetFromSanadDetail finiehsed on {stopwatch.ElapsedMilliseconds}(ms) records({detList?.Count() ?? 0})rows");
					}
				}
			}
		}
	}

	protected bool AddItem(GardeshTafsil item, Guid? tafsilGuid)
	{
		try
		{
			if (item == null)
			{
				return false;
			}
			if (LstTafsiliItems.Count() == 0)
			{
				AddItem_(item, tafsilGuid);
				return true;
			}
			if (item.EnAsType == AsType)
			{
				Guid? asGuid = AsGuid;
				Guid? asGuid2 = item.AsGuid;
				if (asGuid.HasValue == asGuid2.HasValue && (!asGuid.HasValue || asGuid.GetValueOrDefault() == asGuid2.GetValueOrDefault()) && !IsEnteghalHesab)
				{
					AddItem_(item, tafsilGuid);
					return true;
				}
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddItem", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 499);
			return false;
		}
	}

	private void AddItem_(GardeshTafsil item, Guid? tafsilGuid)
	{
		try
		{
			LstTafsiliItems = LstTafsiliItems?.ToList() ?? new List<GardeshTafsil>();
			List<GardeshTafsil> lstTafsiliItems = LstTafsiliItems;
			if (lstTafsiliItems == null || !lstTafsiliItems.Any())
			{
				AsGuid = item.AsGuid;
				AsType = item.EnAsType;
				Date = item.Date;
				Id2 = item.Id2;
				SanadDesc = item.SanadDesc;
				SanadNO = item.SanadNO;
				SanadSt = item.SanadSt;
				UGuid = item.UGuid;
				depId = item.DepId;
				TafsilGuid = item.TafsilGuid;
				MoeinGuid = item.MoeinGuid;
				RowDesc = item.RowDesc;
				IsEnteghalHesab = item.IsEnteghalHesab;
			}
			Price = item.Price;
			LstTafsiliItems.Add(item);
			Debit += item.Debit;
			Credit += item.Credit;
			Rem = _remFromLastRecord + Credit - Debit;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddItem_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 533);
		}
		finally
		{
			if (Debit != 0m && Credit != 0m)
			{
				decimal num = Credit - Debit;
				if (num > 0m)
				{
					Debit = 0m;
					Credit = num;
				}
				else
				{
					Credit = 0m;
					Debit = -num;
				}
			}
		}
	}

	private static void CalcAghsat(List<GardeshHesab> list)
	{
		try
		{
			if (list == null || !list.Any())
			{
				return;
			}
			bool num = list.Any((GardeshHesab q) => q.AsType == Document.AghsatM);
			bool flag = list.Any((GardeshHesab q) => q.AsType == Document.AghsatSh);
			if (!num && !flag)
			{
				return;
			}
			GardeshHesab gardeshHesab = null;
			foreach (GardeshHesab item in list)
			{
				if (gardeshHesab == null)
				{
					gardeshHesab = new GardeshHesab();
				}
				if (item.AsType == Document.AghsatM)
				{
					item.RemWithQest = gardeshHesab.RemWithQest + item.Price;
				}
				else if (item.AsType == Document.AghsatSh)
				{
					item.RemWithQest = gardeshHesab.RemWithQest - item.Price;
				}
				else
				{
					item.RemWithQest = gardeshHesab.RemWithQest + item.Credit - item.Debit;
				}
				gardeshHesab = item;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcAghsat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\GardeshHesab.cs", 571);
		}
	}
}
