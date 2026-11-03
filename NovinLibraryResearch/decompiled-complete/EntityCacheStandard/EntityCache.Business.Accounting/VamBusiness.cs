using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting;

public class VamBusiness : Serializable<VamBusiness>, IVam, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private Guid? _pardakhtGuid;

	private Guid? _aghsatGuid;

	private Guid? _zamenGuid1;

	private Guid? _zamenGuid2;

	private Guid? _zamenGuid3;

	private Guid? _zamenGuid4;

	private AghsatM_GroupBusiness _installmentGroup;

	private PardakhtBusiness _payment;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public DateTime Date1 { get; set; } = DateTime.Now.AddYears(-5);


	public DateTime Date2 { get; set; } = DateTime.Now;


	public int Id2 { get; set; }

	public decimal Zarib { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public Guid? PardakhtGuid
	{
		get
		{
			if (!((_pardakhtGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _pardakhtGuid;
			}
			return null;
		}
		set
		{
			_pardakhtGuid = value;
		}
	}

	public Guid SepordeMoeinGuid { get; set; }

	public Guid SepordeTafsilGuid { get; set; }

	public Guid VamMoeinGuid { get; set; }

	public Guid VamTafsilGuid { get; set; }

	public Guid? AghsatGuid
	{
		get
		{
			if (!((_aghsatGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _aghsatGuid;
			}
			return null;
		}
		set
		{
			_aghsatGuid = value;
		}
	}

	public decimal Sum { get; set; }

	public Guid? ZamenGuid1
	{
		get
		{
			if (!((_zamenGuid1 ?? Guid.Empty) == Guid.Empty))
			{
				return _zamenGuid1;
			}
			return null;
		}
		set
		{
			_zamenGuid1 = value;
		}
	}

	public Guid? ZamenGuid2
	{
		get
		{
			if (!((_zamenGuid2 ?? Guid.Empty) == Guid.Empty))
			{
				return _zamenGuid2;
			}
			return null;
		}
		set
		{
			_zamenGuid2 = value;
		}
	}

	public Guid? ZamenGuid3
	{
		get
		{
			if (!((_zamenGuid3 ?? Guid.Empty) == Guid.Empty))
			{
				return _zamenGuid3;
			}
			return null;
		}
		set
		{
			_zamenGuid3 = value;
		}
	}

	public Guid? ZamenGuid4
	{
		get
		{
			if (!((_zamenGuid4 ?? Guid.Empty) == Guid.Empty))
			{
				return _zamenGuid4;
			}
			return null;
		}
		set
		{
			_zamenGuid4 = value;
		}
	}

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public Guid UserGuid { get; set; }

	public short Average { get; set; }

	public ENMoadel AverageType { get; set; }

	public decimal AverageSum { get; set; }

	public decimal ProfitPercent { get; set; }

	public VamBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<VamBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVamRepository.GetAsync(guid, token, tranName);
	}

	public static async Task<VamBusiness> GenerateNewWithDefaultsAsync(string contextGuid, Guid? vamTafsilGuid = null, string description = "", Guid? guid = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			VamBusiness vamBusiness = new VamBusiness(contextGuid)
			{
				ChangeStatus = EntityChangeSt.New,
				UserGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			};
			VamBusiness vamBusiness2 = vamBusiness;
			vamBusiness2.Id2 = await NextId2Async(contextGuid, token);
			vamBusiness.SepordeMoeinGuid = setting.Vam.BaseMoeinGuid;
			vamBusiness.VamMoeinGuid = setting.Vam.LoanMoeinGuid;
			vamBusiness.Zarib = (decimal)cashier.Settings.OtherSettings.VamZarib;
			vamBusiness.AverageType = ENMoadel.Minimum;
			vamBusiness.DateM = DateTime.Now;
			VamBusiness vamBusiness3 = vamBusiness;
			if ((vamTafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				vamBusiness3.VamTafsilGuid = vamTafsilGuid.Value;
			}
			if (!string.IsNullOrEmpty(description))
			{
				vamBusiness3.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				vamBusiness3.Guid = guid.Value;
			}
			else
			{
				vamBusiness3.Guid = Guid.NewGuid();
			}
			return vamBusiness3;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 86);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 11;
			ReturnedSaveFuncInfo result;
			try
			{
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "VamBusiness/SaveAsync", 60000, token);
				int sanadNo;
				PardakhtBusiness payment2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (dis.Token.IsCancellationRequested)
				{
					result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 99);
				}
				else
				{
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 100), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 100);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 103);
					if (res.HasError)
					{
						result = res;
					}
					else
					{
						sanadNo = 0;
						AghsatM_GroupBusiness aghsatM_GroupBusiness = await GetInstallmentGroupAsync(token, tranName);
						if ((aghsatM_GroupBusiness?.SanadNo).GetValueOrDefault() != 0)
						{
							sanadNo = aghsatM_GroupBusiness.SanadNo.Value;
						}
						payment2 = null;
						if ((PardakhtGuid ?? Guid.Empty) != Guid.Empty)
						{
							payment2 = await GetPaymentAsync(system, token, tranName);
						}
						if (ChangeStatus != EntityChangeSt.Edited || payment2 == null)
						{
							goto IL_05c1;
						}
						if (await PardakhtBusiness.GetReportAsync(ContextGuid, PardakhtGuid.Value, token, tranName) != null)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await payment2.RemoveAsync(system, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 117);
						}
						if (!res.HasError)
						{
							_payment = null;
							goto IL_05c1;
						}
						result = res;
					}
				}
				goto end_IL_0145;
				IL_05c1:
				if (res.HasError)
				{
					result = res;
				}
				else
				{
					if (system == EnSystem.Transfer)
					{
						goto IL_094c;
					}
					if ((payment2?.Sum ?? 0m) <= 0m)
					{
						payment2 = await PardakhtBusiness.GetAsync(ContextGuid, PardakhtGuid ?? Guid.Empty, fromDatabase: true, token, tranName);
						if (payment2 != null)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await payment2.RemoveAsync(system, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 127);
						}
						PardakhtGuid = null;
						VamBusiness vamBusiness = this;
						PardakhtBusiness payment3 = null;
						vamBusiness._payment = payment3;
						goto IL_094c;
					}
					if (sanadNo != 0)
					{
						payment2.SanadNo = sanadNo;
					}
					payment2.ServerStatus = ServerStatusForClient.LinkToOrder;
					payment2.Date = DateM.AddSeconds(1.0);
					payment2.SetId2(0, token);
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await payment2.SaveAsync(system, sendToServer: false, Priority.Low, token, awaitOnSemaphore, 1, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 137);
					if (!res.HasError)
					{
						goto IL_0936;
					}
					await payment2.RemoveAsync(system, 1, token, tranName);
					if (!res.HasError)
					{
						goto IL_0936;
					}
					result = res;
				}
				goto end_IL_0145;
				IL_094c:
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IVamRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 147);
				goto end_IL_006e;
				IL_0936:
				PardakhtGuid = payment2.Guid;
				goto IL_094c;
				end_IL_0145:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 151);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 152);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 157), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 157);
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (SepordeMoeinGuid == Guid.Empty)
			{
				await res.AddErrorAsync("حساب معین مبنای میانگین معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 166);
			}
			if (SepordeTafsilGuid == Guid.Empty)
			{
				await res.AddErrorAsync("حساب تفصیلی مبنای میانگین معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 167);
			}
			if (VamMoeinGuid == Guid.Empty)
			{
				await res.AddErrorAsync("حساب معین گیرنده وام معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 169);
			}
			if (VamTafsilGuid == Guid.Empty)
			{
				await res.AddErrorAsync("حساب تفصیلی گیرنده وام معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 170);
			}
			if (Sum <= 0m)
			{
				await res.AddErrorAsync("مبلغ وام معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 172);
			}
			if (Zarib <= 0m)
			{
				await res.AddErrorAsync("ضریب محاسبه مبلغ وام معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 173);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 177);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 178);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<VamBusiness> lst, bool send2server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			foreach (VamBusiness item in lst)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 189);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 193);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 194);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs"))
		{
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				_ = 6;
				ReturnedSaveFuncInfo result;
				try
				{
					using (await _databaseSemaphore.GaurdAsync(true, "VamBusiness/RemoveAsync", 60000, token))
					{
						if (autoCommit)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 210), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 210);
						}
						transactionHasBeenCreated = true;
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IVamRepository.RemoveAsync(Guid, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 214);
							if (!res.HasError)
							{
								if ((PardakhtGuid ?? Guid.Empty) != Guid.Empty)
								{
									PardakhtBusiness pardakhtBusiness = await GetPaymentAsync(system, token, tranName);
									if ((pardakhtBusiness?.Sum ?? 0m) > 0m)
									{
										returnedSaveFuncInfo = res;
										returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.RemoveAsync(system, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 221);
									}
								}
								if ((AghsatGuid ?? Guid.Empty) != Guid.Empty)
								{
									AghsatM_GroupBusiness aghsatM_GroupBusiness = await GetInstallmentGroupAsync(token, tranName);
									if (aghsatM_GroupBusiness != null)
									{
										returnedSaveFuncInfo = res;
										returnedSaveFuncInfo.AddReturnedValue(await aghsatM_GroupBusiness.RemoveAsync(system, sendToServer: true, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 227);
									}
								}
								goto end_IL_0147;
							}
							result = res;
						}
						goto end_IL_009c;
						end_IL_0147:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 232);
					res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 233);
					goto end_IL_008b;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 238), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 238);
				}
			}
			return res;
		}
	}

	public static Task<int> NextId2Async(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVamRepository.NextId2Async(token, tranName);
	}

	public static Task<IEnumerable<VamModel>> GetAllAsync(string contextGuid, bool? status, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IVamRepository.GetAllModelAsync(status, token, tranName);
	}

	public async Task<AghsatM_GroupBusiness> GetInstallmentGroupAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if ((AghsatGuid ?? Guid.Empty) == Guid.Empty)
			{
				return null;
			}
			if (_installmentGroup == null)
			{
				_installmentGroup = await AghsatM_GroupBusiness.GetAsync(ContextGuid, AghsatGuid.Value, token, tranName);
			}
			return _installmentGroup;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstallmentGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 256);
			throw;
		}
	}

	public async Task<PardakhtBusiness> GetPaymentAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_payment == null && (PardakhtGuid ?? Guid.Empty) != Guid.Empty)
			{
				_payment = await PardakhtBusiness.GetAsync(ContextGuid, PardakhtGuid.Value, fromDatabase: true, token, tranName);
			}
			if (_payment == null)
			{
				_payment = new PardakhtBusiness(ContextGuid)
				{
					UserGuid = UserGuid,
					MoeinGuid = VamMoeinGuid,
					Date = DateM,
					Description = "بابت وام شماره".GetTranslate() + $" ({Id2})",
					SanadSt = SanadSt.Movaghat,
					ChangeStatus = EntityChangeSt.New,
					Guid = Guid.NewGuid()
				};
				await _payment.SetTafsilGuidAsync(VamTafsilGuid, system, token, tranName);
				PardakhtGuid = _payment.Guid;
			}
			return _payment;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPaymentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\VamBusiness.cs", 285);
			throw;
		}
	}

	public void SetPayment(PardakhtBusiness payment)
	{
		_payment = payment;
	}
}
