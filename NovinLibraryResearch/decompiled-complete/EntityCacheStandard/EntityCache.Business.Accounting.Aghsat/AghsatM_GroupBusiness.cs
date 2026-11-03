using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Aghsat;
using PacketParser.EventArgs;
using PacketParser.Records.Aghsat;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.Aghsat;

public class AghsatM_GroupBusiness : BaseBusiness<AghsatM_GroupBusiness>, IAghsatM_Group, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<AghsatM_GroupBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private Guid _moeinGuid;

	private Guid? _creditMoeinGuid;

	private Guid? _creditTafsilGuid;

	public Guid Guid { get; set; }

	public decimal Sum { get; set; }

	public int Count { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid
	{
		set
		{
			_moeinGuid = value;
		}
	}

	public Guid CashierGuid { get; set; }

	public int? SanadNo { get; set; }

	public decimal ProfitSum { get; set; }

	public override string ContextGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public Guid? CreditMoeinGuid
	{
		set
		{
			_creditMoeinGuid = value;
		}
	}

	public Guid? CreditTafsilGuid
	{
		set
		{
			_creditTafsilGuid = value;
		}
	}

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public AghsatM_GroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<Guid> GetMoeinGuidAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			if (_moeinGuid != Guid.Empty)
			{
				return _moeinGuid;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs");
			if (tafsilBusiness == null)
			{
				return _moeinGuid;
			}
			TafsilGroupBusiness tafsilGroupBusiness = await TafsilGroupBusiness.GetAsync(ContextGuid, tafsilBusiness.TafsilGroupGuid, system, token, tranName);
			if (tafsilGroupBusiness == null)
			{
				return _moeinGuid;
			}
			_moeinGuid = await tafsilGroupBusiness.GetMoeinGuidAsync(token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 64);
		}
		return _moeinGuid;
	}

	public void SetMoeinGuid(Guid guid)
	{
		_moeinGuid = guid;
	}

	public async Task<Guid?> GetCreditMoeinGuidAsync(CancellationToken token, string tranName = "")
	{
		try
		{
			if ((_creditMoeinGuid ?? Guid.Empty) == Guid.Empty)
			{
				_creditMoeinGuid = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Installments.VajhEltezam.CreditPenaltyMoeinGuid;
			}
			return _creditMoeinGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 81);
			throw;
		}
	}

	public async Task<Guid?> GetCreditTafsilGuidAsync(CancellationToken token, string tranName = "")
	{
		try
		{
			if ((_creditTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				_creditTafsilGuid = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Installments.VajhEltezam.CreditPenaltyTafsilGuid;
			}
			return _creditTafsilGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 98);
			throw;
		}
	}

	public Task<CashierBusiness> GetCashierAsync(CancellationToken token, string tranName)
	{
		return CashierBusiness.GetAsync(ContextGuid, CashierGuid, token, tranName);
	}

	public Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token, string tranName)
	{
		return TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs");
	}

	public static Task<AghsatM_GroupBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMGroupRepository?.GetAsync(guid, token, tranName);
	}

	public static async Task<IEnumerable<AghsatM_GroupBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await (ClsCache.GetCache(contextGuid).CacheRepository.AghsatMGroupRepository?.GetAllAsync(status, token, tranName))).OrderByDescending((AghsatM_GroupBusiness p) => p.Sum);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 115);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), Priority priority = Priority.High, bool sendToServer = true, bool await4Send = false, bool awaitOnSemaphore = true, string tranName = "", SanadBusiness orderSanad = null, AghsatMSanadRecord sanadRecord = null)
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
			_ = 11;
			ReturnedSaveFuncInfo result;
			try
			{
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 128), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 128);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 130);
				if (ret.HasError)
				{
					result = ret;
					goto end_IL_012a;
				}
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "AghsatM_GroupBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 134);
					}
					else
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMGroupRepository.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 135);
						if ((SanadNo.GetValueOrDefault() == 0 && !(ProfitSum != 0m)) || system == EnSystem.Transfer)
						{
							goto end_IL_03a3;
						}
						if (SanadNo.GetValueOrDefault() <= 0)
						{
							SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
						}
						SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName, sanadRecord);
						if (sanad != null)
						{
							if (orderSanad == null)
							{
								await sanad.CheckValidationAsync(system, true, tranName, false, token, new Tuple<Document, Guid>(Document.AghsatMProfit, Guid));
								if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 147), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 147);
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMGroupRepository.SetSanadNoAsync(Guid, SanadNo.Value, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 148);
								}
								sanad.SanadNo = SanadNo.Value;
								List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, sanadRecord, token, tranName);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.AghsatMProfit, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 153);
							}
							else
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await orderSanad.JoinAsync(sanad, system, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 155);
							}
							goto end_IL_03a3;
						}
						result = ret;
					}
					goto end_IL_012a;
					end_IL_03a3:;
				}
				goto end_IL_00ee;
				end_IL_012a:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 160);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 161);
				goto end_IL_00ee;
			}
			return result;
			end_IL_00ee:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 166), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 166);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 169);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (TafsilGuid == Guid.Empty)
			{
				return await ret.AddErrorAsync("طرف حساب قسط دریافتنی معتبر نمی باشد!", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 180);
			}
			if (await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs") == null)
			{
				await ret.AddErrorAsync("طرف حساب قسط دریافتنی یافت نشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 182);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 186);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 187);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, bool sendToServer = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 7;
			try
			{
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 200), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 200);
				}
				transactionHasBeenCreated = true;
				Status = false;
				Sum = 0m;
				Count = 0;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMGroupRepository.SaveAsync(this, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 205);
				if (res.HasError)
				{
					result = res;
					goto IL_091c;
				}
				IEnumerable<AghsatMBusiness> enumerable = await AghsatMBusiness.GetAllByGroupGuidAsync(ContextGuid, Guid, token, tranName);
				if (enumerable != null && enumerable.Any())
				{
					List<Guid> eConnectionData = enumerable?.Select((AghsatMBusiness q) => q.Guid)?.ToList();
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await AghsatMBusiness.DeleteRangeAsync(ContextGuid, eConnectionData, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 211);
				}
				if (res.HasError)
				{
					result = res;
					goto IL_091c;
				}
				if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, Document.AghsatMProfit, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 215);
				}
				if (res.HasError)
				{
					result = res;
					goto IL_091c;
				}
				if (SanadNo.GetValueOrDefault() != 0 || ProfitSum != 0m)
				{
					SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 220), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 220);
					if (sanad != null)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await sanad.RemoveAsync(system, 1, tranName, token, new Tuple<Document, Guid>(Document.AghsatMProfit, Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 222);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 227);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 228);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 233), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 233);
			}
			if (!res.HasError && sendToServer)
			{
				Task.Run(() => Send2ServerAsync(token, system, Priority.High));
			}
		}
		return res;
		IL_091c:
		return result;
	}

	public static Task<AghsatM_GroupBusiness> GenerateNewWithDefaultsAsync(string contextGuid, int count = 0, Guid? tafsilGuid = null)
	{
		try
		{
			AghsatM_GroupBusiness aghsatM_GroupBusiness = new AghsatM_GroupBusiness(contextGuid)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				ContextGuid = contextGuid,
				Count = count,
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Status = true
			};
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				aghsatM_GroupBusiness.TafsilGuid = tafsilGuid.Value;
			}
			return Task.FromResult(aghsatM_GroupBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 257);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSumAsync(CancellationToken token, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 269), ReturnedState.None, "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 269);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMGroupRepository.UpdateSumAsync(Guid, token, tranName), ReturnedState.None, "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 271);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 275);
				res.AddReturnedValue(ex, "", "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 276);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 281), ReturnedState.None, "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 281);
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 293), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 293);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMGroupRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 295);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 297);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 300);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 301);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 306), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 306);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMGroupRepository;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, Priority priority, List<AghsatM_GroupBusiness> lst, bool sendtoServer, bool awit4Send = false, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (AghsatM_GroupBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 324);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, priority, sendtoServer, awit4Send, awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 325);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, "تعداد".GetTranslate() + ":" + $"{item.Count}".GetTranslate() + "-" + "جمع".GetTranslate() + ":" + $" {item.Sum:N0}", token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 337);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 338);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(lst?.Count() ?? 0, lst?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs"))
		{
			_ = 3;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "AghsatM_GroupBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 362);
				}
				List<WebAghsatMGroup> list = new List<WebAghsatMGroup>();
				List<WebAghsatMGroup> list2 = list;
				list2.Add(await AghsatMGroupMapper.Instance.MapAsync(ContextGuid, this, new WebAghsatMGroup(), "", token));
				List<Packet> paks = WebAghsatMGroup.GenerateSaveRequest(list, 0, "ارسال از کلاینت به سرور");
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 364);
				}
				using AghsatMGroupWaiter waiter = new AghsatMGroupWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("AghsatMGroupBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 368);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 370);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 372);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 375);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 376);
			}
			return ret;
		}
	}

	public static Task<AghsatM_GroupBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token, string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMGroupRepository.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<AghsatM_GroupBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs"))
		{
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.GhestMGroup;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال گروه اقساط اشخاص".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 397);
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				int num;
				if (cache == null)
				{
					num = 1;
				}
				else
				{
					_ = cache.BufferCancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 399);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 400);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.AghsatMGroupRepository == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.AghsatMGroupRepository is null");
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 403);
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.AghsatMGroupRepository.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 406);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("گروه اقساط اشخاص سینک هستند تعداد گروه اقساط اشخاص سینک شده ".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 407);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 414);
						}
						current = checked(current + 1);
						AghsatM_GroupBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj == null)
						{
							continue;
						}
						if (uiUpdater != null)
						{
							uiUpdater.Count = current;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 419);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: AghsatMGroup \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 422);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 428);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 432);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 435);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 436);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 438);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 441);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 442);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName, AghsatMSanadRecord sanadRecord = null)
	{
		using (new JaegerMonitor("GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs"))
		{
			try
			{
				if (SanadNo.GetValueOrDefault(-1) < 0 || ProfitSum <= 0m || TafsilGuid == Guid.Empty || _moeinGuid == Guid.Empty)
				{
					return null;
				}
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token, tranName);
				if (SanadNo.GetValueOrDefault() <= 0)
				{
					SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
				}
				int oldSanadNumber = (await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName))?.SanadNo.GetValueOrDefault() ?? 0;
				Guid oldGuid = Guid.NewGuid();
				SanadBusiness sanad = await SanadBusiness.GetAsync(ContextGuid, SanadNo.Value, token, tranName);
				if (sanad != null)
				{
					oldGuid = sanad.Guid;
				}
				sanad = new SanadBusiness(ContextGuid, oldSanadNumber)
				{
					Description = ((!string.IsNullOrEmpty(sanadRecord?.Description)) ? sanadRecord.Description : setting.Installments.VajhEltezam.ProfitSanadDescription),
					DateM = (sanadRecord?.DateM ?? DateTime.Now),
					UserGuid = CashierGuid,
					Guid = oldGuid,
					SanadType = SanadType.Automatic
				};
				Sanad_DetBusiness debit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AghsatMProfit, sanad.NextOrdinal())
				{
					Guid = Guid.NewGuid(),
					Credit = 0m,
					Debit = ProfitSum,
					MasterGuid = sanad.Guid,
					RowDesc = ((!string.IsNullOrEmpty(sanadRecord?.Description)) ? sanadRecord.Description : setting.Installments.VajhEltezam.ProfitSanadDescription)
				};
				Sanad_DetBusiness sanad_DetBusiness = debit;
				await sanad_DetBusiness.SetMoeinGuidAsync(await GetMoeinGuidAsync(system, token, tranName), token, tranName);
				await debit.SetTafsilGuidAsync(TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debit, system, token);
				Sanad_DetBusiness credit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AghsatMProfit, sanad.NextOrdinal())
				{
					Guid = Guid.NewGuid(),
					Credit = ProfitSum,
					Debit = 0m,
					MasterGuid = sanad.Guid,
					RowDesc = ((!string.IsNullOrEmpty(sanadRecord?.Description)) ? sanadRecord.Description : setting.Installments.VajhEltezam.ProfitSanadDescription)
				};
				await credit.SetMoeinGuidAsync((await GetCreditMoeinGuidAsync(token, tranName)).Value, token, tranName);
				sanad_DetBusiness = credit;
				await sanad_DetBusiness.SetTafsilGuidAsync(await GetCreditTafsilGuidAsync(token, tranName), tranName, token);
				await sanad.AddToListSanadAsync(credit, system, token);
				return sanad;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 513);
				throw;
			}
		}
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, AghsatMSanadRecord sanadRecord, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs"))
		{
			_ = 6;
			try
			{
				if (system == EnSystem.Transfer)
				{
					return null;
				}
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid);
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.DebitMoeinGuid = await GetMoeinGuidAsync(system, token, tranName);
				viewSanadDetailCoverBusiness.DebitTafsilGuid = TafsilGuid;
				viewSanadDetailCoverBusiness.Date = sanadRecord?.DateM ?? DateTime.Now;
				viewSanadDetailCoverBusiness.AsGuid = Guid;
				viewSanadDetailCoverBusiness.AsType = DocumentDet.AghsatM;
				viewSanadDetailCoverBusiness.AsTypeDocument = Document.AghsatMProfit;
				viewSanadDetailCoverBusiness.Debit = ProfitSum;
				viewSanadDetailCoverBusiness.Credit = 0m;
				viewSanadDetailCoverBusiness.CashierGuid = CashierGuid;
				viewSanadDetailCoverBusiness.SanadNo = SanadNo.GetValueOrDefault();
				viewSanadDetailCoverBusiness.SanadSt = SanadSt.Movaghat;
				viewSanadDetailCoverBusiness.Id2 = 0;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = await GetCreditMoeinGuidAsync(token, tranName);
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = await GetCreditTafsilGuidAsync(token, tranName);
				viewSanadDetailCoverBusiness.SanadDesc = setting.Installments.VajhEltezam.ProfitSanadDescription;
				viewSanadDetailCoverBusiness.RDesc = setting.Installments.VajhEltezam.ProfitSanadDescription;
				viewSanadDetailCoverBusiness.DateMoaser = sanadRecord?.DateM ?? DateTime.Now;
				viewSanadDetailCoverBusiness.DetailGuid = Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = null;
				viewSanadDetailCoverBusiness.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness.DecimalQt = 0m;
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
				List<ViewSanadDetailCoverBusiness> list3 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness5 = new ViewSanadDetailCoverBusiness(ContextGuid);
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness6 = viewSanadDetailCoverBusiness5;
				viewSanadDetailCoverBusiness6.DebitMoeinGuid = await GetMoeinGuidAsync(system, token, tranName);
				viewSanadDetailCoverBusiness5.DebitTafsilGuid = TafsilGuid;
				viewSanadDetailCoverBusiness5.Date = sanadRecord?.DateM ?? DateTime.Now;
				viewSanadDetailCoverBusiness5.AsGuid = Guid;
				viewSanadDetailCoverBusiness5.AsType = DocumentDet.AghsatM;
				viewSanadDetailCoverBusiness5.AsTypeDocument = Document.AghsatMProfit;
				viewSanadDetailCoverBusiness5.Debit = 0m;
				viewSanadDetailCoverBusiness5.Credit = ProfitSum;
				viewSanadDetailCoverBusiness5.CashierGuid = CashierGuid;
				viewSanadDetailCoverBusiness5.SanadNo = SanadNo.GetValueOrDefault();
				viewSanadDetailCoverBusiness5.SanadSt = SanadSt.Movaghat;
				viewSanadDetailCoverBusiness5.Id2 = 0;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness7 = viewSanadDetailCoverBusiness5;
				viewSanadDetailCoverBusiness7.CreditMoeinGuid = await GetCreditMoeinGuidAsync(token, tranName);
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness8 = viewSanadDetailCoverBusiness5;
				viewSanadDetailCoverBusiness8.CreditTafsilGuid = await GetCreditTafsilGuidAsync(token, tranName);
				viewSanadDetailCoverBusiness5.SanadDesc = setting.Installments.VajhEltezam.ProfitSanadDescription;
				viewSanadDetailCoverBusiness5.RDesc = setting.Installments.VajhEltezam.ProfitSanadDescription;
				viewSanadDetailCoverBusiness5.DateMoaser = sanadRecord?.DateM ?? DateTime.Now;
				viewSanadDetailCoverBusiness5.DetailGuid = Guid;
				viewSanadDetailCoverBusiness5.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness5.DecimalGuid = null;
				viewSanadDetailCoverBusiness5.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness5.DecimalQt = 0m;
				viewSanadDetailCoverBusiness5.DepotGuid = null;
				viewSanadDetailCoverBusiness5.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness5.Price = 0m;
				viewSanadDetailCoverBusiness5.ProductGuid = null;
				viewSanadDetailCoverBusiness5.Qt = 0m;
				viewSanadDetailCoverBusiness5.Qt2 = 0m;
				viewSanadDetailCoverBusiness5.RBazaryab = 0m;
				viewSanadDetailCoverBusiness5.RDiscount = 0m;
				viewSanadDetailCoverBusiness5.Serial = "";
				viewSanadDetailCoverBusiness5.Status = true;
				viewSanadDetailCoverBusiness5.VahedGuid = null;
				list3.Add(viewSanadDetailCoverBusiness5);
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatM_GroupBusiness.cs", 603);
				throw;
			}
		}
	}
}
