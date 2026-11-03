using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Evaluation;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Filters;

namespace EntityCache.Business.Evaluation;

public class EvaluationBusiness : IEvaluation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IHasChangeStatus, IDeliverableEntity
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private string _cashierName;

	private List<EvaluationDetailModel> _detailModelList;

	private PrdAnbarGardaniBusiness _anbarGardaniEzafe;

	private PrdAnbarGardaniBusiness _anbarGardaniKasr;

	private List<PrdAnbarGardaniDetBusiness> _anbarGardaniDetailList;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public Guid CashierGuid { get; set; }

	public string Description { get; set; }

	public Guid AnbarGardaniEzafeGuid { get; set; }

	public Guid AnbarGardaniKasrGuid { get; set; }

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public IReadOnlyList<EvaluationDetailModel> ListDetail
	{
		get
		{
			_detailModelList = _detailModelList?.Where((EvaluationDetailModel p) => p != null)?.ToList() ?? new List<EvaluationDetailModel>();
			return _detailModelList;
		}
	}

	private EvaluationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public static async Task<IEnumerable<EvaluationModel>> GetAllAsync(string contextGuid, EvaluationFilter filter, CancellationToken token, string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<EvaluationModel> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.Evaluation.GetAllAsync(filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return enumerable;
			}
			return await ApplyAccessAsync(contextGuid, enumerable?.ToList(), token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 71);
			throw;
		}
	}

	private static async Task<IEnumerable<EvaluationModel>> ApplyAccessAsync(string contextGuid, List<EvaluationModel> items, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (!(items?.Any() ?? false))
			{
				return items;
			}
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			if (currentCashier == null)
			{
				return items;
			}
			UserAccBusiness user = await currentCashier.GetUserAsync(token, tranName);
			if (user == null)
			{
				return items;
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			items = items?.Where((EvaluationModel q) => user.UserAccess.Evaluation.Report || (user.UserAccess.Evaluation.ReportSelf && q.CashierGuid == currentCashier.Guid))?.ToList();
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 90);
			throw;
		}
	}

	public static Task<EvaluationBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Evaluation.GetAsync(guid, token, tranName);
	}

	public static Task<EvaluationBusiness> GetByAnbargardaniAsync(string contextGuid, Guid anbarGardaniGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Evaluation.GetByAnbargardaniAsync(anbarGardaniGuid, token, tranName);
	}

	public static Task<EvaluationBusiness> GetAsync(string contextGuid, int number, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Evaluation.GetAsync(number, token, tranName);
	}

	public static Task<List<EvaluationDetailModel>> GetAllDetailsAsync(string contextGuid, Guid? guid, DateTime dateM, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Evaluation.GetAllDetailsAsync(guid, dateM, token, tranName);
	}

	public static Task<int> GetNextId2Async(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Evaluation.GetNextId2Async(token, tranName);
	}

	public static async Task<EvaluationBusiness> GenerateDefaultAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			EvaluationBusiness evaluationBusiness = new EvaluationBusiness(contextGuid)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				DateM = DateTime.Now,
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Status = true
			};
			EvaluationBusiness evaluationBusiness2 = evaluationBusiness;
			evaluationBusiness2.Id2 = await GetNextId2Async(contextGuid, token, tranName);
			return evaluationBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDefaultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 121);
			throw;
		}
	}

	public async Task<string> GetCashierNameAsync(CancellationToken token, string tranName = "")
	{
		try
		{
			if (CashierGuid == Guid.Empty)
			{
				return "";
			}
			if (string.IsNullOrEmpty(_cashierName))
			{
				_cashierName = (await CashierBusiness.GetAsync(ContextGuid, CashierGuid, token, tranName))?.Name ?? "";
			}
			return _cashierName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 139);
			throw;
		}
	}

	public void AddRangeToDetList(List<EvaluationDetailModel> detList, bool clearList)
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs"))
		{
			try
			{
				if (detList != null && detList.Any())
				{
					if (clearList)
					{
						_detailModelList?.Clear();
					}
					if (_detailModelList == null)
					{
						_detailModelList = new List<EvaluationDetailModel>();
					}
					_detailModelList.AddRange(detList);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 155);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 10;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "EvaluationBusiness/SaveAsync", 300000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 168);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 169), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 169);
						}
						transactionHasBeenCreated = true;
						if (SanadNo <= 0)
						{
							SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
						}
						if (Id2 <= 0)
						{
							Id2 = await GetNextId2Async(ContextGuid, token, tranName);
						}
						if (ListDetail?.Any() ?? false)
						{
							_anbarGardaniDetailList = await PrdAnbarGardaniBusiness.GetAllProductWithDetListAsync(ContextGuid, null, new AnbarGardaniFilter
							{
								DateFrom = DateM
							}, token, tranName);
							_anbarGardaniDetailList = _anbarGardaniDetailList?.Where((PrdAnbarGardaniDetBusiness q) => ListDetail.Select((EvaluationDetailModel q) => q.ProductGuid).Contains(q.PrdGuid) && q.Amount > 0m)?.ToList();
						}
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(isDeleteAction: false, system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 178);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							PrdAnbarGardaniBusiness obj = await GetAnbarGardaniKasrAsync(isDeleteAction: false, token, tranName);
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, 0, isGenerateSanad: true, isTemp: false, token, awaitOnSemaphore: true, 1, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 182);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								PrdAnbarGardaniBusiness obj2 = await GetAnbarGardaniEzafeAsync(isDeleteAction: false, token, tranName);
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await obj2.SaveAsync(system, 0, isGenerateSanad: true, isTemp: false, token, awaitOnSemaphore: true, 1, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 186);
								if (!res.HasError)
								{
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Evaluation.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 189);
									goto end_IL_0141;
								}
								result = res;
							}
						}
					}
					goto end_IL_0081;
					end_IL_0141:;
				}
				goto end_IL_006e;
				end_IL_0081:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 193);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 194);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 199), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 199);
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(bool isDeleteAction, EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(ListDetail?.Any() ?? false) && !isDeleteAction)
			{
				await res.AddErrorAsync("برگه جاری فاقد اقلام می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 208);
			}
			if (!isDeleteAction)
			{
				EvaluationBusiness evaluationBusiness = await GetAsync(ContextGuid, Id2, token, tranName);
				if (evaluationBusiness != null && evaluationBusiness.Guid != Guid)
				{
					await res.AddErrorAsync("شماره برگه ارزیابی".GetTranslate() + " " + $" {Id2}" + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 214);
				}
			}
			SanadBusiness obj = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token);
			if (obj != null && obj.SanadSt == SanadSt.Daemi)
			{
				await res.AddErrorAsync("این برگه به علت دائمی شدن سند غیر قابل ویرایش و ابطال می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 219);
			}
			PrdAnbarGardaniBusiness anbarKasr = await GetAnbarGardaniKasrAsync(isDeleteAction, token, tranName);
			if ((anbarKasr?.KasrSum ?? 0m) <= 0m)
			{
				await res.AddErrorAsync("انبارگردانی خروج معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 222);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await anbarKasr.CheckValidationAsync(system, isDeleteAction: false, 0, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 223);
			PrdAnbarGardaniBusiness anbarEzafe = await GetAnbarGardaniEzafeAsync(isDeleteAction, token, tranName);
			if ((anbarEzafe?.EzafeSum ?? 0m) <= 0m)
			{
				await res.AddErrorAsync("انبارگردانی ورود معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 226);
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await anbarEzafe.CheckValidationAsync(system, isDeleteAction: false, 0, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 227);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 231);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 232);
		}
		return res;
	}

	private async Task<PrdAnbarGardaniBusiness> GetAnbarGardaniKasrAsync(bool isDeleteAction, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			if (_anbarGardaniKasr != null)
			{
				return _anbarGardaniKasr;
			}
			if (AnbarGardaniKasrGuid != Guid.Empty)
			{
				_anbarGardaniKasr = await PrdAnbarGardaniBusiness.GetAsync(ContextGuid, AnbarGardaniKasrGuid, token, tranName);
			}
			if (isDeleteAction)
			{
				return _anbarGardaniKasr;
			}
			if (!(_anbarGardaniDetailList?.Any() ?? false))
			{
				return null;
			}
			if (_anbarGardaniKasr == null)
			{
				_anbarGardaniKasr = await PrdAnbarGardaniBusiness.GenerateNewAsync(ContextGuid, Guid.NewGuid(), "خروج از انبار بابت تجدید ارزیابی شماره".GetTranslate() + $" {Id2}", DateM, token, tranName);
			}
			_anbarGardaniKasr.ClearListDet();
			_anbarGardaniKasr.SanadNo = SanadNo;
			_anbarGardaniKasr.UserGuid = CashierGuid;
			foreach (PrdAnbarGardaniDetBusiness item in _anbarGardaniDetailList)
			{
				EvaluationDetailModel evaluation = GetFromList(item.PrdGuid);
				if (!((evaluation?.LastKardexPrice ?? 0m) <= 0m))
				{
					PrdAnbarGardaniDetBusiness factory = PrdAnbarGardaniDetBusiness.GetFactory(ContextGuid, _anbarGardaniKasr, evaluation.ProductGuid);
					factory = await PrdAnbarGardaniDetMapper.Instance.MapAsync(ContextGuid, item, factory, tranName, token);
					factory.Guid = Guid.NewGuid();
					factory.Amount = item.Amount;
					factory.Modified = DateTime.Now;
					factory.QtRealAmount = default(decimal);
					factory.UnitPrice = evaluation.LastKardexPrice;
					_anbarGardaniKasr.AddToDetList(factory);
				}
			}
			AnbarGardaniKasrGuid = _anbarGardaniKasr.Guid;
			return _anbarGardaniKasr;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAnbarGardaniKasrAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 269);
			throw;
		}
	}

	private async Task<PrdAnbarGardaniBusiness> GetAnbarGardaniEzafeAsync(bool isDeleteAction, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			if (_anbarGardaniEzafe != null)
			{
				return _anbarGardaniEzafe;
			}
			if (AnbarGardaniEzafeGuid != Guid.Empty)
			{
				_anbarGardaniEzafe = await PrdAnbarGardaniBusiness.GetAsync(ContextGuid, AnbarGardaniEzafeGuid, token, tranName);
			}
			if (isDeleteAction)
			{
				return _anbarGardaniEzafe;
			}
			if (!(_anbarGardaniDetailList?.Any() ?? false))
			{
				return null;
			}
			if (_anbarGardaniEzafe == null)
			{
				_anbarGardaniEzafe = await PrdAnbarGardaniBusiness.GenerateNewAsync(ContextGuid, Guid.NewGuid(), "ورود به انبار بابت تجدید ارزیابی شماره".GetTranslate() + $" {Id2}", DateM.AddSeconds(1.0), token, tranName);
			}
			_anbarGardaniEzafe.ClearListDet();
			_anbarGardaniEzafe.SanadNo = SanadNo;
			_anbarGardaniEzafe.UserGuid = CashierGuid;
			foreach (PrdAnbarGardaniDetBusiness item in _anbarGardaniDetailList)
			{
				EvaluationDetailModel evaluation = GetFromList(item.PrdGuid);
				if (!((evaluation?.NewPrice ?? 0m) <= 0m))
				{
					PrdAnbarGardaniDetBusiness factory = PrdAnbarGardaniDetBusiness.GetFactory(ContextGuid, _anbarGardaniEzafe, evaluation.ProductGuid);
					factory = await PrdAnbarGardaniDetMapper.Instance.MapAsync(ContextGuid, item, factory, tranName, token);
					factory.Guid = Guid.NewGuid();
					factory.Amount = 0m;
					factory.Modified = DateTime.Now;
					factory.QtRealAmount = item.Amount;
					factory.UnitPrice = evaluation.NewPrice;
					_anbarGardaniEzafe.AddToDetList(factory);
				}
			}
			AnbarGardaniEzafeGuid = _anbarGardaniEzafe.Guid;
			return _anbarGardaniEzafe;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAnbarGardaniEzafeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 306);
			throw;
		}
	}

	public EvaluationDetailModel GetFromList(Guid productGuid)
	{
		using (new JaegerMonitor("GetFromList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs"))
		{
			try
			{
				if (_detailModelList == null)
				{
					_detailModelList = new List<EvaluationDetailModel>();
				}
				return _detailModelList.FirstOrDefault((EvaluationDetailModel p) => p.ProductGuid == productGuid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFromList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 321);
				throw;
			}
		}
	}

	public async Task<bool> CanInsertAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token)).Evaluation.Insert;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 335);
			return false;
		}
	}

	public async Task<bool> CanUpdateAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return accessLevel.Evaluation.Update || (accessLevel.Evaluation.Update_Self && CashierGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 349);
			return false;
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return accessLevel.Evaluation.Delete || (accessLevel.Evaluation.Delete_Self && CashierGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 363);
			return false;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 4;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 375), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 375);
				}
				transactionHasBeenCreated = true;
				if (res.HasError)
				{
					result = res;
					goto IL_05d2;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(isDeleteAction: true, system, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 378);
				if (res.HasError)
				{
					result = res;
					goto IL_05d2;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Evaluation.RemoveAsync(Guid, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 380);
				if (res.HasError)
				{
					result = res;
					goto IL_05d2;
				}
				if (_anbarGardaniEzafe != null)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _anbarGardaniEzafe.RemoveAsync(system, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 382);
				}
				if (_anbarGardaniKasr != null)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _anbarGardaniKasr.RemoveAsync(system, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 383);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 387);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 388);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 393), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Evaluation\\EvaluationBusiness.cs", 393);
			}
		}
		return res;
		IL_05d2:
		return result;
	}
}
