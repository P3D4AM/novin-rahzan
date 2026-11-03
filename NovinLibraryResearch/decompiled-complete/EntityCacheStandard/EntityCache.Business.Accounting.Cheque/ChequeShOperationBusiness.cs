using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Pardakht;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Report;
using EntityCache.Core.Repository.Accounting.Cheque;
using EntityCache.Mapping.BusinessMappers.Accounting.Cheque;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Cheque;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.Cheque;

public class ChequeShOperationBusiness : IChequeShOperation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<ChequeShOperationBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public string ContextGuid { get; set; }

	public SanadSt SanadSt { get; set; }

	public int SanadNo { get; set; }

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid ChequeGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public short Ordinal { get; set; }

	public ENCheckSHStatus? OldState { get; set; }

	public ENCheckSHStatus NewState { get; set; }

	public string SanadDescription { get; private set; } = "";


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public short StatusId { get; set; }

	public DateTime? DateUtc { get; set; }

	public decimal Price { get; set; }

	public ChequeShOperationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ChequeShOperationBusiness> GetLastOperationAsync(string contextGuid, Guid? chequeGuid = null, ENCheckSHStatus? lastStatus = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ChequeShOperationFilter filter = new ChequeShOperationFilter
			{
				ChequeGuid = chequeGuid,
				LastStatus = lastStatus
			};
			IEnumerable<ChequeShOperationBusiness> enumerable = await GetAllAsync(contextGuid, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return enumerable?.OrderByDescending((ChequeShOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 74);
			throw;
		}
	}

	public static Task<IEnumerable<ChequeShOperationBusiness>> GetAllAsync(string contextGuid, ChequeShOperationFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation.GetAllAsync(filter, token, tranName);
	}

	public static Task<ChequeShOperationBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> SaveAsync(EnSystem system, bool sendToServer, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfoWithValue<SanadBusiness> ret = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 6;
			ReturnedSaveFuncInfoWithValue<SanadBusiness> result2;
			try
			{
				sendToServer &= ClsCache.GetCache(ContextGuid).BufferEnable && system != EnSystem.Transfer;
				ret.value = null;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ChequeShOperationBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result2 = ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 93);
					}
					else
					{
						ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
						if (autoTran)
						{
							returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 94), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 94);
						}
						transactionHasBeenCreated = true;
						short num = await SanadStatusReportBusiness.GetSanadStatusIdAsync(ContextGuid, CashierGuid, Document.CheckSanad, StatusId, EntityChangeSt.New, system, token, tranName);
						if (num != 0 || StatusId != num)
						{
							StatusId = num;
						}
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeShOperation.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 100);
						if (ret.HasError)
						{
							result2 = ret;
						}
						else if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting && system == EnSystem.SignalR)
						{
							result2 = ret;
						}
						else
						{
							CashiersLogBusiness.Logger(ContextGuid, system, sendToServer, EnLog.CheckSanad, ENAction.Add, this, null, SanadDescription, ChequeGuid);
							SanadBusiness sanad = (await GenerateSanadAsync(system, token, tranName)) ?? throw new Exception("Sanad is null");
							List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
							returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 107);
							if (!ret.HasError)
							{
								ret.value = sanad;
								goto end_IL_01d1;
							}
							result2 = ret;
						}
					}
					goto end_IL_00dd;
					end_IL_01d1:;
				}
				goto end_IL_00c9;
				end_IL_00dd:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 113);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 114);
				goto end_IL_00c9;
			}
			return result2;
			end_IL_00c9:;
		}
		finally
		{
			TaskCompletionSource<ReturnedSaveFuncInfo> result = await ClsCache.ObserveTransactionDistinity(ContextGuid, tranName);
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 120), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 120);
			}
			if (sendToServer && !ret.HasError)
			{
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					Task.Run(() => ViewCheckShBusiness.ResendNotSentAsync(ContextGuid, token, "", 20));
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, result));
				}
			}
			if (!ret.HasError && system == EnSystem.UserInterface)
			{
				ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.ChequeSh.RaiseOperationSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, ChequeShOperationVersion5 operation, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 6;
			try
			{
				if (operation == null)
				{
					result = res;
					goto IL_0797;
				}
				PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(contextGuid, operation.ChequeGuid, token);
				if (pardakhtCheckShBusiness != null && pardakhtCheckShBusiness.Status == ENCheckSHStatus.BatelShode)
				{
					result = res;
					goto IL_0797;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 147), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 147);
				}
				transactionHasBeenCreated = true;
				if (operation.CashierGuid == Guid.Empty)
				{
					operation.CashierGuid = (await CashierBusiness.GetAllAsync(contextGuid, true, token, tranName)).FirstOrDefault((CashierBusiness q) => q.Status).Guid;
				}
				ChequeShOperationConfig config = new ChequeShOperationConfig
				{
					CashierGuid = operation.CashierGuid,
					System = system,
					ChequeGuid = operation.ChequeGuid,
					DateM = operation.DateM,
					Guid = operation.Guid,
					SanadNo = operation.SanadNo
				};
				switch (operation.NewState)
				{
				case ENCheckSHStatus.BargashtShode:
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await BargashtZadanAsync(contextGuid, config, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 169);
					break;
				case ENCheckSHStatus.VosolShode:
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtAsync(contextGuid, config, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 172);
					break;
				case ENCheckSHStatus.BatelShode:
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await EbtalAsync(contextGuid, config, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 175);
					break;
				}
				if (res.HasError)
				{
					result = res;
					goto IL_0797;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 179), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 179);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 183);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 184);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 189), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 189);
			}
		}
		return res;
		IL_0797:
		return result;
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			string state = "";
			switch (NewState)
			{
			case ENCheckSHStatus.BargashtShode:
				state = "اعلام برگشت چک شخصی".GetTranslate();
				break;
			case ENCheckSHStatus.VosolShode:
				state = "اعلام پرداخت چک شخصی".GetTranslate();
				break;
			case ENCheckSHStatus.BatelShode:
				state = "ابطال چک شخصی".GetTranslate();
				break;
			}
			PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(ContextGuid, ChequeGuid, token, tranName);
			SanadBusiness sanadBusiness = null;
			Guid oldGuid = Guid.NewGuid();
			if (SanadNo > 0)
			{
				sanadBusiness = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
				if (sanadBusiness != null)
				{
					oldGuid = sanadBusiness.Guid;
				}
			}
			SanadBusiness sanad = new SanadBusiness(ContextGuid, sanadBusiness?.SanadNo ?? 0)
			{
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = ContextGuid,
				DateM = DateM,
				Guid = oldGuid,
				Id = 0,
				IsTemp = true,
				Modified = DateTime.Now,
				SanadNo = SanadNo,
				SanadSt = SanadSt,
				SanadType = SanadType.Automatic,
				Source = ClsCache.GetCache(ContextGuid).Source,
				Status = true,
				UserGuid = CashierGuid,
				Description = SanadDescription
			};
			string description = state + " (" + pardakhtCheque?.Number + ") " + pardakhtCheque?.BankName + " " + "سررسید".GetTranslate() + " " + pardakhtCheque?.DateSarresidSh;
			if (string.IsNullOrEmpty(sanad.Description))
			{
				sanad.Description = description;
			}
			Sanad_DetBusiness debit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.CheckSanad, sanad.NextOrdinal())
			{
				Credit = 0m,
				Debit = (pardakhtCheque?.Price ?? 0m),
				RowDesc = sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad.Guid,
				Modified = DateTime.Now,
				Serial = pardakhtCheque?.Number,
				DateSarresid = pardakhtCheque?.DateSaresid
			};
			await debit.SetMoeinGuidAsync(DebitMoeinGuid, token, tranName);
			await debit.SetTafsilGuidAsync(DebitTafsilGuid, tranName, token);
			await sanad.AddToListSanadAsync(debit, system, token);
			Sanad_DetBusiness credit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.CheckSanad, sanad.NextOrdinal())
			{
				Credit = (pardakhtCheque?.Price ?? 0m),
				Debit = 0m,
				RowDesc = sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad.Guid,
				Modified = DateTime.Now,
				Serial = pardakhtCheque?.Number,
				DateSarresid = pardakhtCheque?.DateSaresid
			};
			await credit.SetMoeinGuidAsync(CreditMoeinGuid, token, tranName);
			await credit.SetTafsilGuidAsync(CreditTafsilGuid, tranName, token);
			await sanad.AddToListSanadAsync(credit, system, token);
			return sanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 269);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> ElamPardakhtAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
		try
		{
			_ = 6;
			checked
			{
				try
				{
					int ordinal = 1;
					ENCheckSHStatus oldState = ENCheckSHStatus.Pardakhtani;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 284), ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 284);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
						oldState = chequeShOperationBusiness.NewState;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (pardakhtCheckShBusiness == null)
					{
						result = res;
						goto IL_09da;
					}
					ChequeShOperationBusiness op = new ChequeShOperationBusiness(contextGuid)
					{
						CashierGuid = config.CashierGuid.Value,
						ChequeGuid = config.ChequeGuid,
						SanadNo = config.SanadNo,
						Guid = (config.Guid ?? Guid.NewGuid()),
						ContextGuid = contextGuid,
						CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein4,
						CreditTafsilGuid = pardakhtCheckShBusiness.BankTafsilGuid,
						DateM = config.DateM,
						DebitMoeinGuid = pardakhtCheckShBusiness.CreditMoeinGuid,
						DebitTafsilGuid = pardakhtCheckShBusiness.CreditTafsilGuid,
						Modified = DateTime.Now,
						NewState = ENCheckSHStatus.VosolShode,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						SanadDescription = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = pardakhtCheckShBusiness.Price
					};
					if (string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "اعلام پرداخت چک شخصی".GetTranslate() + "(" + pardakhtCheckShBusiness.Number + ") " + (" " + pardakhtCheckShBusiness.BankName).GetTranslate() + " " + " سررسید".GetTranslate() + " " + (" " + pardakhtCheckShBusiness.DateSarresidSh).GetTranslate();
					}
					if (op.DebitMoeinGuid == Guid.Empty)
					{
						op.DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					}
					if (op.CreditTafsilGuid.HasValue && op.CreditTafsilGuid == Guid.Empty)
					{
						op.CreditTafsilGuid = null;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 330);
					if (res.HasError)
					{
						result = res;
						goto IL_09da;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.VosolShode, token, tranName), ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 334);
					if (res.HasError)
					{
						result = res;
						goto IL_09da;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 336);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 340);
					res.AddReturnedValue(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 341);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 346), ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 346);
			}
		}
		return res;
		IL_09da:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> BargashtZadanAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
		try
		{
			_ = 7;
			checked
			{
				try
				{
					int ordinal = 1;
					ENCheckSHStatus oldState = ENCheckSHStatus.Pardakhtani;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 361), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 361);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
						oldState = chequeShOperationBusiness.NewState;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (pardakhtCheque == null)
					{
						result = res;
						goto IL_0b14;
					}
					ChequeShOperationBusiness op = new ChequeShOperationBusiness(contextGuid)
					{
						CashierGuid = config.CashierGuid.Value,
						ChequeGuid = config.ChequeGuid,
						SanadNo = config.SanadNo,
						Guid = (config.Guid ?? Guid.NewGuid()),
						ContextGuid = contextGuid,
						DateM = config.DateM,
						Modified = DateTime.Now,
						NewState = ENCheckSHStatus.BargashtShode,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						SanadDescription = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = pardakhtCheque.Price
					};
					if (pardakhtCheque != null && string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "برگشت زدن چک شخصی".GetTranslate() + " (" + pardakhtCheque.Number + ") " + (" " + pardakhtCheque.BankName).GetTranslate() + " " + " سررسید".GetTranslate() + " " + (" " + pardakhtCheque.DateSarresidSh).GetTranslate();
					}
					if (pardakhtCheque?.Aval2re ?? false)
					{
						op.CreditMoeinGuid = pardakhtCheque?.A2re_MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
						op.CreditTafsilGuid = pardakhtCheque.A2re_TafsilGuid;
						op.DebitMoeinGuid = pardakhtCheque.CreditMoeinGuid;
						op.DebitTafsilGuid = pardakhtCheque.CreditTafsilGuid;
					}
					else
					{
						PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque.MasterGuid.Value, fromDatabase: true, token, tranName);
						op.CreditMoeinGuid = pardakhtBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
						op.CreditTafsilGuid = pardakhtBusiness?.TafsilGuid;
						op.DebitMoeinGuid = pardakhtCheque.CreditMoeinGuid;
						op.DebitTafsilGuid = pardakhtCheque.CreditTafsilGuid;
					}
					if (op.DebitMoeinGuid == Guid.Empty)
					{
						op.DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 418);
					if (res.HasError)
					{
						result = res;
						goto IL_0b14;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.BargashtShode, token, tranName), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 422);
					if (res.HasError)
					{
						result = res;
						goto IL_0b14;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 424);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 428);
					res.AddReturnedValue(ex, "", "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 429);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 434), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 434);
			}
		}
		return res;
		IL_0b14:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> EbtalAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 14;
			checked
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
				try
				{
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					int ordinal = 1;
					ENCheckSHStatus oldState = ENCheckSHStatus.Pardakhtani;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 450), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 450);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
						oldState = chequeShOperationBusiness.NewState;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeShOperationBusiness op;
					if (pardakhtCheque == null)
					{
						result = res;
					}
					else if (pardakhtCheque.Aval2re && pardakhtCheque.IsEnteghali)
					{
						result = res.AddError("کاربرگرامی شما مجاز به ابطال چک ابتدای دوره انتقالی نمی باشید", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 468);
					}
					else
					{
						op = new ChequeShOperationBusiness(contextGuid)
						{
							CashierGuid = config.CashierGuid.Value,
							ChequeGuid = config.ChequeGuid,
							SanadNo = config.SanadNo,
							Guid = (config.Guid ?? Guid.NewGuid()),
							ContextGuid = contextGuid,
							DateM = config.DateM,
							Modified = DateTime.Now,
							NewState = ENCheckSHStatus.BatelShode,
							OldState = oldState,
							Ordinal = (short)ordinal,
							SanadSt = SanadSt.Movaghat,
							Status = true,
							SanadDescription = config.SanadDesc,
							StatusId = config.SanadStatusId,
							Price = pardakhtCheque.Price
						};
						if (string.IsNullOrEmpty(op.SanadDescription))
						{
							op.SanadDescription = "ابطال چک شخصی".GetTranslate() + "  (" + (pardakhtCheque.Number ?? "").GetTranslate() + ") " + (" " + pardakhtCheque.BankName).GetTranslate() + " " + " سررسید".GetTranslate() + "  " + pardakhtCheque.DateSarresidSh;
						}
						if (pardakhtCheque.Aval2re)
						{
							if (source != ENSource.Accounting)
							{
								op.CreditMoeinGuid = pardakhtCheque.A2re_MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
								op.CreditTafsilGuid = pardakhtCheque.A2re_TafsilGuid;
								op.DebitMoeinGuid = pardakhtCheque.CreditMoeinGuid;
								op.DebitTafsilGuid = pardakhtCheque.CreditTafsilGuid;
								goto IL_0b22;
							}
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.BatelShode, token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 494);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(contextGuid, pardakhtCheque.SanadNo.GetValueOrDefault(), token, tranName);
								if (sanadBusiness != null)
								{
									returnedSaveFuncInfoWithValue = res;
									returnedSaveFuncInfoWithValue.AddReturnedValue(await sanadBusiness.RemoveAsync(config.System, 10, tranName, token, new Tuple<Document, Guid>(Document.CheckSanad, pardakhtCheque.Guid)), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 498);
								}
								result = res;
							}
						}
						else
						{
							PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque.MasterGuid.Value, fromDatabase: true, token, tranName);
							if (source != ENSource.Accounting)
							{
								op.CreditMoeinGuid = pardakhtBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
								op.CreditTafsilGuid = pardakhtBusiness?.TafsilGuid;
								op.DebitMoeinGuid = pardakhtCheque.CreditMoeinGuid;
								op.DebitTafsilGuid = pardakhtCheque.CreditTafsilGuid;
								goto IL_0b22;
							}
							pardakhtBusiness.RemoveFromDetList(pardakhtCheque);
							if (pardakhtBusiness.Sum == 0m && pardakhtBusiness.SumBank == 0m && pardakhtBusiness.SumNaghd == 0m && pardakhtBusiness.SumCheckM == 0m && pardakhtBusiness.SumCheckSh == 0m && pardakhtBusiness.SumEnteghal == 0m)
							{
								returnedSaveFuncInfoWithValue = res;
								returnedSaveFuncInfoWithValue.AddReturnedValue(await pardakhtBusiness.RemoveAsync(config.System, 10, token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 513);
							}
							else
							{
								returnedSaveFuncInfoWithValue = res;
								returnedSaveFuncInfoWithValue.AddReturnedValue(await pardakhtBusiness.SaveAsync(config.System, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 515);
							}
							result = res;
						}
					}
					goto end_IL_00af;
					IL_0b22:
					if (op.DebitMoeinGuid == Guid.Empty)
					{
						op.DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 528);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00af;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.BatelShode, token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 533);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00af;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 536);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00af;
					}
					ChequeBookBusiness chequeBookBusiness = await ChequeBookBusiness.GetAsync(contextGuid, pardakhtCheque.Number, pardakhtCheque.BankTafsilGuid, token, tranName);
					if (chequeBookBusiness != null)
					{
						chequeBookBusiness.IsSaderShode = false;
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeBookBusiness.SaveAsync(token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 543);
					}
					goto end_IL_0067;
					end_IL_00af:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 548);
					res.AddReturnedValue(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 549);
					goto end_IL_0067;
				}
				return result;
			}
			end_IL_0067:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 554), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 554);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> PardakhtShodeToPardakhtaniAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
		try
		{
			_ = 6;
			checked
			{
				try
				{
					int ordinal = 1;
					ENCheckSHStatus oldState = ENCheckSHStatus.VosolShode;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 569), ReturnedState.None, "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 569);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
						oldState = chequeShOperationBusiness.NewState;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeShOperationBusiness op = new ChequeShOperationBusiness(contextGuid)
					{
						CashierGuid = config.CashierGuid.Value,
						ChequeGuid = config.ChequeGuid,
						SanadNo = config.SanadNo,
						Guid = Guid.NewGuid(),
						ContextGuid = contextGuid,
						DateM = config.DateM,
						Modified = DateTime.Now,
						NewState = ENCheckSHStatus.Pardakhtani,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						CreditMoeinGuid = pardakhtCheckShBusiness.CreditMoeinGuid,
						CreditTafsilGuid = pardakhtCheckShBusiness.CreditTafsilGuid,
						DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein4,
						DebitTafsilGuid = pardakhtCheckShBusiness.BankTafsilGuid,
						SanadDescription = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = pardakhtCheckShBusiness.Price
					};
					if (string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "تبدیل چک شخصی پرداخت شده به پرداختنی".GetTranslate() + " (" + pardakhtCheckShBusiness.Number + ") " + (" " + pardakhtCheckShBusiness.BankName).GetTranslate() + " " + "سررسید".GetTranslate() + " " + (pardakhtCheckShBusiness.DateSarresidSh ?? "").GetTranslate();
					}
					if (op.CreditMoeinGuid == Guid.Empty)
					{
						op.CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					}
					if (op.DebitTafsilGuid.HasValue && op.DebitTafsilGuid == Guid.Empty)
					{
						op.DebitTafsilGuid = null;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 616);
					if (res.HasError)
					{
						result = res;
						goto IL_09a5;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.Pardakhtani, token, tranName), ReturnedState.None, "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 621);
					if (res.HasError)
					{
						result = res;
						goto IL_09a5;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 624);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 628);
					res.AddReturnedValue(ex, "", "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 629);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 634), ReturnedState.None, "PardakhtShodeToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 634);
			}
		}
		return res;
		IL_09a5:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> BargashtiToPardakhtaniAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
		try
		{
			_ = 7;
			checked
			{
				try
				{
					int ordinal = 1;
					ENCheckSHStatus oldState = ENCheckSHStatus.BargashtShode;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 649), ReturnedState.None, "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 649);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
						oldState = chequeShOperationBusiness.NewState;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeShOperationBusiness op = new ChequeShOperationBusiness(contextGuid)
					{
						CashierGuid = config.CashierGuid.Value,
						ChequeGuid = config.ChequeGuid,
						SanadNo = config.SanadNo,
						Guid = Guid.NewGuid(),
						ContextGuid = contextGuid,
						DateM = config.DateM,
						Modified = DateTime.Now,
						NewState = ENCheckSHStatus.Pardakhtani,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						CreditMoeinGuid = pardakhtCheckShBusiness.CreditMoeinGuid,
						CreditTafsilGuid = pardakhtCheckShBusiness.CreditTafsilGuid,
						SanadDescription = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = pardakhtCheckShBusiness.Price
					};
					if (string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "تبدیل چک شخصی برگشت شده به پرداختنی".GetTranslate() + "(" + (pardakhtCheckShBusiness.Number ?? "").GetTranslate() + ") " + (pardakhtCheckShBusiness.BankName ?? "").GetTranslate() + " " + " سررسید".GetTranslate() + " " + (" " + pardakhtCheckShBusiness.DateSarresidSh).GetTranslate();
					}
					if (pardakhtCheckShBusiness.Aval2re)
					{
						op.DebitMoeinGuid = pardakhtCheckShBusiness.CreditMoeinGuid;
						op.DebitTafsilGuid = pardakhtCheckShBusiness.CreditTafsilGuid;
						if (pardakhtCheckShBusiness.IsEnteghali)
						{
							op.CreditMoeinGuid = pardakhtCheckShBusiness.CreditMoeinGuid;
							op.CreditTafsilGuid = pardakhtCheckShBusiness.CreditTafsilGuid;
							op.DebitMoeinGuid = pardakhtCheckShBusiness.A2re_MoeinGuid.Value;
							op.DebitTafsilGuid = pardakhtCheckShBusiness.A2re_TafsilGuid;
						}
						else
						{
							op.CreditMoeinGuid = pardakhtCheckShBusiness.DebitA2reMoeinGuid.Value;
							op.CreditTafsilGuid = pardakhtCheckShBusiness.DebitA2reTafsiGuid;
						}
					}
					else
					{
						PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheckShBusiness.MasterGuid.Value, fromDatabase: true, token, tranName);
						op.DebitMoeinGuid = pardakhtBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
						op.DebitTafsilGuid = pardakhtBusiness?.TafsilGuid;
					}
					if (op.CreditMoeinGuid == Guid.Empty)
					{
						op.CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 717);
					if (res.HasError)
					{
						result = res;
						goto IL_0ac8;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.Pardakhtani, token, tranName), ReturnedState.None, "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 722);
					if (res.HasError)
					{
						result = res;
						goto IL_0ac8;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 725);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 729);
					res.AddReturnedValue(ex, "", "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 730);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 735), ReturnedState.None, "BargashtiToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 735);
			}
		}
		return res;
		IL_0ac8:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> EbtalToPardakhtaniAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 15;
			checked
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
				try
				{
					int ordinal = 1;
					ENCheckSHStatus oldState = ENCheckSHStatus.BatelShode;
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 751), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 751);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
						oldState = chequeShOperationBusiness.NewState;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeShOperationBusiness op = new ChequeShOperationBusiness(contextGuid)
					{
						CashierGuid = config.CashierGuid.Value,
						ChequeGuid = config.ChequeGuid,
						SanadNo = config.SanadNo,
						Guid = (config.Guid ?? Guid.NewGuid()),
						ContextGuid = contextGuid,
						DateM = config.DateM,
						Modified = DateTime.Now,
						NewState = ENCheckSHStatus.Pardakhtani,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						CreditMoeinGuid = pardakhtCheque.CreditMoeinGuid,
						CreditTafsilGuid = pardakhtCheque.CreditTafsilGuid,
						SanadDescription = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = pardakhtCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "تبدیل چک شخصی ابطال شده به پرداختنی".GetTranslate() + " (" + (pardakhtCheque.Number ?? "").GetTranslate() + ") " + (" " + pardakhtCheque.BankName).GetTranslate() + " سررسید " + (" " + pardakhtCheque.DateSarresidSh).GetTranslate();
					}
					if (pardakhtCheque.Aval2re)
					{
						if (source != ENSource.Accounting)
						{
							op.DebitMoeinGuid = pardakhtCheque.DebitA2reMoeinGuid.Value;
							op.DebitTafsilGuid = pardakhtCheque.DebitA2reTafsiGuid;
							goto IL_0b41;
						}
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.Pardakhtani, token, tranName), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 794);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							PardakhtCheckShBusiness pardakhtCheckShBusiness = pardakhtCheque;
							pardakhtCheckShBusiness.SanadNo = await SanadBusiness.NextIdAsync(contextGuid, token, tranName);
							SanadBusiness newSanad = await pardakhtCheque.GenerateSanadA2reAsync(config.System, token, tranName);
							List<ViewSanadDetailCoverBusiness> lst = await newSanad.GetSanadCoverListAsync(config.System);
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await newSanad.SaveAsync(config.System, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, pardakhtCheque.Guid)), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 799);
							result = res;
						}
					}
					else
					{
						PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque.MasterGuid.Value, fromDatabase: true, token, tranName);
						if (source != ENSource.Accounting)
						{
							op.DebitMoeinGuid = pardakhtBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
							op.DebitTafsilGuid = pardakhtBusiness?.TafsilGuid;
							goto IL_0b41;
						}
						PardakhtCheckShBusiness pardakhtCheckShBusiness2 = pardakhtBusiness?.CheckSh?.FirstOrDefault((PardakhtCheckShBusiness q) => q.Guid == pardakhtCheque.Guid);
						if (pardakhtCheckShBusiness2 != null)
						{
							pardakhtCheckShBusiness2.Status = ENCheckSHStatus.Pardakhtani;
						}
						else
						{
							pardakhtCheque.Status = ENCheckSHStatus.Pardakhtani;
							if (!(pardakhtBusiness?.DetailList?.Any()).GetValueOrDefault())
							{
								pardakhtBusiness.SanadNo = 0;
							}
							pardakhtBusiness.AddToDetList(pardakhtCheque, config.System, token);
						}
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await pardakhtBusiness.SaveAsync(config.System, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 818);
						result = res;
					}
					goto end_IL_00b3;
					IL_0b41:
					if (op.CreditMoeinGuid == Guid.Empty)
					{
						op.CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 829);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00b3;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.Pardakhtani, token, tranName), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 834);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00b3;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 837);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00b3;
					}
					ChequeBookBusiness chequeBookBusiness = await ChequeBookBusiness.GetAsync(contextGuid, pardakhtCheque.Number, pardakhtCheque.BankTafsilGuid, token, tranName);
					if (chequeBookBusiness != null)
					{
						chequeBookBusiness.IsSaderShode = false;
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeBookBusiness.SaveAsync(token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 844);
					}
					goto end_IL_0067;
					end_IL_00b3:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 849);
					res.AddReturnedValue(ex, "", "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 850);
					goto end_IL_0067;
				}
				return result;
			}
			end_IL_0067:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 855), ReturnedState.None, "EbtalToPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 855);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatEntezamiAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
		try
		{
			_ = 9;
			checked
			{
				try
				{
					int ordinal = 1;
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 870), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 870);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtChequeEntezamiBusiness cheque = await PardakhtChequeEntezamiBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeShOperationBusiness op = new ChequeShOperationBusiness(contextGuid)
					{
						CashierGuid = config.CashierGuid.Value,
						ChequeGuid = config.ChequeGuid,
						SanadNo = config.SanadNo,
						Guid = (config.Guid ?? Guid.NewGuid()),
						ContextGuid = contextGuid,
						DateM = config.DateM,
						Modified = DateTime.Now,
						NewState = ENCheckSHStatus.Entezami_Odat,
						OldState = ENCheckSHStatus.Entezami_Pardakhtani,
						Ordinal = (short)ordinal,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						CreditMoeinGuid = (cheque?.DebitMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein29995),
						CreditTafsilGuid = cheque?.DebitTafsilGuid,
						SanadDescription = config.SanadDesc,
						StatusId = config.SanadStatusId,
						DebitMoeinGuid = (cheque?.CreditMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Ma),
						DebitTafsilGuid = cheque?.CreditTafsilGuid,
						Price = pardakhtCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "عودت چک انتظامی".GetTranslate() + "  (" + (pardakhtCheque?.Number ?? "").GetTranslate() + ") " + (pardakhtCheque?.BankInfo?.Name ?? "").GetTranslate() + " " + "سررسید".GetTranslate() + " " + (pardakhtCheque?.DateSarresidSh ?? "").GetTranslate();
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 910);
					if (res.HasError)
					{
						result = res;
						goto IL_0be4;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.Entezami_Odat, token, tranName), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 915);
					if (res.HasError)
					{
						result = res;
						goto IL_0be4;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 918);
					if (res.HasError)
					{
						result = res;
						goto IL_0be4;
					}
					if (source != ENSource.Accounting)
					{
						result = res;
						goto IL_0be4;
					}
					ChequeBookBusiness chequeBookBusiness = await ChequeBookBusiness.GetAsync(contextGuid, pardakhtCheque?.Number, pardakhtCheque.BankTafsilGuid, token, tranName);
					if (chequeBookBusiness != null)
					{
						chequeBookBusiness.IsSaderShode = false;
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeBookBusiness.SaveAsync(token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 926);
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 931);
					res.AddReturnedValue(ex, "", "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 932);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 937), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 937);
			}
		}
		return res;
		IL_0be4:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatPardakhtaniAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 7;
			checked
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
				try
				{
					int ordinal = 1;
					_ = ClsCache.GetCache(contextGuid).Source;
					ENCheckSHStatus oldState = ENCheckSHStatus.Pardakhtani;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 953), ReturnedState.None, "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 953);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
						oldState = chequeShOperationBusiness.NewState;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeShOperationBusiness op;
					if (pardakhtCheque == null)
					{
						result = res.AddError("سند پرداختنی یافت نشد", "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 969);
					}
					else
					{
						op = new ChequeShOperationBusiness(contextGuid)
						{
							CashierGuid = config.CashierGuid.Value,
							ChequeGuid = config.ChequeGuid,
							SanadNo = config.SanadNo,
							Guid = (config.Guid ?? Guid.NewGuid()),
							ContextGuid = contextGuid,
							DateM = config.DateM,
							Modified = DateTime.Now,
							NewState = ENCheckSHStatus.OdatShode,
							OldState = oldState,
							Ordinal = (short)ordinal,
							SanadSt = SanadSt.Movaghat,
							Status = true,
							SanadDescription = config.SanadDesc,
							StatusId = config.SanadStatusId,
							DebitMoeinGuid = pardakhtCheque.CreditMoeinGuid,
							DebitTafsilGuid = pardakhtCheque.CreditTafsilGuid,
							Price = pardakhtCheque.Price
						};
						if (pardakhtCheque.Aval2re)
						{
							op.CreditMoeinGuid = pardakhtCheque.A2re_MoeinGuid.Value;
							op.CreditTafsilGuid = pardakhtCheque.A2re_TafsilGuid;
							goto IL_05cc;
						}
						if ((pardakhtCheque.MasterGuid ?? Guid.Empty) == Guid.Empty)
						{
							result = res;
						}
						else
						{
							PardakhtReportBusiness pardakhtReportBusiness = await PardakhtBusiness.GetReportAsync(contextGuid, pardakhtCheque.MasterGuid.Value, token, tranName);
							if (pardakhtReportBusiness != null)
							{
								op.CreditMoeinGuid = pardakhtReportBusiness.MoeinGuid;
								op.CreditTafsilGuid = pardakhtReportBusiness.TafsilGuid;
								goto IL_05cc;
							}
							result = res.AddError("سند پرداخت یافت نشد", "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1001);
						}
					}
					goto end_IL_0090;
					IL_05cc:
					if (string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "عودت از گیرنده".GetTranslate() + "  (" + (pardakhtCheque?.Number ?? "").GetTranslate() + ") " + (pardakhtCheque?.BankInfo?.Name ?? "").GetTranslate() + " " + "سررسید".GetTranslate() + " " + (pardakhtCheque?.DateSarresidSh ?? "").GetTranslate();
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1011);
					if (res.HasError)
					{
						result = res;
						goto end_IL_0090;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.OdatShode, token, tranName), ReturnedState.None, "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1016);
					if (res.HasError)
					{
						result = res;
						goto end_IL_0090;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1019);
					goto end_IL_0065;
					end_IL_0090:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1023);
					res.AddReturnedValue(ex, "", "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1024);
					goto end_IL_0065;
				}
				return result;
			}
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1029), ReturnedState.None, "OdatPardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1029);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatToPardakhtaniEntezamiAsync(string contextGuid, ChequeShOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
		try
		{
			_ = 9;
			checked
			{
				try
				{
					int ordinal = 1;
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1044), ReturnedState.None, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1044);
					}
					transactionHasBeenCreated = true;
					ChequeShOperationBusiness chequeShOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, token, tranName);
					if (chequeShOperationBusiness != null)
					{
						ordinal = chequeShOperationBusiness.Ordinal + 1;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					PardakhtChequeEntezamiBusiness cheque = await PardakhtChequeEntezamiBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeShOperationBusiness op = new ChequeShOperationBusiness(contextGuid)
					{
						CashierGuid = config.CashierGuid.Value,
						ChequeGuid = config.ChequeGuid,
						SanadNo = config.SanadNo,
						Guid = (config.Guid ?? Guid.NewGuid()),
						ContextGuid = contextGuid,
						DateM = config.DateM,
						Modified = DateTime.Now,
						OldState = ENCheckSHStatus.Entezami_Odat,
						NewState = ENCheckSHStatus.Entezami_Pardakhtani,
						Ordinal = (short)ordinal,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						CreditMoeinGuid = (cheque?.CreditMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Ma),
						CreditTafsilGuid = cheque?.CreditTafsilGuid,
						SanadDescription = config.SanadDesc,
						StatusId = config.SanadStatusId,
						DebitMoeinGuid = (cheque?.DebitMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein29995),
						DebitTafsilGuid = cheque?.DebitTafsilGuid,
						Price = pardakhtCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDescription))
					{
						op.SanadDescription = "خروج از عودت چک انتظامی".GetTranslate() + "(" + (pardakhtCheque?.Number ?? "").GetTranslate() + ") " + (" " + pardakhtCheque?.BankInfo?.Name).GetTranslate() + " " + " سررسید".GetTranslate() + " " + (" " + pardakhtCheque?.DateSarresidSh).GetTranslate();
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1084);
					if (res.HasError)
					{
						result = res;
						goto IL_0bdf;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(config.ChequeGuid, ENCheckSHStatus.Entezami_Pardakhtani, token, tranName), ReturnedState.None, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1089);
					if (res.HasError)
					{
						result = res;
						goto IL_0bdf;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeShOperationRepository chequeShOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeShOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1092);
					if (res.HasError)
					{
						result = res;
						goto IL_0bdf;
					}
					if (source != ENSource.Accounting)
					{
						result = res;
						goto IL_0bdf;
					}
					ChequeBookBusiness chequeBookBusiness = await ChequeBookBusiness.GetAsync(contextGuid, pardakhtCheque.Number, pardakhtCheque.BankTafsilGuid, token, tranName);
					if (chequeBookBusiness != null)
					{
						chequeBookBusiness.IsSaderShode = true;
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeBookBusiness.SaveAsync(token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1099);
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1104);
					res.AddReturnedValue(ex, "", "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1105);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1110), ReturnedState.None, "OdatToPardakhtaniEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1110);
			}
		}
		return res;
		IL_0bdf:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(string contextGuid, Guid chequeGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1122), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1122);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation.RemoveByChequeGuidAsync(chequeGuid, tryCount, token, tranName), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1124);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1128);
				ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1129);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1134), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1134);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 2;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1146), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1146);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeShOperation.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1148);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0483;
				}
				if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, Document.CheckSanad, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1151);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0483;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1156);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1157);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1162), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1162);
			}
		}
		return ret;
		IL_0483:
		return result;
	}

	public static Task<ChequeShOperationBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation.GetFromDatabaseAsync(guid, token, tranName);
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<ChequeShOperationBusiness> lst, EnSystem system, bool sendToServer, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return res;
			}
			foreach (ChequeShOperationBusiness item in lst)
			{
				item.ServerStatus = ServerStatusForClient.Delivered;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1177);
				if (res.HasError)
				{
					return res;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1183);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1184);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1197), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1197);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeShOperation.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1199);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1201);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1204);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1205);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1210), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1210);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxparalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1221);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1224);
				}
				if (cache.Source == ENSource.Accounting)
				{
					return ret;
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1227);
				}
				if (cache.CacheRepository.ChequeShOperation == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ChequeMOperation is null");
				}
				List<Guid> list = (await cache.CacheRepository.ChequeShOperation.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1232);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1234);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1240);
						}
						current = checked(current + 1);
						ChequeShOperationBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: ChequeShOperation \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1246);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1251);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR, null), token));
						if (tasks.Count >= maxparalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1256);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1260);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1261);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1264);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1267);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1268);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, TaskCompletionSource<ReturnedSaveFuncInfo> transactionDistinityResult, Priority priority = Priority.High)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs"))
		{
			_ = 4;
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					throw new NotImplementedException("این کد برای حسابداری نباید اجرا شود".GetTranslate());
				}
				if (transactionDistinityResult != null && (await transactionDistinityResult.Task).HasError)
				{
					return ret.AddError("خطا در ثبت عملیات".GetTranslate(), "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1283);
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "ChequeShOperationBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1289);
				}
				List<WebChequeShOperation> list = new List<WebChequeShOperation>();
				List<WebChequeShOperation> list2 = list;
				list2.Add(await ChequeShOperationMapper.Instance.MapAsync(ContextGuid, this, new WebChequeShOperation(), "", token));
				List<Packet> lstPacket = WebChequeShOperation.GenerateSaveRequest(list, Guid.NewGuid());
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1291);
				}
				using ChequeShOperationWaiter waiter = new ChequeShOperationWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("ChequeShOperationWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1295);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1297);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1299);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1302);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1303);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1316), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1316);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeShOperation.SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1318);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1320);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1323);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1324);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1329), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1329);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			token.ThrowIfCancellationRequested();
			IEnumerable<PardakhtCheckShReportBusiness> enumerable = await PardakhtCheckShBusiness.GetAllAsync(contextGuid, new PardakhtChequeFilter(), token);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			IEnumerable<Guid> enumerable2 = enumerable.Select((PardakhtCheckShReportBusiness q) => q.Guid).Distinct();
			if (enumerable2 == null || !enumerable2.Any())
			{
				return res;
			}
			foreach (Guid chequeGuid in enumerable2)
			{
				PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, chequeGuid, token);
				Guid? chequeGuid2 = chequeGuid;
				CancellationToken token2 = token;
				ChequeShOperationBusiness lastOperation = await GetLastOperationAsync(contextGuid, chequeGuid2, null, token2);
				if (lastOperation == null)
				{
					continue;
				}
				if (lastOperation.NewState == ENCheckSHStatus.BatelShode)
				{
					if (pardakhtCheque == null || !pardakhtCheque.MasterGuid.HasValue)
					{
						continue;
					}
					PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque.MasterGuid.Value, fromDatabase: true, token);
					if (pardakhtBusiness != null)
					{
						if (pardakhtBusiness.Sum == 0m || pardakhtBusiness.Sum == pardakhtCheque.Price)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.RemoveAsync(system, 10, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1354);
						}
						else
						{
							pardakhtBusiness.RemoveFromDetList(pardakhtCheque);
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.SaveAsync(system, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1358);
						}
					}
				}
				else
				{
					if (lastOperation.NewState != ENCheckSHStatus.BargashtShode)
					{
						continue;
					}
					bool hasChanged = false;
					PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(contextGuid, chequeGuid, token);
					if (pardakhtCheckShBusiness != null && pardakhtCheckShBusiness.Aval2re)
					{
						if (pardakhtCheckShBusiness.A2re_MoeinGuid.HasValue && pardakhtCheckShBusiness.A2re_MoeinGuid != lastOperation.CreditMoeinGuid)
						{
							hasChanged = true;
							lastOperation.CreditMoeinGuid = pardakhtCheckShBusiness.A2re_MoeinGuid.Value;
						}
						if (pardakhtCheckShBusiness.A2re_TafsilGuid != lastOperation.CreditTafsilGuid)
						{
							hasChanged = true;
							lastOperation.CreditTafsilGuid = pardakhtCheckShBusiness.A2re_TafsilGuid;
						}
						if (hasChanged)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await lastOperation.SaveAsync(system, sendToServer: false, token, awaitOnSemaphore: true, 10), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1377);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1383);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1384);
		}
		return res;
	}

	public static Task<List<ChequeShInstantModel>> GetChequeIntstantAsync(string contextGuid, DateTime dateM, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation.GetAllChequeShInstantAsync(dateM, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveFromWebAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ChequeShOperationConfig config = new ChequeShOperationConfig
			{
				CashierGuid = CashierGuid,
				ChequeGuid = ChequeGuid,
				DateM = DateM,
				Guid = Guid,
				SanadNo = 0,
				System = system
			};
			switch (NewState)
			{
			case ENCheckSHStatus.Pardakhtani:
				if (OldState == ENCheckSHStatus.VosolShode)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await PardakhtShodeToPardakhtaniAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1409);
				}
				else if (OldState == ENCheckSHStatus.BargashtShode)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await BargashtiToPardakhtaniAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1411);
				}
				if (OldState == ENCheckSHStatus.BatelShode)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await EbtalToPardakhtaniAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1413);
				}
				break;
			case ENCheckSHStatus.VosolShode:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1416);
				break;
			}
			case ENCheckSHStatus.BargashtShode:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await BargashtZadanAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1419);
				break;
			}
			case ENCheckSHStatus.BatelShode:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await EbtalAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1422);
				break;
			}
			case ENCheckSHStatus.Entezami_Pardakhtani:
				if (OldState == ENCheckSHStatus.Entezami_Odat)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await OdatToPardakhtaniEntezamiAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1426);
				}
				break;
			case ENCheckSHStatus.Entezami_Odat:
				if (OldState == ENCheckSHStatus.Entezami_Pardakhtani)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await OdatEntezamiAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1430);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1436);
			res.AddReturnedValue(ex, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1437);
		}
		return res;
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, CancellationToken token, string tranName)
	{
		try
		{
			if (system == EnSystem.Transfer)
			{
				return null;
			}
			PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(ContextGuid, ChequeGuid, token, tranName);
			return new List<ViewSanadDetailCoverBusiness>
			{
				new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = DebitMoeinGuid,
					DebitTafsilGuid = DebitTafsilGuid,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = 0m,
					Credit = pardakhtCheckShBusiness.Price,
					CashierGuid = CashierGuid,
					SanadNo = SanadNo,
					SanadSt = SanadSt,
					Id2 = 0,
					CreditMoeinGuid = CreditMoeinGuid,
					CreditTafsilGuid = CreditTafsilGuid,
					SanadDesc = SanadDescription,
					RDesc = "",
					DateMoaser = DateM,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = 0m,
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = pardakhtCheckShBusiness.Number,
					Status = true,
					VahedGuid = null
				},
				new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = DebitMoeinGuid,
					DebitTafsilGuid = DebitTafsilGuid,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = pardakhtCheckShBusiness.Price,
					Credit = 0m,
					CashierGuid = CashierGuid,
					SanadNo = SanadNo,
					SanadSt = SanadSt,
					Id2 = 0,
					CreditMoeinGuid = CreditMoeinGuid,
					CreditTafsilGuid = CreditTafsilGuid,
					SanadDesc = SanadDescription,
					RDesc = "",
					DateMoaser = DateM,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = 0m,
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = pardakhtCheckShBusiness.Number,
					Status = true,
					VahedGuid = null
				}
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1526);
			throw;
		}
	}

	public static Task<IEnumerable<ChequeShOperationReportBusiness>> GetAllReportAsync(string contextGuid, Guid chequeGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeShOperation.GetAllReportAsync(chequeGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateAsync(string contextGuid, EnSystem system, Guid guidCheque, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 8;
			try
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1540), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1540);
				}
				transactionHasBeenCreated = true;
				PardakhtCheckShBusiness pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, guidCheque, token, tranName);
				if (pardakhtCheque == null)
				{
					result = res;
					goto IL_09dc;
				}
				if (pardakhtCheque.Status <= (ENCheckSHStatus)0 && (pardakhtCheque?.MasterGuid ?? Guid.Empty) != Guid.Empty)
				{
					PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque.MasterGuid.Value, fromDatabase: true, token, tranName);
					if (pardakhtBusiness == null)
					{
						result = res;
						goto IL_09dc;
					}
					PardakhtCheckShBusiness pardakhtCheckShBusiness = pardakhtBusiness.CheckSh?.SingleOrDefault((PardakhtCheckShBusiness q) => q.Guid == pardakhtCheque.Guid);
					if (pardakhtCheckShBusiness != null)
					{
						pardakhtCheckShBusiness.Status = ENCheckSHStatus.Pardakhtani;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.SaveAsync(system, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, tranName, token), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1552);
						pardakhtCheque = await PardakhtCheckShBusiness.GetAsync(contextGuid, guidCheque, token, tranName);
					}
				}
				if (res.HasError)
				{
					result = res;
					goto IL_09dc;
				}
				List<ChequeShOperationReportBusiness> list = (await GetAllReportAsync(contextGuid, guidCheque, token, tranName))?.OrderBy((ChequeShOperationReportBusiness q) => q.Ordinal)?.ToList();
				if ((list == null || !list.Any()) && (pardakhtCheque.Status == ENCheckSHStatus.Pardakhtani || pardakhtCheque.Status == ENCheckSHStatus.Entezami_Pardakhtani))
				{
					result = res;
					goto IL_09dc;
				}
				ChequeShOperationReportBusiness chequeShOperationReportBusiness = list?.LastOrDefault();
				if (chequeShOperationReportBusiness != null && chequeShOperationReportBusiness.NewState == pardakhtCheque.Status)
				{
					result = res;
					goto IL_09dc;
				}
				ChequeShOperationConfig chequeShOperationConfig = new ChequeShOperationConfig
				{
					CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
					ChequeGuid = pardakhtCheque.Guid,
					DateM = DateTime.Now,
					System = EnSystem.UserInterface
				};
				switch (pardakhtCheque.Status)
				{
				case ENCheckSHStatus.VosolShode:
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtAsync(contextGuid, chequeShOperationConfig, token, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1571);
					break;
				}
				case ENCheckSHStatus.BatelShode:
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await EbtalAsync(contextGuid, chequeShOperationConfig, token, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1573);
					break;
				}
				case ENCheckSHStatus.Pardakhtani:
					if (chequeShOperationReportBusiness != null && chequeShOperationReportBusiness.NewState == ENCheckSHStatus.VosolShode)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.ForceUpdateChecqueStatusAsync(chequeShOperationConfig.ChequeGuid, ENCheckSHStatus.VosolShode, token, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1576);
					}
					break;
				default:
					throw new NotImplementedException();
				case ENCheckSHStatus.BargashtShode:
				case ENCheckSHStatus.Entezami_Pardakhtani:
				case ENCheckSHStatus.Entezami_Odat:
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1585);
				res.AddReturnedValue(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1586);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1591), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeShOperationBusiness.cs", 1591);
			}
		}
		return res;
		IL_09dc:
		return result;
	}
}
