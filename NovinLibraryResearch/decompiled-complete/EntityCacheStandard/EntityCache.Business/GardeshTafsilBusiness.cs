using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Mapping.BusinessMappers;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Compression;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace EntityCache.Business;

public class GardeshTafsilBusiness : Serializable<GardeshTafsilBusiness>, IGardeshTafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, INotifyPropertyChanged
{
	private bool _isDisposed;

	private CancellationTokenSource _loadProductToken;

	private CancellationTokenSource _loadTafsilToken;

	private DocumentDet asTypeEnum;

	private short asType;

	private string _description = "";

	private string _tafsilName = "";

	private string _rowDesc = "";

	private string _tafsilForDisplay;

	private string _sanadPrintDesc = "";

	private Guid? _tafsilGuid;

	private Guid? _prdGuid;

	private Guid? _decimalGuid;

	private Guid _kolGuid;

	private Guid _moeinGuid;

	private Guid _uGuid;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private decimal credit;

	private long _tafsilCode;

	public int Ordinal { get; set; }

	private string TranName { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public MoeinBusiness Moein => MoeinBusiness.Get(ContextGuid, MoeinGuid);

	[JsonIgnore]
	[XmlIgnore]
	public TafsilBusiness Tafsil => TafsilBusiness.Get(ContextGuid, TafsilGuid ?? Guid.Empty);

	[JsonIgnore]
	[XmlIgnore]
	public string TafsilItemName
	{
		get
		{
			object obj;
			if (string.IsNullOrEmpty(_tafsilName))
			{
				obj = Tafsil?.Name;
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

	[JsonIgnore]
	[XmlIgnore]
	public long TafsilItemCode
	{
		get
		{
			if (_tafsilCode <= 0)
			{
				return Tafsil?.Code ?? 0;
			}
			return _tafsilCode;
		}
		set
		{
			_tafsilCode = value;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public string KolName => Moein?.GetKol()?.Name ?? "";

	[JsonIgnore]
	[XmlIgnore]
	public int KolCode => (Moein?.GetKol()?.Code).GetValueOrDefault();

	[JsonIgnore]
	[XmlIgnore]
	public string MoeinName => Moein?.Name ?? "";

	[JsonIgnore]
	[XmlIgnore]
	public long MoeinCode => Moein?.Code ?? 0;

	public DateTime Date { get; set; } = DateTime.Now;


	public bool IsEnteghalHesab { get; set; }

	public string DateSh
	{
		get
		{
			if (SanadNO == 0 && AsType != 10 && AsType != 26 && AsType != 27 && AsType != 23 && AsType != 211 && AsType != 212)
			{
				return "";
			}
			return Date.MiladiToCalture();
		}
	}

	public string Time
	{
		get
		{
			if (!string.IsNullOrEmpty(DateSh))
			{
				return $"{Date.Hour}:{Date.Minute}:{Date.Second}";
			}
			return "";
		}
	}

	public Guid? DepotGuid { get; set; }

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

	public Guid? AsGuid { get; set; }

	public short AsType
	{
		get
		{
			try
			{
				if (asType == 0 && asType != 0)
				{
					WebErrorLog.Instance.StartLog($"documentDet not defined! value:{asType}", "", "AsType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 90);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AsType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 94);
				throw;
			}
			return asType;
		}
		set
		{
			asType = value;
		}
	}

	public Document EnAsType
	{
		get
		{
			try
			{
				short num = AsType;
				if (AsType == 206)
				{
					return Document.OrderPreBuy;
				}
				if (AsType == 211)
				{
					return Document.AghsatM;
				}
				if (AsType == 212)
				{
					return Document.AghsatSh;
				}
				if (AsType == 215)
				{
					return Document.AghsatMProfit;
				}
				if (AsType > 99)
				{
					num = short.Parse(AsType.ToString().Substring(0, 2));
				}
				if (num > 90)
				{
					num = short.Parse(AsType.ToString().Substring(1, 1));
				}
				return (Document)num;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnAsType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 120);
				throw;
			}
		}
		set
		{
		}
	}

	public DocumentDet AsTypeEnum
	{
		get
		{
			try
			{
				if (checked((short)asTypeEnum) != AsType)
				{
					asTypeEnum = (DocumentDet)AsType;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AsTypeEnum", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 135);
			}
			return asTypeEnum;
		}
		set
		{
			asTypeEnum = value;
		}
	}

	public decimal Debit { get; set; }

	public decimal Credit
	{
		get
		{
			return credit;
		}
		set
		{
			credit = value;
		}
	}

	public int SanadNO { get; set; }

	public int SanadSt { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPercent { get; }

	public int Id2 { get; set; }

	public decimal QT { get; set; }

	public string QTString => $"{Math.Round(QT, 3)}".Replace(".", "/");

	public decimal QT2 { get; set; }

	public string QT2String => $"{Math.Round(QT2, 3)}".Replace(".", "/");

	public decimal Bazaryab { get; set; }

	public Guid? VGuid { get; set; }

	public string SanadDesc { get; set; }

	public DateTime DateMoaser { get; set; }

	public string DateMoaserSh => DateMoaser.MiladiToCalture();

	public decimal Rem { get; set; }

	public int RowIndex { get; set; }

	public Guid? DecimalGuid
	{
		get
		{
			if (_decimalGuid.HasValue && !(_decimalGuid.Value == Guid.Empty))
			{
				return _decimalGuid;
			}
			return null;
		}
		set
		{
			_decimalGuid = value;
		}
	}

	public string RowDesc
	{
		get
		{
			return _rowDesc;
		}
		set
		{
			_rowDesc = value?.FixString() ?? "";
		}
	}

	public Guid? TafsilGuid
	{
		get
		{
			if (_tafsilGuid.HasValue && !(_tafsilGuid.Value == Guid.Empty))
			{
				return _tafsilGuid;
			}
			return null;
		}
		set
		{
			_tafsilGuid = value;
		}
	}

	public Guid MoeinGuid
	{
		get
		{
			return _moeinGuid;
		}
		set
		{
			_moeinGuid = value;
		}
	}

	public Guid KolGuid
	{
		get
		{
			return _kolGuid;
		}
		set
		{
			_kolGuid = value;
		}
	}

	public Guid UGuid
	{
		get
		{
			return _uGuid;
		}
		set
		{
			_uGuid = value;
		}
	}

	public decimal DecimalPrice { get; set; }

	public decimal DecimalQt { get; set; }

	public decimal DecimalDebitQt
	{
		get
		{
			if (!(DecimalQt < 0m))
			{
				return 0m;
			}
			return -DecimalQt;
		}
	}

	public decimal DecimalCreditQt
	{
		get
		{
			if (!(DecimalQt > 0m))
			{
				return 0m;
			}
			return DecimalQt;
		}
	}

	public decimal DecimalRemQt { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public bool IsChecked { get; set; }

	public decimal Sood { get; set; }

	[JsonIgnore]
	public ProductCompactBusiness PrdInfo { get; private set; }

	[JsonIgnore]
	public string RowSumForDisplay => Math.Abs(Credit - Debit).NpToString("N0", 0);

	[JsonIgnore]
	public string vahedForDisplay => PrdInfo?.VahedName ?? "";

	[JsonIgnore]
	public static bool LoadTafsilName { get; set; } = false;


	[JsonIgnore]
	public string DescForDisplay
	{
		get
		{
			try
			{
				if ((_prdGuid ?? Guid.Empty) != Guid.Empty)
				{
					if (PrdInfo != null)
					{
						string text = PrdInfo?.Name ?? "";
						string text2 = PrdInfo?.Code.ToString() ?? "";
						string text3 = "";
						text3 = ((EnAsType == Document.CheckSanad) ? ((!string.IsNullOrEmpty(text2)) ? $"{EnAsType.GetDisplay()}({SanadNO}) - ({text2}) {text}" : $"{EnAsType.GetDisplay()}({SanadNO}) - {text}") : ((!string.IsNullOrEmpty(text2)) ? $"{EnAsType.GetDisplay()}({Id2}) - ({text2}) {text}" : $"{EnAsType.GetDisplay()}({Id2}) - {text}"));
						if (!string.IsNullOrEmpty(_tafsilForDisplay) && LoadTafsilName)
						{
							text3 = text3 + "(" + _tafsilForDisplay + ")";
						}
						if (!string.IsNullOrEmpty(RowDesc))
						{
							text3 = text3 + " - " + RowDesc;
						}
						return text3;
					}
					_loadProductToken?.Cancel();
					_loadProductToken = new CancellationTokenSource();
					CancellationToken token = _loadProductToken.Token;
					Task.Run(() => LoadProductInfoAsync(token));
				}
				if (_tafsilForDisplay == null && LoadTafsilName)
				{
					_loadTafsilToken?.Cancel();
					_loadTafsilToken = new CancellationTokenSource();
					CancellationToken tafsilToken = _loadTafsilToken.Token;
					Task.Run(() => LoadTafsilInfoAsync(tafsilToken));
				}
				if (EnAsType == Document.None || EnAsType == Document.Note)
				{
					return SanadDesc;
				}
				if (EnAsType == Document.CheckSanad)
				{
					string text4 = string.Format("{0} ({1}) {2}", EnAsType.GetDisplay(), SanadNO, SanadDesc ?? "");
					if (!string.IsNullOrEmpty(_tafsilForDisplay) && LoadTafsilName)
					{
						text4 = text4 + "(" + _tafsilForDisplay + ")";
					}
					if (!string.IsNullOrEmpty(_sanadPrintDesc))
					{
						text4 = text4 + "(" + _sanadPrintDesc + ")";
					}
					return text4;
				}
				if (EnAsType == Document.Sanad)
				{
					string text5 = string.Format("{0} ({1}) {2}", EnAsType.GetDisplay(), SanadNO, RowDesc ?? "");
					if (!string.IsNullOrEmpty(_tafsilForDisplay) && LoadTafsilName)
					{
						text5 = text5 + "(" + _tafsilForDisplay + ")";
					}
					if (!string.IsNullOrEmpty(_sanadPrintDesc))
					{
						text5 = text5 + "(" + _sanadPrintDesc + ")";
					}
					if (!string.IsNullOrEmpty(Serial) && !text5.Contains(Serial))
					{
						text5 = text5 + "(شماره: " + Serial + ")";
					}
					return text5;
				}
				if (EnAsType == Document.AghsatSh || EnAsType == Document.AghsatM || EnAsType == Document.AghsatMProfit)
				{
					if (Id2 > 0)
					{
						return string.Format("{0} ({1}) {2}", EnAsType.GetDisplay(), Id2, SanadDesc ?? "");
					}
					return EnAsType.GetDisplay() + " - " + SanadDesc;
				}
				if (IsEnteghalHesab)
				{
					return "انتقال حساب: " + RowDesc;
				}
				if (AsTypeEnum == DocumentDet.DaryaftEnteghal)
				{
					return string.IsNullOrEmpty(RowDesc) ? ("انتقال حساب (" + SanadDesc + ")") : ("انتقال حساب (" + SanadDesc + " ** " + RowDesc + ")");
				}
				if (AsTypeEnum == DocumentDet.PardakhtEnteghal)
				{
					return string.IsNullOrEmpty(RowDesc) ? ("انتقال حساب (" + SanadDesc + ")") : ("انتقال حساب (" + SanadDesc + " ** " + RowDesc + ")");
				}
				string text6 = string.Format("{0} ({1}) {2} {3}", EnAsType.GetDisplay(), Id2, SanadDesc ?? "", RowDesc ?? "");
				if (!string.IsNullOrEmpty(_tafsilForDisplay) && LoadTafsilName)
				{
					text6 = text6 + "(" + _tafsilForDisplay + ")";
				}
				if (!string.IsNullOrEmpty(_sanadPrintDesc))
				{
					text6 = text6 + "(" + _sanadPrintDesc + ")";
				}
				return text6;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DescForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 292);
				throw;
			}
		}
	}

	[JsonIgnore]
	public string CreditForDisplay => Credit.NpToString("N0", 0);

	[JsonIgnore]
	public string DebitForDisplay => Debit.NpToString("N0", 0);

	[JsonIgnore]
	public string DiscountForDisplay => Discount.NpToString("N0", 0);

	[JsonIgnore]
	public string PriceForDisplay => Price.NpToString("N0", 0);

	[JsonIgnore]
	public string RemForDisplay
	{
		get
		{
			decimal rem = Rem;
			if (rem == 0m)
			{
				return "تسویه";
			}
			if (rem < 0m)
			{
				rem *= -1m;
			}
			return rem.NpToString("N0", 0);
		}
	}

	public string Tashkhis => Rem.AccountForReport();

	public string ContextGuid { get; set; }

	public Guid DetailGuid { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public GardeshTafsilBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 58);
		}
	}

	public Guid? GetPrdGuid()
	{
		return _prdGuid;
	}

	public void SetPrdGuid(Guid? value)
	{
		_prdGuid = value;
	}

	public async Task SetPrdGuidAsync(Guid? value, EnSystem system, bool setProduct)
	{
		_prdGuid = value;
		if (system == EnSystem.UserInterface && setProduct)
		{
			await SetPrdAsync(system);
		}
	}

	public async Task SetPrdAsync(EnSystem system)
	{
		try
		{
			if (system == EnSystem.UserInterface && (_prdGuid ?? Guid.Empty) != Guid.Empty && (PrdInfo?.Guid ?? Guid.Empty) != _prdGuid.Value)
			{
				PrdInfo = await ProductCompactBusiness.GetAsync(ContextGuid, _prdGuid.Value, system, null, "", default(CancellationToken), true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 180);
			throw;
		}
	}

	private async Task LoadProductInfoAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar)
			{
				await Task.Delay(100);
			}
			PrdInfo = await ProductCompactBusiness.GetAsync(ContextGuid, _prdGuid ?? Guid.Empty, EnSystem.UserInterface, null, "", token, true);
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("PrdInfo"));
					this.PropertyChanged(this, new PropertyChangedEventArgs("DescForDisplay"));
					this.PropertyChanged(this, new PropertyChangedEventArgs("vahedForDisplay"));
				}, "LoadProductInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs");
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "LoadProductInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 313);
		}
	}

	private async Task LoadTafsilInfoAsync(CancellationToken token)
	{
		_ = 4;
		try
		{
			if (_tafsilForDisplay != null)
			{
				return;
			}
			await Task.Delay(1000);
			if (token.IsCancellationRequested || (AsGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			Guid guid = Guid.Empty;
			switch (EnAsType)
			{
			case Document.Reception:
				guid = (await ReceptionBusiness.GetReportAsync(ContextGuid, AsGuid.Value, token))?.TafsilGuid ?? Guid.Empty;
				break;
			case Document.Pardakht:
				guid = (await PardakhtBusiness.GetReportAsync(ContextGuid, AsGuid.Value, token))?.TafsilGuid ?? Guid.Empty;
				break;
			case Document.OrderSell:
			case Document.OrderBuy:
			case Document.OrderBackBuy:
			case Document.OrderBackSell:
			case Document.PishFactor:
			case Document.Zayeat:
			case Document.AmaniVorod:
			case Document.AmaniKhoroj:
			case Document.OrderPreBuy:
				guid = (await OrderBusiness.GetReportAsync(ContextGuid, AsGuid.Value, token))?.TafsilGuid ?? Guid.Empty;
				break;
			}
			if (token.IsCancellationRequested || guid == Guid.Empty)
			{
				_tafsilForDisplay = "";
				return;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.UserInterface, guid, "", token, "LoadTafsilInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs");
			if (token.IsCancellationRequested || tafsilBusiness == null)
			{
				_tafsilForDisplay = "";
				return;
			}
			_tafsilForDisplay = tafsilBusiness.Name;
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
					WebErrorLog.Instance.StartLog(ex3, "", "LoadTafsilInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 360);
				}
			}, "LoadTafsilInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs");
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "LoadTafsilInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 363);
		}
	}

	public static Task<GardeshTafsilBusiness> GetLastTransactionAsync(string contextGuid, Guid tafsilGuid, EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil.GetLastTransactionAsync(tafsilGuid, system, tranName, token);
	}

	[Obsolete("", true)]
	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, IEnumerable<GardeshTafsilBusiness> hesabItem1, IEnumerable<WebHesabTree> hesabs, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		int line = 0;
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs"))
		{
			try
			{
				_ = 3;
				ReturnedSaveFuncInfo result;
				try
				{
					line = 1;
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "GardeshTafsilBusiness/SaveRangeAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 403);
						}
						else
						{
							if (autoTran)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 404), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 404);
							}
							if (token.IsCancellationRequested)
							{
								result = ret.AddError(system, "TokenCanceled", "token", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 405);
							}
							else
							{
								line = 2;
								IEnumerable<Guid?> tafsilGuid = (from p in hesabItem1
									where p.TafsilGuid.HasValue
									select p.TafsilGuid)?.Distinct();
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil.DeleteRangeAsync(null, null, tafsilGuid, 10, tranName, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 408);
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									line = 6;
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil.SaveRangeAsync(hesabItem1, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 411);
									if (!ret.HasError)
									{
										line = 8;
										goto end_IL_013b;
									}
									result = ret;
								}
							}
						}
						goto end_IL_0095;
						end_IL_013b:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null{ClsCache.GetCache(contextGuid)?.DatabaseManager == null}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 417);
					ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 418);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 423), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 423);
				}
			}
			return ret;
		}
	}

	[Obsolete("Apply access implemented on client side", true)]
	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> guids, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			_ = 1;
			try
			{
				line = 1;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 439), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 439);
				}
				line = 2;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil.KeepRangeAsync(guids, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 441);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(contextGuid)?.DatabaseManager == null}", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 445);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 446);
			}
		}
		finally
		{
			if (autoTran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 452), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 452);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>>> GetAllAsync(string contextGuid, GardeshTafsiliFilter filter, EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken), int timeOutMiliSeconds = 30000)
	{
		Stopwatch sw = Stopwatch.StartNew();
		ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>> ret = new ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>>();
		if (token.IsCancellationRequested)
		{
			return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 460);
		}
		try
		{
			LoadTafsilName = filter.IsLoadTafsilName;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 464);
			}
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
			{
				ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = (await ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil.GetAllAsync(filter, system, tranName, token))?.ToList();
				UpdateList(ret.value, tryReOrdinal: false);
			}
			else
			{
				if ((filter?.MoeinGuids?.Any()).GetValueOrDefault())
				{
					filter.MoeinGuids = filter.MoeinGuids.Where((Guid q) => q != Guid.Empty)?.ToList();
				}
				if (!(filter?.MoeinGuids?.Any()).GetValueOrDefault() && filter.TafsilGuid.HasValue)
				{
					filter.MoeinGuids = (await AccountBusiness.GetAllAsync(contextGuid, filter.TafsilGuid.Value, isLoadDefMoein: false, tranName: tranName, token: token, status: true))?.Select((AccountTableViewModel p) => p.MoeinGuid)?.ToList();
				}
				if ((filter?.MoeinGuids?.Any()).GetValueOrDefault())
				{
					foreach (Guid moeinGuid in filter.MoeinGuids)
					{
						ReturnedSaveFuncInfoWithValue<byte[]> returnedSaveFuncInfoWithValue2 = await AccountBusiness.GetBallanceAsync(contextGuid, moeinGuid, filter.TafsilGuid.Value, tranName, token);
						if (returnedSaveFuncInfoWithValue2 != null)
						{
							List<WebGardeshTafsiliCompact> source = Json.FromJson<List<WebGardeshTafsiliCompact>>(await GZip.Decompress(returnedSaveFuncInfoWithValue2.value), sendErrorIfException: true, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 485);
							if (ret.value == null)
							{
								ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>> returnedSaveFuncInfoWithValue = ret;
								returnedSaveFuncInfoWithValue.value = await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, source, new List<GardeshTafsilBusiness>(), "", token);
							}
							else
							{
								List<GardeshTafsilBusiness> value2 = ret.value;
								value2.AddRange(await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, source, new List<GardeshTafsilBusiness>(), "", token));
							}
						}
					}
				}
				UpdateList(ret.value, (filter?.MoeinGuids?.Count).GetValueOrDefault() > 1);
				if ((ret?.value?.Any()).GetValueOrDefault())
				{
					foreach (GardeshTafsilBusiness item in ret.value)
					{
						if (!((item?.TafsilGuid ?? Guid.Empty) == Guid.Empty))
						{
							TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, system, item.TafsilGuid.Value, tranName, token, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs");
							item.TafsilItemCode = tafsilBusiness?.Code ?? 0;
							item.TafsilItemName = tafsilBusiness?.Name ?? "";
						}
					}
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 505);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 508);
			WebErrorLog.Instance.StartLog(ex, $"ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil is null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.IGardeshTafil == null}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 509);
		}
		finally
		{
			ILogger obj = LogCache.Instance.Value.Get("UiLogger");
			if (obj != null)
			{
				obj.Debug($"GardeshTafsilBusiness.GetAsync finiehsed on {sw.ElapsedMilliseconds}(ms) records({ret.value?.Count() ?? 0})rows");
			}
		}
		return ret;
	}

	public static Task<decimal> GardeshBallanceAsync(string contextGuid, Guid? tafsilGuid = null, Guid? moeinGuid = null, DateTime? date1 = null, DateTime? date2 = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil.GardeshBallanceAsync(tafsilGuid, moeinGuid, date1, date2, tranName, token);
	}

	public static async Task<IEnumerable<GardeshTafsilBusiness>> GetFromVSanadDetAsync(string contextGuid, EnSystem system, List<Guid> listTafsilGuid, int top, DateTime? date1 = null, DateTime? date2 = null, List<Guid> listMoeinGuid = null, Guid? kolGuid = null, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			listTafsilGuid?.Remove(Guid.Empty);
			listMoeinGuid?.Remove(Guid.Empty);
			IEnumerable<GardeshTafsilBusiness> obj = await ClsCache.GetCache(contextGuid).CacheRepository.IGardeshTafil.GetAllFromVSanadDetAsync(listTafsilGuid, system, top, date1, date2, listMoeinGuid, kolGuid, status, tranName, token);
			UpdateList(obj, tryReOrdinal: false);
			return obj;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 531);
			throw;
		}
	}

	private static void UpdateList(IEnumerable<GardeshTafsilBusiness> list, bool tryReOrdinal)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			int num = 0;
			if (tryReOrdinal && list != null && list.Any())
			{
				using List<GardeshTafsilBusiness>.Enumerator enumerator = (from p in list
					orderby p.Date, p.RowIndex
					select p).ToList().GetEnumerator();
				while (enumerator.MoveNext())
				{
					num = (enumerator.Current.RowIndex = checked(num + 1));
				}
			}
			decimal num3 = default(decimal);
			decimal num4 = default(decimal);
			foreach (GardeshTafsilBusiness item in list?.OrderBy((GardeshTafsilBusiness p) => p.RowIndex)?.ToList() ?? new List<GardeshTafsilBusiness>())
			{
				item.DecimalRemQt = num4 + item.DecimalQt;
				num4 = item.DecimalCreditQt;
				num3 = (item.Rem = num3 + item.Credit - item.Debit);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 558);
			throw;
		}
		finally
		{
			ILogger obj = LogCache.Instance.Value.Get("UiLogger");
			if (obj != null)
			{
				obj.Debug($"GardeshTafsilBusiness.UpdateList finiehsed on {stopwatch.ElapsedMilliseconds}(ms) records({list?.Count() ?? 0})rows");
			}
		}
	}

	public static async Task<decimal> SodoZian_BuyKhalesAsync(string contextGuid, DateTime? endDate = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			decimal kh_nakhales = await GardeshBallanceAsync(contextGuid, null, ParentDefaults.MoeinCoding.CLSMoein17, null, endDate, "", token);
			decimal discount = -(await GardeshBallanceAsync(contextGuid, null, ParentDefaults.MoeinCoding.CLSMoein21, null, endDate, "", token));
			decimal num = -(await GardeshBallanceAsync(contextGuid, null, ParentDefaults.MoeinCoding.CLSMoein33, null, endDate, "", token));
			return kh_nakhales - discount - num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SodoZian_BuyKhalesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 576);
			throw;
		}
	}

	public static async Task<decimal> SodoZian_SellKhalesAsync(string contextGuid, DateTime? endDate = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			decimal sell_nakhales = await GardeshBallanceAsync(contextGuid, null, ParentDefaults.MoeinCoding.CLSMoein16, null, endDate, "", token);
			decimal discount = -(await GardeshBallanceAsync(contextGuid, null, ParentDefaults.MoeinCoding.CLSMoein20, null, endDate, "", token));
			decimal num = -(await GardeshBallanceAsync(contextGuid, null, ParentDefaults.MoeinCoding.CLSMoein32, null, endDate, "", token));
			return -(sell_nakhales - discount - num);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SodoZian_SellKhalesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 593);
			throw;
		}
	}

	public RasGiriDetBusiness GetRasGiriItem()
	{
		return new RasGiriDetBusiness(ContextGuid)
		{
			DateM = DateMoaser,
			Description = DescForDisplay,
			Price = Credit - Debit
		};
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadProductToken?.Cancel();
				_loadProductToken = null;
				_loadTafsilToken?.Cancel();
				_loadTafsilToken = null;
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 620);
		}
	}
}
