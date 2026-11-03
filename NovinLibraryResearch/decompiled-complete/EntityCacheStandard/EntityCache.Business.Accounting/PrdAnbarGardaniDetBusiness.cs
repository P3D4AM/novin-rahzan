using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting;

public class PrdAnbarGardaniDetBusiness : Serializable<PrdAnbarGardaniDetBusiness>, IPrdAnbarGardaniDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 50);

	private List<PrdSerialBusiness> _serialStr;

	private decimal _amount;

	private decimal _qtEzafe;

	private decimal _qtKasr;

	private decimal unitPrice;

	private decimal? _qtRealAmount;

	private Guid prdGuid;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid MasterGuid
	{
		get
		{
			return AnbarGardaniMaster?.Guid ?? Guid.Empty;
		}
		set
		{
		}
	}

	public Guid PrdGuid
	{
		get
		{
			return prdGuid;
		}
		set
		{
			if (value == Guid.Empty)
			{
				throw new ArgumentException("Guid empty is not valid value for PrdGuid");
			}
			prdGuid = value;
		}
	}

	public Guid DptGuid { get; set; }

	public decimal QtKasr
	{
		get
		{
			return _qtKasr;
		}
		set
		{
			if (value != 0m)
			{
				QtEzafe = 0m;
			}
			_qtKasr = value;
		}
	}

	public decimal? QtRealAmountForDisplay
	{
		get
		{
			return _qtRealAmount;
		}
		set
		{
			_qtRealAmount = value;
		}
	}

	public decimal QtKasrForDisplay
	{
		get
		{
			return _qtKasr;
		}
		set
		{
			_qtKasr = value;
		}
	}

	public decimal QtEzafeForDisplay
	{
		get
		{
			return _qtEzafe;
		}
		set
		{
			_qtEzafe = value;
		}
	}

	public decimal QtEzafe
	{
		get
		{
			return _qtEzafe;
		}
		set
		{
			if (value != 0m)
			{
				QtKasr = 0m;
			}
			_qtEzafe = value;
		}
	}

	public decimal UnitPrice
	{
		get
		{
			return unitPrice;
		}
		set
		{
			unitPrice = value;
			if (unitPrice < 1m && unitPrice > 0m)
			{
				unitPrice = 1m;
			}
		}
	}

	public decimal? QtRealAmount
	{
		get
		{
			return _qtRealAmount;
		}
		set
		{
			if (!value.HasValue)
			{
				_qtRealAmount = null;
				QtEzafe = 0m;
				QtKasr = 0m;
				return;
			}
			_qtRealAmount = Math.Round(value.Value, 3);
			decimal num = Amount - _qtRealAmount.Value;
			if (num < 0m)
			{
				QtEzafe = Math.Abs(num);
			}
			else
			{
				QtKasr = num;
			}
		}
	}

	public decimal Amount
	{
		get
		{
			return _amount;
		}
		set
		{
			_amount = Math.Round(value, 3);
		}
	}

	public string PrdName { get; set; }

	public string PrdCode { get; set; }

	public string PrdVahedName { get; set; }

	public string PrdGroupName { get; set; }

	public string PrdDepotName { get; set; }

	[JsonIgnore]
	public decimal KasrSum => Math.Round(QtKasr * UnitPrice, DigitAfterPoint);

	[JsonIgnore]
	public decimal EzafeSum => Math.Round(QtEzafe * UnitPrice, DigitAfterPoint);

	public string ContextGuid { get; set; }

	public DateTime LastAnbarGardaniDate { get; set; }

	[JsonIgnore]
	public string LastAnbarGardaniDateSh => LastAnbarGardaniDate.MiladiToCalture();

	public DateTime? FirstSanadDate { get; set; }

	[JsonIgnore]
	public string FirstSanadDateSh => FirstSanadDate.MiladiToCalture();

	public DateTime? LastSanadDate { get; set; }

	[JsonIgnore]
	public string LastSanadDateSh => LastSanadDate.MiladiToCalture();

	[XmlIgnore]
	[JsonIgnore]
	public PrdAnbarGardaniBusiness AnbarGardaniMaster { get; internal set; }

	[JsonIgnore]
	public PrdAnarGardaniDetStatus PrdAnarGardaniDetStatus
	{
		get
		{
			if (!QtRealAmount.HasValue)
			{
				return PrdAnarGardaniDetStatus.None;
			}
			if (QtEzafe > 0m)
			{
				return PrdAnarGardaniDetStatus.Ezafat;
			}
			if (QtKasr > 0m)
			{
				return PrdAnarGardaniDetStatus.Kosorat;
			}
			return PrdAnarGardaniDetStatus.Motabeghat;
		}
	}

	public IEnumerable<BarcodeBusiness> PrdBarcodeList { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public short DigitAfterPoint { get; }

	[XmlIgnore]
	[JsonIgnore]
	public bool SerialHasBeenChanged => _serialStr?.Any() ?? false;

	public PrdAnbarGardaniDetBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 127);
		}
	}

	private PrdAnbarGardaniDetBusiness(string contextGuid, PrdAnbarGardaniBusiness prdAnbarGardaniBusiness, Guid? prdGuid = null, Guid? depotGuid = null)
	{
		try
		{
			ContextGuid = contextGuid;
			AnbarGardaniMaster = prdAnbarGardaniBusiness;
			if (prdGuid.HasValue)
			{
				PrdGuid = prdGuid.Value;
			}
			if (depotGuid.HasValue)
			{
				DptGuid = depotGuid.Value;
			}
			DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 140);
		}
	}

	public static PrdAnbarGardaniDetBusiness GetFactory(string contextGuid, PrdAnbarGardaniBusiness prdAnbarGardaniBusiness, Guid? prdGuid = null, Guid? depotGuid = null)
	{
		return new PrdAnbarGardaniDetBusiness(contextGuid, prdAnbarGardaniBusiness, prdGuid, depotGuid);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(IEnumerable<ProductCompactBusiness> _prdList, EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		try
		{
			semaphoreHasBeenTaken = await _connectionPoolSemaphore.WaitAsync(60000, token);
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			if (system != EnSystem.UserInterface || source != ENSource.Accounting)
			{
				return ret;
			}
			if ((QtEzafe != 0m || QtKasr != 0m) && UnitPrice == 0m && !isDeleteAction)
			{
				ret.AddError(string.Concat(str1: (" (" + ((source != ENSource.Android_HesabYar) ? _prdList?.SingleOrDefault((ProductCompactBusiness q) => q.Guid == PrdGuid) : (await ProductCompactBusiness.GetAsync(ContextGuid, PrdGuid, system, null, tranName, token, true)))?.Name + ")").GetTranslate(), str0: "مبلغ برای کالای".GetTranslate(), str2: " وارد نشده است.".GetTranslate()), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 158);
			}
			if (system == EnSystem.UserInterface && !isDeleteAction && AnbarGardaniMaster.Type != SanadType.Eftetahiye && AnbarGardaniMaster.Type != SanadType.Ekhtetamiye)
			{
				ProductCompactBusiness prd = ((source != ENSource.Android_HesabYar) ? _prdList?.SingleOrDefault((ProductCompactBusiness q) => q.Guid == PrdGuid) : (await ProductCompactBusiness.GetAsync(ContextGuid, PrdGuid, system, null, tranName, token, true)));
				if (prd == null)
				{
					throw new NullReferenceException("در اعتبار سنجی اقلام انبارگردانی کالا با شناسه".GetTranslate() + $"({prdGuid}) " + "در نرم افزار حسابداری پیدانشد!".GetTranslate());
				}
				if (prd.Kind == EnProductKind.Variation && (prd.MasterGuid ?? Guid.Empty) == Guid.Empty)
				{
					ret.AddError("کالای".GetTranslate() + " (" + prd.Name + ") " + "به علت اینکه تنوع دار هست امکان درج شدن در برگه انبارگردانی را ندارد. شما صرفا میتوانید تنوع های این کالا را انبارگردانی نمایید. ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 169);
				}
				if (!(await prd.GetIsSerialAsync(token, tranName)))
				{
					return ret;
				}
				AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				int num = _serialStr?.Count() ?? 0;
				if ((decimal)num != Math.Abs(QtRealAmount.GetValueOrDefault()))
				{
					ret.AddReturnedValue(accGlobalSettings.DepotSetting.SanadWithoutSerial, "تعداد کالای".GetTranslate() + "  " + prd?.Name + " " + " انبارگردانی شده".GetTranslate() + $" {Math.Abs(QtRealAmount.GetValueOrDefault()).ToString().ParseToDecimal(0m, 3)}" + " و تعداد سریال وارد شده".GetTranslate() + " " + $" {num}".GetTranslate() + " " + " منطبق نمی باشند".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 177);
				}
				Tuple<string, int> tuple = _serialStr?.GroupBy((PrdSerialBusiness q) => q.Serial)?.Select((IGrouping<string, PrdSerialBusiness> q) => new Tuple<string, int>(q.Key, q.Count()))?.FirstOrDefault((Tuple<string, int> q) => q.Item2 > 1);
				if (tuple != null)
				{
					ret.AddReturnedValue(accGlobalSettings.DepotSetting.SanadWithoutSerial, "سریال".GetTranslate() + " " + (tuple.Item1 ?? "").GetTranslate() + " " + "مربوط به کالای".GetTranslate() + " " + (" " + prd?.Name).GetTranslate() + " " + " در بیش از یک انبار ثبت شده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 181);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await PrdSerialBusiness.CheckValidationAsync(ContextGuid, _serialStr, system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 183);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 188);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
		return ret;
	}

	public static Task<Guid> GetDepotGuidAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetDepotGuidAsync(guid, token, tranName);
	}

	public async Task<List<PrdSerialBusiness>> GetSerialsAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_serialStr?.Any() ?? false)
			{
				return _serialStr;
			}
			ProductSerialFilter filter = new ProductSerialFilter
			{
				AsType = Document.AnbarGardani,
				ProductGuid = PrdGuid,
				DetailGuid = Guid
			};
			_serialStr = (await PrdSerialBusiness.GetAllAsync(ContextGuid, filter, token, tranName))?.ToList();
			return _serialStr;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSerialsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 216);
			throw;
		}
	}

	public void AddRangeToSerialList(EnSystem system, List<PrdSerialBusiness> listSerial, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("AddRangeToSerialList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs"))
		{
			try
			{
				if (system == EnSystem.Mapper)
				{
					_serialStr = listSerial;
					return;
				}
				_serialStr = new List<PrdSerialBusiness>();
				List<PrdSerialBusiness> list = null;
				List<PrdSerialBusiness> list2 = listSerial;
				if (list2 != null && list2.Any())
				{
					list = _serialStr.Where((PrdSerialBusiness q) => !listSerial.Select((PrdSerialBusiness q) => q.Serial).Contains(q.Serial))?.ToList();
				}
				else
				{
					List<PrdSerialBusiness> list3 = listSerial;
					if (list3 == null || !list3.Any())
					{
						list = _serialStr;
					}
				}
				if (list != null && list.Any())
				{
					foreach (PrdSerialBusiness item in list)
					{
						item.Status = false;
					}
				}
				List<PrdSerialBusiness> list4 = listSerial.Where((PrdSerialBusiness q) => !_serialStr.Select((PrdSerialBusiness q) => q.Serial).Contains(q.Serial))?.ToList();
				if (list4 == null || !list4.Any())
				{
					return;
				}
				foreach (PrdSerialBusiness item2 in list4)
				{
					int num = 1;
					if ((item2?.DetList?.Any()).GetValueOrDefault())
					{
						num = item2.DetList.Max((PrdSerialDetailBusiness q) => q.Ordinal);
					}
					PrdSerialDetailBusiness det = new PrdSerialDetailBusiness(ContextGuid)
					{
						AsType = Document.AnbarGardani,
						DetailGuid = Guid,
						Guid = Guid.NewGuid(),
						IsVorod = true,
						MasterGuid = item2.Guid,
						Modified = DateTime.Now,
						Ordinal = checked((short)num),
						SanadDate = AnbarGardaniMaster?.DateM
					};
					item2.AddDet(det);
				}
				_serialStr.AddRange(list4);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToSerialList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 267);
				throw;
			}
		}
	}

	public void Reset()
	{
		try
		{
			_qtEzafe = default(decimal);
			_qtKasr = default(decimal);
			_qtRealAmount = default(decimal);
			Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Reset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniDetBusiness.cs", 283);
			throw;
		}
	}
}
