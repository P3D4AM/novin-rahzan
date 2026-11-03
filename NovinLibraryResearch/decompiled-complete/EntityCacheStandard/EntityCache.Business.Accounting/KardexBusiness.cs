using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Selection;
using EntityCache.EventArguments;
using EntityCache.Filters;
using EntityCache.Models.Accounting.Prd;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Filters;
using PacketParser.Records.Kardex;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting;

public class KardexBusiness : Serializable<KardexBusiness>, IKardexProduct, IHasContext, INotifyPropertyChanged, IHasIsChecked, IHasJustGuid
{
	private CancellationTokenSource _fillDescForDisplayToken;

	private static BaseLocks _getKardexByTranName = new BaseLocks();

	private static ConcurrentDictionary<string, SemaphoreSlim> _semaphoreDic = new ConcurrentDictionary<string, SemaphoreSlim>();

	private Guid productGuid;

	private Guid? _v2Guid;

	internal ProductCompactBusiness _prd;

	internal PrdVahedBusiness _vahed2;

	private string _descForDisplay;

	private string _depotName;

	private static SemaphoreSlim _semaphoreProgressRem = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _semaphoreProgressRem_ControlMaxCount = new SemaphoreSlim(1, 30);

	private clsKardexAccess _access;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public int PrdId { get; set; }

	public Guid DepotGuid { get; set; }

	public short DepotId { get; set; }

	public DateTime DateM { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPercent
	{
		get
		{
			try
			{
				if (Discount <= 0m || UnitPrice <= 0m)
				{
					return 0m;
				}
				decimal value = Discount * 100m;
				decimal? num = RowSum + (decimal?)Discount;
				return Math.Round(((decimal?)value / num).Value, DigitForRound);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DiscountPercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 59);
			}
			return 0m;
		}
	}

	public string DiscountPercentString { get; set; }

