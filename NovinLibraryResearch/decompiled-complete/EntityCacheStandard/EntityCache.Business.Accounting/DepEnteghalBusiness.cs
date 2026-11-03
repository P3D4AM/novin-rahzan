using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Extensions;
using EntityCache.Entities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting;

public class DepEnteghalBusiness : Serializable<DepEnteghalBusiness>, IDepEnteghal, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IChangeIntegrity
{
	private List<DepEnteghalDetBusiness> _lstEnteghalDet;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private DepEnteghalBusiness _oldDptEnteghal;

	private DepEnteghalBusiness _oldPage;

	private Guid _debitMoeinGuid = Guid.Empty;

	private Guid _creditMoeinGuid = Guid.Empty;

	private Guid _debitTafsilGuid = Guid.Empty;

	private Guid _creditTafsilGuid = Guid.Empty;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid UserGuid { get; set; }

	public int No { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture();

	public DateTime ModifiedM { get; set; } = DateTime.Now;


	public string MashinNo { get; set; }

	public string GirandeName { get; set; }

	public string RanandehName { get; set; }

	public short StatusId { get; set; }

	public string ContextGuid { get; set; }

	public Guid? DebitMoeinGuid
	{
		set
		{
			_debitMoeinGuid = value ?? Guid.Empty;
		}
	}

	public Guid? DebitTafsilGuid
	{
		set
		{
			_debitTafsilGuid = value ?? Guid.Empty;
		}
	}

	public Guid? CreditMoeinGuid
	{
		set
		{
			_creditMoeinGuid = value ?? Guid.Empty;
		}
	}

	public Guid? CreditTafsilGuid
	{
		set
		{
			_creditTafsilGuid = value ?? Guid.Empty;
		}
	}

	public int SanadNo { get; set; }

	public IReadOnlyList<DepEnteghalDetBusiness> LstEnteghalDet
	{
		get
		{
			_lstEnteghalDet = _lstEnteghalDet?.Where((DepEnteghalDetBusiness p) => p != null)?.OrderBy((DepEnteghalDetBusiness q) => q.Ordinal)?.ToList() ?? new List<DepEnteghalDetBusiness>();
			return _lstEnteghalDet;
		}
	}

	public DepEnteghalBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<Guid> GetDebitMoeinGuidAsync()
	{
		try
		{
			if (_debitMoeinGuid != Guid.Empty)
			{
				return Task.FromResult(_debitMoeinGuid);
			}
			_debitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein50;
			return Task.FromResult(_debitMoeinGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDebitMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 72);
			throw;
		}
	}

	public Task<Guid> GetDebitTafsilGuidAsync()
	{
		try
		{
			if (_debitTafsilGuid != Guid.Empty)
			{
				return Task.FromResult(_debitTafsilGuid);
			}
			_debitTafsilGuid = ParentDefaults.TafsilCoding.clsTafsilInventory;
			return Task.FromResult(_debitTafsilGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDebitTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 86);
			throw;
		}
	}

	public Task<Guid> GetCreditMoeinGuidAsync()
	{
		try
		{
			if (_creditMoeinGuid != Guid.Empty)
			{
				return Task.FromResult(_creditMoeinGuid);
			}
			_creditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein50;
			return Task.FromResult(_creditMoeinGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 100);
			throw;
		}
	}

	public Task<Guid> GetCreditTafsilGuidAsync()
	{
		try
		{
			if (_creditTafsilGuid != Guid.Empty)
			{
				return Task.FromResult(_creditTafsilGuid);
			}
			_creditTafsilGuid = ParentDefaults.TafsilCoding.clsTafsilInventory;
			return Task.FromResult(_creditTafsilGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 114);
			throw;
		}
	}

	public static Task<IEnumerable<DepEnteghalBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepEnteghal.GetAllAsync(status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 5;
			try
			{
				ClearListDet();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 129), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 129);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: true, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 131);
				if (ret.HasError)
				{
					result = ret;
					goto IL_06b9;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: true, this, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 133);
				if (ret.HasError)
				{
					result = ret;
					goto IL_06b9;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IDepEnteghal.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 135);
				if (ret.HasError)
				{
					result = ret;
					goto IL_06b9;
				}
				if (system != EnSystem.Transfer)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 139), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 139);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, Document.EnteghalBeyneAnbar, DateM, 10, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 140);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 145);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 146);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 151), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 151);
			}
		}
		return ret;
		IL_06b9:
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, int oldNumber = 0, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 13;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "DepEnteghalBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 165);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 166), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 166);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 168);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							Modified = DateTime.Now;
							foreach (DepEnteghalDetBusiness item in LstEnteghalDet)
							{
								item.Modified = DateTime.Now;
								item.MasterGuid = Guid;
								if (item.Ordinal <= 0)
								{
									item.Ordinal = NextOrdinal();
								}
							}
							if (system != EnSystem.Transfer)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: false, this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 178);
							}
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								if (oldNumber > 0)
								{
									DepEnteghalBusiness depEnteghalBusiness = await GetAsync(ContextGuid, oldNumber, token, tranName);
									if (depEnteghalBusiness != null)
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await depEnteghalBusiness.RemoveAsync(system, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 183);
									}
								}
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IDepEnteghal.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 186);
									if (!ret.HasError)
									{
										if (system != EnSystem.Transfer)
										{
											SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
											if (SanadNo <= 0 && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
											{
												SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IDepEnteghal.SetSanadNoAsync(Guid, SanadNo, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 194);
											}
											sanad.SanadNo = SanadNo;
											List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.EnteghalBeyneAnbar, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 199);
											if (!ret.HasError)
											{
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 203), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 203);
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, Document.EnteghalBeyneAnbar, DateM, 10, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 204);
											}
										}
										goto end_IL_014d;
									}
									result = ret;
								}
							}
						}
					}
					goto end_IL_0081;
					end_IL_014d:;
				}
				goto end_IL_006e;
				end_IL_0081:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 210);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 211);
				goto end_IL_006e;
			}
			return result;
			end_IL_006e:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 216), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 216);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, IEnumerable<DepEnteghalBusiness> lst, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CancellationTokenSource invokeToken = new CancellationTokenSource();
			int current = 0;
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			foreach (DepEnteghalBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 231);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, 0, token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 232);
				if (percentCompleted != null)
				{
					invokeToken?.Cancel();
					invokeToken = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.No.ToString(), token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 242);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 243);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			if (system == EnSystem.UserInterface && (LstEnteghalDet?.Any() ?? false) && !isDeleteAction)
			{
				foreach (DepEnteghalDetBusiness item in LstEnteghalDet)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.CheckValidationAsync(token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 254);
				}
			}
			List<ProductsChanges> prdChanges = await GetLstProductsAnbarChangesAsync(system, token, tranName, isDeleteAction);
			CheckProductAmountConfig config = new CheckProductAmountConfig(ContextGuid)
			{
				Date = DateM,
				FactorType = Document.EnteghalBeyneAnbar,
				IsDeleteAction = isDeleteAction,
				IsTempOrder = false,
				System = system
			};
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await prdChanges.CheckPrdAmountAsync(config, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 265);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 269);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 270);
		}
		return res;
	}

	private async Task<DepEnteghalBusiness> GetOldDptEnteghalAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_oldDptEnteghal = await ClsCache.GetCache(ContextGuid).CacheRepository.IDepEnteghal.GetFromDbAsync(Guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldDptEnteghalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 280);
		}
		return _oldDptEnteghal;
	}

	public static Task<DepEnteghalBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepEnteghal.GetAsync(guid, token, tranName);
	}

	public static Task<DepEnteghalBusiness> GetAsync(string contextGuid, int no, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepEnteghal.GetAsync(no, token, tranName);
	}

	public static Task<int> NextNoAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepEnteghal.SelectLastIdGAsync(token, tranName);
	}

	public bool RemoveFromListDet(Guid detGuid)
	{
		checked
		{
			using (new JaegerMonitor("RemoveFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs"))
			{
				try
				{
					if (_lstEnteghalDet == null)
					{
						_lstEnteghalDet = new List<DepEnteghalDetBusiness>();
					}
					DepEnteghalDetBusiness depEnteghalDetBusiness = _lstEnteghalDet.FirstOrDefault((DepEnteghalDetBusiness p) => p.Guid == detGuid);
					if (depEnteghalDetBusiness != null)
					{
						_lstEnteghalDet.Remove(depEnteghalDetBusiness);
						List<DepEnteghalDetBusiness> list = (from p in LstEnteghalDet
							where p.Guid != detGuid
							orderby p.Ordinal
							select p).ToList();
						for (int i = 0; i < list.Count(); i++)
						{
							list[i].Ordinal = (short)(i + 1);
						}
					}
					return true;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 307);
					throw;
				}
			}
		}
	}

	public DepEnteghalDetBusiness GetFromListDet(Guid detGuid)
	{
		using (new JaegerMonitor("GetFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs"))
		{
			try
			{
				if (_lstEnteghalDet == null)
				{
					_lstEnteghalDet = new List<DepEnteghalDetBusiness>();
				}
				return _lstEnteghalDet.FirstOrDefault((DepEnteghalDetBusiness p) => p.Guid == detGuid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 322);
				throw;
			}
		}
	}

	public void AddToDetList(DepEnteghalDetBusiness item)
	{
		using (new JaegerMonitor("AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs"))
		{
			try
			{
				if (item == null)
				{
					return;
				}
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				if (_lstEnteghalDet == null)
				{
					_lstEnteghalDet = new List<DepEnteghalDetBusiness>();
				}
				if (!_lstEnteghalDet.Any((DepEnteghalDetBusiness p) => p.Guid == item.Guid))
				{
					item.MasterGuid = Guid;
					if (item.Ordinal <= 0)
					{
						item.Ordinal = NextOrdinal();
					}
					_lstEnteghalDet.Add(item);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 341);
				throw;
			}
		}
	}

	public void AddRangeToDetList(List<DepEnteghalDetBusiness> detList, bool clearList = false)
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs"))
		{
			try
			{
				if (detList == null || detList.Count <= 0)
				{
					return;
				}
				if (clearList)
				{
					_lstEnteghalDet?.Clear();
				}
				if (_lstEnteghalDet == null)
				{
					_lstEnteghalDet = new List<DepEnteghalDetBusiness>();
				}
				foreach (DepEnteghalDetBusiness det in detList)
				{
					if (det.Guid == Guid.Empty)
					{
						det.Guid = Guid.NewGuid();
					}
					det.MasterGuid = Guid;
				}
				_lstEnteghalDet.AddRange(detList);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 364);
				throw;
			}
		}
	}

	public void ClearListDet()
	{
		using (new JaegerMonitor("ClearListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs"))
		{
			try
			{
				if (_lstEnteghalDet == null)
				{
					_lstEnteghalDet = new List<DepEnteghalDetBusiness>();
				}
				_lstEnteghalDet.Clear();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ClearListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 378);
				throw;
			}
		}
	}

	public static Task<IEnumerable<DepEnteghalReportBusiness>> GetAllAsync(string contextGuid, DepEnteghalFilter filter, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepEnteghal.GetAllAsync(filter, token, tranName);
	}

	public static Task<int> CountQtByMasterGuidAsync(string contextGuid, Guid masterGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepEnteghal.CountQtByMasterGuidAsync(masterGuid, token, tranName);
	}

	private async Task<DepEnteghalBusiness> GetOldPage(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_oldPage = await ClsCache.GetCache(ContextGuid).CacheRepository.IDepEnteghal.GetFromDbAsync(Guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldPage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 394);
		}
		return _oldPage;
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 11;
		try
		{
			DepEnteghalBusiness old = await GetOldPage(token, tranName);
			Guid oldGuid = Guid.NewGuid();
			SanadBusiness sanad = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
			if (sanad != null)
			{
				oldGuid = sanad.Guid;
			}
			sanad = new SanadBusiness(ContextGuid, old?.SanadNo ?? 0)
			{
				Description = $"انتقال کالا بین انبارها ({No}) {Description}",
				UserGuid = UserGuid,
				DateM = DateM,
				SanadSt = SanadSt.Movaghat,
				Guid = oldGuid,
				SanadNo = SanadNo,
				SanadType = SanadType.Automatic
			};
			decimal sum = LstEnteghalDet?.Sum((DepEnteghalDetBusiness q) => q.TotalPrice) ?? 0m;
			if (sum != 0m)
			{
				Sanad_DetBusiness debit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.EnteghalBeyneAnbar, sanad.NextOrdinal())
				{
					Debit = sum,
					Credit = 0m,
					RowDesc = $"انتقال کالا بین انبارها ({No}) {Description}",
					DecimalGuid = Guid.Empty,
					Guid = Guid.NewGuid(),
					Qt = 0m,
					MasterGuid = sanad.Guid,
					Modified = DateTime.Now,
					Status = true
				};
				Sanad_DetBusiness sanad_DetBusiness = debit;
				await sanad_DetBusiness.SetMoeinGuidAsync(await GetDebitMoeinGuidAsync(), token, tranName);
				sanad_DetBusiness = debit;
				await sanad_DetBusiness.SetTafsilGuidAsync(await GetDebitTafsilGuidAsync(), tranName, token);
				await sanad.AddToListSanadAsync(debit, system, token);
				Sanad_DetBusiness credit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.EnteghalBeyneAnbar, sanad.NextOrdinal())
				{
					Debit = 0m,
					Credit = sum,
					RowDesc = $"انتقال کالا بین انبارها ({No}) {Description}",
					DecimalGuid = Guid.Empty,
					Guid = Guid.NewGuid(),
					Qt = 0m,
					MasterGuid = sanad.Guid,
					Modified = DateTime.Now,
					Status = true
				};
				sanad_DetBusiness = credit;
				await sanad_DetBusiness.SetMoeinGuidAsync(await GetCreditMoeinGuidAsync(), token, tranName);
				sanad_DetBusiness = credit;
				await sanad_DetBusiness.SetTafsilGuidAsync(await GetCreditTafsilGuidAsync(), tranName, token);
				await sanad.AddToListSanadAsync(credit, system, token);
			}
			return sanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 458);
			throw;
		}
	}

	public async Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction)
	{
		using (new JaegerMonitor("GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs"))
		{
			List<ProductsChanges> ret = new List<ProductsChanges>();
			try
			{
				DepEnteghalBusiness depEnteghalBusiness = await GetOldDptEnteghalAsync(token, tranName);
				if (depEnteghalBusiness != null && depEnteghalBusiness.LstEnteghalDet?.Count > 0)
				{
					foreach (DepEnteghalDetBusiness item in depEnteghalBusiness.LstEnteghalDet.ToList())
					{
						CalcChanges.ComputeDepEnteghalChange(ContextGuid, item, ret, isNew: false, isDeleteAction);
					}
				}
				IReadOnlyList<DepEnteghalDetBusiness> lstEnteghalDet = LstEnteghalDet;
				if (lstEnteghalDet != null && lstEnteghalDet.Count > 0)
				{
					foreach (DepEnteghalDetBusiness item2 in LstEnteghalDet.ToList())
					{
						if (!isDeleteAction)
						{
							CalcChanges.ComputeDepEnteghalChange(ContextGuid, item2, ret, isNew: true, isDeleteAction: false);
						}
					}
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 481);
				throw;
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

	public short NextOrdinal()
	{
		try
		{
			IReadOnlyList<DepEnteghalDetBusiness> lstEnteghalDet = LstEnteghalDet;
			if (lstEnteghalDet != null && lstEnteghalDet.Any())
			{
				return checked((short)((LstEnteghalDet?.Max((DepEnteghalDetBusiness p) => p.Ordinal) ?? (-1)) + 1));
			}
			return 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 498);
			throw;
		}
	}

	public async Task<bool> CanInsertAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAccessAsync(token)).HavaleBeyneAnbar.HavaleBeyneAnbar_Insert;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 512);
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
			return accessLevel.HavaleBeyneAnbar.HavaleBeyneAnbar_Update || (accessLevel.HavaleBeyneAnbar.HavaleBeyneAnbar_Update_Self && UserGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 526);
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
			return accessLevel.HavaleBeyneAnbar.HavaleBeyneAnbar_Delete || (accessLevel.HavaleBeyneAnbar.HavaleBeyneAnbar_Delete_Self && UserGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 540);
			return false;
		}
	}

	public async Task<bool> CanPrintAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return accessLevel.HavaleBeyneAnbar.HavaleBeyneAnbar_Print || (accessLevel.HavaleBeyneAnbar.HavaleBeyneAnbar_Print_Self && UserGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 554);
			return false;
		}
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			if (system == EnSystem.Transfer)
			{
				return null;
			}
			List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
			if (LstEnteghalDet?.Any() ?? false)
			{
				foreach (DepEnteghalDetBusiness item in LstEnteghalDet)
				{
					List<ViewSanadDetailCoverBusiness> list2 = list;
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid);
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness2.DebitMoeinGuid = await GetDebitMoeinGuidAsync();
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness3.DebitTafsilGuid = await GetDebitTafsilGuidAsync();
					viewSanadDetailCoverBusiness.Date = DateM;
					viewSanadDetailCoverBusiness.AsGuid = Guid;
					viewSanadDetailCoverBusiness.AsType = DocumentDet.DepEnteghal;
					viewSanadDetailCoverBusiness.AsTypeDocument = Document.EnteghalBeyneAnbar;
					viewSanadDetailCoverBusiness.Debit = 0m;
					viewSanadDetailCoverBusiness.Credit = item.TotalPrice;
					viewSanadDetailCoverBusiness.CashierGuid = UserGuid;
					viewSanadDetailCoverBusiness.SanadNo = SanadNo;
					viewSanadDetailCoverBusiness.SanadSt = SanadSt.Movaghat;
					viewSanadDetailCoverBusiness.Id2 = No;
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness4.CreditMoeinGuid = await GetCreditMoeinGuidAsync();
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness5.CreditTafsilGuid = await GetCreditTafsilGuidAsync();
					viewSanadDetailCoverBusiness.SanadDesc = Description;
					viewSanadDetailCoverBusiness.RDesc = item.Description;
					viewSanadDetailCoverBusiness.DateMoaser = DateM;
					viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness.DecimalGuid = null;
					viewSanadDetailCoverBusiness.DecimalPrice = 0m;
					viewSanadDetailCoverBusiness.DecimalQt = 0m;
					viewSanadDetailCoverBusiness.DepotGuid = item.DptGuidSrc;
					viewSanadDetailCoverBusiness.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness.Price = item.Price;
					viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
					viewSanadDetailCoverBusiness.Qt = item.Quantity;
					viewSanadDetailCoverBusiness.Qt2 = item.V2Qt;
					viewSanadDetailCoverBusiness.RBazaryab = 0m;
					viewSanadDetailCoverBusiness.RDiscount = 0m;
					viewSanadDetailCoverBusiness.Serial = "";
					viewSanadDetailCoverBusiness.Status = true;
					viewSanadDetailCoverBusiness.VahedGuid = item.VahedGuid;
					list2.Add(viewSanadDetailCoverBusiness);
					list2 = list;
					viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid);
					viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness.DebitMoeinGuid = await GetDebitMoeinGuidAsync();
					viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness5.DebitTafsilGuid = await GetDebitTafsilGuidAsync();
					viewSanadDetailCoverBusiness2.Date = DateM;
					viewSanadDetailCoverBusiness2.AsGuid = Guid;
					viewSanadDetailCoverBusiness2.AsType = DocumentDet.DepEnteghal;
					viewSanadDetailCoverBusiness2.AsTypeDocument = Document.EnteghalBeyneAnbar;
					viewSanadDetailCoverBusiness2.Debit = item.TotalPrice;
					viewSanadDetailCoverBusiness2.Credit = 0m;
					viewSanadDetailCoverBusiness2.CashierGuid = UserGuid;
					viewSanadDetailCoverBusiness2.SanadNo = SanadNo;
					viewSanadDetailCoverBusiness2.SanadSt = SanadSt.Movaghat;
					viewSanadDetailCoverBusiness2.Id2 = No;
					viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness4.CreditMoeinGuid = await GetCreditMoeinGuidAsync();
					viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness3.CreditTafsilGuid = await GetCreditTafsilGuidAsync();
					viewSanadDetailCoverBusiness2.SanadDesc = Description;
					viewSanadDetailCoverBusiness2.RDesc = item.Description;
					viewSanadDetailCoverBusiness2.DateMoaser = DateM;
					viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness2.DecimalGuid = null;
					viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
					viewSanadDetailCoverBusiness2.DecimalQt = 0m;
					viewSanadDetailCoverBusiness2.DepotGuid = item.DptGuidDst;
					viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness2.Price = item.Price;
					viewSanadDetailCoverBusiness2.ProductGuid = item.ProductGuid;
					viewSanadDetailCoverBusiness2.Qt = item.Quantity;
					viewSanadDetailCoverBusiness2.Qt2 = item.V2Qt;
					viewSanadDetailCoverBusiness2.RBazaryab = 0m;
					viewSanadDetailCoverBusiness2.RDiscount = 0m;
					viewSanadDetailCoverBusiness2.Serial = "";
					viewSanadDetailCoverBusiness2.Status = true;
					viewSanadDetailCoverBusiness2.VahedGuid = item.VahedGuid;
					list2.Add(viewSanadDetailCoverBusiness2);
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalBusiness.cs", 646);
			throw;
		}
	}
}
