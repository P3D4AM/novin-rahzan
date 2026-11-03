using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.TransferLayer.Waiters.Accounting;

namespace EntityCache.Business.Accounting;

public class ViewCheckMBusiness : BaseBusiness<ViewCheckMBusiness>, IViewCheckM, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<ViewCheckMBusiness>, IDeliverableEntity, IEditableDelivery
{
	private ReceptionBusiness _reception;

	private PardakhtBusiness _Pardakht;

	private DateTime _dateSanadM;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private CancellationTokenSource _pardazandeToken = new CancellationTokenSource();

	private string _name = "";

	private TafsilBusiness Pardazande;

	private Guid _pardazandeGuid;

	public string Sayadi_Name { get; set; }

	public string Sayadi_Identity { get; set; }

	public string Sayadi_NationalCode { get; set; }

	public Guid Guid { get; set; }

	public string BankName { get; set; }

	public decimal Price { get; set; }

	public ENUMCheck Status { get; set; }

	public string STName => Status.GetDisplay().GetTranslate();

	public string DateSanad => DateSanadM.MiladiToCalture();

	public DateTime DateSanadM
	{
		get
		{
			if (checked(_dateSanadM.Year < DateTime.Now.Year - 50 || _dateSanadM.Year > DateTime.Now.Year + 50))
			{
				return DateTime.Now;
			}
			return _dateSanadM;
		}
		set
		{
			_dateSanadM = value;
		}
	}

	public string DateCheck => DateCheckM.MiladiToCalture();

	public DateTime DateCheckM { get; set; }

	public string DateBargashtORVosol => DateBargashtORVosolM.MiladiToCalture();

	public DateTime DateBargashtORVosolM { get; set; }

	public string DateToPlace { get; set; }

	public string Place { get; set; }

	public string Description { get; set; }

	public string Number { get; set; }

	public Guid GuidSanad { get; set; }

	public Guid PardakhtGuid { get; set; }

	public bool Personal { get; set; }

	public string HesabNum { get; set; }

