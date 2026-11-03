using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Reception;
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
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.Cheque;

public class ChequeMOperationBusiness : IChequeMOperation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<ChequeMOperationBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public Guid CashierGuid { get; set; }

	public int SanadNO { get; set; }

	public SanadSt SanadSt { get; set; }

	public DateTime DateSabt { get; set; }

	public bool? Avale2re_Transfer { get; set; }

	public Guid GuidCheque { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid Guid { get; set; }

	public string ContextGuid { get; set; }

	public short Ordinal { get; set; }

	public ENUMCheck? OldState { get; set; }

	public string OldStateForDispaly
	{
		get
		{
			if (OldState.HasValue)
			{
				return OldState.Value.GetDisplay();
			}
			return "";
		}
	}

	public ENUMCheck NewState { get; set; }

	public string NewStateForDisplay => NewState.GetDisplay().GetTranslate();

	public bool? IsDebitTafsilAffective { get; set; }

	public bool? IsCreditTafsilAffective { get; set; }

	public string Description { get; set; } = "";


	public string SanadDesc { get; set; } = "";


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public Document? AsType { get; set; }

	public Guid? AsGuid { get; set; }

	public bool IsSanadAffective { get; set; } = true;


	public short StatusId { get; set; }

	public DateTime? DateUtc { get; set; }

	public decimal Price { get; set; }

	public ChequeMOperationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<IEnumerable<ChequeMOperationBusiness>> GetAllAsync(string contextGuid, ChequeMOperationFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.GetAllAsync(filter, token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public static Task<ChequeMOperationBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.GetAsync(guid, token, tranName);
	}

	public static async Task<ChequeMOperationBusiness> GetLastOperationAsync(string contextGuid, Guid? chequeGuid = null, Guid? guid = null, ENUMCheck? lastStatus = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ChequeMOperationFilter filter = new ChequeMOperationFilter
			{
				ChequeGuid = chequeGuid,
				LastStatus = lastStatus
			};
			IEnumerable<ChequeMOperationBusiness> enumerable = await GetAllAsync(contextGuid, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			if (!guid.HasValue || guid.Value == Guid.Empty)
			{
				return enumerable?.OrderByDescending((ChequeMOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
			}
			ChequeMOperationBusiness obj = enumerable.FirstOrDefault((ChequeMOperationBusiness q) => q.Guid == guid.Value);
			if (obj == null)
			{
				return enumerable?.OrderByDescending((ChequeMOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
			}
			return enumerable.FirstOrDefault((ChequeMOperationBusiness q) => q.Ordinal == checked(obj.Ordinal - 1));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 106);
			throw;
		}
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
			_ = 7;
			ReturnedSaveFuncInfoWithValue<SanadBusiness> result2;
			try
			{
				sendToServer &= ClsCache.GetCache(ContextGuid).BufferEnable && system != EnSystem.Transfer && system != EnSystem.TransferMaliYear;
				ret.value = null;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ChequeMOperationBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result2 = ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 121);
					}
					else
					{
						ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
						if (autoTran)
						{
							returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 122), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 122);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 124);
						if (ret.HasError)
						{
							result2 = ret;
						}
						else
						{
							if (system != EnSystem.Transfer && system != EnSystem.TransferMaliYear && NewState != ENUMCheck.KharjShode)
							{
								IsSanadAffective = true;
								IsCreditTafsilAffective = true;
								IsDebitTafsilAffective = true;
							}
							short num = await SanadStatusReportBusiness.GetSanadStatusIdAsync(ContextGuid, CashierGuid, Document.CheckSanad, StatusId, EntityChangeSt.New, system, token, tranName);
							if (num != 0 || StatusId != num)
							{
								StatusId = num;
							}
							returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeMOperation.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 136);
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
								if (system != EnSystem.Transfer)
								{
									CashiersLogBusiness.Logger(ContextGuid, system, sendToServer, EnLog.CheckSanad, ENAction.Add, this, null, SanadDesc, GuidCheque);
								}
								if (!IsSanadAffective)
								{
									goto end_IL_01e7;
								}
								SanadBusiness sanad = (await GenerateSanadAsync(system, token, tranName)) ?? throw new Exception("Sanad is Null !!!!!");
								List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
								returnedSaveFuncInfoWithValue = ret;
								returnedSaveFuncInfoWithValue.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 146);
								if (!ret.HasError)
								{
									ret.value = sanad;
									goto end_IL_01e7;
								}
								result2 = ret;
							}
						}
					}
					goto end_IL_00e1;
					end_IL_01e7:;
				}
				goto end_IL_00cd;
				end_IL_00e1:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 153);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 154);
				goto end_IL_00cd;
			}
			return result2;
			end_IL_00cd:;
		}
		finally
		{
			TaskCompletionSource<ReturnedSaveFuncInfo> result = await ClsCache.ObserveTransactionDistinity(ContextGuid, tranName);
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 160), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 160);
			}
			if (sendToServer && !ret.HasError)
			{
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					Task.Run(() => ViewCheckMBusiness.ResendNotSentAsync(ContextGuid, token, "", 20));
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, result));
				}
			}
			if (!ret.HasError && system == EnSystem.UserInterface)
			{
				ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.ChequM.RaiseOperationSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<ChequeMOperationBusiness> lst, bool sendToServer, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return res;
			}
			foreach (ChequeMOperationBusiness item in lst)
			{
				item.ServerStatus = ServerStatusForClient.Delivered;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 182);
				if (res.HasError)
				{
					return res;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 188);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 189);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, IEnumerable<ChequeMOperationVersion5> lst, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 15;
			try
			{
				if (!(lst?.Any() ?? false))
				{
					result = res;
					goto IL_13a1;
				}
				_ = lst.FirstOrDefault().ChequeGuid;
				SetStatus(lst.OrderBy((ChequeMOperationVersion5 p) => p.DateM));
				Guid chequeGuid = lst.FirstOrDefault().ChequeGuid;
				ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, chequeGuid, token);
				ENUMCheck finalChequeState = receptionCheque.Status;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (finalChequeState == ENUMCheck.BatelShode)
				{
					ChequeMOperationVersion5 kharjItem = lst?.OrderBy((ChequeMOperationVersion5 q) => q.DateM)?.FirstOrDefault((ChequeMOperationVersion5 q) => q.NewState == ENUMCheck.KharjShode);
					ChequeMOperationVersion5 chequeMOperationVersion = lst?.OrderBy((ChequeMOperationVersion5 q) => q.DateM)?.FirstOrDefault((ChequeMOperationVersion5 q) => q.NewState == ENUMCheck.Bargashti && q.OldState == ENUMCheck.KharjShode);
					if (kharjItem != null && chequeMOperationVersion == null)
					{
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 213), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 213);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BargashtBatelShodeAsync(contextGuid, kharjItem, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 215);
						if (autoTran && transactionHasBeenCreated)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 217), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 217);
						}
					}
					result = res;
					goto IL_13a1;
				}
				ReturnedSaveFuncInfoWithValue<List<ChequeMOperationVersion5>> returnedSaveFuncInfoWithValue = FixWrongData(lst.ToList(), finalChequeState, 10);
				res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 222);
				lst = returnedSaveFuncInfoWithValue?.value?.Where((ChequeMOperationVersion5 p) => p.Guid != Guid.Empty)?.OrderBy((ChequeMOperationVersion5 q) => q.Ordinal);
				ENUMCheck newState = lst.OrderByDescending((ChequeMOperationVersion5 q) => q.Ordinal).FirstOrDefault().NewState;
				if (receptionCheque != null && receptionCheque.Status != newState && (receptionCheque.Status != ENUMCheck.KharjShode || newState != ENUMCheck.Bargashti) && newState != ENUMCheck.KharjShode)
				{
					throw new Exception("خطا در ثبت عملیات چک اشخاص".GetTranslate() + "\r\n" + "وضعیت چک در جدول والد".GetTranslate() + ":" + receptionCheque.Status.GetDisplay().GetTranslate() + " \r\n" + "آخرین وضعیت چک در گردش".GetTranslate() + ":" + newState.GetDisplay().GetTranslate() + "\r\n" + $"ChequeGuid:{receptionCheque.Guid}");
				}
				res.AddReturnedValue(PreValidation(lst), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 238);
				if (res.HasError)
				{
					result = res;
					goto IL_13a1;
				}
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 241), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 241);
				}
				transactionHasBeenCreated = true;
				ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
				{
					ChequeGuid = chequeGuid,
					Status = ENUMCheck.Mojod
				};
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 250);
				if (res.HasError)
				{
					result = res;
					goto IL_13a1;
				}
				List<ChequeMOperationVersion5> list = SetStatus(lst.OrderBy((ChequeMOperationVersion5 q) => q.Ordinal));
				foreach (ChequeMOperationVersion5 kharjItem in list)
				{
					if (kharjItem.CashierGuid == Guid.Empty)
					{
						kharjItem.CashierGuid = (await CashierBusiness.GetAllAsync(contextGuid, true, token, tranName)).FirstOrDefault((CashierBusiness q) => q.Status).Guid;
					}
					if (kharjItem.DateM < DateTime.Now.AddYears(-50))
					{
						if (receptionCheque != null)
						{
							kharjItem.DateM = receptionCheque.DateM;
						}
						else
						{
							kharjItem.DateM = DateTime.Now.AddYears(-50);
						}
					}
					ChequeMOperationConfig config = new ChequeMOperationConfig
					{
						CashierGuid = kharjItem.CashierGuid,
						ChequeGuid = kharjItem.ChequeGuid,
						DateM = kharjItem.DateM,
						DebitTafsilGuid = kharjItem.TafsilGuid,
						Description = kharjItem.Description,
						Guid = kharjItem.Guid,
						IsAvalDoreTransfered = kharjItem.AvalDoreTransfer,
						SanadNo = kharjItem.SanadNo,
						System = system,
						OldState = kharjItem.OldState,
						Ordinal = kharjItem.Ordinal
					};
					switch (kharjItem.NewState)
					{
					case ENUMCheck.Bargashti:
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BargashtZadanAsync(contextGuid, config, null, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 281);
						break;
					case ENUMCheck.VagozarShode:
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await VagozarKardanAsync(contextGuid, config, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 282);
						break;
					case ENUMCheck.NaghdShode:
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await NaghdKardanAsync(contextGuid, config, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 283);
						break;
					case ENUMCheck.VosolShode:
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await VosolKardanAsync(contextGuid, config, null, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 284);
						break;
					case ENUMCheck.Mojod:
						if (kharjItem.OldState == ENUMCheck.KharjShode)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await OdatKharjShodeNewAsync(contextGuid, config, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 287);
						}
						break;
					case ENUMCheck.KharjShode:
					{
						PardakhtCheckMBusiness pardakhtCheckMBusiness = (await PardakhtCheckMBusiness.GetAllAsync(contextGuid, config.ChequeGuid, token, tranName))?.FirstOrDefault();
						if (pardakhtCheckMBusiness != null)
						{
							PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheckMBusiness.MasterGuid, fromDatabase: true, token, tranName);
							if (pardakhtBusiness != null)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await KharjKardanAsync(contextGuid, config, pardakhtBusiness.Guid, pardakhtBusiness.MoeinGuid, pardakhtBusiness.TafsilGuid, 0, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 296);
							}
						}
						break;
					}
					}
					if (res.HasError)
					{
						res.throwExceptionIfError();
					}
				}
				ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(contextGuid, chequeGuid, token, tranName);
				if (finalChequeState != receptionCheckBusiness.Status && (finalChequeState != ENUMCheck.KharjShode || receptionCheckBusiness.Status != ENUMCheck.Bargashti))
				{
					throw new InvalidOperationException("وضعیت نهایی چک قبل از انتقال".GetTranslate() + ":" + $" {finalChequeState}".GetTranslate() + "\r\n" + "وضعیت نهایی چک پس از انتقال".GetTranslate() + ":" + $" {receptionCheckBusiness.Status}".GetTranslate() + "\r\n" + $"Guid Cheque : {receptionCheckBusiness.Guid}");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 316);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 317);
				Console.WriteLine();
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 323), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 323);
			}
		}
		return res;
		IL_13a1:
		return result;
	}

	private static List<ChequeMOperationVersion5> SetStatus(IOrderedEnumerable<ChequeMOperationVersion5> lst)
	{
		checked
		{
			try
			{
				ENUMCheck eNUMCheck = ENUMCheck.Mojod;
				short num = 1;
				foreach (ChequeMOperationVersion5 item in lst)
				{
					if ((item?.Guid ?? Guid.Empty) == Guid.Empty || item.NewState == ENUMCheck.Mojod)
					{
						eNUMCheck = ENUMCheck.Mojod;
						item.Ordinal = num;
						num++;
						continue;
					}
					item.Ordinal = num;
					if (eNUMCheck != item.OldState)
					{
						_ = item.OldState;
						item.OldState = eNUMCheck;
					}
					num++;
					eNUMCheck = item.NewState;
				}
				return lst.OrderBy((ChequeMOperationVersion5 q) => q.Ordinal).ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 353);
			}
			return null;
		}
	}

	private static ReturnedSaveFuncInfoWithValue<List<ChequeMOperationVersion5>> FixWrongData(List<ChequeMOperationVersion5> lst, ENUMCheck finalStatus, short tryCount = 10)
	{
		ReturnedSaveFuncInfoWithValue<List<ChequeMOperationVersion5>> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<List<ChequeMOperationVersion5>>();
		checked
		{
			try
			{
				if (lst == null || !lst.Any())
				{
					throw new InvalidOperationException("lst is null or empty!");
				}
				foreach (ChequeMOperationVersion5 item in lst.ToList())
				{
					try
					{
						ENUMCheck oldState = item.OldState;
						switch (item.NewState)
						{
						case ENUMCheck.Mojod:
							if (oldState == ENUMCheck.KharjShode && lst?.FirstOrDefault((ChequeMOperationVersion5 q) => q.OldState == ENUMCheck.Mojod && q.NewState == ENUMCheck.KharjShode) == null)
							{
								ChequeMOperationVersion5 item3 = new ChequeMOperationVersion5
								{
									ChequeGuid = item.ChequeGuid,
									CashierGuid = item.CashierGuid,
									AvalDoreTransfer = false,
									Guid = Guid.NewGuid(),
									NewState = ENUMCheck.KharjShode,
									OldState = ENUMCheck.Mojod,
									Ordinal = (short)(item.Ordinal - 1),
									TafsilGuid = item.TafsilGuid,
									DateM = item.DateM.AddSeconds(-1.0)
								};
								lst.Add(item3);
								if (tryCount <= 1)
								{
									throw new StackOverflowException();
								}
								return FixWrongData(lst, finalStatus, --tryCount);
							}
							break;
						case ENUMCheck.VagozarShode:
							if (oldState == ENUMCheck.KharjShode)
							{
								item.NewState = ENUMCheck.Mojod;
								ChequeMOperationVersion5 chequeMOperationVersion2 = lst?.SingleOrDefault((ChequeMOperationVersion5 q) => q.NewState == ENUMCheck.VosolShode);
								ChequeMOperationVersion5 chequeMOperationVersion3 = new ChequeMOperationVersion5
								{
									ChequeGuid = item.ChequeGuid,
									CashierGuid = item.CashierGuid,
									AvalDoreTransfer = false,
									Guid = Guid.NewGuid(),
									NewState = ENUMCheck.VagozarShode,
									OldState = ENUMCheck.Mojod,
									Ordinal = (short)(item.Ordinal + 1),
									TafsilGuid = item.TafsilGuid
								};
								if (chequeMOperationVersion2 != null)
								{
									chequeMOperationVersion3.DateM = chequeMOperationVersion2.DateM.AddSeconds(-1.0);
								}
								else
								{
									chequeMOperationVersion3.DateM = item.DateM.AddSeconds(1.0);
								}
								if (item.DateM > chequeMOperationVersion3.DateM)
								{
									item.DateM = chequeMOperationVersion3.DateM.AddSeconds(-1.0);
								}
								lst.Add(chequeMOperationVersion3);
								SetStatus(lst.OrderBy((ChequeMOperationVersion5 q) => q.DateM));
								if (tryCount <= 1)
								{
									throw new StackOverflowException();
								}
								return FixWrongData(lst, finalStatus, --tryCount);
							}
							break;
						case ENUMCheck.NaghdShode:
							if (item.OldState == ENUMCheck.VagozarShode)
							{
								int ordinal = item.Ordinal - 1;
								ChequeMOperationVersion5 chequeMOperationVersion4 = lst.FirstOrDefault((ChequeMOperationVersion5 q) => q.Ordinal == ordinal);
								if (chequeMOperationVersion4 != null)
								{
									chequeMOperationVersion4.Guid = Guid.Empty;
									SetStatus(lst.OrderBy((ChequeMOperationVersion5 q) => q.Ordinal));
								}
							}
							else if (oldState == ENUMCheck.KharjShode)
							{
								item.NewState = ENUMCheck.Mojod;
								ChequeMOperationVersion5 item2 = new ChequeMOperationVersion5
								{
									ChequeGuid = item.ChequeGuid,
									CashierGuid = item.CashierGuid,
									AvalDoreTransfer = false,
									Guid = Guid.NewGuid(),
									NewState = ENUMCheck.NaghdShode,
									OldState = ENUMCheck.Mojod,
									Ordinal = (short)(item.Ordinal + 1),
									TafsilGuid = item.TafsilGuid,
									DateM = item.DateM.AddSeconds(1.0)
								};
								lst.Add(item2);
								SetStatus(lst.OrderBy((ChequeMOperationVersion5 q) => q.Ordinal));
								if (tryCount <= 1)
								{
									throw new StackOverflowException();
								}
								return FixWrongData(lst, finalStatus, --tryCount);
							}
							break;
						case ENUMCheck.KharjShode:
						{
							int num = lst?.Count((ChequeMOperationVersion5 q) => (q.OldState == ENUMCheck.Mojod || q.OldState == ENUMCheck.KharjShode) && q.NewState == ENUMCheck.KharjShode) ?? 0;
							if (num > 1)
							{
								int num2 = lst?.Count((ChequeMOperationVersion5 q) => q.OldState == ENUMCheck.KharjShode && q.NewState == ENUMCheck.Mojod) ?? 0;
								int num3 = lst?.Count((ChequeMOperationVersion5 q) => q.OldState == ENUMCheck.KharjShode && q.NewState == ENUMCheck.Bargashti) ?? 0;
								if (num2 + num3 != num - 1)
								{
									ChequeMOperationVersion5 chequeMOperationVersion5 = lst?.OrderBy((ChequeMOperationVersion5 q) => q.DateM)?.First((ChequeMOperationVersion5 q) => q.OldState == ENUMCheck.Mojod && q.NewState == ENUMCheck.KharjShode);
									ChequeMOperationVersion5 item4 = new ChequeMOperationVersion5
									{
										ChequeGuid = item.ChequeGuid,
										CashierGuid = item.CashierGuid,
										AvalDoreTransfer = false,
										Guid = Guid.NewGuid(),
										NewState = ENUMCheck.Mojod,
										OldState = ENUMCheck.KharjShode,
										Ordinal = (short)(chequeMOperationVersion5.Ordinal + 1),
										TafsilGuid = item.TafsilGuid,
										DateM = chequeMOperationVersion5.DateM.AddSeconds(1.0)
									};
									lst.Add(item4);
									ChequeMOperationVersion5 chequeMOperationVersion6 = lst.FirstOrDefault((ChequeMOperationVersion5 q) => q.OldState == ENUMCheck.KharjShode && q.NewState == ENUMCheck.KharjShode);
									if (chequeMOperationVersion6 != null)
									{
										chequeMOperationVersion6.OldState = ENUMCheck.Mojod;
									}
									SetStatus(lst.OrderBy((ChequeMOperationVersion5 q) => q.DateM));
									if (tryCount <= 1)
									{
										throw new StackOverflowException();
									}
									return FixWrongData(lst, finalStatus, --tryCount);
								}
							}
							if ((lst?.Count() ?? 0) == 1 && finalStatus == ENUMCheck.Mojod)
							{
								ChequeMOperationVersion5 item5 = new ChequeMOperationVersion5
								{
									ChequeGuid = item.ChequeGuid,
									CashierGuid = item.CashierGuid,
									AvalDoreTransfer = false,
									Guid = Guid.NewGuid(),
									NewState = ENUMCheck.KharjShode,
									OldState = ENUMCheck.Mojod,
									Ordinal = (short)(lst.First().Ordinal + 1),
									TafsilGuid = item.TafsilGuid,
									DateM = lst.First().DateM.AddSeconds(1.0)
								};
								lst.Add(item5);
								SetStatus(lst.OrderBy((ChequeMOperationVersion5 q) => q.DateM));
								if (tryCount <= 1)
								{
									throw new StackOverflowException();
								}
								return FixWrongData(lst, finalStatus, --tryCount);
							}
							if (oldState == ENUMCheck.Bargashti)
							{
								item.OldState = ENUMCheck.Mojod;
								ChequeMOperationVersion5 item6 = new ChequeMOperationVersion5
								{
									ChequeGuid = item.ChequeGuid,
									CashierGuid = item.CashierGuid,
									AvalDoreTransfer = false,
									Guid = Guid.NewGuid(),
									NewState = ENUMCheck.Mojod,
									OldState = ENUMCheck.Bargashti,
									Ordinal = (short)(item.Ordinal - 1),
									TafsilGuid = item.TafsilGuid,
									DateM = item.DateM.AddSeconds(-1.0)
								};
								lst.Add(item6);
								SetStatus(lst.OrderBy((ChequeMOperationVersion5 q) => q.Ordinal));
								if (tryCount <= 1)
								{
									throw new StackOverflowException();
								}
								return FixWrongData(lst, finalStatus, --tryCount);
							}
							break;
						}
						case ENUMCheck.VosolShode:
						{
							if (oldState == ENUMCheck.VagozarShode)
							{
								break;
							}
							ChequeMOperationVersion5 chequeMOperationVersion = (from x in lst
								where x.NewState == ENUMCheck.VagozarShode && x.Ordinal > item.Ordinal
								select x into p
								orderby p.Ordinal descending
								select p).FirstOrDefault();
							if (chequeMOperationVersion == null)
							{
								chequeMOperationVersion = (from x in lst
									where x.NewState == ENUMCheck.VagozarShode
									select x into p
									orderby p.Ordinal descending
									select p).FirstOrDefault();
							}
							if (chequeMOperationVersion == null)
							{
								throw new NotImplementedException("این چک هیچ وقت به وضعیت واگذار شده نرفته است.".GetTranslate());
							}
							ReOrderItems(lst, item, chequeMOperationVersion);
							break;
						}
						default:
							throw new NotImplementedException();
						case ENUMCheck.Bargashti:
							break;
						}
					}
					finally
					{
						_ = item;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FixWrongData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 548);
				returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "FixWrongData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 549);
			}
			finally
			{
				returnedSaveFuncInfoWithValue.value = lst;
			}
			return returnedSaveFuncInfoWithValue;
		}
	}

	private static ReturnedSaveFuncInfo PreValidation(IEnumerable<ChequeMOperationVersion5> lst)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (lst == null || !lst.Any())
			{
				throw new InvalidOperationException("lst is null or empty!");
			}
			foreach (ChequeMOperationVersion5 item in lst)
			{
				ENUMCheck oldState = item.OldState;
				switch (item.NewState)
				{
				case ENUMCheck.Bargashti:
					if (oldState != ENUMCheck.KharjShode && oldState != ENUMCheck.Mojod && oldState != ENUMCheck.VagozarShode)
					{
						returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + oldState.GetDisplay().GetTranslate() + " \r\n" + "وضعیت جدید".GetTranslate() + ":" + item.NewState.GetDisplay().GetTranslate() + " \r\n" + $"ChequeGuid :{item.ChequeGuid} \r\n" + "ایجاد حالت خطا در PreValidation عملیات چک اشخاص".GetTranslate(), "", "PreValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 570);
					}
					break;
				case ENUMCheck.VagozarShode:
					switch (oldState)
					{
					case ENUMCheck.VagozarShode:
						item.Guid = Guid.Empty;
						break;
					default:
						returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + (oldState.GetDisplay() ?? "").GetTranslate() + "\r\n" + "وضعیت جدید".GetTranslate() + ":" + item.NewState.GetDisplay().GetTranslate() + " \r\n" + $"ChequeGuid :{item.ChequeGuid} \r\n" + "ایجاد حالت خطا در PreValidation عملیات چک اشخاص".GetTranslate(), "", "PreValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 585);
						break;
					case ENUMCheck.Mojod:
					case ENUMCheck.Bargashti:
					case ENUMCheck.VosolShode:
						break;
					}
					break;
				case ENUMCheck.NaghdShode:
					if (oldState != ENUMCheck.Mojod)
					{
						returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + (oldState.GetDisplay() ?? "").GetTranslate() + "\r\n" + "وضعیت جدید".GetTranslate() + ":" + item.NewState.GetDisplay().GetTranslate() + " \r\n" + $"ChequeGuid :{item.ChequeGuid} \r\n" + "ایجاد حالت خطا در PreValidation عملیات چک اشخاص".GetTranslate(), "", "PreValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 592);
					}
					break;
				case ENUMCheck.VosolShode:
					switch (oldState)
					{
					case ENUMCheck.VosolShode:
						item.Guid = Guid.Empty;
						break;
					default:
						returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + (oldState.GetDisplay() ?? "").GetTranslate() + "\r\n" + "وضعیت جدید".GetTranslate() + ":" + item.NewState.GetDisplay().GetTranslate() + " \r\n" + $"ChequeGuid :{item.ChequeGuid} \r\n" + "ایجاد حالت خطا در PreValidation عملیات چک اشخاص".GetTranslate(), "", "PreValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 604);
						break;
					case ENUMCheck.Mojod:
					case ENUMCheck.VagozarShode:
						break;
					}
					break;
				default:
					throw new NotImplementedException();
				case ENUMCheck.Mojod:
				case ENUMCheck.KharjShode:
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PreValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 617);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "PreValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 618);
		}
		return returnedSaveFuncInfo;
	}

	private static async Task<ReturnedSaveFuncInfo> BargashtBatelShodeAsync(string contextGuid, ChequeMOperationVersion5 kharjItem, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReceptionCheckBusiness cheque = await ReceptionCheckBusiness.GetAsync(contextGuid, kharjItem.ChequeGuid, token, tranName);
			if (cheque == null || (kharjItem?.TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return res;
			}
			Guid debitTafsilGuid = ((!cheque.Aval2re) ? ((await ReceptionBusiness.GetAsync(contextGuid, cheque.MasterGuid.Value, EnSystem.Transfer, null, fromDatabase: true, token, tranName))?.TafsilGuid ?? Guid.Empty) : (cheque.A2re_PardazandeTafsilGuid ?? Guid.Empty));
			DateTime? date = await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetSanadBargashtDateAsync(cheque.Price, debitTafsilGuid, kharjItem.TafsilGuid.Value, kharjItem.SanadNo, token, tranName);
			if (!date.HasValue)
			{
				date = kharjItem.DateM.AddSeconds(1.0);
			}
			if (kharjItem.CashierGuid == Guid.Empty)
			{
				kharjItem.CashierGuid = (await CashierBusiness.GetAllAsync(contextGuid, true, token, tranName)).FirstOrDefault((CashierBusiness q) => q.Status).Guid;
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				CashierGuid = kharjItem.CashierGuid,
				ChequeGuid = kharjItem.ChequeGuid,
				DateM = date.Value,
				DebitTafsilGuid = kharjItem.TafsilGuid,
				Description = kharjItem.Description,
				Guid = kharjItem.Guid,
				IsAvalDoreTransfered = kharjItem.AvalDoreTransfer,
				SanadNo = 0,
				System = EnSystem.Transfer,
				OldState = kharjItem.OldState,
				Ordinal = kharjItem.Ordinal
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await OdatKharjShodeAsync(contextGuid, config, null, token, tranName), ReturnedState.None, "BargashtBatelShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 659);
			if (res.HasError)
			{
				return res;
			}
			ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
			{
				ChequeGuid = config.ChequeGuid,
				Status = ENUMCheck.BatelShode
			};
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "BargashtBatelShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 667);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BargashtBatelShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 671);
			res.AddReturnedValue(ex, "", "BargashtBatelShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 672);
		}
		return res;
	}

	private async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			string state = "";
			switch (NewState)
			{
			case ENUMCheck.Bargashti:
				state = "برگشت زدن".GetTranslate();
				break;
			case ENUMCheck.KharjShode:
				state = "خرج کردن".GetTranslate();
				break;
			case ENUMCheck.VagozarShode:
				state = "واگذار کردن".GetTranslate();
				break;
			case ENUMCheck.NaghdShode:
				state = "نقد کردن".GetTranslate();
				break;
			case ENUMCheck.BatelShode:
				state = "ابطال".GetTranslate();
				break;
			case ENUMCheck.VosolShode:
				state = "وصول کردن".GetTranslate();
				break;
			}
			ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(ContextGuid, GuidCheque, token, tranName);
			SanadBusiness sanadBusiness = null;
			Guid oldGuid = Guid.NewGuid();
			if (SanadNO > 0)
			{
				sanadBusiness = await SanadBusiness.GetAsync(ContextGuid, SanadNO, token, tranName);
				if (sanadBusiness != null)
				{
					oldGuid = sanadBusiness.Guid;
				}
			}
			SanadBusiness sanad = new SanadBusiness(ContextGuid, sanadBusiness?.SanadNo ?? 0)
			{
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = ContextGuid,
				Guid = oldGuid,
				Id = 0,
				DateM = DateM,
				IsTemp = true,
				Modified = DateTime.Now,
				SanadNo = SanadNO,
				SanadSt = SanadSt,
				SanadType = SanadType.Automatic,
				Source = ClsCache.GetCache(ContextGuid).Source,
				Status = true,
				UserGuid = CashierGuid,
				Description = SanadDesc
			};
			string description = state + " چک (" + receptionCheque?.Number + ") " + receptionCheque?.BankName + " سررسید " + receptionCheque?.DateSh;
			if (string.IsNullOrEmpty(sanad.Description))
			{
				sanad.Description = description;
			}
			Sanad_DetBusiness debit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.CheckSanad, sanad.NextOrdinal())
			{
				Credit = 0m,
				Debit = (receptionCheque?.Price ?? 0m),
				RowDesc = sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad.Guid,
				Modified = DateTime.Now,
				Serial = receptionCheque?.Number,
				DateSarresid = receptionCheque?.Date
			};
			await debit.SetMoeinGuidAsync(DebitMoeinGuid, token, tranName);
			await debit.SetTafsilGuidAsync(IsDebitTafsilAffective.Value ? DebitTafsilGuid : null, tranName, token);
			await sanad.AddToListSanadAsync(debit, system, token);
			Sanad_DetBusiness credit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.CheckSanad, sanad.NextOrdinal())
			{
				Credit = (receptionCheque?.Price ?? 0m),
				Debit = 0m,
				RowDesc = sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad.Guid,
				Modified = DateTime.Now,
				Serial = receptionCheque?.Number,
				DateSarresid = receptionCheque?.Date
			};
			await credit.SetMoeinGuidAsync(CreditMoeinGuid, token, tranName);
			await credit.SetTafsilGuidAsync(IsCreditTafsilAffective.Value ? CreditTafsilGuid : null, tranName, token);
			await sanad.AddToListSanadAsync(credit, system, token);
			return sanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 755);
			throw;
		}
	}

	public static Task<short> GetNextOrdinalAsync(string contextGuid, Guid chequeGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.GetNextOrdinalAsync(chequeGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			ENUMCheck value = OldState.Value;
			if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.SignalR)
			{
				if (IsDebitTafsilAffective.GetValueOrDefault() && (DebitTafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					throw new InvalidOperationException($"NewState({NewState}) debitTafsilGuid is null or empty!");
				}
				if (DebitMoeinGuid == Guid.Empty)
				{
					throw new InvalidOperationException($"NewState({NewState}) DebitMoeinGuid is empty!");
				}
				if (IsCreditTafsilAffective.GetValueOrDefault() && (CreditTafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					throw new InvalidOperationException($"NewState({NewState}) CreditTafsilGuid is null or empty!");
				}
				if (CreditMoeinGuid == Guid.Empty)
				{
					throw new InvalidOperationException($"NewState({NewState}) CreditMoeinGuid is empty!");
				}
			}
			switch (NewState)
			{
			case ENUMCheck.Bargashti:
				if (value == ENUMCheck.KharjShode || value == ENUMCheck.Mojod || value == ENUMCheck.VagozarShode)
				{
					return Task.FromResult(returnedSaveFuncInfo);
				}
				returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + value.GetDisplay().GetTranslate() + " \r\n" + "وضعیت جدید".GetTranslate() + ":" + NewState.GetDisplay().GetTranslate() + " \r\n" + "ایجاد حالت خطا در عملیات چک اشخاص".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 782);
				break;
			case ENUMCheck.VagozarShode:
				if (value == ENUMCheck.Bargashti || value == ENUMCheck.Mojod || value == ENUMCheck.VosolShode)
				{
					return Task.FromResult(returnedSaveFuncInfo);
				}
				returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + value.GetDisplay().GetTranslate() + " \r\n" + "وضعیت جدید".GetTranslate() + ":" + NewState.GetDisplay().GetTranslate() + " \r\n" + "ایجاد حالت خطا در عملیات چک اشخاص".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 789);
				break;
			case ENUMCheck.KharjShode:
			case ENUMCheck.NaghdShode:
				if (value == ENUMCheck.Mojod || value == ENUMCheck.Bargashti)
				{
					return Task.FromResult(returnedSaveFuncInfo);
				}
				returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + value.GetDisplay().GetTranslate() + " \r\n" + "وضعیت جدید".GetTranslate() + ":" + NewState.GetDisplay().GetTranslate() + " \r\n" + "ایجاد حالت خطا در عملیات چک اشخاص".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 796);
				break;
			case ENUMCheck.VosolShode:
				if (value == ENUMCheck.VagozarShode)
				{
					return Task.FromResult(returnedSaveFuncInfo);
				}
				returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + value.GetDisplay().GetTranslate() + " \r\n" + "وضعیت جدید".GetTranslate() + ":" + NewState.GetDisplay().GetTranslate() + " \r\n" + "ایجاد حالت خطا در عملیات چک اشخاص".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 802);
				break;
			case ENUMCheck.OdatShode:
				if (value == ENUMCheck.KharjShode || value == ENUMCheck.Mojod)
				{
					return Task.FromResult(returnedSaveFuncInfo);
				}
				returnedSaveFuncInfo.AddError("وضعیت قبلی".GetTranslate() + ":" + value.GetDisplay().GetTranslate() + " \r\n" + "وضعیت جدید".GetTranslate() + ":" + NewState.GetDisplay().GetTranslate() + " \r\n" + "ایجاد حالت خطا در عملیات چک اشخاص".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 812);
				break;
			default:
				throw new NotImplementedException();
			case ENUMCheck.Mojod:
			case ENUMCheck.BatelShode:
			case ENUMCheck.Entezami_Mojod:
			case ENUMCheck.Entezami_Odat:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 821);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 822);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> VagozarKardanAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 10;
			checked
			{
				try
				{
					int? ordinal = config.Ordinal;
					ordinal.GetValueOrDefault();
					if (!ordinal.HasValue)
					{
						int value = 1;
						ordinal = value;
					}
					ENUMCheck? oldState = config.OldState;
					oldState.GetValueOrDefault();
					if (!oldState.HasValue)
					{
						ENUMCheck value2 = ENUMCheck.Mojod;
						oldState = value2;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 842), ReturnedState.None, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 842);
					}
					transactionHasBeenCreated = true;
					if (!config.OldState.HasValue)
					{
						ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
						if (chequeMOperationBusiness != null && config.System != EnSystem.Transfer)
						{
							ordinal = chequeMOperationBusiness.Ordinal + 1;
							oldState = chequeMOperationBusiness.NewState;
						}
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = config.IsAvalDoreTransfered,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29976,
						CreditTafsilGuid = (receptionCheque?.DebitTafsilGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception),
						DateSabt = config.DateM,
						DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29978,
						DebitTafsilGuid = config.DebitTafsilGuid,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = false,
						Modified = DateTime.Now,
						NewState = ENUMCheck.VagozarShode,
						OldState = oldState,
						Ordinal = (short)ordinal.Value,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						IsSanadAffective = config.IsSanadAffective,
						StatusId = config.SanadStatusId,
						SanadDesc = config.SanadDesc,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "واگذار کردن چک".GetTranslate() + "(" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + "سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 889);
					if (res.HasError)
					{
						result = res;
						goto IL_0de3;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 893), ReturnedState.None, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 893);
					ViewChequeMForceUpdateConfig forceUpdateConfig = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.VagozarShode
					};
					if (oldState.GetValueOrDefault() == ENUMCheck.VosolShode)
					{
						forceUpdateConfig.Place = (await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, op.CreditTafsilGuid ?? Guid.Empty, tranName, token, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs"))?.Name ?? "";
						forceUpdateConfig.DateToPlace = op.DateM;
					}
					else
					{
						forceUpdateConfig.Place = (await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, config.DebitTafsilGuid ?? Guid.Empty, tranName, token, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs"))?.Name ?? "";
						forceUpdateConfig.DateToPlace = op.DateM;
					}
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(forceUpdateConfig, token, tranName), ReturnedState.None, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 911);
					if (res.HasError || !op.IsSanadAffective)
					{
						result = res;
						goto IL_0de3;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 913);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 917);
					res.AddReturnedValue(ex, "", "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 918);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 923), ReturnedState.None, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 923);
			}
		}
		return res;
		IL_0de3:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> VagozarToMojodAsync(string contextGuid, ChequeMOperationConfig config, ChequeMOperationBusiness oldOperation = null, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				try
				{
					int ordinal = 1;
					ENUMCheck oldState = ENUMCheck.Mojod;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 941), ReturnedState.None, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 941);
					}
					transactionHasBeenCreated = true;
					if (oldOperation == null)
					{
						oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, ENUMCheck.VagozarShode, token, tranName);
					}
					if (oldOperation != null)
					{
						ordinal = oldOperation.Ordinal + 1;
						oldState = oldOperation.NewState;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					ViewChequeMForceUpdateConfig forceUpdateConfig = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Mojod
					};
					if (config.System != EnSystem.Transfer && config.System != EnSystem.TransferMaliYear)
					{
						Guid guid = receptionCheque?.DebitTafsilGuid ?? Guid.Empty;
						if (guid != Guid.Empty && guid == ParentDefaults.TafsilCoding.CLSTafsil29766)
						{
							EnSystem system = config.System;
							string tranName2 = tranName;
							IEnumerable<SandoghBusiness> enumerable = await SandoghBusiness.GetAllAsync(contextGuid, system, "", true, tranName2, token);
							if (enumerable == null || !enumerable.Any())
							{
								result = res;
								goto IL_0c42;
							}
							receptionCheque.DebitTafsilGuid = enumerable.FirstOrDefault().Guid;
							forceUpdateConfig.DebitTafsilGuid = receptionCheque.DebitTafsilGuid;
						}
					}
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = oldOperation.Avale2re_Transfer,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = oldOperation.DebitMoeinGuid,
						CreditTafsilGuid = oldOperation.DebitTafsilGuid,
						DateSabt = config.DateM,
						DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29976,
						DebitTafsilGuid = (receptionCheque.DebitTafsilGuid ?? ParentDefaults.TafsilCoding.CLSTafsil29766),
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = false,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Mojod,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						StatusId = config.SanadStatusId,
						SanadDesc = config.SanadDesc,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "تبدیل چک واگذار شده به موجود چک".GetTranslate() + "(" + receptionCheque?.Number + ") " + (" " + receptionCheque?.BankName).GetTranslate() + " " + " سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1000);
					if (res.HasError)
					{
						result = res;
						goto IL_0c42;
					}
					SanadBusiness sanad = returnedSaveFuncInfoWithValue2.value;
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1003), ReturnedState.None, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1003);
					if (res.HasError)
					{
						result = res;
						goto IL_0c42;
					}
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(forceUpdateConfig, token, tranName), ReturnedState.None, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1005);
					if (res.HasError)
					{
						result = res;
						goto IL_0c42;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid2 = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid2, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1007);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1011);
					res.AddReturnedValue(ex, "", "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1012);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1017), ReturnedState.None, "VagozarToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1017);
			}
		}
		return res;
		IL_0c42:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> NaghdKardanAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				try
				{
					int? ordinal = config.Ordinal;
					ordinal.GetValueOrDefault();
					if (!ordinal.HasValue)
					{
						int value = 1;
						ordinal = value;
					}
					ENUMCheck? oldState = config.OldState;
					oldState.GetValueOrDefault();
					if (!oldState.HasValue)
					{
						ENUMCheck value2 = ENUMCheck.Mojod;
						oldState = value2;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1037), ReturnedState.None, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1037);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (chequeMOperationBusiness != null && config.System != EnSystem.Transfer)
					{
						ordinal = chequeMOperationBusiness.Ordinal + 1;
						oldState = chequeMOperationBusiness.NewState;
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29976,
						CreditTafsilGuid = config.DebitTafsilGuid,
						DateSabt = config.DateM,
						DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein7,
						DebitTafsilGuid = config.DebitTafsilGuid,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = false,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.NaghdShode,
						OldState = oldState,
						Ordinal = (short)ordinal.Value,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = receptionCheckBusiness.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "نقد کردن چک".GetTranslate() + "(" + receptionCheckBusiness?.Number + ") " + (" " + receptionCheckBusiness?.BankName).GetTranslate() + " " + "سررسید".GetTranslate() + " " + receptionCheckBusiness?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1079);
					if (res.HasError)
					{
						result = res;
						goto IL_0c07;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1083), ReturnedState.None, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1083);
					ViewChequeMForceUpdateConfig forceUpdateConfig = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.NaghdShode
					};
					if ((op?.DebitTafsilGuid ?? Guid.Empty) != Guid.Empty)
					{
						forceUpdateConfig.Place = (await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, op.DebitTafsilGuid.Value, tranName, token, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs"))?.Name ?? "";
						forceUpdateConfig.DateToPlace = op.DateM;
					}
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(forceUpdateConfig, token, tranName), ReturnedState.None, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1095);
					if (res.HasError)
					{
						result = res;
						goto IL_0c07;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1097);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1101);
					res.AddReturnedValue(ex, "", "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1102);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1107), ReturnedState.None, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1107);
			}
		}
		return res;
		IL_0c07:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> BargashtZadanAsync(string contextGuid, ChequeMOperationConfig config, ChequeMOperationBusiness oldOperation = null, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 11;
			checked
			{
				try
				{
					int? ordinal = config.Ordinal;
					ordinal.GetValueOrDefault();
					if (!ordinal.HasValue)
					{
						int value = 1;
						ordinal = value;
					}
					ENUMCheck? oldState = config.OldState;
					oldState.GetValueOrDefault();
					if (!oldState.HasValue)
					{
						ENUMCheck value2 = ENUMCheck.Mojod;
						oldState = value2;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1127), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1127);
					}
					transactionHasBeenCreated = true;
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (oldOperation == null)
					{
						oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					}
					if (oldOperation != null && config.System != EnSystem.Transfer)
					{
						if (oldOperation.NewState == ENUMCheck.Mojod || oldOperation.NewState == ENUMCheck.VagozarShode)
						{
							if (oldOperation != null)
							{
								ordinal = oldOperation.Ordinal + 1;
								oldState = oldOperation.NewState;
							}
							else if (oldOperation.NewState != ENUMCheck.Mojod)
							{
								throw new NotImplementedException();
							}
						}
						else
						{
							if (oldOperation.NewState != ENUMCheck.KharjShode)
							{
								throw new NotImplementedException();
							}
							ordinal = (oldOperation?.Ordinal ?? 0) + 1;
							oldState = ENUMCheck.KharjShode;
						}
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					ReceptionBusiness receptionBusiness = await receptionCheque.GetReceptionMasterAsync(config.System, token, tranName);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsDebitTafsilAffective = true,
						IsCreditTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Bargashti,
						OldState = oldState,
						Ordinal = (short)ordinal.Value,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						DebitMoeinGuid = (receptionCheque.Aval2re ? receptionCheque.A2re_PardazandeMoeinGuid.Value : (receptionBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9)),
						DebitTafsilGuid = (receptionCheque.Aval2re ? new Guid?(receptionCheque.A2re_PardazandeTafsilGuid.Value) : receptionBusiness?.TafsilGuid),
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						IsSanadAffective = true,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "برگشت زدن چک".GetTranslate() + "(" + receptionCheque?.Number + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + "سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					if (oldState.GetValueOrDefault() == ENUMCheck.Mojod)
					{
						AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
						op.CreditMoeinGuid = accGlobalSettings.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception;
						op.CreditTafsilGuid = receptionCheque?.DebitTafsilGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception;
					}
					else if (oldState.GetValueOrDefault() == ENUMCheck.VagozarShode)
					{
						op.CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29978;
						op.CreditTafsilGuid = oldOperation.DebitTafsilGuid;
					}
					else
					{
						if (oldState.GetValueOrDefault() != ENUMCheck.KharjShode)
						{
							throw new NotImplementedException();
						}
						if (!(oldOperation?.Avale2re_Transfer).GetValueOrDefault())
						{
							PardakhtCheckMBusiness pardakhtCheckMBusiness = (await PardakhtCheckMBusiness.GetAllAsync(contextGuid, config.ChequeGuid, token, tranName))?.FirstOrDefault();
							if (pardakhtCheckMBusiness != null)
							{
								PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheckMBusiness.MasterGuid, fromDatabase: true, token, tranName);
								op.CreditMoeinGuid = pardakhtBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
								op.CreditTafsilGuid = pardakhtBusiness?.TafsilGuid;
							}
							else
							{
								op.CreditMoeinGuid = oldOperation.DebitMoeinGuid;
								op.CreditTafsilGuid = oldOperation.DebitTafsilGuid;
							}
						}
						else
						{
							op.CreditMoeinGuid = oldOperation.DebitMoeinGuid;
							op.CreditTafsilGuid = oldOperation.DebitTafsilGuid;
						}
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1218);
					if (res.HasError)
					{
						result = res;
						goto IL_0fa0;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1222), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1222);
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Bargashti
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1228);
					if (res.HasError)
					{
						result = res;
						goto IL_0fa0;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1230);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1234);
					res.AddReturnedValue(ex, "", "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1235);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1240), ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1240);
			}
		}
		return res;
		IL_0fa0:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> VosolKardanAsync(string contextGuid, ChequeMOperationConfig config, ChequeMOperationBusiness oldOperation = null, CancellationToken token = default(CancellationToken), string tranName = "")
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
					int? ordinal = config.Ordinal;
					ordinal.GetValueOrDefault();
					if (!ordinal.HasValue)
					{
						int value = 1;
						ordinal = value;
					}
					ENUMCheck? oldState = config.OldState;
					oldState.GetValueOrDefault();
					if (!oldState.HasValue)
					{
						ENUMCheck value2 = ENUMCheck.VagozarShode;
						oldState = value2;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1260), ReturnedState.None, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1260);
					}
					transactionHasBeenCreated = true;
					if (oldOperation == null)
					{
						oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					}
					if (oldOperation != null && config.System != EnSystem.Transfer)
					{
						ordinal = oldOperation.Ordinal + 1;
						oldState = oldOperation.NewState;
					}
					if (config.SanadNo < 0 || config.System == EnSystem.Transfer)
					{
						config.SanadNo = 0;
					}
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = (oldOperation?.DebitMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein29978),
						CreditTafsilGuid = (oldOperation?.DebitTafsilGuid ?? ParentDefaults.TafsilCoding.CLSTafsil29766),
						DateSabt = config.DateM,
						DebitTafsilGuid = (oldOperation?.DebitTafsilGuid ?? config.DebitTafsilGuid),
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = false,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.VosolShode,
						OldState = oldState,
						Ordinal = (short)ordinal.Value,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = receptionCheckBusiness.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "وصول کردن چک".GetTranslate() + "(" + (receptionCheckBusiness?.Number ?? "").GetTranslate() + ")  " + receptionCheckBusiness?.BankName + "  " + "سررسید".GetTranslate() + " " + receptionCheckBusiness?.DateSh;
					}
					TafsilBusiness bankTafsil = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, op.DebitTafsilGuid ?? Guid.Empty, tranName, token, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs");
					IEnumerable<MoeinTafsilGroupRelationsBusiness> obj = await MoeinTafsilGroupRelationsBusiness.GetAllAsync(contextGuid, null, bankTafsil?.TafsilGroupGuid ?? Guid.Empty, token, tranName);
					Guid? guid = obj?.FirstOrDefault()?.MoeinGuid;
					if ((obj?.Select((MoeinTafsilGroupRelationsBusiness q) => q.MoeinGuid)?.Contains(ParentDefaults.MoeinCoding.CLSMoein4)).GetValueOrDefault() || (guid ?? Guid.Empty) == Guid.Empty)
					{
						guid = ParentDefaults.MoeinCoding.CLSMoein4;
					}
					op.DebitMoeinGuid = guid.Value;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1308);
					if (res.HasError)
					{
						result = res;
						goto IL_0dc8;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1312), ReturnedState.None, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1312);
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.VosolShode,
						DateToPlace = op.DateM,
						Place = bankTafsil?.Name
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1320);
					if (res.HasError)
					{
						result = res;
						goto IL_0dc8;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid2 = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid2, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1322);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1326);
					res.AddReturnedValue(ex, "", "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1327);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1332), ReturnedState.None, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1332);
			}
		}
		return res;
		IL_0dc8:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> EbtalAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "", EnSystem system = EnSystem.UserInterface)
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
			_ = 23;
			checked
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
				try
				{
					int ordinal = 1;
					ENUMCheck oldState = ENUMCheck.Mojod;
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					IEnumerable<ChequeMOperationBusiness> enumerable = await GetAllAsync(contextGuid, new ChequeMOperationFilter
					{
						ChequeGuid = config.ChequeGuid
					}, token);
					ChequeMOperationBusiness op;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (enumerable != null && enumerable.Count() != 0)
					{
						if (config.System != EnSystem.UserInterface)
						{
							throw new InvalidOperationException($"OperationCount:{enumerable?.Count() ?? 0} must be unique");
						}
						res.AddError("به دلیل وجود عملیات دیگر، شما قادر به ابطال چک جاری نمی باشید ".GetTranslate() + "\r\n" + "لطفا از منوی لیست عملیات های چک، عملیات های چک را حذف، و سپس اقدام به ابطال چک نمایید".GetTranslate(), "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1353);
						result = res;
					}
					else
					{
						if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
						{
							config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
						}
						if (autoTran)
						{
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1363), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1363);
						}
						transactionHasBeenCreated = true;
						ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
						if (chequeMOperationBusiness != null)
						{
							ordinal = chequeMOperationBusiness.Ordinal + 1;
							oldState = chequeMOperationBusiness.NewState;
						}
						ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
						if (receptionCheque == null)
						{
							result = res;
						}
						else if (receptionCheque.Aval2re && receptionCheque.IsEnteghali)
						{
							result = res.AddError("کاربرگرامی شما مجاز به ابطال چک ابتدای دوره انتقالی نمی باشید", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1377);
						}
						else
						{
							op = new ChequeMOperationBusiness(contextGuid)
							{
								Avale2re_Transfer = null,
								CashierGuid = config.CashierGuid.Value,
								DateM = config.DateM,
								ContextGuid = contextGuid,
								CreditMoeinGuid = receptionCheque.DebitMoeinGuid,
								CreditTafsilGuid = receptionCheque.DebitTafsilGuid,
								DateSabt = config.DateM,
								Guid = (config.Guid ?? Guid.NewGuid()),
								GuidCheque = config.ChequeGuid,
								IsCreditTafsilAffective = false,
								IsDebitTafsilAffective = true,
								Modified = DateTime.Now,
								NewState = ENUMCheck.BatelShode,
								OldState = oldState,
								Ordinal = (short)ordinal,
								SanadNO = config.SanadNo,
								SanadSt = SanadSt.Movaghat,
								Status = true,
								Description = "",
								SanadDesc = config.SanadDesc,
								StatusId = config.SanadStatusId,
								Price = receptionCheque.Price
							};
							if (string.IsNullOrEmpty(op.SanadDesc))
							{
								op.SanadDesc = "ابطال چک (" + receptionCheque?.Number + ") " + (" " + receptionCheque?.BankName).GetTranslate() + " " + " سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
							}
							if (receptionCheque.Aval2re)
							{
								if (source == ENSource.Accounting && !receptionCheque.IsEnteghali)
								{
									ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
									{
										ChequeGuid = config.ChequeGuid,
										Status = ENUMCheck.BatelShode
									};
									returnedSaveFuncInfoWithValue = res;
									returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1415);
									if (res.HasError)
									{
										result = res;
									}
									else
									{
										SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(contextGuid, receptionCheque.SanadNo.GetValueOrDefault(), token, tranName);
										if (sanadBusiness != null)
										{
											returnedSaveFuncInfoWithValue = res;
											returnedSaveFuncInfoWithValue.AddReturnedValue(await sanadBusiness.RemoveAsync(config.System, 10, tranName, token, null, new Tuple<Document, Guid>(Document.CheckSanad, sanadBusiness.Guid)), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1419);
										}
										result = res;
									}
								}
								else
								{
									if (source != ENSource.Accounting || !receptionCheque.IsEnteghali)
									{
										op.DebitMoeinGuid = receptionCheque.CreditA2reMoeinGuid.Value;
										op.DebitTafsilGuid = receptionCheque.CreditA2reTafsilGuid;
										goto IL_15b6;
									}
									MoeinBusiness moeinCheque = await MoeinBusiness.GetAsync(contextGuid, receptionCheque.DebitMoeinGuid, tranName, token);
									MoeinBusiness moeinSarmaye = await MoeinBusiness.GetAsync(contextGuid, receptionCheque.CreditA2reMoeinGuid.Value, tranName, token);
									List<SanadDetReportBusiness> list = (await SanadBusiness.GetAllGardeshSanadAsync(contextGuid, ENHesabLevel.Moein, codeMoein: moeinCheque.Code, date1: null, date2: null, code1: 0L, code2: 0L, token: default(CancellationToken), kolGuid: null, maxCount: null, maxParalelThreads: 20))?.Where((SanadDetReportBusiness q) => q.AsType == 5000 || q.AsType == 50)?.ToList();
									if ((list?.Count ?? 0) == 1)
									{
										SanadBusiness sanadEftetahiye = await SanadBusiness.GetAsync(contextGuid, list.Single().AsGuid, token, tranName);
										Sanad_DetBusiness sanad_DetBusiness = sanadEftetahiye.ListSanad_Det.Single((Sanad_DetBusiness q) => q.MoeinGuid == moeinCheque.Guid);
										if (sanad_DetBusiness.Credit > 0m)
										{
											throw new InvalidOperationException("در سند افتتاحیه باید حساب".GetTranslate() + " " + (" " + moeinCheque.Name).GetTranslate() + " " + " بدهکار باشد در صورتیکه به میزان ".GetTranslate() + " " + $"{sanad_DetBusiness.Credit:N0}" + " " + " بستانکار است".GetTranslate());
										}
										if (sanad_DetBusiness.Debit < receptionCheque.Price)
										{
											throw new InvalidOperationException("در سند افتتاحیه باید حساب".GetTranslate() + " " + (" " + moeinCheque.Name).GetTranslate() + " " + " میزان مبلغ بدهی".GetTranslate() + " (" + $"{sanad_DetBusiness.Debit:N0}" + ")" + "کمتر از ".GetTranslate() + " " + $"{receptionCheque.Price:N0}" + " می باشد".GetTranslate());
										}
										sanad_DetBusiness.Debit -= receptionCheque.Price;
										Sanad_DetBusiness detSarmaye = sanadEftetahiye.ListSanad_Det.FirstOrDefault((Sanad_DetBusiness q) => q.MoeinGuid == moeinSarmaye.Guid);
										if (detSarmaye == null)
										{
											detSarmaye = new Sanad_DetBusiness(contextGuid, config.System, sanadEftetahiye.Guid, Document.Sanad, sanadEftetahiye.NextOrdinal())
											{
												Debit = 0m,
												RowDesc = "سند افتتاحیه".GetTranslate(),
												Guid = Guid.NewGuid(),
												MasterGuid = sanadEftetahiye.Guid,
												Modified = DateTime.Now
											};
											await detSarmaye.SetMoeinGuidAsync(moeinSarmaye.Guid, token, tranName);
											await detSarmaye.SetTafsilGuidAsync(receptionCheque.CreditA2reTafsilGuid, tranName, token);
											await sanadEftetahiye.AddToListSanadAsync(detSarmaye, config.System, token);
										}
										if (detSarmaye.Debit > 0m && detSarmaye.Debit > receptionCheque.Price)
										{
											detSarmaye.Debit -= receptionCheque.Price;
										}
										else if (detSarmaye.Debit > 0m && detSarmaye.Debit < receptionCheque.Price)
										{
											detSarmaye.Credit = receptionCheque.Price - detSarmaye.Debit;
										}
										else
										{
											detSarmaye.Credit += receptionCheque.Price;
										}
										List<ViewSanadDetailCoverBusiness> lst = await sanadEftetahiye.GetSanadCoverListAsync(config.System);
										returnedSaveFuncInfoWithValue = res;
										returnedSaveFuncInfoWithValue.AddReturnedValue(await sanadEftetahiye.SaveAsync(config.System, token, false, true, tranName, lst, new Tuple<Document, Guid>(Document.Sanad, sanadEftetahiye.Guid)), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1458);
									}
									result = res;
								}
							}
							else
							{
								ReceptionBusiness receptionBusiness = await receptionCheque.GetReceptionMasterAsync(config.System, token, tranName);
								if (source != ENSource.Accounting)
								{
									op.DebitMoeinGuid = receptionBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
									op.DebitTafsilGuid = receptionBusiness?.TafsilGuid;
									goto IL_15b6;
								}
								receptionBusiness.RemoveFromDetList(receptionCheque);
								if (receptionBusiness.Sum == 0m && receptionBusiness.SumBank == 0m && receptionBusiness.SumNaghd == 0m && receptionBusiness.SumCheck == 0m && receptionBusiness.SumEnteghal == 0m)
								{
									returnedSaveFuncInfoWithValue = res;
									returnedSaveFuncInfoWithValue.AddReturnedValue(await receptionBusiness.RemoveAsync(config.System, awaitOnSemaphore: true, 10, token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1472);
								}
								else
								{
									returnedSaveFuncInfoWithValue = res;
									returnedSaveFuncInfoWithValue.AddReturnedValue(await receptionBusiness.SaveAsync(config.System, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, tranName, token), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1474);
								}
								result = res;
							}
						}
					}
					goto end_IL_00d3;
					IL_15b6:
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1482);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00d3;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1486), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1486);
					ViewChequeMForceUpdateConfig config3 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.BatelShode
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config3, token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1492);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00d3;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1494);
					goto end_IL_0067;
					end_IL_00d3:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1498);
					res.AddReturnedValue(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1499);
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
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1504), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1504);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatBargashtiAsync(string contextGuid, ChequeMOperationConfig config, ChequeMOperationBusiness oldOperation = null, CancellationToken token = default(CancellationToken), string tranName = "")
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
					ENUMCheck oldState = ENUMCheck.Mojod;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1522), ReturnedState.None, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1522);
					}
					transactionHasBeenCreated = true;
					if (oldOperation == null)
					{
						oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					}
					if (oldOperation != null)
					{
						ordinal = oldOperation.Ordinal + 1;
						oldState = oldOperation.NewState;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ReceptionBusiness reception = await receptionCheque.GetReceptionMasterAsync(config.System, token, tranName);
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = (reception?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9),
						CreditTafsilGuid = reception?.TafsilGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Mojod,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						IsSanadAffective = config.IsSanadAffective,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "عودت چک برگشتی".GetTranslate() + "(" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (" " + receptionCheque?.BankName).GetTranslate() + " " + " سررسید ".GetTranslate() + " " + receptionCheque?.DateSh;
					}
					switch (oldState)
					{
					case ENUMCheck.Mojod:
					case ENUMCheck.Bargashti:
						op.DebitMoeinGuid = receptionCheque.DebitMoeinGuid;
						op.DebitTafsilGuid = receptionCheque.DebitTafsilGuid;
						if (receptionCheque.Aval2re)
						{
							op.DebitMoeinGuid = accGlobalSettings.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception;
							op.DebitTafsilGuid = accGlobalSettings.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception;
							op.CreditMoeinGuid = receptionCheque.A2re_PardazandeMoeinGuid.Value;
							op.CreditTafsilGuid = receptionCheque.A2re_PardazandeTafsilGuid;
						}
						break;
					case ENUMCheck.KharjShode:
						op.DebitMoeinGuid = reception?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
						op.DebitTafsilGuid = reception?.TafsilGuid;
						break;
					case ENUMCheck.VagozarShode:
						op.DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29978;
						op.DebitTafsilGuid = null;
						break;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1588);
					if (res.HasError)
					{
						result = res;
						goto IL_0d15;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1592), ReturnedState.None, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1592);
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Mojod
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1598);
					if (res.HasError || sanad == null)
					{
						result = res;
						goto IL_0d15;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1600);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1604);
					res.AddReturnedValue(ex, "", "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1605);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1610), ReturnedState.None, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1610);
			}
		}
		return res;
		IL_0d15:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> BargashtiToBeforeStateAsync(string contextGuid, ChequeMOperationConfig config, ChequeMOperationBusiness oldOperation = null, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 11;
			checked
			{
				try
				{
					int ordinal = 1;
					ENUMCheck oldState = ENUMCheck.Mojod;
					ENUMCheck newState = ENUMCheck.Mojod;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1629), ReturnedState.None, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1629);
					}
					transactionHasBeenCreated = true;
					if (oldOperation == null)
					{
						oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					}
					if (oldOperation != null)
					{
						ordinal = oldOperation.Ordinal + 1;
						oldState = oldOperation.NewState;
						newState = oldOperation.OldState.Value;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ReceptionBusiness reception = await receptionCheque.GetReceptionMasterAsync(config.System, token, tranName);
					ViewChequeMForceUpdateConfig forceUpdateConfig = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = newState
					};
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = (reception?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9),
						CreditTafsilGuid = reception?.TafsilGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = newState,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "تبدیل چک برگشتی به حالت قبل".GetTranslate() + " (" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (" " + receptionCheque?.BankName).GetTranslate() + " " + " سررسید".GetTranslate() + " " + (receptionCheque?.DateSh ?? "").GetTranslate();
					}
					switch (newState)
					{
					case ENUMCheck.Mojod:
						op.DebitMoeinGuid = receptionCheque.DebitMoeinGuid;
						op.DebitTafsilGuid = receptionCheque.DebitTafsilGuid;
						if ((op?.DebitTafsilGuid ?? Guid.Empty) == Guid.Empty)
						{
							op.DebitTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil29766;
						}
						if (receptionCheque.Aval2re)
						{
							op.CreditMoeinGuid = receptionCheque.A2re_PardazandeMoeinGuid.Value;
							op.CreditTafsilGuid = receptionCheque.A2re_PardazandeTafsilGuid;
							op.IsCreditTafsilAffective = true;
						}
						break;
					case ENUMCheck.KharjShode:
					{
						if (receptionCheque.Aval2re)
						{
							if (oldState == ENUMCheck.Mojod)
							{
								op.CreditMoeinGuid = receptionCheque.A2re_PardazandeMoeinGuid.Value;
								op.CreditTafsilGuid = receptionCheque.A2re_PardazandeTafsilGuid;
								op.DebitMoeinGuid = receptionCheque.CreditA2reMoeinGuid.Value;
								op.DebitTafsilGuid = receptionCheque.CreditA2reTafsilGuid;
								op.IsCreditTafsilAffective = true;
								op.IsDebitTafsilAffective = false;
							}
							else
							{
								op.CreditMoeinGuid = oldOperation.DebitMoeinGuid;
								op.CreditTafsilGuid = oldOperation.DebitTafsilGuid;
								op.DebitMoeinGuid = oldOperation.CreditMoeinGuid;
								op.DebitTafsilGuid = oldOperation.CreditTafsilGuid;
							}
						}
						else
						{
							if (config.System == EnSystem.UserInterface)
							{
								PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, (await PardakhtCheckMBusiness.GetAllAsync(contextGuid, config.ChequeGuid, token, tranName)).FirstOrDefault().MasterGuid, fromDatabase: true, token, tranName);
								op.DebitMoeinGuid = pardakhtBusiness.MoeinGuid;
								op.DebitTafsilGuid = pardakhtBusiness.TafsilGuid;
							}
							op.CreditMoeinGuid = reception?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
							op.CreditTafsilGuid = reception?.TafsilGuid;
						}
						TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, op.DebitTafsilGuid ?? Guid.Empty, tranName, token, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs");
						forceUpdateConfig.DateToPlace = op.DateM;
						forceUpdateConfig.Place = tafsilBusiness?.Name;
						break;
					}
					case ENUMCheck.VagozarShode:
						op.IsDebitTafsilAffective = false;
						op.DebitMoeinGuid = oldOperation?.CreditMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein29978;
						op.DebitTafsilGuid = oldOperation?.CreditTafsilGuid;
						if (receptionCheque.Aval2re)
						{
							op.CreditMoeinGuid = receptionCheque.A2re_PardazandeMoeinGuid.Value;
							op.CreditTafsilGuid = receptionCheque.A2re_PardazandeTafsilGuid;
							op.IsCreditTafsilAffective = true;
						}
						break;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1741);
					if (res.HasError)
					{
						result = res;
						goto IL_10a4;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1745), ReturnedState.None, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1745);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(forceUpdateConfig, token, tranName), ReturnedState.None, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1746);
					if (res.HasError)
					{
						result = res;
						goto IL_10a4;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1748);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1752);
					res.AddReturnedValue(ex, "", "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1753);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1758), ReturnedState.None, "BargashtiToBeforeStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1758);
			}
		}
		return res;
		IL_10a4:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> NaghdToMojodAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> result;
				try
				{
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1776), ReturnedState.None, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1776);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (oldOperation == null)
					{
						ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
						{
							ChequeGuid = config.ChequeGuid,
							Status = ENUMCheck.Mojod
						};
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1792);
						result = res;
						goto end_IL_0095;
					}
					int ordinal = oldOperation.Ordinal + 1;
					ENUMCheck oldState = oldOperation.NewState;
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = false,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Mojod,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						CreditMoeinGuid = oldOperation.DebitMoeinGuid,
						CreditTafsilGuid = oldOperation.DebitTafsilGuid,
						DebitMoeinGuid = receptionCheckBusiness.DebitMoeinGuid,
						DebitTafsilGuid = receptionCheckBusiness.DebitTafsilGuid,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = receptionCheckBusiness.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "تبدیل چک نقد شده به موجود چک ".GetTranslate() + "(" + (receptionCheckBusiness?.Number ?? "").GetTranslate() + ") " + (receptionCheckBusiness?.BankName ?? "").GetTranslate() + " " + " سررسید".GetTranslate() + "  " + receptionCheckBusiness?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1828);
					if (res.HasError)
					{
						result = res;
						goto end_IL_0095;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1832), ReturnedState.None, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1832);
					ViewChequeMForceUpdateConfig config3 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Mojod
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config3, token, tranName), ReturnedState.None, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1838);
					if (res.HasError)
					{
						result = res;
						goto end_IL_0095;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1840);
					goto end_IL_0066;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1844);
					res.AddReturnedValue(ex, "", "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1845);
					goto end_IL_0066;
				}
				return result;
			}
			end_IL_0066:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1850), ReturnedState.None, "NaghdToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1850);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> BatelToMojodAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
					int ordinal = 1;
					ENUMCheck oldState = ENUMCheck.BatelShode;
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1869), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1869);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (chequeMOperationBusiness != null)
					{
						ordinal = chequeMOperationBusiness.Ordinal + 1;
						oldState = chequeMOperationBusiness.NewState;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Mojod,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						DebitMoeinGuid = receptionCheque.DebitMoeinGuid,
						DebitTafsilGuid = receptionCheque.DebitTafsilGuid,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "تبدیل چک ابطال شده به موجود چک".GetTranslate() + "(" + receptionCheque?.Number + ") " + (" " + receptionCheque?.BankName).GetTranslate() + " " + " سررسید ".GetTranslate() + " " + receptionCheque?.DateSh;
					}
					if (receptionCheque.Aval2re)
					{
						if (source != ENSource.Accounting)
						{
							op.CreditMoeinGuid = receptionCheque.CreditA2reMoeinGuid.Value;
							op.CreditTafsilGuid = receptionCheque.CreditA2reTafsilGuid;
							goto IL_0be2;
						}
						ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
						{
							ChequeGuid = config.ChequeGuid,
							Status = ENUMCheck.Mojod
						};
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1917);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							op.CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15;
							ReceptionCheckBusiness receptionCheckBusiness = receptionCheque;
							receptionCheckBusiness.SanadNo = await SanadBusiness.NextIdAsync(contextGuid, token, tranName);
							SanadBusiness newSanad = await receptionCheque.GenerateSanadA2reAsync(config.System, token, tranName);
							List<ViewSanadDetailCoverBusiness> lst = await op.GetSanadCoverListAsync(config.System, token, tranName);
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await newSanad.SaveAsync(config.System, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, receptionCheque.Guid)), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1923);
							result = res;
						}
					}
					else
					{
						ReceptionBusiness receptionBusiness = await receptionCheque.GetReceptionMasterAsync(config.System, token, tranName);
						if (source != ENSource.Accounting)
						{
							op.CreditMoeinGuid = receptionBusiness?.MoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein9;
							op.CreditTafsilGuid = receptionBusiness?.TafsilGuid;
							goto IL_0be2;
						}
						ReceptionCheckBusiness receptionCheckBusiness2 = receptionBusiness?.ListCheck?.FirstOrDefault((ReceptionCheckBusiness q) => q.Guid == receptionCheque.Guid);
						if (receptionCheckBusiness2 != null)
						{
							receptionCheckBusiness2.Status = ENUMCheck.Mojod;
						}
						else
						{
							receptionCheque.Status = ENUMCheck.Mojod;
							receptionBusiness.AddToDetList(receptionCheque, config.System, token);
						}
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await receptionBusiness.SaveAsync(config.System, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, tranName, token), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1941);
						result = res;
					}
					goto end_IL_00af;
					IL_0be2:
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1949);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00af;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1953), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1953);
					ViewChequeMForceUpdateConfig config3 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Mojod
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config3, token, tranName), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1959);
					if (res.HasError)
					{
						result = res;
						goto end_IL_00af;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1961);
					goto end_IL_0067;
					end_IL_00af:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1965);
					res.AddReturnedValue(ex, "", "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1966);
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
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1971), ReturnedState.None, "BatelToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1971);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> VosolToVagozarAsync(string contextGuid, ChequeMOperationConfig config, short autoFixTryCount = 0, CancellationToken token = default(CancellationToken), string tranName = "")
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
					ENUMCheck oldState = ENUMCheck.VosolShode;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1989), ReturnedState.None, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 1989);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (oldOperation != null)
					{
						ordinal = oldOperation.Ordinal + 1;
						oldState = oldOperation.NewState;
					}
					ChequeMOperationBusiness bankVagozarOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, null, ENUMCheck.VagozarShode, token, tranName);
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (bankVagozarOperation == null)
					{
						throw new NullReferenceException("bankVagozarOperation is null!!\r\nconfig:" + Json.ToStringJson(config));
					}
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = false,
						Modified = DateTime.Now,
						NewState = ENUMCheck.VagozarShode,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						DebitMoeinGuid = oldOperation.CreditMoeinGuid,
						DebitTafsilGuid = bankVagozarOperation.DebitTafsilGuid,
						CreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein4,
						CreditTafsilGuid = bankVagozarOperation.DebitTafsilGuid,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = receptionCheckBusiness.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "تبدیل چک وصول شده به واگذار شده چک".GetTranslate() + "(" + (receptionCheckBusiness?.Number ?? "").GetTranslate() + ")  " + receptionCheckBusiness?.BankName + " " + " سررسید".GetTranslate() + "  " + receptionCheckBusiness?.DateSh;
					}
					if (autoFixTryCount > 0)
					{
						op.CreditTafsilGuid = bankVagozarOperation.CreditTafsilGuid;
					}
					EnSystem system = config.System;
					string tranName2 = tranName;
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(system, sendToServer: true, default(CancellationToken), awaitOnSemaphore: true, 10, tranName2);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2036);
					if (res.HasError)
					{
						result = res;
						goto IL_0c2d;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2040), ReturnedState.None, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2040);
					TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, op.CreditTafsilGuid ?? Guid.Empty, tranName, token, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs");
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.VagozarShode,
						Place = (tafsilBusiness?.Name ?? ""),
						DateToPlace = op.DateM
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2049);
					if (res.HasError)
					{
						result = res;
						goto IL_0c2d;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2051);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2055);
					res.AddReturnedValue(ex, "", "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2056);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2061), ReturnedState.None, "VosolToVagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2061);
			}
		}
		return res;
		IL_0c2d:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> KharjKardanAsync(string contextGuid, ChequeMOperationConfig config, Guid? pardakhtGuid, Guid debitMoeinGuid, Guid? debitTafsilGuid, short autoFixTryCount = 0, CancellationToken token = default(CancellationToken), string tranName = "", bool foreUpdate = false)
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
					int? ordinal = config.Ordinal;
					ordinal.GetValueOrDefault();
					if (!ordinal.HasValue)
					{
						int value = 1;
						ordinal = value;
					}
					ENUMCheck? oldState = config.OldState;
					oldState.GetValueOrDefault();
					if (!oldState.HasValue)
					{
						ENUMCheck value2 = ENUMCheck.Mojod;
						oldState = value2;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2081), ReturnedState.None, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2081);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if ((chequeMOperationBusiness?.NewState ?? ENUMCheck.Mojod) == ENUMCheck.KharjShode && !foreUpdate)
					{
						result = res;
						goto IL_0b6e;
					}
					if (chequeMOperationBusiness != null && config.System != EnSystem.Transfer && (config?.Ordinal).GetValueOrDefault() <= 0)
					{
						ordinal = chequeMOperationBusiness.Ordinal + 1;
						oldState = chequeMOperationBusiness.NewState;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = config.IsAvalDoreTransfered,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = false,
						IsDebitTafsilAffective = false,
						Modified = DateTime.Now,
						NewState = ENUMCheck.KharjShode,
						OldState = oldState,
						Ordinal = (short)ordinal.Value,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						DebitMoeinGuid = debitMoeinGuid,
						DebitTafsilGuid = debitTafsilGuid,
						CreditMoeinGuid = (receptionCheque?.DebitMoeinGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception),
						CreditTafsilGuid = (receptionCheque?.DebitTafsilGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception),
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						AsGuid = pardakhtGuid,
						AsType = Document.Pardakht,
						IsSanadAffective = false,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "خرج چک دریافتنی".GetTranslate() + "(" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + " سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2129);
					if (res.HasError)
					{
						result = res;
						goto IL_0b6e;
					}
					SanadBusiness value3 = returnedSaveFuncInfoWithValue2.value;
					res.value = value3;
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2134), ReturnedState.None, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2134);
					TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, op.DebitTafsilGuid ?? Guid.Empty, tranName, token, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs");
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.KharjShode,
						DateToPlace = op.DateM,
						Place = tafsilBusiness?.Name
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2144);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2148);
					res.AddReturnedValue(ex, "", "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2149);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2154), ReturnedState.None, "KharjKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2154);
			}
		}
		return res;
		IL_0b6e:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatKharjShodeAsync(string contextGuid, ChequeMOperationConfig config, ChequeMOperationBusiness oldOperation = null, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 14;
			checked
			{
				try
				{
					int ordinal = 1;
					ENUMCheck oldState = ENUMCheck.Mojod;
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2173), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2173);
					}
					transactionHasBeenCreated = true;
					if (oldOperation == null)
					{
						oldOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					}
					if (oldOperation != null)
					{
						ordinal = oldOperation.Ordinal + 1;
						oldState = oldOperation.NewState;
					}
					PardakhtCheckMBusiness pardakhtCheque = (await PardakhtCheckMBusiness.GetAllAsync(contextGuid, config.ChequeGuid, token, tranName))?.FirstOrDefault();
					PardakhtBusiness pardakht = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque?.MasterGuid ?? Guid.Empty, fromDatabase: true, token, tranName);
					if (pardakht == null)
					{
						if (!(oldOperation?.Avale2re_Transfer).GetValueOrDefault())
						{
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await oldOperation.RemoveAsync(config.System, isRemoveOldSanad: false, ENUMCheck.None, 10, token, tranName), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2189);
						}
						result = res;
						goto IL_10f1;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = pardakht.MoeinGuid,
						CreditTafsilGuid = pardakht.TafsilGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Mojod,
						OldState = oldState,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						DebitMoeinGuid = (receptionCheque?.DebitMoeinGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception),
						DebitTafsilGuid = (receptionCheque?.DebitTafsilGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception),
						SanadDesc = config.SanadDesc,
						IsSanadAffective = false,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "عودت چک خرج شده".GetTranslate() + "(" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (" " + receptionCheque?.BankName).GetTranslate() + " " + "سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					if (source == ENSource.Accounting)
					{
						pardakht.RemoveFromDetList(pardakhtCheque);
						if (pardakht.Sum == 0m && pardakht.SumBank == 0m && pardakht.SumNaghd == 0m && pardakht.SumCheckM == 0m && pardakht.SumCheckSh == 0m && pardakht.SumEnteghal == 0m)
						{
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await pardakht.RemoveAsync(config.System, 10, token, tranName), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2231);
							if (res.HasError)
							{
								result = res;
								goto IL_10f1;
							}
							receptionCheque.Status = ENUMCheck.Mojod;
							receptionCheque.ServerStatus = ServerStatusForClient.None;
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await receptionCheque.SaveAsync(config.System, 10, token, tranName), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2235);
						}
						else
						{
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await pardakht.SaveAsync(config.System, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2238);
						}
						result = res;
						goto IL_10f1;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2243);
					if (res.HasError)
					{
						result = res;
						goto IL_10f1;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2247), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2247);
					if (config.System != EnSystem.Transfer)
					{
						ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
						{
							ChequeGuid = config.ChequeGuid,
							Status = ENUMCheck.Mojod
						};
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2255);
						if (res.HasError)
						{
							result = res;
							goto IL_10f1;
						}
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2258);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2262);
					res.AddReturnedValue(ex, "", "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2263);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2268), ReturnedState.None, "OdatKharjShodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2268);
			}
		}
		return res;
		IL_10f1:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatEntezamiAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				try
				{
					int ordinal = 1;
					_ = ClsCache.GetCache(contextGuid).Source;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2286), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2286);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (chequeMOperationBusiness != null)
					{
						ordinal = chequeMOperationBusiness.Ordinal + 1;
					}
					ReceptionCheckBusiness receptionCheque = null;
					ReceptionChequeEntezamiBusiness receptionChequeEntezami = await ReceptionChequeEntezamiBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (receptionChequeEntezami != null)
					{
						receptionCheque = await receptionChequeEntezami.GetReceptionChequeAsync(ENUMCheck.Entezami_Mojod, token, tranName);
					}
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = (receptionChequeEntezami?.DebitMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Digaran),
						CreditTafsilGuid = receptionChequeEntezami?.DebitTafsilGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Entezami_Odat,
						OldState = ENUMCheck.Entezami_Mojod,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						DebitMoeinGuid = (receptionChequeEntezami?.CreditMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein29994),
						DebitTafsilGuid = receptionChequeEntezami?.CreditTafsilGuid,
						SanadDesc = config.SanadDesc,
						IsSanadAffective = true,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "عودت چک انتظامی".GetTranslate() + "(" + receptionCheque?.Number + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + "سررسید".GetTranslate() + " " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2329);
					if (res.HasError)
					{
						result = res;
						goto IL_0b43;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2333), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2333);
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Entezami_Odat
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2339);
					if (res.HasError)
					{
						result = res;
						goto IL_0b43;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2341);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2345);
					res.AddReturnedValue(ex, "", "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2346);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2351), ReturnedState.None, "OdatEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2351);
			}
		}
		return res;
		IL_0b43:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatEntezamiToMojodAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				try
				{
					int ordinal = 1;
					_ = ClsCache.GetCache(contextGuid).Source;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2369), ReturnedState.None, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2369);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (chequeMOperationBusiness != null)
					{
						ordinal = chequeMOperationBusiness.Ordinal + 1;
					}
					ReceptionCheckBusiness receptionCheque = null;
					ReceptionChequeEntezamiBusiness receptionChequeEntezami = await ReceptionChequeEntezamiBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (receptionChequeEntezami != null)
					{
						receptionCheque = await receptionChequeEntezami.GetReceptionChequeAsync(ENUMCheck.Entezami_Mojod, token, tranName);
					}
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = (receptionChequeEntezami?.CreditMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein29994),
						CreditTafsilGuid = receptionChequeEntezami?.CreditTafsilGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Entezami_Mojod,
						OldState = ENUMCheck.Entezami_Odat,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						DebitMoeinGuid = (receptionChequeEntezami?.DebitMoeinGuid ?? ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Digaran),
						DebitTafsilGuid = receptionChequeEntezami?.DebitTafsilGuid,
						SanadDesc = config.SanadDesc,
						IsSanadAffective = true,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "خروج از عودت چک انتظامی".GetTranslate() + " (" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + " سررسید".GetTranslate() + " " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2412);
					if (res.HasError)
					{
						result = res;
						goto IL_0b50;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2416), ReturnedState.None, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2416);
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Entezami_Mojod
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2422);
					if (res.HasError)
					{
						result = res;
						goto IL_0b50;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2424);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2428);
					res.AddReturnedValue(ex, "", "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2429);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2434), ReturnedState.None, "OdatEntezamiToMojodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2434);
			}
		}
		return res;
		IL_0b50:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatKharjShodeNewAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				try
				{
					int? ordinal = config.Ordinal;
					ordinal.GetValueOrDefault();
					if (!ordinal.HasValue)
					{
						int value = 1;
						ordinal = value;
					}
					_ = ClsCache.GetCache(contextGuid).Source;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2453), ReturnedState.None, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2453);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness lastKharjOperation = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, ENUMCheck.KharjShode, token, tranName);
					if (lastKharjOperation == null)
					{
						result = res.AddError("سند خرج کردن یافت نشد", "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2457);
						goto IL_0bfb;
					}
					if (config.System != EnSystem.Transfer && (config?.Ordinal).GetValueOrDefault() <= 0)
					{
						ordinal = lastKharjOperation.Ordinal + 1;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = lastKharjOperation.DebitMoeinGuid,
						CreditTafsilGuid = lastKharjOperation.DebitTafsilGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Mojod,
						OldState = ENUMCheck.KharjShode,
						Ordinal = (short)ordinal.Value,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						DebitMoeinGuid = (lastKharjOperation?.CreditMoeinGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception),
						DebitTafsilGuid = (lastKharjOperation?.CreditTafsilGuid ?? accGlobalSettings.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception),
						SanadDesc = config.SanadDesc,
						IsSanadAffective = true,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "عودت چک خرج شده".GetTranslate() + "(" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + "سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2496);
					if (res.HasError)
					{
						result = res;
						goto IL_0bfb;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2501), ReturnedState.None, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2501);
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.Mojod
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2508);
					if (res.HasError)
					{
						result = res;
						goto IL_0bfb;
					}
					if (sanad != null)
					{
						returnedSaveFuncInfoWithValue = res;
						IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
						Guid guid = op.Guid;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2512);
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2516);
					res.AddReturnedValue(ex, "", "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2517);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2522), ReturnedState.None, "OdatKharjShodeNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2522);
			}
		}
		return res;
		IL_0bfb:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfo> ElamTahvilAsync(string contextGuid, EnSystem system, Guid chequeGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2534), ReturnedState.None, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2534);
				}
				transactionHasBeenCreated = true;
				ReceptionCheckBusiness obj = (await ReceptionCheckBusiness.GetAsync(contextGuid, chequeGuid, token, tranName)) ?? throw new Exception($"چک مورد نظر ({chequeGuid}) یافت نشد");
				obj.VisitorGuid = null;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, 10, token, tranName), ReturnedState.None, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2542);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2546);
				res.AddReturnedValue(ex, "", "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2547);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2552), ReturnedState.None, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2552);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> ChangeAcceptorCashBoxAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				try
				{
					int ordinal = 1;
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2569), ReturnedState.None, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2569);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (chequeMOperationBusiness != null)
					{
						ordinal = chequeMOperationBusiness.Ordinal + 1;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (receptionCheque == null)
					{
						result = res.AddError("چک مورد نظر یافت نشد", "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2576);
						goto IL_0bee;
					}
					if (receptionCheque.DebitTafsilGuid == config.DebitTafsilGuid)
					{
						result = res;
						goto IL_0bee;
					}
					ReceptionBusiness obj = await receptionCheque.GetReceptionMasterAsync(config.System, token, tranName);
					obj.ListCheck.SingleOrDefault((ReceptionCheckBusiness q) => q.Guid == receptionCheque.Guid).DebitTafsilGuid = config.DebitTafsilGuid;
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await obj.SaveAsync(config.System, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, tranName, token), ReturnedState.None, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2583);
					if (res.HasError)
					{
						result = res;
						goto IL_0bee;
					}
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.Mojod,
						OldState = ENUMCheck.Mojod,
						Ordinal = (short)ordinal,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						CreditMoeinGuid = receptionCheque.DebitMoeinGuid,
						CreditTafsilGuid = receptionCheque.DebitTafsilGuid,
						DebitMoeinGuid = receptionCheque.DebitMoeinGuid,
						DebitTafsilGuid = config.DebitTafsilGuid,
						SanadDesc = config.SanadDesc,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "تغییر صندوق پذیرنده چک ".GetTranslate() + "(" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + " سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2617);
					if (res.HasError)
					{
						result = res;
						goto IL_0bee;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2621), ReturnedState.None, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2621);
					if (res.HasError)
					{
						result = res;
						goto IL_0bee;
					}
					returnedSaveFuncInfoWithValue = res;
					IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
					Guid guid = op.Guid;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2623);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2627);
					res.AddReturnedValue(ex, "", "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2628);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2633), ReturnedState.None, "ChangeAcceptorCashBoxAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2633);
			}
		}
		return res;
		IL_0bee:
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> OdatAsync(string contextGuid, ChequeMOperationConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			checked
			{
				try
				{
					int? ordinal = config.Ordinal;
					ordinal.GetValueOrDefault();
					if (!ordinal.HasValue)
					{
						int value = 1;
						ordinal = value;
					}
					ENUMCheck? oldState = config.OldState;
					oldState.GetValueOrDefault();
					if (!oldState.HasValue)
					{
						ENUMCheck value2 = ENUMCheck.Mojod;
						oldState = value2;
					}
					if (!config.CashierGuid.HasValue && config.System == EnSystem.UserInterface)
					{
						config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue;
					if (autoTran)
					{
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2654), ReturnedState.None, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2654);
					}
					transactionHasBeenCreated = true;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, config.ChequeGuid, config.Guid, null, token, tranName);
					if (chequeMOperationBusiness != null && config.System != EnSystem.Transfer && (config?.Ordinal).GetValueOrDefault() <= 0)
					{
						ordinal = chequeMOperationBusiness.Ordinal + 1;
						oldState = chequeMOperationBusiness.NewState;
					}
					ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, config.ChequeGuid, token, tranName);
					if (receptionCheque == null)
					{
						result = res.AddError("سند دریافتنی یافت نشد", "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2665);
						goto IL_0c6e;
					}
					ReceptionBusiness receptionBusiness = await receptionCheque.GetReceptionMasterAsync(config.System, token, tranName);
					if (receptionBusiness == null && !receptionCheque.Aval2re)
					{
						result = res.AddError("سند دریافت یافت نشد", "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2668);
						goto IL_0c6e;
					}
					ChequeMOperationBusiness op = new ChequeMOperationBusiness(contextGuid)
					{
						Avale2re_Transfer = null,
						CashierGuid = config.CashierGuid.Value,
						DateM = config.DateM,
						ContextGuid = contextGuid,
						CreditMoeinGuid = receptionCheque.DebitMoeinGuid,
						CreditTafsilGuid = receptionCheque.DebitTafsilGuid,
						DateSabt = config.DateM,
						Guid = (config.Guid ?? Guid.NewGuid()),
						GuidCheque = config.ChequeGuid,
						IsCreditTafsilAffective = true,
						IsDebitTafsilAffective = true,
						Modified = DateTime.Now,
						NewState = ENUMCheck.OdatShode,
						OldState = oldState,
						Ordinal = (short)ordinal.Value,
						SanadNO = config.SanadNo,
						SanadSt = SanadSt.Movaghat,
						Status = true,
						Description = config.Description,
						SanadDesc = config.SanadDesc,
						IsSanadAffective = true,
						StatusId = config.SanadStatusId,
						Price = receptionCheque.Price
					};
					if (receptionCheque.Aval2re)
					{
						op.DebitMoeinGuid = receptionCheque.A2re_PardazandeMoeinGuid.Value;
						op.DebitTafsilGuid = receptionCheque.A2re_PardazandeTafsilGuid;
					}
					else
					{
						op.DebitMoeinGuid = receptionBusiness.MoeinGuid;
						op.DebitTafsilGuid = receptionBusiness.TafsilGuid;
					}
					if (string.IsNullOrEmpty(op.SanadDesc))
					{
						op.SanadDesc = "عودت چک به پردازنده".GetTranslate() + "(" + (receptionCheque?.Number ?? "").GetTranslate() + ") " + (receptionCheque?.BankName ?? "").GetTranslate() + " " + "سررسید".GetTranslate() + "  " + receptionCheque?.DateSh;
					}
					ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue2 = await op.SaveAsync(config.System, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName);
					res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2712);
					if (res.HasError)
					{
						result = res;
						goto IL_0c6e;
					}
					SanadBusiness sanad = (res.value = returnedSaveFuncInfoWithValue2.value);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2717), ReturnedState.None, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2717);
					ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
					{
						ChequeGuid = config.ChequeGuid,
						Status = ENUMCheck.OdatShode
					};
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2724);
					if (res.HasError)
					{
						result = res;
						goto IL_0c6e;
					}
					if (sanad != null)
					{
						returnedSaveFuncInfoWithValue = res;
						IChequeMOperationRepository chequeMOperation = ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation;
						Guid guid = op.Guid;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await chequeMOperation.SetSanadNoAsync(guid, await sanad.GetSanadNoAsync(token, tranName), token, tranName), ReturnedState.None, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2728);
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2732);
					res.AddReturnedValue(ex, "", "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2733);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<SanadBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2738), ReturnedState.None, "OdatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2738);
			}
		}
		return res;
		IL_0c6e:
		return result;
	}

	private static void ReOrderItems(List<ChequeMOperationVersion5> lst, ChequeMOperationVersion5 item1, ChequeMOperationVersion5 item2)
	{
		checked
		{
			try
			{
				short tempOrdinalNumberForItem2 = item1.Ordinal;
				foreach (ChequeMOperationVersion5 item3 in lst.Where((ChequeMOperationVersion5 x) => x.Ordinal >= tempOrdinalNumberForItem2).ToList())
				{
					item3.Ordinal++;
				}
				item2.Ordinal = tempOrdinalNumberForItem2;
				for (short num = 0; num < lst.Count(); num++)
				{
					lst[num].Ordinal = (short)(num + 1);
				}
				SetStatus(lst.OrderBy((ChequeMOperationVersion5 p) => p.Ordinal));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReOrderItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2766);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> FixAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				CashierGuid = CashierGuid,
				ChequeGuid = GuidCheque,
				DateM = DateM,
				Description = Description,
				Guid = Guid,
				SanadNo = SanadNO,
				System = system
			};
			if (OldState.GetValueOrDefault() == ENUMCheck.VagozarShode && NewState == ENUMCheck.Mojod)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await VagozarToMojodAsync(ContextGuid, config, null, token), ReturnedState.None, "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2785);
			}
			else if (OldState.GetValueOrDefault() == ENUMCheck.VosolShode && NewState == ENUMCheck.VagozarShode)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await VosolToVagozarAsync(ContextGuid, config, 10, token), ReturnedState.None, "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2787);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2791);
			res.AddReturnedValue(ex, "", "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2792);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveByChequeGuidAsync(string contextGuid, Guid chequeGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2804), ReturnedState.None, "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2804);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.RemoveByChequeGuidAsync(chequeGuid, tryCount, token, tranName), ReturnedState.None, "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2806);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2810);
				ret.AddReturnedValue(ex, "", "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2811);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2816), ReturnedState.None, "RemoveByChequeGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2816);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, bool isRemoveOldSanad = false, ENUMCheck defStatus = ENUMCheck.None, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 7;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2828), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2828);
				}
				transactionHasBeenCreated = true;
				if (isRemoveOldSanad)
				{
					SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(ContextGuid, SanadNO, token, tranName);
					if (sanadBusiness != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, tryCount, tranName, token, null, new Tuple<Document, Guid>(Document.CheckSanad, Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2833);
					}
					string contextGuid = ContextGuid;
					Guid? chequeGuid = GuidCheque;
					Guid? guid = Guid;
					string tranName2 = tranName;
					ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, chequeGuid, guid, null, token, tranName2);
					if (chequeMOperationBusiness != null)
					{
						ViewChequeMForceUpdateConfig config = new ViewChequeMForceUpdateConfig
						{
							ChequeGuid = GuidCheque,
							Status = chequeMOperationBusiness.NewState
						};
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2843);
					}
					else if (defStatus != ENUMCheck.None)
					{
						ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
						{
							ChequeGuid = GuidCheque,
							Status = defStatus
						};
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2852);
					}
					if (ret.HasError)
					{
						result = ret;
						goto IL_0855;
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeMOperation.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2856);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0855;
				}
				if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, Document.CheckSanad, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2859);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0855;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2864);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2865);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2870), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2870);
			}
		}
		return ret;
		IL_0855:
		return result;
	}

	public static Task<ChequeMOperationBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.GetFromDatabaseAsync(guid, token, tranName);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2885), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2885);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeMOperation.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2887);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2889);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2892);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2893);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2898), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2898);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxparalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2909);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2912);
				}
				if (cache.Source == ENSource.Accounting)
				{
					return ret;
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2916);
				}
				if (cache.CacheRepository.ChequeMOperation == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ChequeMOperation is null");
				}
				List<Guid> list = (await cache.CacheRepository.ChequeMOperation.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2921);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2923);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2929);
						}
						current = checked(current + 1);
						ChequeMOperationBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: ChequeMOperation \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2934);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2939);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR, null), token));
						if (tasks.Count >= maxparalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2944);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2948);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2949);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2952);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2955);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2956);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs"))
		{
			_ = 4;
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					throw new NotImplementedException("این کد برای حسابداری نباید اجرا شود".GetTranslate());
				}
				if (transactionDistinityResult != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await transactionDistinityResult.Task, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2970);
				}
				if (ret.HasError)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "ChequeMOperationBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2976);
				}
				List<WebChequeMOperation> list = new List<WebChequeMOperation>();
				List<WebChequeMOperation> list2 = list;
				list2.Add(await ChequeMOperationMapper.Instance.MapAsync(ContextGuid, this, new WebChequeMOperation(), "", token));
				List<Packet> lstPacket = WebChequeMOperation.GenerateSaveRequest(list, Guid.NewGuid());
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2978);
				}
				using ChequeMOperationWaiter waiter = new ChequeMOperationWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("ChequeMOperationWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2982);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2984);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2986);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2989);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 2990);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3003), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3003);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeMOperation.SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3005);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3007);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3010);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3011);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3016), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3016);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(string tranName, CancellationToken token = default(CancellationToken))
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3028), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3028);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ChequeMOperation.SetSanadNoAsync(Guid, SanadNO, token, tranName), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3030);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3034);
				ret.AddReturnedValue(ex, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3035);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3040), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3040);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, EnSystem system, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			token.ThrowIfCancellationRequested();
			IEnumerable<ReceptionCheckReportBusiness> enumerable = await ReceptionCheckBusiness.GetAllAsync(contextGuid, new ReceptionChequeFilter(), token);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (ReceptionCheckReportBusiness item in enumerable)
			{
				if (token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3054);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await FixDataAsync(contextGuid, item.Guid, item.Status, item.Number, system, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3055);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3060);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3061);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, Guid chequeGuid, ENUMCheck status, string number, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3070);
				}
				ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, chequeGuid, token, tranName);
				List<ChequeMOperationBusiness> operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
				{
					ChequeGuid = chequeGuid
				}, token, tranName))?.ToList();
				if (!(operatioList?.Any() ?? false))
				{
					if (status == ENUMCheck.Mojod || status == ENUMCheck.BatelShode || status == ENUMCheck.Entezami_Mojod)
					{
						return res;
					}
					operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
					{
						ChequeGuid = chequeGuid
					}, token, tranName))?.ToList();
					if (!(operatioList?.Any() ?? false))
					{
						return res.AddWarning("چک با شماره".GetTranslate() + " " + (number ?? "").GetTranslate() + " " + " در وضعیت".GetTranslate() + " " + (status.GetDisplay() ?? "").GetTranslate() + " " + " می باشد اما Operation ندارد".GetTranslate(), "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3080);
					}
				}
				if (operatioList.Any((ChequeMOperationBusiness p) => p.Ordinal <= 0))
				{
					res.AddReturnedValue(ComputeOrdinal(contextGuid, operatioList, null, ignoreInvalidOperations: true), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3084);
				}
				operatioList = operatioList.OrderBy((ChequeMOperationBusiness p) => p.Ordinal).ToList();
				res.AddReturnedValue(ComputeOrdinalMoghayeratInStatus(contextGuid, operatioList, 10, ignoreInvalidOperation: true), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3087);
				operatioList = operatioList.OrderBy((ChequeMOperationBusiness p) => p.Ordinal).ToList();
				for (int i = 0; i < operatioList.Count; i++)
				{
					operatioList[i].Ordinal = (short)(i + 1);
				}
				List<ChequeMOperationBusiness> existsOperations = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
				{
					ChequeGuid = chequeGuid
				}, token, tranName))?.ToList();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await UpdateDataBaseOperationsAsync(contextGuid, existsOperations, operatioList, token, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3093);
				operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
				{
					ChequeGuid = chequeGuid
				}, token, tranName))?.ToList();
				if (operatioList?.Any() ?? false)
				{
					IEnumerable<SandoghBusiness> listSandouq = await SandoghBusiness.GetAllAsync(contextGuid, EnSystem.None, "", tranName: tranName, token: token, status: true);
					CancellationToken token3 = token;
					string tranName3 = tranName;
					IEnumerable<Guid> bankGuidList = (await BankBusiness.GetAllAsync(contextGuid, EnSystem.None, "", token3, true, tranName3))?.Select((BankBusiness q) => q.Guid);
					IEnumerable<ChequeMOperationBusiness> enumerable = operatioList.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.VosolShode && (q?.CreditTafsilGuid ?? Guid.Empty) == ParentDefaults.TafsilCoding.CLSTafsil29766);
					if (enumerable != null && enumerable.Any())
					{
						foreach (ChequeMOperationBusiness item4 in enumerable)
						{
							ChequeMOperationBusiness chequeMOperationBusiness = await GetLastOperationAsync(contextGuid, chequeGuid, null, ENUMCheck.VagozarShode, token, tranName);
							if (chequeMOperationBusiness != null)
							{
								if ((item4?.DebitTafsilGuid ?? Guid.Empty) == Guid.Empty)
								{
									item4.DebitTafsilGuid = chequeMOperationBusiness.DebitTafsilGuid;
								}
								item4.CreditTafsilGuid = chequeMOperationBusiness.DebitTafsilGuid;
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await item4.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3112);
							}
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
					IEnumerable<ChequeMOperationBusiness> enumerable2 = operatioList.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.VagozarShode && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.VosolShode && (q.DebitTafsilGuid ?? Guid.Empty) != (q.CreditTafsilGuid ?? Guid.Empty));
					if (enumerable2 != null && enumerable2.Any())
					{
						foreach (ChequeMOperationBusiness item3 in enumerable2)
						{
							item3.DebitTafsilGuid = item3.CreditTafsilGuid;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await item3.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3127);
							ChequeMOperationBusiness chequeMOperationBusiness2 = operatioList.FirstOrDefault((ChequeMOperationBusiness q) => q.Ordinal == item3.Ordinal + 1 && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.VagozarShode && q.NewState == ENUMCheck.VosolShode);
							if (chequeMOperationBusiness2 != null)
							{
								bool flag = false;
								if (chequeMOperationBusiness2.CreditTafsilGuid != item3.CreditTafsilGuid)
								{
									chequeMOperationBusiness2.CreditTafsilGuid = item3.CreditTafsilGuid;
									flag = true;
								}
								if (chequeMOperationBusiness2.DebitTafsilGuid != item3.CreditTafsilGuid)
								{
									chequeMOperationBusiness2.DebitTafsilGuid = item3.CreditTafsilGuid;
									flag = true;
								}
								if (flag)
								{
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await chequeMOperationBusiness2.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3142);
								}
							}
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
					IEnumerable<ChequeMOperationBusiness> enumerable3 = operatioList?.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.Bargashti && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.VagozarShode);
					if (enumerable3 != null && enumerable3.Any())
					{
						foreach (ChequeMOperationBusiness item2 in enumerable3)
						{
							ChequeMOperationBusiness chequeMOperationBusiness3 = operatioList?.FirstOrDefault((ChequeMOperationBusiness q) => q.Ordinal == item2.Ordinal - 1 && (q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.Mojod || q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.VosolShode) && q.NewState == ENUMCheck.VagozarShode);
							if (chequeMOperationBusiness3 == null)
							{
								continue;
							}
							bool flag2 = false;
							if (!item2.IsCreditTafsilAffective.HasValue || !item2.IsCreditTafsilAffective.Value)
							{
								item2.IsCreditTafsilAffective = true;
								flag2 = true;
							}
							if (chequeMOperationBusiness3.DebitTafsilGuid != item2.CreditTafsilGuid)
							{
								item2.CreditTafsilGuid = chequeMOperationBusiness3.DebitTafsilGuid;
								flag2 = true;
							}
							if (flag2)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await item2.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3168);
							}
							SanadBusiness sanad = await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAsync(item2.Guid, Document.CheckSanad, LoadDets: true, token, tranName);
							if ((sanad?.ListSanad_Det?.Any()).GetValueOrDefault())
							{
								Sanad_DetBusiness sanad_DetBusiness = sanad.ListSanad_Det.FirstOrDefault((Sanad_DetBusiness q) => q.Credit > 0m && (q.TafsilGuid ?? Guid.Empty) == Guid.Empty);
								if (sanad_DetBusiness != null)
								{
									await sanad_DetBusiness.SetTafsilGuidAsync(item2.CreditTafsilGuid, tranName, token);
									List<ViewSanadDetailCoverBusiness> lst = await item2.GetSanadCoverListAsync(system, token, tranName);
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, item2.Guid)), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3178);
								}
							}
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
					IEnumerable<ChequeMOperationBusiness> enumerable4 = operatioList.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.VagozarShode && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.Mojod && (q.DebitTafsilGuid ?? Guid.Empty) == (q.CreditTafsilGuid ?? Guid.Empty));
					if (enumerable4 != null && enumerable4.Any() && (listSandouq?.Any() ?? false))
					{
						foreach (ChequeMOperationBusiness item5 in enumerable4)
						{
							item5.CreditTafsilGuid = listSandouq.FirstOrDefault().Guid;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await item5.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3196);
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
					IEnumerable<ChequeMOperationBusiness> enumerable5 = operatioList.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.VagozarShode && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.Mojod && q.DebitMoeinGuid == ParentDefaults.MoeinCoding.CLSMoein29976);
					if (enumerable5 != null && enumerable5.Any())
					{
						foreach (ChequeMOperationBusiness item6 in enumerable5)
						{
							item6.DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29978;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await item6.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3211);
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
					IEnumerable<ChequeMOperationBusiness> enumerable6 = operatioList.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.VagozarShode && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.Mojod && (bankGuidList?.Contains(q.CreditTafsilGuid ?? Guid.Empty) ?? false));
					if (enumerable6 != null && enumerable6.Any())
					{
						foreach (ChequeMOperationBusiness item4 in enumerable6)
						{
							receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, item4.GuidCheque, token, tranName);
							if (receptionCheque != null)
							{
								item4.CreditTafsilGuid = receptionCheque.DebitTafsilGuid;
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await item4.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3227);
							}
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
					IEnumerable<ChequeMOperationBusiness> enumerable7 = operatioList.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.Mojod && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.VagozarShode && (bankGuidList?.Contains(q.DebitTafsilGuid ?? Guid.Empty) ?? false));
					if (enumerable7 != null && enumerable7.Any())
					{
						foreach (ChequeMOperationBusiness item4 in enumerable7)
						{
							receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, item4.GuidCheque, token, tranName);
							if (receptionCheque != null)
							{
								item4.DebitTafsilGuid = receptionCheque.DebitTafsilGuid;
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await item4.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3243);
							}
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
					IEnumerable<ChequeMOperationBusiness> enumerable8 = operatioList.Where((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.Bargashti && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.KharjShode && (q.DebitTafsilGuid ?? Guid.Empty) == (q.CreditTafsilGuid ?? Guid.Empty));
					if (enumerable8 != null && enumerable8.Any())
					{
						foreach (ChequeMOperationBusiness item in enumerable8)
						{
							ChequeMOperationBusiness chequeMOperationBusiness4 = operatioList?.FirstOrDefault((ChequeMOperationBusiness q) => q.Ordinal == item.Ordinal - 1 && q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.Mojod && q.NewState == ENUMCheck.KharjShode);
							if (chequeMOperationBusiness4 != null && !(chequeMOperationBusiness4.CreditMoeinGuid == Guid.Empty) && !(chequeMOperationBusiness4.DebitMoeinGuid == Guid.Empty) && !((chequeMOperationBusiness4.CreditTafsilGuid ?? Guid.Empty) == Guid.Empty) && !((chequeMOperationBusiness4.DebitTafsilGuid ?? Guid.Empty) == Guid.Empty))
							{
								item.DebitMoeinGuid = chequeMOperationBusiness4.CreditMoeinGuid;
								item.DebitTafsilGuid = chequeMOperationBusiness4.CreditTafsilGuid;
								item.CreditMoeinGuid = chequeMOperationBusiness4.DebitMoeinGuid;
								item.CreditTafsilGuid = chequeMOperationBusiness4.DebitTafsilGuid;
								item.IsSanadAffective = true;
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3273);
							}
						}
						operatioList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
						{
							ChequeGuid = chequeGuid
						}, token, tranName))?.ToList();
					}
				}
				ChequeMOperationBusiness lastOperation = operatioList?.LastOrDefault();
				if (lastOperation == null)
				{
					return res;
				}
				if (lastOperation.NewState == ENUMCheck.BatelShode)
				{
					if (receptionCheque == null || !receptionCheque.MasterGuid.HasValue)
					{
						return res;
					}
					ReceptionBusiness receptionBusiness = await ReceptionBusiness.GetAsync(contextGuid, receptionCheque.MasterGuid.Value, system, null, fromDatabase: true, token, tranName);
					if (receptionBusiness == null)
					{
						return res;
					}
					if (receptionBusiness.Sum == 0m || receptionBusiness.Sum == receptionCheque.Price)
					{
						bool flag3 = true;
						if ((receptionBusiness?.ListCheck?.Any()).GetValueOrDefault())
						{
							flag3 = !receptionBusiness.ListCheck.Any((ReceptionCheckBusiness q) => q.Status != ENUMCheck.Mojod && q.Status != ENUMCheck.BatelShode);
						}
						if (flag3)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.RemoveAsync(system, awaitOnSemaphore: true, 10, token, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3293);
						}
					}
					else
					{
						receptionBusiness.RemoveFromDetList(receptionCheque);
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.SaveAsync(system, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, tranName, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3298);
					}
				}
				else if (lastOperation.OldState.HasValue && lastOperation.NewState == ENUMCheck.Bargashti && (lastOperation.CreditTafsilGuid ?? Guid.Empty) == ParentDefaults.TafsilCoding.CLSTafsil29766)
				{
					PardakhtCheckMBusiness pardakhtCheque = (await PardakhtCheckMBusiness.GetAllAsync(contextGuid, chequeGuid, token, tranName))?.FirstOrDefault();
					if ((pardakhtCheque?.MasterGuid ?? Guid.Empty) != Guid.Empty)
					{
						PardakhtBusiness pardakhtBusiness = (await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque.MasterGuid, fromDatabase: true, token, tranName)) ?? throw new Exception($"Pardakht with Guid({pardakhtCheque.MasterGuid}) Not Found");
						lastOperation.CreditMoeinGuid = pardakhtBusiness.MoeinGuid;
						lastOperation.CreditTafsilGuid = pardakhtBusiness.TafsilGuid;
						lastOperation.OldState = ENUMCheck.KharjShode;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await lastOperation.SaveAsync(system, sendToServer: false, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3312);
					}
				}
				else if (lastOperation.NewState == ENUMCheck.KharjShode && !(lastOperation?.Avale2re_Transfer).GetValueOrDefault() && (await PardakhtCheckMBusiness.GetAllAsync(contextGuid, chequeGuid, token, tranName))?.FirstOrDefault() == null)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await lastOperation.RemoveAsync(system, isRemoveOldSanad: false, ENUMCheck.None, 10, token, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3320);
				}
				if (lastOperation.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.KharjShode && lastOperation.NewState == ENUMCheck.Bargashti && !((await GetLastOperationAsync(contextGuid, lastOperation.GuidCheque, lastOperation.Guid, null, token, tranName))?.Avale2re_Transfer).GetValueOrDefault())
				{
					IEnumerable<PardakhtCheckMBusiness> enumerable9 = await PardakhtCheckMBusiness.GetAllAsync(contextGuid, lastOperation.GuidCheque, token, tranName);
					if ((enumerable9?.Count() ?? 0) == 1)
					{
						PardakhtCheckMBusiness pardakhtCheckMBusiness = enumerable9.Single();
						PardakhtBusiness pardakhtBusiness2 = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheckMBusiness.MasterGuid, fromDatabase: true, token, tranName);
						if (pardakhtBusiness2 != null)
						{
							bool flag4 = false;
							if (lastOperation.CreditMoeinGuid != pardakhtBusiness2.MoeinGuid)
							{
								lastOperation.CreditMoeinGuid = pardakhtBusiness2.MoeinGuid;
								flag4 = true;
							}
							if ((lastOperation.CreditTafsilGuid ?? Guid.Empty) != pardakhtBusiness2.TafsilGuid)
							{
								lastOperation.CreditTafsilGuid = pardakhtBusiness2.TafsilGuid;
								flag4 = true;
							}
							if (flag4)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await lastOperation.SaveAsync(system, sendToServer: false, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3347);
							}
						}
					}
				}
				if (res.HasError)
				{
					return res;
				}
				if (system == EnSystem.UserInterface)
				{
					ComputeOrdinal(contextGuid, operatioList, null, ignoreInvalidOperations: false);
					ComputeOrdinalMoghayeratInStatus(contextGuid, operatioList, 10, ignoreInvalidOperation: false);
				}
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3360);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3363);
				res.AddReturnedValue(ex2, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3364);
			}
			return res;
		}
	}

	private static async Task<ReturnedSaveFuncInfo> UpdateDataBaseOperationsAsync(string contextGuid, List<ChequeMOperationBusiness> existsOperations, List<ChequeMOperationBusiness> newOperations, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3376), ReturnedState.None, "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3376);
				}
				transactionHasBeenCreated = true;
				List<Tuple<Guid, short>> list = new List<Tuple<Guid, short>>();
				if ((existsOperations?.Count() ?? 0) != (newOperations?.Count() ?? 0))
				{
					throw new ArgumentException("(existsOperations?.Count() ?? 0) != (newOperations?.Count() ?? 0)");
				}
				if ((existsOperations?.Count() ?? 0) == 0)
				{
					return res;
				}
				foreach (ChequeMOperationBusiness current in existsOperations)
				{
					ChequeMOperationBusiness chequeMOperationBusiness = newOperations.Single((ChequeMOperationBusiness p) => p.Guid == current.Guid);
					if (chequeMOperationBusiness.Ordinal != current.Ordinal)
					{
						list.Add(new Tuple<Guid, short>(current.Guid, chequeMOperationBusiness.Ordinal));
					}
				}
				if (list != null && list.Any())
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.UpdateDataBaseOperationsAsync(list, token, tranName), ReturnedState.None, "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3389);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3393);
				res.AddReturnedValue(ex, "", "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3394);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3399), ReturnedState.None, "UpdateDataBaseOperationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3399);
			}
		}
		return res;
	}

	public static Task<IEnumerable<ChequeMOperationReportBusiness>> GetAllReportAsync(string contextGuid, Guid chequeGuid, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.GetAllReportAsync(chequeGuid, token, tranName);
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
		ReceptionCheckBusiness receptionCheque = null;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 15;
			checked
			{
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3414), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3414);
					}
					transactionHasBeenCreated = true;
					receptionCheque = await ReceptionCheckBusiness.GetAsync(contextGuid, guidCheque, token, tranName);
					if (receptionCheque == null)
					{
						result = res;
						goto IL_132e;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await FixDataAsync(contextGuid, receptionCheque.Guid, receptionCheque.Status, receptionCheque.Number, system, token, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3419);
					IEnumerable<ChequeMOperationReportBusiness> _list = await GetAllReportAsync(contextGuid, guidCheque, token, tranName);
					List<ChequeMOperationReportBusiness> listCheque = _list?.OrderBy((ChequeMOperationReportBusiness q) => q.Ordinal)?.ToList();
					if (!(listCheque?.Any() ?? false))
					{
						result = res;
						goto IL_132e;
					}
					ChequeMOperationReportBusiness lastOperation = listCheque.First();
					ChequeMOperationBusiness oldOp2 = await GetAsync(contextGuid, lastOperation.Guid, token, tranName);
					if (_list?.Any((ChequeMOperationReportBusiness q) => q.Ordinal == 0) ?? false)
					{
						throw new NotImplementedException("_list?.Any(q => q.Ordinal == 0) ?? false=>true");
					}
					if (lastOperation.OldState.HasValue && lastOperation.OldState.GetValueOrDefault() != ENUMCheck.Mojod)
					{
						if (lastOperation.OldState.GetValueOrDefault() == ENUMCheck.VagozarShode)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await GenerateOperationAsync(contextGuid, ENUMCheck.Mojod, lastOperation.OldState.Value, receptionCheque, oldOp2, tranName, token), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3433);
							result = res;
						}
						else
						{
							if (lastOperation.OldState.GetValueOrDefault() == ENUMCheck.VosolShode && listCheque?.FirstOrDefault((ChequeMOperationReportBusiness q) => q.NewState == ENUMCheck.VagozarShode && q.Ordinal == lastOperation.Ordinal - 1) == null)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await GenerateOperationAsync(contextGuid, ENUMCheck.Mojod, ENUMCheck.VagozarShode, receptionCheque, oldOp2, tranName, token), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3441);
								if (res.HasError)
								{
									result = res;
									goto IL_132e;
								}
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await GenerateOperationAsync(contextGuid, ENUMCheck.Mojod, lastOperation.OldState.Value, receptionCheque, null, tranName, token), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3445);
							result = res;
						}
						goto IL_132e;
					}
					if (listCheque.Last().NewState != receptionCheque.Status)
					{
						lastOperation = listCheque.Last();
						oldOp2 = await GetAsync(contextGuid, lastOperation.Guid);
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await GenerateOperationAsync(contextGuid, lastOperation.NewState, receptionCheque.Status, receptionCheque, oldOp2, tranName, token), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3454);
						result = res;
						goto IL_132e;
					}
					foreach (ChequeMOperationReportBusiness item in listCheque.Where((ChequeMOperationReportBusiness q) => q.Guid != lastOperation.Guid))
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await GenerateOperationAsync(contextGuid, lastOperation.NewState, item.OldState.Value, receptionCheque, oldOp2, tranName, token), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3461);
						lastOperation = item;
					}
					if (!((await GetMoghayeratInSequenceOrderAsync(contextGuid, guidCheque, null, token, tranName))?.Any() ?? false))
					{
						result = res;
						goto IL_132e;
					}
					IEnumerable<ChequeMOperationBusiness> operationList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
					{
						ChequeGuid = guidCheque
					}, token, tranName))?.OrderBy((ChequeMOperationBusiness q) => q.Ordinal);
					if (!(operationList?.Any() ?? false))
					{
						result = res;
						goto IL_132e;
					}
					foreach (ChequeMOperationBusiness operation in operationList)
					{
						ChequeMOperationBusiness nextOperation = operationList.FirstOrDefault((ChequeMOperationBusiness q) => q.Ordinal == operation.Ordinal + 1);
						if (nextOperation == null)
						{
							continue;
						}
						if (operation.DebitMoeinGuid != nextOperation.CreditMoeinGuid || (operation.DebitTafsilGuid ?? Guid.Empty) != (nextOperation.CreditTafsilGuid ?? Guid.Empty))
						{
							nextOperation.CreditMoeinGuid = operation.DebitMoeinGuid;
							nextOperation.CreditTafsilGuid = operation.DebitTafsilGuid;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await nextOperation.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3478);
						}
						if (nextOperation != null && nextOperation.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.KharjShode && nextOperation.NewState == ENUMCheck.Bargashti && (receptionCheque?.MasterGuid ?? Guid.Empty) != Guid.Empty && (nextOperation?.DebitTafsilGuid ?? Guid.Empty) != Guid.Empty)
						{
							ReceptionReportBusiness receptionReportBusiness = await ReceptionBusiness.GetReportAsync(contextGuid, receptionCheque.MasterGuid.Value, token, tranName);
							if (receptionReportBusiness != null && receptionReportBusiness.TafsilGuid != nextOperation.DebitTafsilGuid.Value)
							{
								nextOperation.DebitTafsilGuid = receptionReportBusiness.TafsilGuid;
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await nextOperation.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3490);
							}
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3497);
					res.AddReturnedValue(ex, "", "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3498);
				}
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3503), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3503);
			}
			if (!res.HasError && receptionCheque != null)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await FixDataAsync(contextGuid, receptionCheque.Guid, receptionCheque.Status, receptionCheque.Number, system, token), ReturnedState.None, "UpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3505);
			}
		}
		return res;
		IL_132e:
		return result;
	}

	private static async Task<ReturnedSaveFuncInfo> GenerateOperationAsync(string contextGuid, ENUMCheck oldSt, ENUMCheck newSt, ReceptionCheckBusiness cheque, ChequeMOperationBusiness lastOperation, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (oldSt == newSt)
			{
				return ret;
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				ChequeGuid = cheque.Guid,
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				System = EnSystem.UserInterface,
				DateM = DateTime.Now
			};
			if (oldSt == ENUMCheck.VosolShode && newSt == ENUMCheck.Mojod)
			{
				newSt = ENUMCheck.VagozarShode;
			}
			ENUMCheck chequeStatus = cheque.Status;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			switch (newSt)
			{
			case ENUMCheck.Mojod:
				switch (oldSt)
				{
				case ENUMCheck.KharjShode:
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3530);
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await OdatKharjShodeAsync(contextGuid, config, lastOperation, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3532);
					break;
				case ENUMCheck.VagozarShode:
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3536);
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await VagozarToMojodAsync(contextGuid, config, lastOperation, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3538);
					break;
				case ENUMCheck.Bargashti:
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3542);
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await OdatBargashtiAsync(contextGuid, config, lastOperation, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3544);
					break;
				default:
					throw new NotImplementedException(oldSt.GetDisplay().GetTranslate() + "==>" + newSt.GetDisplay().GetTranslate());
				}
				break;
			case ENUMCheck.Bargashti:
				if (oldSt == ENUMCheck.KharjShode)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3551);
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await BargashtZadanAsync(contextGuid, config, lastOperation, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3553);
					break;
				}
				throw new NotImplementedException(oldSt.GetDisplay().GetTranslate() + "==>" + newSt.GetDisplay().GetTranslate());
			case ENUMCheck.KharjShode:
				if (oldSt == ENUMCheck.Bargashti)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3560);
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await BargashtiToBeforeStateAsync(contextGuid, config, lastOperation, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3562);
				}
				if (oldSt == ENUMCheck.Mojod)
				{
					if (lastOperation != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation.Ordinal, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3567);
					}
					if (ret.HasError)
					{
						return ret;
					}
					IEnumerable<PardakhtCheckMBusiness> enumerable = await PardakhtCheckMBusiness.GetAllAsync(contextGuid, cheque.Guid, token, tranName);
					if (enumerable != null && enumerable.Any())
					{
						PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, enumerable.First().MasterGuid, fromDatabase: true, token, tranName);
						if (pardakhtBusiness != null)
						{
							config.Ordinal = 1;
							config.DateM = pardakhtBusiness.DateM;
							config.CashierGuid = pardakhtBusiness.UserGuid;
							config.SanadNo = pardakhtBusiness.SanadNo;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await KharjKardanAsync(contextGuid, config, pardakhtBusiness.Guid, pardakhtBusiness.MoeinGuid, pardakhtBusiness.TafsilGuid, 1, token, tranName, foreUpdate: true), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3579);
						}
					}
				}
				if (oldSt == ENUMCheck.VagozarShode)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3585);
					if (ret.HasError)
					{
						return ret;
					}
					config.Ordinal = (((lastOperation?.Ordinal ?? 0) <= 0) ? 1 : checked(lastOperation.Ordinal - 1));
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await OdatKharjShodeAsync(contextGuid, config, lastOperation, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3588);
				}
				break;
			case ENUMCheck.VosolShode:
				if (oldSt == ENUMCheck.VagozarShode)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3594);
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await VosolKardanAsync(contextGuid, config, lastOperation, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3596);
				}
				break;
			case ENUMCheck.VagozarShode:
				if (oldSt == ENUMCheck.Mojod)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3602);
					if (ret.HasError)
					{
						return ret;
					}
					config.DebitTafsilGuid = lastOperation.CreditTafsilGuid;
					config.OldState = ENUMCheck.Mojod;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await VagozarKardanAsync(contextGuid, config, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3606);
				}
				if (oldSt == ENUMCheck.VosolShode)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await IncreaseOrinalAsync(contextGuid, cheque.Guid, lastOperation?.Ordinal ?? 0, tranName, token), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3610);
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await VosolToVagozarAsync(contextGuid, config, 0, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3612);
				}
				break;
			case ENUMCheck.BatelShode:
				if (oldSt == ENUMCheck.Mojod)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await EbtalAsync(contextGuid, config, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3617);
				}
				break;
			default:
				throw new NotImplementedException(oldSt.GetDisplay().GetTranslate() + " ==> " + newSt.GetDisplay().GetTranslate());
			}
			cheque.Status = chequeStatus;
			ViewChequeMForceUpdateConfig config2 = new ViewChequeMForceUpdateConfig
			{
				ChequeGuid = config.ChequeGuid,
				Status = chequeStatus
			};
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.ForceUpdateChecqueStatusAsync(config2, token, tranName), ReturnedState.None, "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3629);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3633);
			ret.AddReturnedValue(ex, "", "GenerateOperationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3634);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfo> IncreaseOrinalAsync(string contextGuid, Guid chequeGuid, short orinal, string tranName, CancellationToken token)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentException("TranName is Null or Empty");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.IncreaseOrinalAsync(chequeGuid, orinal, token, tranName), ReturnedState.None, "IncreaseOrinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3651);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IncreaseOrinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3655);
			ret.AddReturnedValue(ex, "", "IncreaseOrinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3656);
		}
		return ret;
	}

	private static ReturnedSaveFuncInfo ComputeOrdinal(string contextGuid, IEnumerable<ChequeMOperationBusiness> originalList, ChequeMOperationBusiness previwsOperation, bool ignoreInvalidOperations)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				List<ChequeMOperationBusiness> list = originalList?.OrderBy((ChequeMOperationBusiness q) => q.Ordinal)?.ToList();
				if (list == null || !list.Any())
				{
					return returnedSaveFuncInfo;
				}
				if (list.First().OldState.GetValueOrDefault() == ENUMCheck.Mojod)
				{
					ENUMCheck newState = list.First().NewState;
					bool flag = true;
					foreach (ChequeMOperationBusiness item in list)
					{
						if (item.Ordinal != 1 || item.OldState.Value != ENUMCheck.Mojod)
						{
							if (item.OldState.Value != newState)
							{
								flag = false;
								break;
							}
							newState = item.NewState;
						}
					}
					if (flag)
					{
						return returnedSaveFuncInfo;
					}
				}
				IEnumerable<ChequeMOperationBusiness> source = list.Where((ChequeMOperationBusiness p) => p.OldState.Value == (previwsOperation?.NewState ?? ENUMCheck.Mojod));
				ChequeMOperationBusiness chequeMOperationBusiness = null;
				if (source.Count() == 1)
				{
					chequeMOperationBusiness = source.Single();
				}
				else
				{
					if (source.Count() == 0)
					{
						if (ignoreInvalidOperations)
						{
							return returnedSaveFuncInfo;
						}
						return returnedSaveFuncInfo.AddError("mojod operation has not been founded!", "", "ComputeOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3691);
					}
					chequeMOperationBusiness = ((source.First().Ordinal != (previwsOperation?.Ordinal ?? 0) + 1) ? source.OrderBy((ChequeMOperationBusiness p) => p.DateM).First() : source.First());
				}
				SetOrdinal(chequeMOperationBusiness, (short)((previwsOperation?.Ordinal ?? 0) + 1), list);
				list.Remove(chequeMOperationBusiness);
				returnedSaveFuncInfo.AddReturnedValue(ComputeOrdinal(contextGuid, list, chequeMOperationBusiness, ignoreInvalidOperations), ReturnedState.None, "ComputeOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3700);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3704);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "ComputeOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3705);
			}
			return returnedSaveFuncInfo;
		}
	}

	private static ReturnedSaveFuncInfo ComputeOrdinalMoghayeratInStatus(string contextGuid, List<ChequeMOperationBusiness> operatioList, short maxTryCount, bool ignoreInvalidOperation)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				ENUMCheck eNUMCheck = ENUMCheck.Mojod;
				for (int i = 0; i < operatioList.Count; i++)
				{
					if (operatioList[i].OldState != eNUMCheck)
					{
						returnedSaveFuncInfo.AddReturnedValue(ComputeOrdinal(contextGuid, operatioList, null, ignoreInvalidOperation), ReturnedState.None, "ComputeOrdinalMoghayeratInStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3719);
						if (returnedSaveFuncInfo.HasError)
						{
							return returnedSaveFuncInfo;
						}
						if (maxTryCount > 0)
						{
							returnedSaveFuncInfo.AddReturnedValue(ComputeOrdinalMoghayeratInStatus(contextGuid, operatioList, --maxTryCount, ignoreInvalidOperation), ReturnedState.None, "ComputeOrdinalMoghayeratInStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3722);
						}
						else if (!ignoreInvalidOperation)
						{
							return returnedSaveFuncInfo.AddError("MaxTryCount Exceeded", "", "ComputeOrdinalMoghayeratInStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3725);
						}
					}
					eNUMCheck = operatioList[i].NewState;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeOrdinalMoghayeratInStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3732);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "ComputeOrdinalMoghayeratInStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3733);
			}
			return returnedSaveFuncInfo;
		}
	}

	private static void SetOrdinal(ChequeMOperationBusiness item, short newOrdinalForItem, IEnumerable<ChequeMOperationBusiness> list)
	{
		checked
		{
			try
			{
				if (list.SingleOrDefault((ChequeMOperationBusiness p) => p.Guid != item.Guid && p.Ordinal == newOrdinalForItem) != null)
				{
					foreach (ChequeMOperationBusiness item2 in list.Where((ChequeMOperationBusiness p) => p.Ordinal >= newOrdinalForItem))
					{
						item2.Ordinal++;
					}
				}
				item.Ordinal = newOrdinalForItem;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3751);
			}
		}
	}

	public static async Task<List<MoghayeratChequeAvalDoreModel>> GetAllAvalDoreMoghyerat(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			Guid cLSMoein = ParentDefaults.MoeinCoding.CLSMoein29976;
			Guid cLSMoein2 = ParentDefaults.MoeinCoding.CLSMoein29978;
			return await ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.GetAllAvalDoreMoghyerat(cLSMoein, cLSMoein2, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAvalDoreMoghyerat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3763);
			throw;
		}
	}

	public static Task<List<ChequeMInstantModel>> GetChequeIntstantAsync(string contextGuid, DateTime dateM, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ChequeMOperation.GetAllChequeMInstantAsync(dateM, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveFromWebAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				CashierGuid = CashierGuid,
				ChequeGuid = GuidCheque,
				DateM = DateM,
				DebitTafsilGuid = DebitTafsilGuid,
				Description = Description,
				Guid = Guid,
				SanadNo = 0,
				System = system
			};
			switch (NewState)
			{
			case ENUMCheck.Mojod:
				if (OldState.GetValueOrDefault() == ENUMCheck.VagozarShode)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await VagozarToMojodAsync(ContextGuid, config, null, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3789);
				}
				else if (OldState.GetValueOrDefault() == ENUMCheck.NaghdShode)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await NaghdToMojodAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3791);
				}
				else if (OldState.GetValueOrDefault() == ENUMCheck.BatelShode)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await BatelToMojodAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3793);
				}
				else if (OldState.GetValueOrDefault() == ENUMCheck.Bargashti)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await OdatBargashtiAsync(ContextGuid, config, null, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3795);
				}
				else
				{
					string text = $"Old Status: {OldState} \r\n newStatus: {NewState}";
					WebErrorLog.Instance.StartLog(text, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3799);
					res.AddError(text, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3800);
				}
				break;
			case ENUMCheck.Bargashti:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await BargashtZadanAsync(ContextGuid, config, null, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3804);
				break;
			}
			case ENUMCheck.VagozarShode:
				if (OldState.GetValueOrDefault() == ENUMCheck.VosolShode)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await VosolToVagozarAsync(ContextGuid, config, 0, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3808);
				}
				else if (OldState.GetValueOrDefault() == ENUMCheck.Bargashti)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await BargashtiToBeforeStateAsync(ContextGuid, config, null, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3810);
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await VagozarKardanAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3812);
				}
				break;
			case ENUMCheck.NaghdShode:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await NaghdKardanAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3815);
				break;
			}
			case ENUMCheck.BatelShode:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await EbtalAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3818);
				break;
			}
			case ENUMCheck.VosolShode:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await VosolKardanAsync(ContextGuid, config, null, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3821);
				break;
			}
			case ENUMCheck.Entezami_Mojod:
				if (OldState.GetValueOrDefault() == ENUMCheck.Entezami_Odat)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await OdatEntezamiToMojodAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3825);
				}
				break;
			case ENUMCheck.Entezami_Odat:
				if (OldState.GetValueOrDefault() == ENUMCheck.Entezami_Mojod)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await OdatEntezamiAsync(ContextGuid, config, token), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3829);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3835);
			res.AddReturnedValue(ex, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3836);
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
			ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(ContextGuid, GuidCheque, token, tranName);
			return new List<ViewSanadDetailCoverBusiness>
			{
				new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = null,
					DebitTafsilGuid = null,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = 0m,
					Credit = receptionCheckBusiness.Price,
					CashierGuid = CashierGuid,
					SanadNo = SanadNO,
					SanadSt = SanadSt,
					Id2 = 0,
					CreditMoeinGuid = CreditMoeinGuid,
					CreditTafsilGuid = (IsCreditTafsilAffective.GetValueOrDefault() ? CreditTafsilGuid : null),
					SanadDesc = SanadDesc,
					RDesc = Description,
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
					Serial = receptionCheckBusiness.Number,
					Status = true,
					VahedGuid = null
				},
				new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = DebitMoeinGuid,
					DebitTafsilGuid = (IsDebitTafsilAffective.GetValueOrDefault() ? DebitTafsilGuid : null),
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = receptionCheckBusiness.Price,
					Credit = 0m,
					CashierGuid = CashierGuid,
					SanadNo = SanadNO,
					SanadSt = SanadSt,
					Id2 = 0,
					CreditMoeinGuid = null,
					CreditTafsilGuid = null,
					SanadDesc = SanadDesc,
					RDesc = Description,
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
					Serial = receptionCheckBusiness.Number,
					Status = true,
					VahedGuid = null
				}
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3925);
			throw;
		}
	}

	public static async Task<List<ChequeMOperationBusiness>> GetAllMoghayeratInSequenceOrderAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<ChequeMOperationBusiness> list = new List<ChequeMOperationBusiness>();
			IEnumerable<ReceptionCheckReportBusiness> allCheque = await ReceptionCheckBusiness.GetAllAsync(contextGuid, new ReceptionChequeFilter(), token);
			if (!(allCheque?.Any() ?? false))
			{
				return list;
			}
			IEnumerable<SandoghBusiness> sandouq = await SandoghBusiness.GetAllAsync(contextGuid, EnSystem.None, "", true, "", token);
			foreach (ReceptionCheckReportBusiness item in allCheque)
			{
				if (item != null)
				{
					List<ChequeMOperationBusiness> list2 = await GetMoghayeratInSequenceOrderAsync(contextGuid, item.Guid, sandouq?.Select((SandoghBusiness q) => q.Guid)?.ToList(), token);
					if (list2 != null && list2.Any())
					{
						list.AddRange(list2);
					}
				}
			}
			return list?.Distinct()?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllMoghayeratInSequenceOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3948);
			return null;
		}
	}

	private static async Task<List<ChequeMOperationBusiness>> GetMoghayeratInSequenceOrderAsync(string contextGuid, Guid chequeGuid, List<Guid> sandouqTafsilGuidList = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			ReceptionCheckBusiness cheque = await ReceptionCheckBusiness.GetAsync(contextGuid, chequeGuid, token, tranName);
			List<ChequeMOperationBusiness> list = new List<ChequeMOperationBusiness>();
			IEnumerable<ChequeMOperationBusiness> operationList = (await GetAllAsync(contextGuid, new ChequeMOperationFilter
			{
				ChequeGuid = chequeGuid
			}, token, tranName))?.OrderBy((ChequeMOperationBusiness q) => q.Ordinal);
			if (!(operationList?.Any() ?? false))
			{
				return list;
			}
			foreach (ChequeMOperationBusiness operation in operationList)
			{
				if (operation == null)
				{
					continue;
				}
				ChequeMOperationBusiness nextOperation = operationList.FirstOrDefault((ChequeMOperationBusiness q) => q.Ordinal == checked(operation.Ordinal + 1));
				if (nextOperation != null && (operation.DebitMoeinGuid != nextOperation.CreditMoeinGuid || (operation.DebitTafsilGuid ?? Guid.Empty) != (nextOperation.CreditTafsilGuid ?? Guid.Empty)) && (nextOperation.OldState.GetValueOrDefault() != ENUMCheck.KharjShode || nextOperation.NewState != ENUMCheck.Bargashti) && (nextOperation.OldState.GetValueOrDefault() != ENUMCheck.Mojod || nextOperation.NewState != ENUMCheck.Mojod) && (nextOperation.OldState.GetValueOrDefault() != ENUMCheck.Mojod || nextOperation.NewState != ENUMCheck.OdatShode))
				{
					list.Add(operation);
				}
				if (operation.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.Mojod && operation.NewState == ENUMCheck.VagozarShode && (sandouqTafsilGuidList?.Any() ?? false) && (operation.DebitTafsilGuid ?? Guid.Empty) != Guid.Empty && sandouqTafsilGuidList.Contains(operation.DebitTafsilGuid.Value))
				{
					list.Add(operation);
				}
				if (nextOperation != null && nextOperation.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.KharjShode && nextOperation.NewState == ENUMCheck.Bargashti && (cheque?.MasterGuid ?? Guid.Empty) != Guid.Empty && (nextOperation?.DebitTafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					ReceptionReportBusiness receptionReportBusiness = await ReceptionBusiness.GetReportAsync(contextGuid, cheque.MasterGuid.Value, token, tranName);
					if (receptionReportBusiness != null && receptionReportBusiness.TafsilGuid != nextOperation.DebitTafsilGuid.Value)
					{
						list.Add(nextOperation);
					}
				}
			}
			return list?.Distinct()?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMoghayeratInSequenceOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\ChequeMOperationBusiness.cs", 3992);
			throw;
		}
	}
}
