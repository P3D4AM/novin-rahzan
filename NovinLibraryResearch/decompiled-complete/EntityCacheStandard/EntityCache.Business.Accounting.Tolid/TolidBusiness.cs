using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Accounting.Tolid.Base;
using EntityCache.Entities;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidBusiness : BaseTolidBusiness, IChangeIntegrity, IHasSanadNo, IEntityCache, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	private SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private int _id2;

	private TolidBusiness _oldPage;

	private OrderBusiness _listExport;

	private OrderBusiness _listMavad;

	private List<TolidHazineBusiness> _listHazine;

	private string _cashierName;

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; } = SanadSt.Movaghat;


	public Guid CashierGuid { get; set; }

	public string Description { get; set; } = "";


	public Guid MavadMoeinGuid { get; set; }

	public Guid? MavadTafsilGuid { get; set; }

	public Guid MahsolMoeinGuid { get; set; }

	public Guid? MahsolTafsilGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public decimal ExportCount => (_listExport?.ListKala?.Count).GetValueOrDefault();

	public Guid? ExportFormulaGuid => _listExport?.ListKala?.FirstOrDefault()?.TolidFormulaGuid;

	public bool CheckMovazene { get; set; } = true;


	public bool SanadAffective { get; set; }

	public bool KardexAffective { get; set; }

	public override IReadOnlyList<IBaseHazineBusiness> HazineList
	{
		get
		{
			try
			{
				if (_listHazine == null)
				{
					_listHazine = new List<TolidHazineBusiness>();
				}
				_listHazine.RemoveAll((TolidHazineBusiness p) => p?.IsDisposed ?? true);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "HazineList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 59);
			}
			return _listHazine;
		}
	}

	private TolidBusiness(string contextGuid)
		: base(contextGuid)
	{
		ChangeStatus = EntityChangeSt.New;
	}

	public static async Task<TolidBusiness> GenerateNewAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			TolidBusiness tolid = new TolidBusiness(contextGuid)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			};
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			if (system != 0)
			{
				tolid.MavadMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadMoeinGuid;
				tolid.MavadTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadTafsilGuid;
				tolid.MahsolMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportMoeinGuid;
				tolid.MahsolTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportTafsilGuid;
			}
			tolid.SanadAffective = accGlobalSettings.Tolid.SanadAffective;
			tolid.KardexAffective = accGlobalSettings.Tolid.KardexAffective;
			return tolid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 88);
			throw;
		}
	}

	public async Task<int> GetId2Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (_id2 <= 0)
		{
			_id2 = await NextId2Async(base.ContextGuid, token, tranName);
		}
		return _id2;
	}

	public void SetId2(int value)
	{
		_id2 = value;
	}

	public override async Task<OrderBusiness> GetMavadListAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (_listMavad != null)
			{
				return _listMavad;
			}
			_listMavad = await OrderBusiness.GenerateNewWithDefaultsAsync(base.ContextGuid, system, Document.Tolid_Mavad, ENSource.Accounting, null, "", null, tranName, token);
			if (_listMavad == null)
			{
				return _listMavad;
			}
			if (_listMavad.GetDate() != base.DateM)
			{
				await _listMavad.SetDateAsync(base.DateM, system, token, tranName);
			}
			return _listMavad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMavadListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 110);
			throw;
		}
	}

	public override async Task<OrderBusiness> GetExportListAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (_listExport != null)
			{
				return _listExport;
			}
			_listExport = await OrderBusiness.GenerateNewWithDefaultsAsync(base.ContextGuid, system, Document.Tolid_Mahsul, ENSource.Accounting, null, "", null, tranName, token);
			if (_listExport == null)
			{
				return _listExport;
			}
			if (_listExport.GetDate() != base.DateM)
			{
				await _listExport.SetDateAsync(base.DateM, system, token, tranName);
			}
			return _listExport;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetExportListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 126);
			throw;
		}
	}

	public static async Task<TolidBusiness> GenerateToliBusiness(string contextGuid, EnSystem system, TolidFormulaBusiness formula, decimal quantity, bool checkMovazene, CancellationToken token, string tranName, Guid? guid = null, string desc = "", DateTime? dateM = null, int id2 = 0, int sanadNo = 0)
	{
		if (formula == null)
		{
			throw new NullReferenceException("formula");
		}
		try
		{
			TolidBusiness ret = new TolidBusiness(contextGuid);
			if (dateM.HasValue)
			{
				ret.DateM = dateM.Value;
			}
			if (id2 > 0)
			{
				ret.SetId2(id2);
			}
			if (sanadNo > 0)
			{
				ret.SanadNo = sanadNo;
			}
			decimal qt = quantity;
			IReadOnlyList<OrderDetailBusiness> exportList = (await formula.GetExportListAsync(system, tranName, token))?.ListKala;
			decimal valueOrDefault = (exportList?.FirstOrDefault()?.Quantity).GetValueOrDefault();
			if (valueOrDefault > 0m)
			{
				qt = quantity / valueOrDefault;
			}
			OrderBusiness mavad = await formula.GetMavadListAsync(system, tranName, token);
			if ((mavad?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness item2 in mavad.ListKala.ToList())
				{
					await item2.SetQuantityAsync(system, item2.Quantity * qt, token);
					item2.TolidFormulaGuid = formula.Guid;
				}
				await ret.AddMavadListAsync(system, mavad.ListKala, clearDets: false, "", token);
			}
			if (formula.HazineList?.Any() ?? false)
			{
				foreach (IBaseHazineBusiness hazine in formula.HazineList)
				{
					hazine.Price *= qt;
				}
				await ret.AddHazineListAsync(await TolidHazineMapper.Instance.MapListAsync(contextGuid, formula.HazineList as IEnumerable<TolidFormulaHazineBusiness>, null, tranName, token), system);
			}
			if (exportList != null)
			{
				foreach (OrderDetailBusiness item2 in exportList)
				{
					decimal newQt = item2.Quantity * qt;
					await item2.SetQuantityAsync(system, newQt, token);
					decimal num = await formula.GetMavadSumAsync(system, token, tranName);
					if (!(num <= 0m) || !(formula.HazineSum <= 0m))
					{
						decimal value = decimal.Ceiling((num + formula.HazineSum) / newQt);
						await item2.SetPriceAsync(system, value, token);
						item2.TolidFormulaGuid = formula.Guid;
					}
				}
				await ret.AddExportListAsync(system, exportList, clearDets: false, "", token);
			}
			if (guid.HasValue && guid != Guid.Empty)
			{
				ret.Guid = guid.Value;
			}
			if (ret.CashierGuid == Guid.Empty)
			{
				ret.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			if (ret.MahsolMoeinGuid == Guid.Empty)
			{
				ret.MahsolMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportMoeinGuid;
				ret.MahsolTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportTafsilGuid;
			}
			if (ret.MavadMoeinGuid == Guid.Empty)
			{
				ret.MavadMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadMoeinGuid;
				ret.MavadTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadTafsilGuid;
			}
			if (string.IsNullOrEmpty(ret.Description))
			{
				ret.Description = desc;
			}
			ret.CheckMovazene = checkMovazene;
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateToliBusiness", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 199);
			throw;
		}
	}

	public async Task MergeAsync(EnSystem system, TolidFormulaBusiness formula, decimal quantity, CancellationToken token, string tranName)
	{
		if (formula == null)
		{
			throw new NullReferenceException("formula");
		}
		try
		{
			decimal qt = quantity;
			IReadOnlyList<OrderDetailBusiness> exportList = (await formula.GetExportListAsync(system, tranName, token))?.ListKala;
			decimal valueOrDefault = (exportList?.FirstOrDefault()?.Quantity).GetValueOrDefault();
			if (valueOrDefault > 0m)
			{
				qt = quantity / valueOrDefault;
			}
			OrderBusiness mavad = await formula.GetMavadListAsync(system, tranName, token);
			if ((mavad?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness item2 in mavad.ListKala.ToList())
				{
					await item2.SetQuantityAsync(system, item2.Quantity * qt, token);
					item2.TolidFormulaGuid = formula.Guid;
				}
				await AddMavadListAsync(system, mavad.ListKala, clearDets: false, "", token);
			}
			if (formula.HazineList?.Any() ?? false)
			{
				foreach (IBaseHazineBusiness hazine in formula.HazineList)
				{
					hazine.Price *= qt;
				}
				await AddHazineListAsync(await TolidHazineMapper.Instance.MapListAsync(base.ContextGuid, formula.HazineList as IEnumerable<TolidFormulaHazineBusiness>, null, tranName, token), system);
			}
			if (exportList == null)
			{
				return;
			}
			foreach (OrderDetailBusiness item2 in exportList)
			{
				decimal newQt = item2.Quantity * qt;
				await item2.SetQuantityAsync(system, newQt, token);
				decimal num = await formula.GetMavadSumAsync(system, token, tranName);
				if (!(num <= 0m) || !(formula.HazineSum <= 0m))
				{
					decimal value = decimal.Ceiling((num + formula.HazineSum) / newQt);
					await item2.SetPriceAsync(system, value, token);
					item2.TolidFormulaGuid = formula.Guid;
				}
			}
			await AddExportListAsync(system, exportList, clearDets: false, "", token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MergeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 247);
			throw;
		}
	}

	public static Task<int> NextId2Async(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Tolid.NextId2Async(token, tranName);
	}

	public static async Task<IEnumerable<TolidReportBusiness>> GetAllReportAsync(string contextGuid, EnSystem system, TolidFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<TolidReportBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.Tolid.GetAllReportAsync(filter, token, tranName);
			if (system != EnSystem.UserInterface)
			{
				return enumerable;
			}
			return await ApplyAccessAsync(contextGuid, enumerable, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 262);
			throw;
		}
	}

	private static async Task<IEnumerable<TolidReportBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<TolidReportBusiness> list, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			AccessLevel obj = (await ((await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName)) ?? throw new ArgumentException("cashier")).GetUserAccessAsync(token, tranName)) ?? throw new ArgumentException("access");
			if (!obj.Tolid.KardexAffective)
			{
				list = list?.Where((TolidReportBusiness q) => !q.KardexAffective);
			}
			if (!obj.Tolid.SanadAffective)
			{
				list = list?.Where((TolidReportBusiness q) => !q.SanadAffective);
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 283);
			throw;
		}
	}

	public static Task<TolidBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Tolid.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 9;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 297), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 297);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: true, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 299);
				if (ret.HasError)
				{
					result = ret;
					goto IL_099e;
				}
				await ClearListDetAsync(system, token, tranName);
				if (KardexAffective)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(base.ContextGuid, system, isDeleteAction: true, this, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 303);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_099e;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.Tolid.RemoveAsync(base.Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 305);
				if (ret.HasError)
				{
					result = ret;
					goto IL_099e;
				}
				if (system != EnSystem.Transfer)
				{
					SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(base.ContextGuid, SanadNo, token, tranName);
					if (sanadBusiness != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, tryCount, tranName, token, new Tuple<Document, Guid>(Document.Tolid, base.Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 311);
					}
					if (ret.HasError)
					{
						result = ret;
						goto IL_099e;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 313), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 313);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(base.ContextGuid, system, base.Guid, Document.Tolid, base.DateM, 10, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 314);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_099e;
				}
				if (ClsCache.GetCache(base.ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(base.Guid, Document.Tolid, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 318);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 322);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 323);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 328), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 328);
			}
		}
		return ret;
		IL_099e:
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", SanadBusiness orderSanad = null)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 14;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TolidBusiness/SaveAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 341);
					}
					else
					{
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 342), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 342);
						}
						transactionHasBeenCreated = true;
						res = await CheckValidationAsync(system, isDeleteAction: false, token, tranName);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token);
							if (MahsolMoeinGuid == Guid.Empty)
							{
								MahsolMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportMoeinGuid;
							}
							if ((MahsolTafsilGuid ?? Guid.Empty) == Guid.Empty)
							{
								MahsolTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportTafsilGuid;
							}
							if (MavadMoeinGuid == Guid.Empty)
							{
								MavadMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadMoeinGuid;
							}
							if ((MavadTafsilGuid ?? Guid.Empty) == Guid.Empty)
							{
								MavadTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadTafsilGuid;
							}
							if (CashierGuid == Guid.Empty)
							{
								CashierGuid = CashierBusiness.GetCurrentCashierGuid(base.ContextGuid);
							}
							if (system == EnSystem.Transfer || !KardexAffective)
							{
								goto IL_0555;
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(base.ContextGuid, system, isDeleteAction: false, this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 357);
							if (!res.HasError)
							{
								goto IL_0555;
							}
							result = res;
						}
					}
					goto end_IL_0081;
					IL_0555:
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.Tolid.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 360);
					if (!res.HasError)
					{
						if (system != EnSystem.Transfer)
						{
							SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
							if (SanadAffective)
							{
								if (orderSanad == null)
								{
									if (SanadNo <= 0 && ClsCache.GetCache(base.ContextGuid).Source == ENSource.Accounting)
									{
										SanadNo = await SanadBusiness.NextIdAsync(base.ContextGuid, token, tranName);
										returnedSaveFuncInfo = res;
										returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.Tolid.SetSanadNoAsync(base.Guid, SanadNo, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 372);
									}
									sanad.SanadNo = SanadNo;
									List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.Tolid, base.Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 377);
								}
								else
								{
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await orderSanad.JoinAsync(sanad, system, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 379);
								}
							}
							else if (ChangeStatus != EntityChangeSt.New)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await sanad.RemoveAsync(system, 1, tranName, token, new Tuple<Document, Guid>(Document.Tolid, base.Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 382);
							}
							if (system != EnSystem.Transfer && !res.HasError)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, res.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 386), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 386);
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(base.ContextGuid, system, base.Guid, Document.Tolid, base.DateM, 10, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 387);
							}
						}
						goto end_IL_0152;
					}
					result = res;
					goto end_IL_0081;
					end_IL_0152:;
				}
				goto end_IL_006e;
				end_IL_0081:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 393);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 394);
				goto end_IL_006e;
			}
			return result;
			end_IL_006e:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 399), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 399);
			}
		}
		return res;
	}

	public override async Task AddExportListAsync(EnSystem system, IEnumerable<OrderDetailBusiness> lstObjects, bool clearDets = false, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (lstObjects?.Any() ?? false)
			{
				await (await GetExportListAsync(system, tranName, token)).AddDetListAsync(new List<OrderDetailBusiness>(lstObjects), clearDets, system, token);
				if (system != EnSystem.Mapper && system != EnSystem.Transfer)
				{
					RaiseEventAsync(token);
				}
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddExportListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 414);
			throw;
		}
	}

	public override async Task AddMavadListAsync(EnSystem system, IEnumerable<OrderDetailBusiness> lstObjects, bool clearDets = false, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (lstObjects?.Any() ?? false)
			{
				await (await GetMavadListAsync(system, tranName, token)).AddDetListAsync(new List<OrderDetailBusiness>(lstObjects), clearDets, system, token);
				if (system != EnSystem.Mapper && system != EnSystem.Transfer)
				{
					RaiseEventAsync(token);
				}
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddMavadListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 429);
			throw;
		}
	}

	public override Task AddHazineListAsync(IEnumerable<IBaseHazineBusiness> lstObjects, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (lstObjects == null || !lstObjects.Any())
			{
				return Task.CompletedTask;
			}
			List<Task> list = new List<Task>();
			foreach (IBaseHazineBusiness obj in lstObjects?.ToList())
			{
				if (obj != null)
				{
					obj.MasterGuid = base.Guid;
					obj.Price = Math.Round(obj.Price, base.DigitAfterPoint);
					if (_listHazine == null)
					{
						_listHazine = new List<TolidHazineBusiness>();
					}
					TolidHazineBusiness tolidHazineBusiness = _listHazine.FirstOrDefault((TolidHazineBusiness q) => q.Guid == obj.Guid);
					if (tolidHazineBusiness != null)
					{
						_listHazine.Remove(tolidHazineBusiness);
					}
					_listHazine.Add((TolidHazineBusiness)obj);
					list.Add(RaiseEventAsync(token));
				}
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "AddHazineListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 452);
		}
		return Task.CompletedTask;
	}

	public override async Task RemoveFromListMavadAsync(EnSystem system, Guid guid, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveFromListMavadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (_listMavad == null)
				{
					_listMavad = await OrderBusiness.GenerateNewWithDefaultsAsync(base.ContextGuid, system, Document.Tolid_Mavad, ENSource.Accounting, null, "", null, "", token);
				}
				OrderDetailBusiness orderDetailBusiness = _listMavad.ListKala.FirstOrDefault((OrderDetailBusiness p) => p.Guid == guid);
				if (orderDetailBusiness != null)
				{
					orderDetailBusiness.Dispose(disposing: true);
					await RaiseEventAsync(token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListMavadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 466);
			}
		}
	}

	public override async Task RemoveFromListHazineAsync(Guid guid, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveFromListHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs"))
		{
			try
			{
				if (_listHazine == null)
				{
					_listHazine = new List<TolidHazineBusiness>();
				}
				TolidHazineBusiness tolidHazineBusiness = _listHazine.FirstOrDefault((TolidHazineBusiness p) => p.Guid == guid);
				if (tolidHazineBusiness != null)
				{
					_listHazine.Remove(tolidHazineBusiness);
					await RaiseEventAsync(token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 481);
				throw;
			}
		}
	}

	private async Task ClearListDetAsync(EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ClearListDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs"))
		{
			_ = 2;
			try
			{
				foreach (OrderDetailBusiness item in (await GetMavadListAsync(system, tranName, token))?.ListKala?.ToList() ?? new List<OrderDetailBusiness>())
				{
					item.Dispose();
				}
				foreach (OrderDetailBusiness item2 in (await GetExportListAsync(system, tranName, token))?.ListKala?.ToList() ?? new List<OrderDetailBusiness>())
				{
					item2.Dispose();
				}
				foreach (IBaseHazineBusiness item3 in HazineList?.ToList())
				{
					await RemoveFromListHazineAsync(item3.Guid, token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ClearListDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 501);
				throw;
			}
		}
	}

	public async Task ClearListDetByFormulaAsync(TolidFormulaBusiness formul, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (formul == null)
			{
				return;
			}
			List<OrderDetailBusiness> list = (await GetMavadListAsync(system, tranName, token))?.ListKala?.Where((OrderDetailBusiness q) => (q.TolidFormulaGuid ?? Guid.Empty) == formul.Guid)?.ToList();
			if (list != null && list.Any())
			{
				foreach (OrderDetailBusiness item in list)
				{
					item.Dispose();
				}
			}
			List<OrderDetailBusiness> list2 = (await GetExportListAsync(system, tranName, token))?.ListKala?.Where((OrderDetailBusiness q) => (q.TolidFormulaGuid ?? Guid.Empty) == formul.Guid)?.ToList();
			if (list2 != null && list2.Any())
			{
				foreach (OrderDetailBusiness item2 in list2)
				{
					item2.Dispose();
				}
			}
			foreach (TolidHazineBusiness hazine in HazineList)
			{
				if ((hazine?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid)
				{
					hazine.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ClearListDetByFormulaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 527);
			throw;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (system == EnSystem.Transfer)
			{
				return res;
			}
			decimal exportListSum = await GetExportSumAsync(system, token, tranName);
			decimal sumMavad = await GetMavadSumAsync(system, token, tranName);
			decimal sumExport = await GetExportSumAsync(system, token, tranName);
			if (base.HazineSum != 0m || sumMavad != 0m || sumExport != 0m)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await base.CheckValidationAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 541);
			}
			if (MahsolMoeinGuid == Guid.Empty)
			{
				res.AddError("طرف حساب معین انتخاب شده برای محصول نهایی معتبر نمی باشد و یا شما دسترسی به حساب ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 544);
			}
			if (MavadMoeinGuid == Guid.Empty)
			{
				res.AddError("طرف حساب معین انتخاب شده برای مواد مصرفی معتبر نمی باشد و یا شما دسترسی به حساب ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 546);
			}
			if ((base.HazineSum != 0m || sumMavad != 0m || sumExport != 0m) && CheckMovazene && !isDeleteAction)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await (await GetMavadListAsync(system, tranName, token)).CheckValidationAsync(system, isDeleteAction, tranName, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 550);
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await (await GetExportListAsync(system, tranName, token)).CheckValidationAsync(system, isDeleteAction, tranName, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 552);
				if (res.HasError)
				{
					return res;
				}
			}
			if (CheckMovazene && SanadAffective && KardexAffective && !isDeleteAction)
			{
				decimal num = exportListSum;
				if (num - await GetMavadSumAsync(system, token, tranName) - base.HazineSum != 0m && system != EnSystem.Transfer)
				{
					res.AddError(system, "جمع مواد اولیه + هزینه های تولید با کالای ساخته شده یکسان نمیباشد.".GetTranslate() + "\r\n" + "لطفا فرمول تولید را موازنه و سپس اقدام به ذخیره سازی نمایید.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 560);
					return res;
				}
			}
			SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(base.ContextGuid, SanadNo, token, tranName);
			if (sanadBusiness != null)
			{
				if (sanadBusiness.SanadSt == SanadSt.Daemi)
				{
					res.AddError(system, "این برگه به علت دائمی شدن سند غیر قابل ویرایش می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 568);
				}
				if (sanadBusiness.DateSh != base.DateSh)
				{
					res.AddError(system, " این تولید در یک سند تجمیعی قرار دارد. تاریخ سند(".GetTranslate() + sanadBusiness.DateSh + ") با تاریخ فعلی تولید متفاوت است. لطفا یا تاریخ سند را اصلاح کنید و یا تولید را با تغییر شماره سند از این سند تجمیعی خارج نمایید ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 570);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 575);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 576);
		}
		return res;
	}

	private async Task<TolidBusiness> GetOldPage(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_oldPage = await ClsCache.GetCache(base.ContextGuid).CacheRepository.Tolid.GetFromDbAsync(base.Guid, token, tranName);
			return _oldPage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldPage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 589);
			throw;
		}
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 21;
		try
		{
			Guid oldGuid = Guid.NewGuid();
			SanadBusiness sanad = await SanadBusiness.GetAsync(base.ContextGuid, SanadNo, token, tranName);
			if (sanad != null)
			{
				oldGuid = sanad.Guid;
			}
			if (ChangeStatus == EntityChangeSt.New)
			{
				SanadBusiness sanadBusiness = new SanadBusiness(base.ContextGuid, 0);
				SanadBusiness sanadBusiness2 = sanadBusiness;
				sanadBusiness2.Description = $"تولید ({await GetId2Async(token, tranName)}) {Description}";
				sanadBusiness.UserGuid = CashierGuid;
				sanadBusiness.DateM = base.DateM;
				sanadBusiness.SanadSt = SanadSt.Movaghat;
				sanadBusiness.Guid = oldGuid;
				sanadBusiness.SanadNo = SanadNo;
				sanadBusiness.SanadType = SanadType.Automatic;
				sanad = sanadBusiness;
			}
			else
			{
				TolidBusiness tolidBusiness = await GetOldPage(token, tranName);
				SanadBusiness sanadBusiness2 = new SanadBusiness(base.ContextGuid, tolidBusiness?.SanadNo ?? 0);
				SanadBusiness sanadBusiness = sanadBusiness2;
				sanadBusiness.Description = $"تولید ({await GetId2Async(token, tranName)}) {Description}";
				sanadBusiness2.UserGuid = CashierGuid;
				sanadBusiness2.DateM = base.DateM;
				sanadBusiness2.SanadSt = SanadSt.Movaghat;
				sanadBusiness2.Guid = oldGuid;
				sanadBusiness2.SanadNo = SanadNo;
				sanadBusiness2.SanadType = SanadType.Automatic;
				sanad = sanadBusiness2;
			}
			decimal sumMavad = await GetMavadSumAsync(system, token, tranName);
			decimal num = await GetExportSumAsync(system, token, tranName);
			if (num != 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(base.ContextGuid, system, base.Guid, Document.Tolid, sanad.NextOrdinal())
				{
					Debit = num,
					Credit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = $"تولید({await GetId2Async(token, tranName)} {Description})";
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				sanad_DetBusiness.Modified = DateTime.Now;
				sanad_DetBusiness.Status = true;
				Sanad_DetBusiness debitExport4 = sanad_DetBusiness;
				await debitExport4.SetMoeinGuidAsync(MahsolMoeinGuid, token, tranName);
				await debitExport4.SetTafsilGuidAsync(MahsolTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitExport4, system, token);
			}
			if (HazineList?.Any() ?? false)
			{
				foreach (IBaseHazineBusiness hazine2 in HazineList.Where((IBaseHazineBusiness q) => q.Price < 0m))
				{
					Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(base.ContextGuid, system, base.Guid, Document.Tolid, sanad.NextOrdinal())
					{
						Debit = -hazine2.Price,
						Credit = 0m
					};
					Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
					sanad_DetBusiness.RowDesc = $"تولید({await GetId2Async(token, tranName)} {Description})";
					sanad_DetBusiness2.DecimalGuid = Guid.Empty;
					sanad_DetBusiness2.Guid = Guid.NewGuid();
					sanad_DetBusiness2.Qt = 0m;
					sanad_DetBusiness2.MasterGuid = sanad.Guid;
					sanad_DetBusiness2.Modified = DateTime.Now;
					sanad_DetBusiness2.Status = true;
					Sanad_DetBusiness debitExport4 = sanad_DetBusiness2;
					await debitExport4.SetMoeinGuidAsync(hazine2.MoeinGuid, token, tranName);
					await debitExport4.SetTafsilGuidAsync(hazine2.TafsilGuid, tranName, token);
					await sanad.AddToListSanadAsync(debitExport4, system, token);
				}
			}
			if (sumMavad != 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(base.ContextGuid, system, base.Guid, Document.Tolid, sanad.NextOrdinal())
				{
					Debit = 0m,
					Credit = sumMavad
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = $"تولید({await GetId2Async(token, tranName)} {Description})";
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				sanad_DetBusiness.Modified = DateTime.Now;
				sanad_DetBusiness.Status = true;
				Sanad_DetBusiness debitExport4 = sanad_DetBusiness;
				await debitExport4.SetMoeinGuidAsync(MavadMoeinGuid, token, tranName);
				await debitExport4.SetTafsilGuidAsync(MavadTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitExport4, system, token);
			}
			if (HazineList?.Any() ?? false)
			{
				foreach (IBaseHazineBusiness hazine2 in HazineList.Where((IBaseHazineBusiness q) => q.Price > 0m))
				{
					Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(base.ContextGuid, system, base.Guid, Document.Tolid, sanad.NextOrdinal())
					{
						Debit = 0m,
						Credit = hazine2.Price
					};
					Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
					sanad_DetBusiness.RowDesc = $"تولید({await GetId2Async(token, tranName)} {Description})";
					sanad_DetBusiness2.DecimalGuid = Guid.Empty;
					sanad_DetBusiness2.Guid = Guid.NewGuid();
					sanad_DetBusiness2.Qt = 0m;
					sanad_DetBusiness2.MasterGuid = sanad.Guid;
					sanad_DetBusiness2.Modified = DateTime.Now;
					sanad_DetBusiness2.Status = true;
					Sanad_DetBusiness debitExport4 = sanad_DetBusiness2;
					await debitExport4.SetMoeinGuidAsync(hazine2.MoeinGuid, token, tranName);
					await debitExport4.SetTafsilGuidAsync(hazine2.TafsilGuid, tranName, token);
					await sanad.AddToListSanadAsync(debitExport4, system, token);
				}
			}
			return sanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 719);
			throw;
		}
	}

	public async Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction)
	{
		using (new JaegerMonitor("GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs"))
		{
			List<ProductsChanges> ret = new List<ProductsChanges>();
			try
			{
				TolidBusiness oldTolid = await GetOldPage(token, tranName);
				if ((oldTolid?.KardexAffective ?? false) && !KardexAffective && !isDeleteAction)
				{
					isDeleteAction = true;
				}
				if (!(oldTolid?.KardexAffective ?? false) && KardexAffective && !isDeleteAction)
				{
					oldTolid = null;
				}
				if (oldTolid != null)
				{
					foreach (OrderDetailBusiness item in (await oldTolid.GetExportListAsync(system, tranName))?.ListKala?.ToList() ?? new List<OrderDetailBusiness>())
					{
						await CalcChanges.ComputeTolidChangeAsync(base.ContextGuid, item, ret, isNew: false);
					}
					foreach (OrderDetailBusiness item2 in (await oldTolid.GetMavadListAsync(system, tranName, token))?.ListKala?.ToList() ?? new List<OrderDetailBusiness>())
					{
						await CalcChanges.ComputeTolidChangeAsync(base.ContextGuid, item2, ret, isNew: true);
					}
				}
				foreach (OrderDetailBusiness item3 in (await GetMavadListAsync(system, tranName, token))?.ListKala?.ToList() ?? new List<OrderDetailBusiness>())
				{
					await CalcChanges.ComputeTolidChangeAsync(base.ContextGuid, item3, ret, isNew: false);
				}
				foreach (OrderDetailBusiness item4 in (await GetExportListAsync(system, tranName, token))?.ListKala?.ToList() ?? new List<OrderDetailBusiness>())
				{
					await CalcChanges.ComputeTolidChangeAsync(base.ContextGuid, item4, ret, isNew: true);
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 753);
				return ret;
			}
		}
	}

	public Task<List<HesabChanges>> GetLstHesabChangeAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public Task<List<MoeinChanges>> GetLstMoeinChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public Task<List<TafsilChanges>> GetLstTafsilChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SetTolidBalanceAsync(decimal amount, EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			decimal sumMavad = await GetMavadSumAsync(system, token, tranName);
			await GetExportSumAsync(system, token, tranName);
			OrderBusiness exportList = await GetExportListAsync(system, tranName, token);
			OrderBusiness mavadList = await GetMavadListAsync(system, tranName, token);
			if ((exportList?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness t in exportList.ListKala)
				{
					if (!(t.Price != 0m) && !(sumMavad + base.HazineSum <= 0m))
					{
						decimal value = Math.Round((sumMavad + base.HazineSum) / amount, base.DigitForRound);
						await t.SetPriceAsync(system, value, token, tranName);
						await t.SetQuantityAsync(system, amount, token, tranName);
					}
				}
			}
			if ((mavadList?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness t in mavadList.ListKala)
				{
					if (!(t.Price != 0m))
					{
						await t.SetPriceAsync(system, ((await KardexBusiness.GetLastAsync(base.ContextGuid, null, t.ProductGuid, null, token))?.RemUnitPrice).GetValueOrDefault(), token, tranName);
						await t.SetQuantityAsync(system, amount, token, tranName);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTolidBalanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 794);
			res.AddReturnedValue(ex, "", "SetTolidBalanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 795);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetTolidAdjustAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			decimal totalImport = default(decimal);
			decimal exportCount = default(decimal);
			decimal totalHazine2 = HazineList?.Sum((IBaseHazineBusiness p) => p.Price) ?? 0m;
			List<OrderDetailBusiness> i = new List<OrderDetailBusiness>();
			foreach (OrderDetailBusiness item2 in (await GetMavadListAsync(system, tranName, token)).ListKala)
			{
				if (item2.Price <= 0m)
				{
					OrderDetailBusiness orderDetailBusiness = item2;
					EnSystem system2 = system;
					await orderDetailBusiness.SetPriceAsync(system2, await item2.GetTolidPriceAsync(), token);
				}
				totalImport += Math.Round(item2.Quantity * item2.Price);
				i.Add(item2);
			}
			await AddMavadListAsync(system, i, clearDets: true, "", token);
			foreach (OrderDetailBusiness item3 in (await GetExportListAsync(system, tranName, token))?.ListKala ?? new List<OrderDetailBusiness>())
			{
				exportCount += Math.Round(item3.Quantity, 3);
			}
			totalImport += totalHazine2;
			List<OrderDetailBusiness> ex = new List<OrderDetailBusiness>();
			foreach (OrderDetailBusiness item2 in (await GetExportListAsync(system, tranName, token))?.ListKala ?? new List<OrderDetailBusiness>())
			{
				await item2.SetPriceAsync(system, Math.Round(1m / exportCount * totalImport), token);
				ex.Add(item2);
			}
			await AddExportListAsync(EnSystem.UserInterface, ex, clearDets: true, "", token);
			decimal sumMavad = await GetMavadSumAsync(system, token, tranName);
			decimal num = await GetExportSumAsync(system, token, tranName);
			if (base.HazineSum + sumMavad != num)
			{
				totalImport = (await GetMavadListAsync(system, tranName, token))?.SumKalaKhadamat ?? 0m;
				totalHazine2 = HazineList?.Sum((IBaseHazineBusiness q) => q.Price) ?? 0m;
				decimal num2 = (await GetExportListAsync(system, tranName, token))?.SumKalaKhadamat ?? 0m;
				if (num2 != totalHazine2 + totalImport)
				{
					decimal d = num2 - totalHazine2 - totalImport;
					TolidHazineBusiness objects = new TolidHazineBusiness(base.ContextGuid)
					{
						TafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil44,
						Price = Math.Round(d),
						Type = HazineType.Other,
						Description = "موازنه فرمول تولید",
						MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein43
					};
					await AddHazineAsync(objects, system);
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetTolidAdjustAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 860);
			res.AddReturnedValue(ex2, "", "SetTolidAdjustAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 861);
		}
		return res;
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 6;
		try
		{
			if (system == EnSystem.Transfer)
			{
				return null;
			}
			List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
			OrderBusiness orderBusiness = await GetMavadListAsync(system, tranName, token);
			if ((orderBusiness?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness item in orderBusiness.ListKala)
				{
					List<ViewSanadDetailCoverBusiness> list2 = list;
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(base.ContextGuid)
					{
						DebitMoeinGuid = null,
						DebitTafsilGuid = null,
						Date = base.DateM,
						AsGuid = base.Guid,
						AsType = DocumentDet.Tolid,
						AsTypeDocument = Document.Tolid,
						Debit = 0m
					};
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness2.Credit = await GetMavadSumAsync(system, token, tranName);
					viewSanadDetailCoverBusiness.CashierGuid = CashierGuid;
					viewSanadDetailCoverBusiness.SanadNo = SanadNo;
					viewSanadDetailCoverBusiness.SanadSt = SanadSt;
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness3.Id2 = await GetId2Async(token, tranName);
					viewSanadDetailCoverBusiness.CreditMoeinGuid = MavadMoeinGuid;
					viewSanadDetailCoverBusiness.CreditTafsilGuid = MavadTafsilGuid;
					viewSanadDetailCoverBusiness.SanadDesc = Description;
					viewSanadDetailCoverBusiness.RDesc = "";
					viewSanadDetailCoverBusiness.DateMoaser = base.DateM;
					viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness.DepotGuid = null;
					viewSanadDetailCoverBusiness.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness.Price = 0m;
					viewSanadDetailCoverBusiness.ProductGuid = null;
					viewSanadDetailCoverBusiness.Qt = 0m;
					viewSanadDetailCoverBusiness.Qt2 = 0m;
					viewSanadDetailCoverBusiness.RBazaryab = 0m;
					viewSanadDetailCoverBusiness.RDiscount = 0m;
					viewSanadDetailCoverBusiness.Serial = "";
					viewSanadDetailCoverBusiness.Status = true;
					viewSanadDetailCoverBusiness.VahedGuid = null;
					list2.Add(viewSanadDetailCoverBusiness);
				}
			}
			OrderBusiness orderBusiness2 = await GetExportListAsync(system, tranName, token);
			if ((orderBusiness2?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness item in orderBusiness2.ListKala)
				{
					List<ViewSanadDetailCoverBusiness> list2 = list;
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(base.ContextGuid)
					{
						DebitMoeinGuid = MahsolMoeinGuid,
						DebitTafsilGuid = MahsolTafsilGuid,
						Date = base.DateM,
						AsGuid = base.Guid,
						AsType = DocumentDet.Tolid,
						AsTypeDocument = Document.Tolid
					};
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness.Debit = await GetExportSumAsync(system, token, tranName);
					viewSanadDetailCoverBusiness2.Credit = 0m;
					viewSanadDetailCoverBusiness2.CashierGuid = CashierGuid;
					viewSanadDetailCoverBusiness2.SanadNo = SanadNo;
					viewSanadDetailCoverBusiness2.SanadSt = SanadSt;
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness3.Id2 = await GetId2Async(token, tranName);
					viewSanadDetailCoverBusiness2.CreditMoeinGuid = null;
					viewSanadDetailCoverBusiness2.CreditTafsilGuid = null;
					viewSanadDetailCoverBusiness2.SanadDesc = Description;
					viewSanadDetailCoverBusiness2.RDesc = "";
					viewSanadDetailCoverBusiness2.DateMoaser = base.DateM;
					viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness2.DepotGuid = null;
					viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness2.Price = 0m;
					viewSanadDetailCoverBusiness2.ProductGuid = null;
					viewSanadDetailCoverBusiness2.Qt = 0m;
					viewSanadDetailCoverBusiness2.Qt2 = 0m;
					viewSanadDetailCoverBusiness2.RBazaryab = 0m;
					viewSanadDetailCoverBusiness2.RDiscount = 0m;
					viewSanadDetailCoverBusiness2.Serial = "";
					viewSanadDetailCoverBusiness2.Status = true;
					viewSanadDetailCoverBusiness2.VahedGuid = null;
					list2.Add(viewSanadDetailCoverBusiness2);
				}
			}
			if (HazineList?.Any() ?? false)
			{
				foreach (IBaseHazineBusiness item2 in HazineList)
				{
					List<ViewSanadDetailCoverBusiness> list2 = list;
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(base.ContextGuid)
					{
						DebitMoeinGuid = ((item2.Price > 0m) ? null : new Guid?(item2.MoeinGuid)),
						DebitTafsilGuid = ((item2.Price > 0m) ? null : item2.TafsilGuid),
						Date = base.DateM,
						AsGuid = base.Guid,
						AsType = DocumentDet.Tolid,
						AsTypeDocument = Document.Tolid,
						Debit = ((item2.Price < 0m) ? (-item2.Price) : 0m),
						Credit = ((item2.Price > 0m) ? item2.Price : 0m),
						CashierGuid = CashierGuid,
						SanadNo = SanadNo,
						SanadSt = SanadSt
					};
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
					viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
					viewSanadDetailCoverBusiness3.CreditMoeinGuid = ((item2.Price > 0m) ? new Guid?(item2.MoeinGuid) : null);
					viewSanadDetailCoverBusiness3.CreditTafsilGuid = ((item2.Price > 0m) ? item2.TafsilGuid : null);
					viewSanadDetailCoverBusiness3.SanadDesc = Description;
					viewSanadDetailCoverBusiness3.RDesc = "";
					viewSanadDetailCoverBusiness3.DateMoaser = base.DateM;
					viewSanadDetailCoverBusiness3.DetailGuid = item2.Guid;
					viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness3.DecimalGuid = null;
					viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
					viewSanadDetailCoverBusiness3.DecimalQt = 0m;
					viewSanadDetailCoverBusiness3.DepotGuid = null;
					viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness3.Price = item2.Price;
					viewSanadDetailCoverBusiness3.ProductGuid = null;
					viewSanadDetailCoverBusiness3.Qt = 0m;
					viewSanadDetailCoverBusiness3.Qt2 = 0m;
					viewSanadDetailCoverBusiness3.RBazaryab = 0m;
					viewSanadDetailCoverBusiness3.RDiscount = 0m;
					viewSanadDetailCoverBusiness3.Serial = "";
					viewSanadDetailCoverBusiness3.Status = true;
					viewSanadDetailCoverBusiness3.VahedGuid = null;
					list2.Add(viewSanadDetailCoverBusiness3);
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 995);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> BallanceAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			decimal mavadSum2 = await GetMavadSumAsync(system, token);
			decimal exportSum2 = await GetExportSumAsync(system, token);
			if (base.HazineSum + mavadSum2 == exportSum2)
			{
				return res;
			}
			if (((await GetExportListAsync(system, "", token))?.ListKala?.Count).GetValueOrDefault() <= 0)
			{
				res.AddError("هیچ کالایی برای تولید انتخاب نشده است".GetTranslate(), "", "BallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1009);
			}
			if (((await GetMavadListAsync(system, "", token))?.ListKala?.Count).GetValueOrDefault() <= 0)
			{
				res.AddError("هیچ موردی به عنوان مواد مصرفی وجود ندارد".GetTranslate(), "", "BallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1012);
			}
			if (res.HasError)
			{
				return res;
			}
			if ((mavadSum2 + base.HazineSum != exportSum2) | (exportSum2 == 0m))
			{
				await SetPriceAsync(system);
				mavadSum2 = await GetMavadSumAsync(system, token);
				exportSum2 = await GetExportSumAsync(system, token);
				if (base.HazineSum + mavadSum2 != exportSum2)
				{
					await SetPrice2Async(system);
					await GetMavadSumAsync(system, token);
					await GetExportSumAsync(system, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1033);
			res.AddReturnedValue(ex, "", "BallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1034);
		}
		return res;
	}

	private async Task SetPriceAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 7;
		try
		{
			decimal totalImport = default(decimal);
			decimal exportCount = default(decimal);
			decimal totalHazine = HazineList?.Sum((IBaseHazineBusiness p) => p.Price) ?? 0m;
			List<OrderDetailBusiness> i = new List<OrderDetailBusiness>();
			foreach (OrderDetailBusiness item2 in (await GetMavadListAsync(system, "", token)).ListKala)
			{
				if (item2.Price <= 0m)
				{
					OrderDetailBusiness orderDetailBusiness = item2;
					EnSystem system2 = system;
					await orderDetailBusiness.SetPriceAsync(system2, await item2.GetTolidPriceAsync(), token);
				}
				totalImport += Math.Round(item2.Quantity * item2.Price);
				i.Add(item2);
			}
			await AddMavadListAsync(system, i, clearDets: true, "", token);
			foreach (OrderDetailBusiness item3 in (await GetExportListAsync(system, "", token))?.ListKala ?? new List<OrderDetailBusiness>())
			{
				exportCount += Math.Round(item3.Quantity, 3);
			}
			totalImport += totalHazine;
			List<OrderDetailBusiness> ex = new List<OrderDetailBusiness>();
			foreach (OrderDetailBusiness item2 in (await GetExportListAsync(system, "", token))?.ListKala ?? new List<OrderDetailBusiness>())
			{
				await item2.SetPriceAsync(system, Math.Round(1m / exportCount * totalImport), token);
				ex.Add(item2);
			}
			await AddExportListAsync(system, ex, clearDets: true, "", token);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1067);
		}
	}

	private async Task SetPrice2Async(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			decimal totalImport = (await GetMavadListAsync(system, "", token))?.SumKalaKhadamat ?? 0m;
			decimal totalHazine = HazineList?.Sum((IBaseHazineBusiness q) => q.Price) ?? 0m;
			decimal num = (await GetExportListAsync(system, "", token))?.SumKalaKhadamat ?? 0m;
			if (!(num == totalHazine + totalImport))
			{
				decimal d = num - totalHazine - totalImport;
				TolidHazineBusiness objects = new TolidHazineBusiness(base.ContextGuid)
				{
					TafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil44,
					Price = Math.Round(d),
					Type = HazineType.Other,
					Description = "موازنه فرمول تولید",
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein43
				};
				await AddHazineAsync(objects, system);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPrice2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1090);
		}
	}

	public async Task<string> GetUserNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (string.IsNullOrEmpty(_cashierName))
			{
				if (CashierGuid == Guid.Empty)
				{
					CashierGuid = CashierBusiness.GetCurrentCashierGuid(base.ContextGuid);
				}
				_cashierName = (await CashierBusiness.GetAsync(base.ContextGuid, CashierGuid, token, tranName))?.Name ?? "";
			}
			return _cashierName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1106);
			throw;
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return accessLevel.Tolid.Tolid_Delete || (accessLevel.Tolid.Tolid_Delete_Self && CashierGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1120);
			return false;
		}
	}

	public async Task<bool> CanUpdateAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return accessLevel.Tolid.Tolid_Update || (accessLevel.Tolid.Tolid_Update_Self && CashierGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1134);
			return false;
		}
	}

	public static async Task<bool> CanViewAmountsAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await CashierBusiness.GetCurrentCashierAsync(contextGuid, token)).GetUserAccessAsync(token)).Tolid.ShowAmounts;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanViewAmountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidBusiness.cs", 1148);
			return false;
		}
	}
}