	public string CustomerFullName
	{
		get
		{
			if (string.IsNullOrEmpty(_name))
			{
				_name = Pardazande?.Name ?? "";
			}
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public bool Aval2re { get; set; }

	public long PoshtNomreh { get; set; }

	public Guid VisitorGuid { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status => Status == ENUMCheck.BatelShode;

	public bool IsChecked { get; set; }

	public bool IsEnteghali { get; set; }

	public bool SayadiRegistered { get; set; }

	public DateTime? DateUtc { get; set; }

	public Guid PardazandeTafsilGuid
	{
		get
		{
			return _pardazandeGuid;
		}
		set
		{
			try
			{
				_pardazandeGuid = value;
				if (!(_pardazandeGuid == Guid.Empty))
				{
					_pardazandeToken.Cancel();
					_pardazandeToken = new CancellationTokenSource(60000);
					CancellationToken token = _pardazandeToken.Token;
					Task.Run(() => LoadPardazandehAsync(token));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PardazandeTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 101);
			}
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public string FullDescription => "چک دریافتی شماره " + Number + " - مورخه " + DateCheck + " - بانک " + BankName;

	public override string ContextGuid { get; set; }

	public List<ChequeMOperationBusiness> OperationList { get; set; }

	public byte[] StatusImage
	{
		get
		{
			if (IsEnteghali)
			{
				return ImageResourceManager.ChequeAvalDore;
			}
			return ImageResourceManager.ChequeDuringDore;
		}
	}

	public async Task<Guid> GetPardazandeMoeinGuid(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			TafsilGroupBusiness tafsilGroupBusiness = await Pardazande.GetTafsilGroupAsync(system, token, tranName);
			if (tafsilGroupBusiness != null)
			{
				return await tafsilGroupBusiness.GetMoeinGuidAsync(token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardazandeMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 113);
		}
		return Guid.Empty;
	}

	private async Task LoadPardazandehAsync(CancellationToken token)
	{
		try
		{
			if (ClsCache.GetCache(ContextGuid) != null && !token.IsCancellationRequested && !base.IsDisposed)
			{
				Pardazande = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, PardazandeTafsilGuid, "", default(CancellationToken), "LoadPardazandehAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadPardazandehAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 125);
		}
	}

	public async Task<ReceptionBusiness> GetReceptionAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (_reception != null)
			{
				return _reception;
			}
			if (GuidSanad == Guid.Empty)
			{
				return null;
			}
			ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(ContextGuid, GuidSanad, token);
			ReceptionBusiness reception = ((receptionCheckBusiness == null) ? null : (await ReceptionBusiness.GetAsync(ContextGuid, receptionCheckBusiness.MasterGuid ?? Guid.Empty, system, null, fromDatabase: true, token)));
			_reception = reception;
			return _reception;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 139);
			return null;
		}
	}

	public async Task<PardakhtBusiness> GetPardakhtAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_Pardakht == null)
			{
				_Pardakht = await PardakhtBusiness.GetAsync(ContextGuid, PardakhtGuid, fromDatabase: true, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 150);
		}
		return _Pardakht;
	}

	public ViewCheckMBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, List<ViewCheckMBusiness> lst, EnSystem system, bool sendToServer, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			line = 1;
			if (!(lst?.Any() ?? false))
			{
				line = 2;
				return ret.AddInformation("List is null or empty!", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 186);
			}
			line = 3;
			foreach (ViewCheckMBusiness item in lst)
			{
				line = 4;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 192);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 197);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 198);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		short line = 0;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "ViewCheckMBusiness/SaverangeAsync", 60000, token))
				{
					line = 1;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 213);
					}
					else
					{
						line = 2;
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 215), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 215);
						}
						ret.throwExceptionIfError();
						transactionHasBeenCreated = true;
						line = 3;
						line = 4;
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							line = 5;
							IViewCheckMRepository iViewCheckMRepository = ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckMRepository;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await iViewCheckMRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 236);
							line = 6;
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.RemoveByChequeGuidAsync(ContextGuid, Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 239);
								if (!ret.HasError)
								{
									if (OperationList?.Any() ?? false)
									{
										foreach (ChequeMOperationBusiness operation in OperationList)
										{
											if (operation.Price <= 0m)
											{
												if (ClsCache.GetCache(operation.ContextGuid).Source != ENSource.Accounting && system == EnSystem.SignalR && operation.Price > 0m)
												{
													WebErrorLog.Instance.StartLog("یک عملیات چک از طریق سیگنال آر گرفتم ولیکن مبلغ نداره این یک خطااست و شاید مربوط به داده نادرست مربوط به ورژن های قبلی باشد. ", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 248);
												}
												operation.Price = Price;
											}
										}
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.SaveRangeAsync(OperationList, sendToServer, system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 251);
									}
									goto end_IL_0127;
								}
								result = ret;
							}
						}
					}
					goto end_IL_0084;
					end_IL_0127:;
				}
				goto end_IL_0073;
				end_IL_0084:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 254);
				goto end_IL_0073;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Error Line iS :{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 255);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 255);
				goto end_IL_0073;
			}
			return result;
			end_IL_0073:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 259), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 259);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, List<Guid> eConnectionData, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 271), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 271);
				}
				transactionHasBeenCreated = true;
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.DeleteRangeAsync(eConnectionData, 3, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 274);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 278);
				throw;
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 284), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 284);
			}
		}
		return ret;
	}

	public static async Task<List<ViewCheckMBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, ENUMCheck? status = null, DateTime? date1 = null, DateTime? date2 = null, string tranName = "")
	{
		try
		{
			IEnumerable<ViewCheckMBusiness> source = await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.GetAllAsync(token, tranName);
			if (status.HasValue)
			{
				source = source.Where((ViewCheckMBusiness q) => q.Status == status);
			}
			if (date1.HasValue)
			{
				source = source.Where(delegate(ViewCheckMBusiness q)
				{
					DateTime dateCheckM2 = q.DateCheckM;
					DateTime? dateTime2 = date1;
					return dateCheckM2 >= dateTime2;
				});
			}
			if (date2.HasValue)
			{
				source = source.Where(delegate(ViewCheckMBusiness q)
				{
					DateTime dateCheckM = q.DateCheckM;
					DateTime? dateTime = date2;
					return dateCheckM <= dateTime;
				});
			}
			foreach (string item in search.SplitString())
			{
				if (!string.IsNullOrEmpty(item))
				{
					source = source.Where((ViewCheckMBusiness q) => q.BankName.StringComparisons(item) || q.Price.ToString().StringComparisons(item) || q.Place.StringComparisons(item) || q.HesabNum.StringComparisons(item) || q.Number.StringComparisons(item) || (!string.IsNullOrEmpty(q.CustomerFullName) && q.CustomerFullName.StringComparisons(item)));
				}
			}
			return token.IsCancellationRequested ? null : source.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 313);
			throw;
		}
	}

	public static async Task<IEnumerable<ChequeMSummaryViewModel>> GetSummaryAsync(string contextGuid, DateTime? date1 = null, DateTime? date2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.GetSummaryAsync(date1, date2, token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public static Task<ViewCheckMBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<ViewCheckMBusiness>> GetAllAsync(string contextGuid, ENUMCheck? status = null, Guid? pardazandeGuid = null, DateTime? sarresid1 = null, DateTime? sarresid2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository.GetAllAsync(status, pardazandeGuid, sarresid1, sarresid2, token, tranName);
	}

	public Task<Guid> GetBankVagozarGuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckMRepository.GetBankVagozarGuidAsync(Guid, token, tranName);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckMRepository;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
			_ = 2;
			try
			{
				using (await _databaseSemaphore.GaurdAsync(true, "ViewCheckM/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 348), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 348);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckMRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 350);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 352);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 355);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 356);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 361), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 361);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentAsync(string contextGuid, CancellationToken inputToken = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ClsCache cache = ClsCache.GetCache(contextGuid);
			ISendData2Server uiUpdater = cache?.SendData2ServerInstance?.ViewChequeM;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال عملیات چک های اشخاص";
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (cache == null || cache.Source != ENSource.Accounting)
				{
					return ret;
				}
				if (inputToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 385);
				}
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
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 386);
				}
				CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, cache.BufferCancellationToken).Token;
				line = 500;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 389);
				}
				line = 501;
				if (cache.CacheRepository.IViewCheckMRepository == null)
				{
					throw new NullReferenceException("cache.CacheRepository.IViewCheckM is null");
				}
				line = 502;
				List<Guid> list = (await cache.CacheRepository.IViewCheckMRepository.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 398);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation($"چک های اشخاص سینک هستند تعداد چک اشخاص سینک شده:{list?.Count ?? 0}", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 399);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 400);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 408);
						}
						ViewCheckMBusiness obj = await GetAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "شماره: " + obj?.Number + " پردازنده: " + obj?.CustomerFullName;
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: ViewCheckM \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 418);
							continue;
						}
						ChequeMOperationFilter filter = new ChequeMOperationFilter
						{
							ChequeGuid = obj.Guid
						};
						IEnumerable<ChequeMOperationBusiness> enumerable = await ChequeMOperationBusiness.GetAllAsync(contextGuid, filter, token, tranName);
						if (enumerable != null && enumerable.Any())
						{
							obj.OperationList = enumerable?.ToList();
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 429);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 434);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 438);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 440);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 443);
				ret.AddReturnedValue(ex, "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 444);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs"))
		{
			_ = 3;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "ViewCheckM/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 474);
				}
				List<WebViewCheckM> list = new List<WebViewCheckM>();
				List<WebViewCheckM> list2 = list;
				list2.Add(await ViewChechMMapper.Instance.MapAsync(ContextGuid, this, new WebViewCheckM(), "", token));
				List<Packet> lstPacket = WebViewCheckM.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 476);
				}
				using ViewCheckMWaiter waiter = new ViewCheckMWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("ViewCheckMBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 480);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 482);
				}
				if (!token.IsCancellationRequested)
				{
					ret.throwExceptionIfError();
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 486);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 489);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 490);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(string contextGuid, ViewChequeMForceUpdateConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "ViewCheckMBusiness/ForceUpdateChecqueStatusAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 503);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 504), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 504);
						}
						transactionHasBeenCreated = true;
						if (!ret.HasError)
						{
							IViewCheckMRepository iViewCheckMRepository = ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckMRepository;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await iViewCheckMRepository.ForceUpdateChecqueStatusAsync(config, token, tranName), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 508);
							goto end_IL_010d;
						}
						result = ret;
					}
					goto end_IL_0076;
					end_IL_010d:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 510);
				goto end_IL_0065;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 513);
				ret.AddReturnedValue(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 514);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 519), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckMBusiness.cs", 519);
			}
		}
		return ret;
	}
}