	public string DiscountForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_Takhfif)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = Discount.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DiscountForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 77);
			}
			return text;
		}
	}

	public decimal Fare { get; set; }

	public decimal VisitorPrice { get; set; }

	public string VisitorPriceForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_Bazaryab)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = VisitorPrice.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VisitorPriceForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 96);
			}
			return text;
		}
	}

	public decimal Tax { get; set; }

	public decimal TaxForDisplay
	{
		get
		{
			if (!IsTaxImpactOnCost)
			{
				return Tax;
			}
			return 0m;
		}
	}

	public decimal TaxInpactOfCost
	{
		get
		{
			if (!IsTaxImpactOnCost)
			{
				return 0m;
			}
			return Tax;
		}
	}

	public Guid SanadGuid { get; set; }

	public Document SanadType { get; set; }

	public Guid DetailGuid { get; set; }

	public decimal VorodQT { get; set; }

	public string VorodQtForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null)
				{
					switch (SanadType)
					{
					case Document.OrderBuy:
						if (!_access.Qt_Buy_Vorod)
						{
							text = "---";
						}
						break;
					case Document.OrderBackSell:
						if (!_access.Qt_BackSell_Vorod)
						{
							text = "---";
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Ezafat_Vorod && VorodQT > 0m)
						{
							text = "---";
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mahsul:
						if (!_access.Qt_Tolid_Mahsol_Vorod && VorodQT > 0m)
						{
							text = "---";
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Vorod && VorodQT > 0m)
						{
							text = "---";
						}
						break;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = VorodQT.ToString().Replace(Text.DecimalPoint, "/").Replace("/000", "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VorodQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 130);
			}
			return text;
		}
	}

	public decimal KhorojQT { get; set; }

	public string KhorojQtForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null)
				{
					switch (SanadType)
					{
					case Document.OrderSell:
						if (!_access.Qt_Sell_Khoroj)
						{
							text = "---";
						}
						break;
					case Document.OrderBackBuy:
						if (!_access.Qt_BackBuy_Khoroj)
						{
							text = "---";
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Kasr_Khoroj && KhorojQT > 0m)
						{
							text = "---";
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mavad:
						if (!_access.Qt_Tolid_Mavad_Khoroj && KhorojQT > 0m)
						{
							text = "---";
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Khoroj && KhorojQT > 0m)
						{
							text = "---";
						}
						break;
					case Document.Zayeat:
						if (!_access.Qt_Zayeat_Khoroj)
						{
							text = "---";
						}
						break;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = KhorojQT.ToString().Replace(Text.DecimalPoint, "/").Replace("/000", "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KhorojQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 159);
			}
			return text;
		}
	}

	public decimal UnitPrice { get; set; }

	public string UnitPriceForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_Fi_Kardex)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = UnitPrice.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UnitPriceForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 177);
			}
			return text;
		}
	}

	public decimal? RemQT { get; set; }

	public string RemQtForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Qt_Mande)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = RemQT?.ToString()?.Replace(Text.DecimalPoint, "/")?.Replace("/000", "") ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 195);
			}
			return text;
		}
	}

	public decimal? UnitSood { get; set; }

	public string UnitSoodForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_Sood)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = UnitSood?.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UnitSoodForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 213);
			}
			return text;
		}
	}

	public decimal? TotalSood { get; set; }

	public string TotalSoodForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_TotalSood)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = TotalSood?.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TotalSoodForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 231);
			}
			return text;
		}
	}

	public decimal? RowSood { get; set; }

	public string RowSoodForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_Sood)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = RowSood?.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RowSoodForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 249);
			}
			return text;
		}
	}

	public decimal? RemUnitPrice { get; set; }

	public string RemUnitPriceForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_baha_Vahed)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = RemUnitPrice?.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemUnitPriceForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 267);
			}
			return text;
		}
	}

	public decimal? RowSum { get; set; }

	public decimal? RowSumForDiplay
	{
		get
		{
			if (!IsTaxImpactOnCost)
			{
				return RowSum.GetValueOrDefault() + Tax;
			}
			return RowSum;
		}
	}

	public decimal? RemTotalPrice { get; set; }

	public string RemTotalPriceForDisplay
	{
		get
		{
			string text = "";
			try
			{
				if (_access != null && !_access.Rialy_Baha_Finished)
				{
					text = "---";
				}
				if (string.IsNullOrEmpty(text))
				{
					text = RemTotalPrice?.NpToString("N0", 0) ?? "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemTotalPriceForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 287);
			}
			return text;
		}
	}

	public Guid? UserGuid { get; set; }

	public string Description { get; set; }

	[JsonIgnore]
	public string DescForDisplay
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(_descForDisplay))
				{
					switch (SanadType)
					{
					case Document.OrderSell:
					case Document.OrderBuy:
					case Document.OrderBackBuy:
					case Document.OrderBackSell:
					case Document.PishFactor:
					case Document.AmaniVorod:
					case Document.AmaniKhoroj:
					case Document.OrderPreBuy:
					{
						_fillDescForDisplayToken?.Cancel();
						_fillDescForDisplayToken = new CancellationTokenSource();
						CancellationToken token = _fillDescForDisplayToken.Token;
						if (string.IsNullOrEmpty(Description))
						{
							_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ") (...)";
						}
						else
						{
							_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ") (...)\r\n" + Description;
						}
						UpdateDescForDisplayAsync(token);
						break;
					}
					case Document.Tolid:
					case Document.Zayeat:
					case Document.EnteghalBeyneAnbar:
					case Document.AnbarGardani:
					case Document.Tolid_Mahsul:
					case Document.Tolid_Mavad:
					case Document.DepotReceipt:
					case Document.DepotHavale:
						if (string.IsNullOrEmpty(Description))
						{
							_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ")";
							break;
						}
						_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ")\r\n" + Description;
						break;
					default:
						throw new Exception($"{SanadType} not defined!");
					}
				}
			}
			catch (ThreadAbortException)
			{
				return "";
			}
			catch (OperationCanceledException)
			{
				return "";
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DescForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 339);
			}
			return _descForDisplay;
		}
	}

	public string RDescription { get; set; }

	public string ID2 { get; set; }

	public decimal? TotalHazine { get; set; }

	public decimal TotalSumWithHazine => UnitPrice + TotalHazine.GetValueOrDefault();

	public string Serial { get; set; }

	public decimal? Avarez { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? MoeinGuid { get; set; }

	public string PrdName => _prd?.Name ?? "";

	public string prdCode => _prd?.IdProduct;

	public short Ordinal { get; set; }

	public string DepotName
	{
		get
		{
			if (!string.IsNullOrEmpty(_depotName))
			{
				return _depotName;
			}
			_depotName = DepotBusiness.Get(ContextGuid, DepotGuid)?.Name ?? "";
			return _depotName;
		}
	}

	public string DateSh => DateM.MiladiToCalture();

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public string ContextGuid { get; set; }

	public decimal AmaniCount { get; set; }

	public string AmaniCountForDisplay => (AmaniCount + RemQT.GetValueOrDefault()).ToString().Replace(Text.DecimalPoint, "/").Replace("/000", "");

	public Guid? V2Guid => _v2Guid;

	public decimal V2Qt { get; set; }

	public string Vahed2Name => _vahed2?.Name ?? "";

	public bool KardexAffective { get; set; }

	public bool SanadAffective { get; private set; }

	public Guid ProductGuid => productGuid;

	public bool IsTaxImpactOnCost { get; set; }

	public decimal CostPrice { get; set; }

	public decimal TotalCostPrice { get; set; }

	public Guid? ProductTafsilGuid { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public event PropertyChangedEventHandler PropertyChanged;

	public event Func<object, KardexUpdateEventArgs, Task> OnRefreshFinished;

	public KardexBusiness(string contextGuid, clsKardexAccess access)
	{
		ContextGuid = contextGuid;
		_access = access;
		DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
	}

	public Guid GetProductGuid()
	{
		return productGuid;
	}

	public async Task SetProductGuidAsync(Guid value, EnSystem system, CancellationToken token, string tranName)
	{
		productGuid = value;
		_prd = await ProductCompactBusiness.GetAsync(ContextGuid, value, system, null, tranName, token, true);
	}

	public async Task SetV2GuidAsync(Guid? value, CancellationToken token, string tranName)
	{
		try
		{
			_v2Guid = value;
			if (!((_v2Guid ?? Guid.Empty) == Guid.Empty))
			{
				_vahed2 = await PrdVahedBusiness.GetAsync(ContextGuid, _v2Guid.Value, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetV2GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 401);
		}
	}

	private async Task UpdateDescForDisplayAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(new Random().Next(500));
			if (token.IsCancellationRequested)
			{
				return;
			}
			string text = (await TafsilBusiness.GetAsync(ContextGuid, EnSystem.UserInterface, TafsilGuid ?? Guid.Empty, "", token, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs"))?.Name;
			if (string.IsNullOrEmpty(Description) && string.IsNullOrEmpty(RDescription))
			{
				_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ") (" + text + ")";
			}
			else if (!string.IsNullOrEmpty(Description) && string.IsNullOrEmpty(RDescription))
			{
				_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ") (" + text + ")\r\n " + Description;
			}
			else if (string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(RDescription))
			{
				_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ") (" + text + ")\r\n " + RDescription;
			}
			else if (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(RDescription))
			{
				_descForDisplay = SanadType.GetDisplay() + "(" + ID2 + ") (" + text + ")\r\n " + Description + " ** " + RDescription;
			}
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					handler(this, new PropertyChangedEventArgs("DescForDisplay"));
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 424);
				}
			}, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs");
		}
		catch (OperationCanceledException)
		{
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 428);
		}
	}

	private static async Task<ReturnedSaveFuncInfo> ProgressRemAsync(string contextGuid, Guid prdGuid, Guid dptGuid, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		SemaphoreSlim semaphore = GetSemaphore(prdGuid, dptGuid);
		bool isSemaphoreTaken = false;
		int line = 0;
		bool semaphoreHasBeenTaken = false;
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 3;
			try
			{
				await _semaphoreProgressRem_ControlMaxCount.WaitAsync();
				semaphoreHasBeenTaken = true;
				await semaphore.WaitAsync(token);
				isSemaphoreTaken = true;
				if (token.IsCancellationRequested)
				{
					result = ret.AddErrorTokenIsCancelled("", "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 446);
					goto IL_05ed;
				}
				line = 2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 449), ReturnedState.None, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 449);
				}
				transactionHasBeenCreated = true;
				line = 4;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.ProgressRemAsync(prdGuid, dptGuid, token, tranName), ReturnedState.None, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 452);
				line = 5;
				if (token.IsCancellationRequested)
				{
					result = ret.AddErrorTokenIsCancelled("", "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 454);
					goto IL_05ed;
				}
				line = 6;
				if (ret.HasError)
				{
					result = ret;
					goto IL_05ed;
				}
				line = 700;
				Func<object, KardexUpdateEventArgs, Task> onRefreshFinished = ClsCache.GetCache(contextGuid).BusinessAggregator.KardexBusiness.OnRefreshFinished;
				if (onRefreshFinished != null)
				{
					PacketParser.Utilities.RaiseEvent(onRefreshFinished, ClsCache.GetCache(contextGuid).BusinessAggregator.KardexBusiness, new KardexUpdateEventArgs(null, null), token, 60, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs");
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 462);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 465);
				ret.AddReturnedValue(ex, "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 466);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 471), ReturnedState.None, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 471);
			}
			if (isSemaphoreTaken)
			{
				semaphore.Release();
			}
			if (semaphoreHasBeenTaken)
			{
				_semaphoreProgressRem_ControlMaxCount.Release();
			}
		}
		return ret;
		IL_05ed:
		return result;
	}

	private static SemaphoreSlim GetSemaphore(Guid prdGuid, Guid dptGuid)
	{
		try
		{
			if (_semaphoreDic.TryGetValue($"{prdGuid}/{dptGuid}", out var value) && value != null)
			{
				return value;
			}
			value = new SemaphoreSlim(1, 1);
			_semaphoreDic.TryAdd($"{prdGuid}/{dptGuid}", value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSemaphore", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 490);
			return null;
		}
	}

	private static async Task<ReturnedSaveFuncInfo> ProgressRemAsync(string contextGuid, CancellationToken token, short maxParalelTasks, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _semaphoreProgressRem.WaitAsync();
			semaphoreHasBeenTaken = true;
			if (maxParalelTasks > 20)
			{
				maxParalelTasks = 20;
			}
			ReturnedSaveFuncInfoWithValue<IEnumerable<KardexInvalidatedModel>> returnedSaveFuncInfoWithValue = await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllInvalidated_PrdId_DepotId_Async(token, tranName);
			line = 1;
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 506);
			line = 2;
			if (ret.HasError)
			{
				return ret;
			}
			line = 3;
			List<Task<ReturnedSaveFuncInfo>> taskList = new List<Task<ReturnedSaveFuncInfo>>();
			line = 4;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			if (returnedSaveFuncInfoWithValue?.value != null)
			{
				foreach (KardexInvalidatedModel item in returnedSaveFuncInfoWithValue.value)
				{
					line = 5;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 516);
					}
					line = 6;
					taskList.Add(Task.Run(() => ProgressRemAsync(contextGuid, item.ProductGuid, item.DepotGuid, token)));
					line = 70;
					if (taskList.Count >= maxParalelTasks)
					{
						line = 8;
						Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(taskList);
						line = 9;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 524);
						line = 10;
						taskList.Remove(t);
					}
				}
			}
			line = 11;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 529);
			}
			line = 12;
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(taskList), ReturnedState.None, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 531);
			line = 13;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 533);
			}
			line = 14;
			if (ret.HasError)
			{
				return ret;
			}
			line = 15;
			await PacketParser.Utilities.RaiseEvent(ClsCache.GetCache(contextGuid).BusinessAggregator.KardexBusiness.OnRefreshFinished, ClsCache.GetCache(contextGuid).BusinessAggregator.KardexBusiness, new KardexUpdateEventArgs(null, null), token, 60, "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 541);
			ret.AddReturnedValue(ex, "", "ProgressRemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 542);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphoreProgressRem.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> FillAsync(string contextGuid, EnSystem system, Guid? sanadGuid, Document? sanadType, DateTime? newDate, short maxParalelThreads = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				if (system == EnSystem.Transfer)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 560), ReturnedState.None, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 560);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.FillAsync(sanadGuid, sanadType, newDate, token, tranName), ReturnedState.None, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 562);
				Task.Run(() => ProgressRemAsync(contextGuid, default(CancellationToken), maxParalelThreads));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 567);
				ret.AddReturnedValue(ex, "", "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 568);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 573), ReturnedState.None, "FillAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 573);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<KardexBusiness>>> GetAllAsync(string contextGuid, KardexFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<IEnumerable<KardexBusiness>> res = new ReturnedSaveFuncInfoWithValue<IEnumerable<KardexBusiness>>();
		SemaphoreSlim semaphoreSlim = ((!string.IsNullOrEmpty(tranName)) ? (await _getKardexByTranName.GetSemaphoreAsync(tranName)) : new SemaphoreSlim(1, 1));
		SemaphoreSlim semaphore = semaphoreSlim;
		await semaphore.WaitAsync();
		try
		{
			IEnumerable<KardexBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllAsync(filter, token, tranName);
			ProductCompactBusiness prd = await ProductCompactBusiness.GetAsync(contextGuid, filter?.PrdGuid ?? Guid.Empty, EnSystem.UserInterface, (filter?.Order == null) ? null : ((OrderBusiness)filter.Order), tranName, token, true);
			DepotBusiness depot = await DepotBusiness.GetAsync(contextGuid, filter?.DepotGuid ?? Guid.Empty, token, tranName);
			decimal reportRem = (list?.LastOrDefault()?.AmaniCountForDisplay?.Replace("/", Text.DecimalPoint) ?? "").ParseToDecimal(0m, 3);
			if ((filter?.PrdGuid ?? Guid.Empty) != Guid.Empty && (filter?.DepotGuid ?? Guid.Empty) != Guid.Empty)
			{
				decimal num = (await PrdAnbarRelationBusiness.GetAsync(contextGuid, EnSystem.UserInterface, filter?.PrdGuid ?? Guid.Empty, filter?.DepotGuid ?? Guid.Empty, token, tranName))?.Amount ?? 0m;
				if (Math.Abs(reportRem - num) > 0m)
				{
					res.AddWarning("در مانده حساب کالای".GetTranslate() + "(" + prd?.Name + ")" + "در انبار".GetTranslate() + "(" + depot?.Name + ")\r\n" + " اختلاف وجود دارد لطفا به دفتر خدمات پس از فروش نرم افزار مراجعه کنید".GetTranslate() + "\r\n" + "میزان اختلاف".GetTranslate() + ":" + (reportRem - num).ToString(3) + "\r\n" + "مانده در گزارش".GetTranslate() + ": " + reportRem.ToString(3) + "\r\n" + "مانده کالا در انبار".GetTranslate() + ":" + num.ToString(3), "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 592);
				}
			}
			ReturnedSaveFuncInfoWithValue<IEnumerable<KardexBusiness>> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.value = await ApplyAccessAsync(contextGuid, list?.ToList(), token);
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 603);
			return null;
		}
		finally
		{
			semaphore.Release();
		}
	}

	public static async Task<KardexBusiness> GetLastAsync(string contextGuid, Guid? tafsilGuid, Guid? prdGuid, Document? factorType = null, CancellationToken token = default(CancellationToken), string tranName = "", IOrder order = null)
	{
		try
		{
			KardexFilter filter = new KardexFilter
			{
				TafsilGuid = tafsilGuid,
				PrdGuid = prdGuid,
				FactorType = factorType,
				CountForDisplay = (short)1,
				Order = order
			};
			return (await GetAllAsync(contextGuid, filter, token, tranName))?.value?.SingleOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 623);
			throw;
		}
	}

	public static Task<KardexSumModel> GetSummaryInfoAsync(string contextGuid, KardexSummaryFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetSummaryInfoAsync(filter, token, tranName);
	}

	public static Task<decimal> LastVorodPriceAvgAsync(string contextGuid, Guid prdGuid, DateTime? date, Guid? depotGuid, bool? kardexAffective, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.LastVorodPriceAvgAsync(prdGuid, date, depotGuid, kardexAffective, token, tranName);
	}

	public static Task<KardexBusiness> GetLastRecordsByDateAsync(string contextGuid, DateTime date, Guid prdGuid, Guid depotGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetLastRecordsByDateAsync(date, prdGuid, depotGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<KardexViewModel>>> GetAllViewModelAsync(string contextGuid, EnSystem system, CancellationToken token, KardexSumFilter filter, string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<List<KardexViewModel>> ret = new ReturnedSaveFuncInfoWithValue<List<KardexViewModel>>();
		int line = 0;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			IEnumerable<KardexViewModel> all2 = await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllViewModelAsync(token, filter.DateFrom, filter.DateTo, filter.HasGardeshManfi, filter.HasOrderPoint, filter.KardexAffective, tranName);
			line = 1;
			if (token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 642);
			}
			line = 2;
			if (ret.HasError)
			{
				return ret;
			}
			line = 3;
			if (token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 646);
			}
			line = 4;
			if (ret.HasError)
			{
				return ret;
			}
			line = 10;
			if (token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 650);
			}
			if (ret.HasError)
			{
				return ret;
			}
			line = 15;
			List<string> list = filter?.SearchString?.ToLower()?.RemoveCharactersForLinkSearch()?.SplitString()?.ToList();
			line = 16;
			if (list != null && list.Any())
			{
				foreach (string item in list)
				{
					line = 20;
					token.ThrowIfCancellationRequested();
					line = 21;
					if (!string.IsNullOrEmpty(item) && item.Trim() != "")
					{
						all2 = all2?.Where((KardexViewModel x) => x.ProductCode.StringComparisons(item) || x.ProductName.StringComparisons(item) || x.DepotName.StringComparisons(item));
					}
					line = 25;
					if (token.IsCancellationRequested)
					{
						ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 666);
					}
					line = 26;
					if (ret.HasError)
					{
						return ret;
					}
					line = 27;
				}
			}
			line = 30;
			ProductCompactFilter filterPC = new ProductCompactFilter
			{
				Status = filter.ProductStatus,
				Sort = (cashier?.Settings?.Products?.ProductSort).GetValueOrDefault(),
				LoadJustParents = false
			};
			if ((filter?.ProductGroupGuids?.Any()).GetValueOrDefault())
			{
				List<Guid> validProductGuids = (await ProductGroupRelationBusiness.GetAllAsync(contextGuid, filter.ProductGroupGuids, token, tranName))?.Select((ProductGroupRelationBusiness q) => q.ProductGuid)?.ToList();
				if (validProductGuids?.Any() ?? false)
				{
					all2 = all2?.Where((KardexViewModel q) => validProductGuids.Contains(q.ProductGuid));
				}
			}
			List<Guid> products = (await ProductCompactBusiness.GetAllAsync(contextGuid, system, filterPC, token))?.Select((ProductCompactBusiness p) => p.Guid)?.ToList() ?? new List<Guid>();
			line = 31;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 689);
			}
			line = 32;
			all2 = all2?.Where((KardexViewModel p) => products.Contains(p.ProductGuid));
			line = 33;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 693);
			}
			line = 34;
			ReturnedSaveFuncInfoWithValue<List<KardexViewModel>> returnedSaveFuncInfoWithValue = ret;
			if (returnedSaveFuncInfoWithValue.value == null)
			{
				returnedSaveFuncInfoWithValue.value = new List<KardexViewModel>();
			}
			line = 35;
			ret.value = all2?.OrderBy((KardexViewModel o) => o.ProductName)?.ToList();
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 699);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 702);
			ret.AddReturnedValue(ex, "", "GetAllViewModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 703);
		}
		return ret;
	}

	private static async Task<IEnumerable<KardexBusiness>> ApplyAccessAsync(string contextGuid, List<KardexBusiness> items, CancellationToken token)
	{
		_ = 2;
		try
		{
			if (!(items?.Any() ?? false))
			{
				return items;
			}
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashierBusiness == null)
			{
				return items;
			}
			UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token);
			if (userAccBusiness == null)
			{
				return items;
			}
			List<clsKardexAccess> kardexAccessList = userAccBusiness.UserAccess.KardexAccessList;
			if (kardexAccessList == null || !kardexAccessList.Any())
			{
				return items;
			}
			List<KardexBusiness> deletedList = new List<KardexBusiness>();
			foreach (clsKardexAccess access in kardexAccessList)
			{
				List<Guid> list = items?.Select((KardexBusiness q) => q.ProductGuid)?.ToList();
				if (list == null || !list.Any())
				{
					continue;
				}
				ConcurrentDictionary<Guid, List<Guid>> concurrentDictionary = await ProductGroupRelationBusiness.GetDictionaryAsync(contextGuid, list, token);
				if (concurrentDictionary == null || !concurrentDictionary.Any())
				{
					continue;
				}
				List<Guid> list2 = concurrentDictionary?.SelectMany((KeyValuePair<Guid, List<Guid>> q) => q.Value)?.ToList();
				if (list2 == null || !list2.Any() || (!list2.Contains(access.Guid) && access.Type == EnKardexAccessType.PrdGroup))
				{
					continue;
				}
				List<Guid> accessProducts = concurrentDictionary.Where((KeyValuePair<Guid, List<Guid>> q) => q.Value.Contains(access.Guid))?.Select((KeyValuePair<Guid, List<Guid>> q) => q.Key)?.ToList();
				if (!(accessProducts?.Any() ?? false))
				{
					continue;
				}
				IEnumerable<KardexBusiness> enumerable = items?.Where((KardexBusiness q) => accessProducts.Contains(q.ProductGuid));
				if (enumerable == null || !enumerable.Any())
				{
					continue;
				}
				if (!access.Qt_Buy_Vorod && !access.Rialy_Buy_Vorod)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.OrderBuy));
				}
				if (!access.Qt_BackSell_Vorod && !access.Rialy_BackSell_Vorod)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.OrderBackSell));
				}
				if (!access.Qt_Anbargardani_Ezafat_Vorod && !access.Rialy_Anbargardani_Ezafat_Vorod)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.AnbarGardani && q.VorodQT != 0m));
				}
				if (!access.Qt_Tolid_Mahsol_Vorod && !access.Rialy_Tolid_Mahsol_Vorod)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.Tolid && q.VorodQT != 0m));
				}
				if (!access.Qt_Enteghal_Vorod && !access.Rialy_Enteghal_Vorod)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.EnteghalBeyneAnbar && q.VorodQT != 0m));
				}
				if (!access.Qt_Anbargardani_Kasr_Khoroj && !access.Rialy_Anbargardani_Kasr_khoroj)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.AnbarGardani && q.KhorojQT != 0m));
				}
				if (!access.Qt_Sell_Khoroj && !access.Rialy_Sell_Khoroj)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.OrderSell));
				}
				if (!access.Qt_BackBuy_Khoroj && !access.Rialy_BackBuy_Khoroj)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.OrderBackBuy));
				}
				if (!access.Qt_Zayeat_Khoroj && !access.Rialy_Zayeat_Khoroj)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.Zayeat));
				}
				if (!access.Qt_Tolid_Mavad_Khoroj && !access.Rialy_Tolid_Mavad_Khoroj)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.Tolid && q.KhorojQT != 0m));
				}
				if (!access.Qt_Enteghal_Khoroj && !access.Rialy_Enteghal_Khoroj)
				{
					deletedList.AddRange(enumerable.Where((KardexBusiness q) => q.SanadType == Document.EnteghalBeyneAnbar && q.KhorojQT != 0m));
				}
			}
			if (deletedList?.Any() ?? false)
			{
				foreach (KardexBusiness item in deletedList)
				{
					items.Remove(item);
				}
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			items = items?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 782);
		}
		return items;
	}

	public static async Task<List<KardexCustomerModel>> GetAllCustomerGardeshBetweenAsync(string contextGuid, CancellationToken token, DateTime date1, DateTime date2, bool? kardexAffective = true, Guid? groupGuid = null, string tranName = "")
	{
		try
		{
			if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
			{
				return null;
			}
			List<KardexCustomerModel> list = await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllCustomerGardeshBetweenAsync(token, date1, date2, kardexAffective, tranName);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (groupGuid.HasValue && groupGuid != Guid.Empty)
			{
				list = list?.Where(delegate(KardexCustomerModel q)
				{
					Guid groupGuid2 = q.GroupGuid;
					Guid? guid = groupGuid;
					return groupGuid2 == guid;
				})?.ToList();
			}
			return token.IsCancellationRequested ? null : list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllCustomerGardeshBetweenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 798);
			return null;
		}
	}

	public static async Task<List<KardexTajmieReportBusiness>> GetAllGardeshProductAsync(string contextGuid, DateTime date1, DateTime date2, bool? kardexAffective = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllGardeshProductAsync(date1, date2, kardexAffective, token, tranName);
	}

	public static Task<double> GetPrdAmountAsync(string contextGuid, Guid prdGuid, Guid depotGuid, DateTime date, bool? kardexAffective, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetPrdAmountAsync(prdGuid, depotGuid, date, kardexAffective, token, tranName);
	}

	public static Task<List<OrderMoghayeratModel>> GetAllOrderMoghayeratAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllOrderMoghayeratAsync(token, tranName);
	}

	public static Task<DateTime?> GetLastRecordDateAsync(string contextGuid, bool? kardexAffective = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetLastRecordDateAsync(kardexAffective, token, tranName);
	}

	public static Task<IEnumerable<KardexLinkedOrder>> GetAllLinkSellAsync(string contextGuid, Guid tafsilGuid, Guid productGuid, DateTime dateM, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllLinkSellAsync(tafsilGuid, productGuid, dateM, token, tranName);
	}

	public static Task<KardexBusiness> GetAsync(string contextGuid, Guid productGuid, Guid depotGuid, DateTime date, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAsync(productGuid, depotGuid, date, token, tranName);
	}

	public static async Task<IEnumerable<KardexConflictRecord>> GetAllConflictsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			return await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllConflictsAsync((MaliYearSanadType)accGlobalSettings.CompanyInfo.SanadType, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 822);
			throw;
		}
	}

	public static async Task<string> GetLastBuyAsync(string contextGuid, Guid productGuid, EnPriceType priceType, Guid? tafsilGuid, Document document, CancellationToken token)
	{
		_ = 6;
		try
		{
			string kardexPrice = "";
			if (productGuid == Guid.Empty)
			{
				return kardexPrice;
			}
			ENSource source = ClsCache.GetCache(contextGuid).Source;
			IEnumerable<ProductGroupRelationBusiness> groupRelation = await ProductGroupRelationBusiness.GetAllAsync(contextGuid, productGuid, null, token);
			if (!(groupRelation?.Any() ?? false) || token.IsCancellationRequested)
			{
				return kardexPrice;
			}
			List<clsKardexAccess> kardexAccessList = null;
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashierBusiness != null)
			{
				kardexAccessList = (await cashierBusiness.GetUserAsync(token)).UserAccess.KardexAccessList;
			}
			if (token.IsCancellationRequested)
			{
				return kardexPrice;
			}
			foreach (ProductGroupRelationBusiness group in groupRelation)
			{
				if (token.IsCancellationRequested)
				{
					return "";
				}
				if ((group?.GroupGuid ?? Guid.Empty) == Guid.Empty)
				{
					continue;
				}
				clsKardexAccess clsKardexAccess = kardexAccessList?.FirstOrDefault((clsKardexAccess q) => q.Guid == group.GroupGuid);
				if (clsKardexAccess != null && !clsKardexAccess.Rialy_Buy_Vorod)
				{
					kardexPrice = "---";
					continue;
				}
				switch (priceType)
				{
				case EnPriceType.BuyLastFromAll:
					kardexPrice = ((await GetLastAsync(contextGuid, null, productGuid, Document.OrderBuy, token))?.UnitPrice ?? 0m).NpToString("N0", 0);
					break;
				case EnPriceType.BuyLastFromCus:
					kardexPrice = ((source != ENSource.Accounting) ? ((await LastPriceBusiness.GetAllAsync(contextGuid, productGuid, tafsilGuid ?? Guid.Empty, document, token, ""))?.SingleOrDefault()?.Price).GetValueOrDefault().NpToString("N0", 0) : ((await GetLastAsync(contextGuid, tafsilGuid, productGuid, Document.OrderBuy, token))?.UnitPrice ?? 0m).NpToString("N0", 0));
					return kardexPrice;
				case EnPriceType.BuyLast:
				{
					CancellationToken token2 = token;
					kardexPrice = (await ProductCompactBusiness.LastVorodPriceAvgAsync(contextGuid, productGuid, null, null, token2)).NpToString("N0", 0);
					return kardexPrice;
				}
				default:
					throw new NotImplementedException();
				}
			}
			return kardexPrice;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexBusiness.cs", 886);
			throw;
		}
	}

	public static Task<KardexBusiness> GetLastEnteredRecordAsync(string contextGuid, Guid productGuid, Guid? depotGuid, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetLastEnteredRecordAsync(productGuid, depotGuid, token, tranName);
	}
}
