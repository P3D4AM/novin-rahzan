using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting.Coding;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.SerializeData;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting;

public class BankBusiness : Serializable<BankBusiness>, IBank, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private BankData _bankData;

	public BankData BankData => _bankData ?? (_bankData = (Tafsil?.SerializedData).FromJson<BankData>("BankData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 27) ?? new BankData());

	public bool Status
	{
		get
		{
			return Tafsil.Status;
		}
		set
		{
			Tafsil.Status = value;
		}
	}

	public ServerStatusForClient ServerStatus
	{
		get
		{
			return Tafsil.ServerStatus;
		}
		set
		{
			Tafsil.ServerStatus = value;
		}
	}

	public long Code
	{
		get
		{
			return Tafsil.Code;
		}
		set
		{
			Tafsil.Code = value;
		}
	}

	public Guid Guid
	{
		get
		{
			return Tafsil.Guid;
		}
		set
		{
			Tafsil.Guid = value;
		}
	}

	public DateTime Modified
	{
		get
		{
			return Tafsil.Modified;
		}
		set
		{
			Tafsil.Modified = value;
		}
	}

	public string Name
	{
		get
		{
			return Tafsil.Name;
		}
		set
		{
			Tafsil.Name = value;
		}
	}

	public string Description
	{
		get
		{
			return Tafsil?.Description;
		}
		set
		{
			Tafsil.Description = value;
		}
	}

	public DateTime DateM
	{
		get
		{
			return Tafsil.DateM;
		}
		set
		{
			Tafsil.DateM = value;
		}
	}

	public decimal Account { get; private set; }

	public bool IsPos
	{
		get
		{
			return BankData.IsPos;
		}
		set
		{
			BankData.IsPos = value;
		}
	}

	public string Shobe
	{
		get
		{
			return BankData.Shobe;
		}
		set
		{
			BankData.Shobe = value;
		}
	}

	public string ShobeCode
	{
		get
		{
			return BankData.ShobeCode;
		}
		set
		{
			BankData.ShobeCode = value;
		}
	}

	public string HesabNo
	{
		get
		{
			return BankData.HesabNo;
		}
		set
		{
			BankData.HesabNo = value;
		}
	}

	public string HesabType
	{
		get
		{
			return BankData.HesabType;
		}
		set
		{
			BankData.HesabType = value;
		}
	}

	public string Tel1
	{
		get
		{
			return BankData.Tel1;
		}
		set
		{
			BankData.Tel1 = value;
		}
	}

	public string Tel2
	{
		get
		{
			return BankData.Tel2;
		}
		set
		{
			BankData.Tel2 = value;
		}
	}

	public string Fax
	{
		get
		{
			return BankData.Fax;
		}
		set
		{
			BankData.Fax = value;
		}
	}

	[Obsolete("Use NegativeAmount")]
	public bool Manfi
	{
		get
		{
			return BankData?.BankManfi ?? false;
		}
		set
		{
			BankData.BankManfi = value;
		}
	}

	public ReturnedState NegativeAmount
	{
		get
		{
			if (BankData == null)
			{
				return ReturnedState.Warning;
			}
			if (!BankData.NegativeAmount.HasValue)
			{
				if (!BankData.BankManfi)
				{
					return ReturnedState.Error;
				}
				return ReturnedState.Warning;
			}
			return BankData.NegativeAmount.Value;
		}
		set
		{
			BankData.NegativeAmount = value;
		}
	}

	public string DarandeName
	{
		get
		{
			return BankData.DarandeName;
		}
		set
		{
			BankData.DarandeName = value;
		}
	}

	public string DarandeTel
	{
		get
		{
			return BankData.DarandeTel;
		}
		set
		{
			BankData.DarandeTel = value;
		}
	}

	public string DarandeADD
	{
		get
		{
			return BankData.DarandeADD;
		}
		set
		{
			BankData.DarandeADD = value;
		}
	}

	public string VName
	{
		get
		{
			return BankData.VName;
		}
		set
		{
			BankData.VName = value;
		}
	}

	public string VFName
	{
		get
		{
			return BankData.VFName;
		}
		set
		{
			BankData.VFName = value;
		}
	}

	public string VShenas
	{
		get
		{
			return BankData.VShenas;
		}
		set
		{
			BankData.VShenas = value;
		}
	}

	public string VMeli
	{
		get
		{
			return BankData.VMeli;
		}
		set
		{
			BankData.VMeli = value;
		}
	}

	public string VPosti
	{
		get
		{
			return BankData.VPosti;
		}
		set
		{
			BankData.VPosti = value;
		}
	}

	public string VTel
	{
		get
		{
			return BankData.VTel;
		}
		set
		{
			BankData.VTel = value;
		}
	}

	public string VAdd
	{
		get
		{
			return BankData.VAdd;
		}
		set
		{
			BankData.VAdd = value;
		}
	}

	public string ParentFullCode
	{
		get
		{
			return BankData.ParentFullCode;
		}
		set
		{
			BankData.ParentFullCode = value;
		}
	}

	public string PortName
	{
		get
		{
			return BankData.PortName ?? "";
		}
		set
		{
			BankData.PortName = value;
		}
	}

	public string ShabaNumber
	{
		get
		{
			return BankData.ShabaNumber ?? "";
		}
		set
		{
			BankData.ShabaNumber = value;
		}
	}

	public string CardNumber
	{
		get
		{
			return BankData.CardNumber ?? "";
		}
		set
		{
			BankData.CardNumber = value;
		}
	}

	public EnumConnectionType ConnectionType
	{
		get
		{
			return BankData?.ConnectionType ?? EnumConnectionType.None;
		}
		set
		{
			BankData.ConnectionType = value;
		}
	}

	public long PortSpeed
	{
		get
		{
			return BankData?.PortSpeed ?? 0;
		}
		set
		{
			BankData.PortSpeed = value;
		}
	}

	public Guid TafsilGroupGuid
	{
		get
		{
			return Tafsil?.TafsilGroupGuid ?? ParentDefaults.TafsilLevelGuids.Banks;
		}
		set
		{
			Tafsil.TafsilGroupGuid = value;
		}
	}

	public List<string> BarcodeFormatList
	{
		get
		{
			return BankData?.BarcodeFormatList;
		}
		set
		{
			BankData.BarcodeFormatList = value;
		}
	}

	public double PosMultiplier
	{
		get
		{
			return BankData?.PosMultiplier ?? 0.0;
		}
		set
		{
			BankData.PosMultiplier = value;
		}
	}

	public TafsilBusiness Tafsil { get; private set; }

	[JsonIgnore]
	[XmlIgnore]
	public string FullName
	{
		get
		{
			if (!string.IsNullOrEmpty(Shobe))
			{
				return Name + " (" + Shobe + ")";
			}
			return Name;
		}
	}

	public string ContextGuid
	{
		get
		{
			return Tafsil.ContextGuid;
		}
		set
		{
			if (Tafsil != null)
			{
				Tafsil.ContextGuid = value;
			}
		}
	}

	public EntityChangeSt ChangeStatus => Tafsil.ChangeStatus;

	public BankBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<BankBusiness> GetFactoryAsync(TafsilBusiness tafsil, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (tafsil == null)
		{
			throw new ArgumentNullException("tafsil");
		}
		try
		{
			BankBusiness item = new BankBusiness(tafsil.ContextGuid)
			{
				Tafsil = tafsil
			};
			if (tafsil.Guid != Guid.Empty)
			{
				BankBusiness bankBusiness = item;
				bankBusiness.Account = await GetAccountAsync(tafsil, system, token, tranName);
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 105);
			throw;
		}
	}

	private static async Task<decimal> GetAccountAsync(TafsilBusiness tafsil, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			if (tafsil == null)
			{
				return default(decimal);
			}
			List<Guid> list = await CashierBusiness.GetAllTafsilGroupGuidsForTurnoverAsync(tafsil.ContextGuid, system, token, tranName);
			if (list == null)
			{
				return await AccountBusiness.GetAccountSumAsync(tafsil.ContextGuid, tafsil.Guid, new List<Guid>(1) { ParentDefaults.MoeinCoding.CLSMoein4 }, tranName, token);
			}
			if (list == null || !list.Any())
			{
				return default(decimal);
			}
			if (!list.Contains(tafsil.TafsilGroupGuid))
			{
				return default(decimal);
			}
			return await AccountBusiness.GetAccountSumAsync(tafsil.ContextGuid, tafsil.Guid, new List<Guid>(1) { ParentDefaults.MoeinCoding.CLSMoein4 }, tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 123);
			throw;
		}
	}

	public static Task<IEnumerable<BankBusiness>> GetAllAsync(string contextGuid, EnSystem system, string searchString = "", CancellationToken token = default(CancellationToken), bool? status = true, string tranName = "")
	{
		return GetAllAsync(contextGuid, system, searchString, token, status, tranName, new List<Guid> { ParentDefaults.MoeinCoding.CLSMoein4 });
	}

	public static async Task<IEnumerable<BankBusiness>> GetAllAsync(string contextGuid, EnSystem system, string searchString, CancellationToken token, bool? status, string tranName, List<Guid> lstMoeinGuid)
	{
		try
		{
			TafsilFilter filter = new TafsilFilter
			{
				Status = status,
				NameSearchString = searchString,
				HesabType = PacketParser.Entities.HesabType.Bank,
				MoeinGuidList = lstMoeinGuid
			};
			IEnumerable<TafsilBusiness> enumerable = await TafsilBusiness.GetAllAsync(contextGuid, system, filter, token, tranName, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs");
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return await Task.WhenAll(enumerable.Select(async (TafsilBusiness p) => await GetFactoryAsync(p, system, token, tranName)));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 147);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 159), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 159);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 161);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 165);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 166);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 171), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 171);
			}
		}
		return ret;
	}

	public static async Task<BankBusiness> GetAsync(string contextGuid, Guid guid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (Guid.Empty == guid)
			{
				return null;
			}
			TafsilBusiness tafsilBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetAsync(guid, token, tranName);
			if (tafsilBusiness == null)
			{
				return null;
			}
			return await GetFactoryAsync(tafsilBusiness, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 188);
			throw;
		}
	}

	public static async Task<BankBusiness> GetAsync(string contextGuid, long code, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			TafsilBusiness tafsilBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetAsync(code, token, tranName);
			if (tafsilBusiness == null)
			{
				return null;
			}
			return await GetFactoryAsync(tafsilBusiness, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 202);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool awaitOnSemaphore, bool updateAccount, short tryCount = 10, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BankBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 215);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 216), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 216);
						}
						transactionHasBeenCreated = true;
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							Tafsil.SerializedData = Json.ToStringJson(BankData);
							Tafsil.HType = PacketParser.Entities.HesabType.Bank;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName, 0), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 221);
							ret.throwExceptionIfError();
							Tafsil.Modified = DateTime.Now;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await Tafsil.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, token, updateAccount, awaitOnSemaphore: false, 0, send2Server: true, await4sent: false, 10, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 224);
							if (!ret.HasError)
							{
								goto end_IL_0121;
							}
							result = ret;
						}
					}
					goto end_IL_007d;
					end_IL_0121:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 229);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 230);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 235), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 235);
			}
		}
		return ret;
	}

	public static async Task<decimal> TotalAccountAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await AccountBusiness.GetAllAsync(contextGuid, ParentDefaults.MoeinCoding.CLSMoein4, null, true, token))?.Sum((AccountBusiness q) => q.Account) ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TotalAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 248);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName, short autoFixValidationErrorsCount)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, ParentDefaults.MoeinCoding.CLSMoein4, Guid, true, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Tafsil.CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, tranName, autoFixValidationErrorsCount, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 258);
			if (system == EnSystem.UserInterface && account > 0m)
			{
				ret.AddReturnedValue(NegativeAmount, "موجودی حساب".GetTranslate() + " " + (Name ?? "").GetTranslate() + $"({Math.Abs(account):N0})" + " " + " بستانکار(منفی) می باشد".GetTranslate() + "\r\n" + "در چنین شرایطی امکان غیرفعال کردن موجودی منفی وجود ندارد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 260);
			}
			if (string.IsNullOrEmpty(Name))
			{
				ret.AddError(system, "نام بانک وارد نشده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 263);
			}
			if (ChangeStatus == EntityChangeSt.New && !string.IsNullOrEmpty(Shobe) && (((await GetAllAsync(ContextGuid, system, Name, token, true))?.Where((BankBusiness p) => p.Shobe == Shobe))?.Any() ?? false))
			{
				ret.AddError(system, "بانک با نام".GetTranslate() + " " + (FullName ?? "").GetTranslate() + "تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 267);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 272);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 273);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveFromTafsil(string contextGuid, EnSystem system, CancellationToken token, List<TafsilBusiness> lstHesabsListTafsil, bool awaitOnSemaphore, bool updateAccount, short tryCount = 10, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BankBusiness/SaveFromTafsil", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 286);
					}
					else
					{
						if (ClsCache.GetCache(contextGuid)?.DatabaseManager != null && !(ClsCache.GetCache(contextGuid)?.IsDisposed ?? true))
						{
							if (autoTran)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 288), ReturnedState.None, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 288);
							}
							transactionHasBeenCreated = true;
							foreach (TafsilBusiness item in lstHesabsListTafsil)
							{
								if (item != null && item.HType == PacketParser.Entities.HesabType.Bank)
								{
									BankBusiness obj = await GetFactoryAsync(item, system, token, tranName);
									ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, token, awaitOnSemaphore: false, updateAccount, tryCount, tranName), ReturnedState.None, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 294);
								}
							}
							goto end_IL_0114;
						}
						result = ret.AddError(system, "cache is disposed!", "", "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 287);
					}
					goto end_IL_0076;
					end_IL_0114:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 299);
				ret.AddReturnedValue(ex, "", "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 300);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 305), ReturnedState.None, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 305);
			}
		}
		return ret;
	}

	public static async Task<BankBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? guid = null, Guid? tafsilGroupGuid = null, string description = "")
	{
		try
		{
			TafsilBusiness tafsilBusiness = new TafsilBusiness(contextGuid)
			{
				DateM = DateTime.Now,
				Modified = DateTime.Now,
				ChangeStatus = EntityChangeSt.New,
				System = false
			};
			if ((tafsilGroupGuid ?? Guid.Empty) != Guid.Empty)
			{
				tafsilBusiness.TafsilGroupGuid = tafsilGroupGuid.Value;
			}
			if (!string.IsNullOrEmpty(description))
			{
				tafsilBusiness.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				tafsilBusiness.Guid = guid.Value;
			}
			else
			{
				tafsilBusiness.Guid = Guid.NewGuid();
			}
			return await GetFactoryAsync(tafsilBusiness, system);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 328);
			throw;
		}
	}

	public static EnBank DetecetBank(string number, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (string.IsNullOrEmpty(number) || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			EnBank enBank = DetecetBankByCardNumber(number, token);
			if (enBank == EnBank.None)
			{
				enBank = DetecetBankByShabaNumber(number, token);
			}
			return enBank;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DetecetBank", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 343);
			throw;
		}
	}

	private static EnBank DetecetBankByCardNumber(string cardNumber, CancellationToken token)
	{
		try
		{
			if (string.IsNullOrEmpty(cardNumber) || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			if (cardNumber.Length < 6 || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			string value = cardNumber.Substring(0, 6);
			if (string.IsNullOrEmpty(value) || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			long num = value.ParseToLong(0L);
			if (num <= 0 || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			EnBank result;
			switch (num)
			{
			case 606373L:
				result = EnBank.MehrIran;
				break;
			case 603799L:
				result = EnBank.Melli;
				break;
			case 603770L:
			case 639217L:
				result = EnBank.Keshavarzi;
				break;
			case 603769L:
				result = EnBank.Saderat;
				break;
			case 627353L:
				result = EnBank.Tejarat;
				break;
			case 627961L:
				result = EnBank.SanatMadan;
				break;
			case 610433L:
			case 991975L:
				result = EnBank.Mellat;
				break;
			case 589463L:
				result = EnBank.Refah;
				break;
			case 628023L:
				result = EnBank.Maskan;
				break;
			case 589210L:
				result = EnBank.Sepah;
				break;
			case 207177L:
			case 627648L:
				result = EnBank.ToseeSaderat;
				break;
			case 627760L:
				result = EnBank.PostBank;
				break;
			case 621986L:
				result = EnBank.Saman;
				break;
			case 502910L:
			case 627488L:
				result = EnBank.KarAfarin;
				break;
			case 622106L:
			case 627884L:
			case 639194L:
				result = EnBank.Parsian;
				break;
			case 627412L:
				result = EnBank.EghtesadNovin;
				break;
			case 502229L:
			case 639347L:
				result = EnBank.Pasargad;
				break;
			case 639346L:
				result = EnBank.Sina;
				break;
			case 502806L:
				result = EnBank.Shahr;
				break;
			case 639607L:
				result = EnBank.Sarmaye;
				break;
			case 502908L:
				result = EnBank.ToseeTaavon;
				break;
			case 636214L:
				result = EnBank.Tat;
				break;
			case 627381L:
				result = EnBank.Ansar;
				break;
			case 505785L:
				result = EnBank.IranZamin;
				break;
			case 636949L:
				result = EnBank.Hekmat;
				break;
			case 502938L:
				result = EnBank.Dey;
				break;
			case 639599L:
				result = EnBank.Ghavamin;
				break;
			case 505416L:
				result = EnBank.Gardeshgari;
				break;
			case 636795L:
				result = EnBank.Markazi;
				break;
			case 639370L:
				result = EnBank.MehrEghtesad;
				break;
			case 628157L:
				result = EnBank.EtebariTosee;
				break;
			case 505801L:
				result = EnBank.EtebariKosar;
				break;
			case 606256L:
				result = EnBank.EtebariAskariye;
				break;
			case 504172L:
				result = EnBank.Resalat;
				break;
			default:
				result = EnBank.None;
				break;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DetecetBankByCardNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 398);
			throw;
		}
	}

	public static EnBank DetecetBankByShabaNumber(string shabaNumber, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (string.IsNullOrEmpty(shabaNumber) || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			if (shabaNumber.Length < 7 || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			shabaNumber = shabaNumber.Replace(" ", "").FixString().Trim();
			string value = shabaNumber.Substring(4, 3);
			if (string.IsNullOrEmpty(value) || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			long num = value.ParseToLong(0L);
			if (num <= 0 || token.IsCancellationRequested)
			{
				return EnBank.None;
			}
			long num2 = num - 10;
			if ((ulong)num2 <= 12uL)
			{
				switch (num2)
				{
				case 0L:
					goto IL_0140;
				case 1L:
					goto IL_0148;
				case 2L:
					goto IL_0150;
				case 3L:
					goto IL_0158;
				case 4L:
					goto IL_015d;
				case 5L:
					goto IL_0162;
				case 6L:
					goto IL_0167;
				case 7L:
					goto IL_016c;
				case 8L:
					goto IL_0171;
				case 9L:
					goto IL_0175;
				case 10L:
					goto IL_017a;
				case 11L:
					goto IL_017f;
				case 12L:
					goto IL_0184;
				}
			}
			long num3 = num - 51;
			if ((ulong)num3 > 19uL)
			{
				goto IL_01d8;
			}
			switch (num3)
			{
			case 0L:
				break;
			case 2L:
				goto IL_018d;
			case 3L:
				goto IL_0192;
			case 4L:
				goto IL_0196;
			case 5L:
				goto IL_019a;
			case 6L:
				goto IL_019f;
			case 7L:
				goto IL_01a3;
			case 8L:
				goto IL_01a8;
			case 9L:
				goto IL_01ad;
			case 10L:
				goto IL_01b2;
			case 11L:
				goto IL_01b7;
			case 12L:
				goto IL_01bc;
			case 13L:
				goto IL_01c0;
			case 14L:
				goto IL_01c5;
			case 15L:
				goto IL_01ca;
			case 16L:
				goto IL_01cf;
			case 19L:
				goto IL_01d3;
			default:
				goto IL_01d8;
			}
			EnBank result = EnBank.EtebariTosee;
			goto IL_01da;
			IL_01ad:
			result = EnBank.MehrIran;
			goto IL_01da;
			IL_01b2:
			result = EnBank.Shahr;
			goto IL_01da;
			IL_01bc:
			result = EnBank.Ansar;
			goto IL_01da;
			IL_01c0:
			result = EnBank.Gardeshgari;
			goto IL_01da;
			IL_01ca:
			result = EnBank.Dey;
			goto IL_01da;
			IL_01cf:
			result = EnBank.IranZamin;
			goto IL_01da;
			IL_01d8:
			result = EnBank.None;
			goto IL_01da;
			IL_01da:
			return result;
			IL_01c5:
			result = EnBank.Hekmat;
			goto IL_01da;
			IL_0140:
			result = EnBank.Markazi;
			goto IL_01da;
			IL_0148:
			result = EnBank.SanatMadan;
			goto IL_01da;
			IL_0150:
			result = EnBank.Mellat;
			goto IL_01da;
			IL_0158:
			result = EnBank.Refah;
			goto IL_01da;
			IL_015d:
			result = EnBank.Maskan;
			goto IL_01da;
			IL_0162:
			result = EnBank.Sepah;
			goto IL_01da;
			IL_0167:
			result = EnBank.Keshavarzi;
			goto IL_01da;
			IL_016c:
			result = EnBank.Melli;
			goto IL_01da;
			IL_0171:
			result = EnBank.Tejarat;
			goto IL_01da;
			IL_0175:
			result = EnBank.Saderat;
			goto IL_01da;
			IL_017a:
			result = EnBank.ToseeSaderat;
			goto IL_01da;
			IL_017f:
			result = EnBank.PostBank;
			goto IL_01da;
			IL_0184:
			result = EnBank.ToseeTaavon;
			goto IL_01da;
			IL_01d3:
			result = EnBank.Resalat;
			goto IL_01da;
			IL_01b7:
			result = EnBank.Ayande;
			goto IL_01da;
			IL_01a8:
			result = EnBank.Sina;
			goto IL_01da;
			IL_018d:
			result = EnBank.KarAfarin;
			goto IL_01da;
			IL_0192:
			result = EnBank.Parsian;
			goto IL_01da;
			IL_0196:
			result = EnBank.EghtesadNovin;
			goto IL_01da;
			IL_019a:
			result = EnBank.Saman;
			goto IL_01da;
			IL_019f:
			result = EnBank.Pasargad;
			goto IL_01da;
			IL_01a3:
			result = EnBank.Sarmaye;
			goto IL_01da;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DetecetBankByShabaNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BankBusiness.cs", 450);
			throw;
		}
	}
}
