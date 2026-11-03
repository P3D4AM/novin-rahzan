using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.Records;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.Aghsat;

public class AghsatMBusiness : BaseBusiness<AghsatMBusiness>, IAghsatAshkhas, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<AghsatMBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext, IHasIsChecked
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private EnAghsatMStatus aghsatStatus;

	private AghsatM_GroupBusiness _group;

	private Guid? _creditMoeinGuid;

	private Guid? _creditTafsilGuid;

	private decimal _price;

	[JsonIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public int Id2 { get; set; }

	public AghsatMMetaData MetaData { get; set; }

	public string Description { get; set; }

	public DateTime DateSarResid { get; set; } = DateTime.Now.AddMonths(1);


	public DateTime Date { get; set; } = DateTime.Now;


	public Guid? ReceptionGuid { get; set; }

	public EnAghsatMStatus AghsatStatus
	{
		get
		{
			return aghsatStatus;
		}
		set
		{
			try
			{
				if (value < EnAghsatMStatus.Recieved || value > EnAghsatMStatus.NotRecieved)
				{
					aghsatStatus = EnAghsatMStatus.NotRecieved;
				}
				else
				{
					aghsatStatus = value;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AghsatStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 57);
			}
		}
	}

	public string StatusName => AghsatStatus.GetDisplay();

	public decimal Price
	{
		set
		{
			_price = value;
		}
	}

	public DateTime? ReceptionDateM { get; set; }

	public decimal ReceptionSum { get; set; }

	[JsonIgnore]
	public string ReceptionDateSh => ReceptionDateM.MiladiToCalture();

	public string UserName { get; set; }

	public string TafsilCode { get; set; }

	public string FullName { get; set; }

	public string DateSh => Date.MiladiToCalture();

	public string DateSarResidSh => DateSarResid.MiladiToCalture();

	public int TypePardakht { get; set; }

	public string Mounth { get; set; }

	public Guid GuidGroup { get; set; }

	public override string ContextGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string CustomerFirstNumber { get; set; }

	public EnRasGiriType RasGiriType { get; set; }

	public decimal VajhEltezam { get; set; }

	public DateTime? VajhEltezamModified { get; set; }

	public DateTime? DateUtc { get; set; }

	public int? SanadNo { get; set; }

	public bool Checked { get; set; }

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

	public decimal TotalPrice => _price + VajhEltezam;

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public AghsatMBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			ChangeStatus = EntityChangeSt.New;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 99);
		}
	}

	public decimal GetPrice()
	{
		return _price;
	}

	public async Task<Guid?> GetCreditMoeinGuidAsync(CancellationToken token)
	{
		try
		{
			if ((_creditMoeinGuid ?? Guid.Empty) == Guid.Empty)
			{
				_creditMoeinGuid = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Installments.VajhEltezam.CreditMoeinGuid;
			}
			return _creditMoeinGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 116);
			throw;
		}
	}

	public async Task<Guid?> GetCreditTafsilGuidAsync(CancellationToken token)
	{
		try
		{
			if ((_creditTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				_creditTafsilGuid = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Installments.VajhEltezam.CreditTafsilGuid;
			}
			return _creditTafsilGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCreditTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 133);
			throw;
		}
	}

	public async Task<AghsatM_GroupBusiness> GetGroupAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (GuidGroup == Guid.Empty)
			{
				throw new ArgumentException("Installment GroupGuid is Empty!!");
			}
			if (_group == null)
			{
				_group = await AghsatM_GroupBusiness.GetAsync(ContextGuid, GuidGroup, token, tranName);
			}
			return _group;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 147);
			throw;
		}
	}

	public void SetGroup(AghsatM_GroupBusiness group)
	{
		try
		{
			_group = group;
			GuidGroup = group?.Guid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 158);
		}
	}

	public static Task<IEnumerable<AghsatMBusiness>> GetAllByGroupGuidAsync(string contextGuid, Guid groupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAllByGroupGuidAsync(groupGuid, token, tranName);
	}

	public static Task<AghsatMBusiness> GetAsync(string contextGuid, int id2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAsync(id2, token, tranName);
	}

	public static Task<AghsatMBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (_price <= 0m)
			{
				await res.AddErrorAsync("مبلغ قسط باید بیشتر از صفر باشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 171);
			}
			if (system == EnSystem.UserInterface && !isDeleteAction)
			{
				AccGlobalSettings obj = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				DateTime chequeAndInstallmentStartDate = obj.DateAndTimePriod.ChequeAndInstallmentStartDate;
				DateTime chequeAndInstallmentEndDate = obj.DateAndTimePriod.ChequeAndInstallmentEndDate;
				if (DateSarResid < chequeAndInstallmentStartDate || DateSarResid > chequeAndInstallmentEndDate)
				{
					res.AddError(system, "شماره".GetTranslate() + " " + $"{Id2}".GetTranslate() + "\r\n" + " تاریخ سررسید".GetTranslate() + " (" + DateSarResid.MiladiToCalture() + ")\r\n" + " در بازه مجاز".GetTranslate() + "(" + (chequeAndInstallmentStartDate.MiladiToCalture() ?? "").GetTranslate() + "تا".GetTranslate() + " " + chequeAndInstallmentEndDate.MiladiToCalture() + ")" + " نیست.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 179);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 184);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 185);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), Priority priority = Priority.High, bool sendToServer = true, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			ClsCache cache = ClsCache.GetCache(ContextGuid);
			try
			{
				_ = 11;
				ReturnedSaveFuncInfo result;
				try
				{
					sendToServer = sendToServer && cache.BufferEnable;
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "AghsatMBusiness/SaveAsync", 6000, token))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 201);
						}
						else
						{
							if (autoTran)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 203), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 203);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 205);
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								await GetGroupAsync(token, tranName);
								if (system == EnSystem.Transfer || cache.Source != ENSource.Accounting)
								{
									goto IL_05f9;
								}
								AghsatMBusiness aghsatMBusiness = await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName);
								if (aghsatMBusiness != null && _group != null)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateSumAghsatAsync(ContextGuid, _group.TafsilGuid, -aghsatMBusiness._price, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 212);
								}
								if (!ret.HasError)
								{
									goto IL_05f9;
								}
								result = ret;
							}
						}
						goto end_IL_0135;
						IL_0887:
						if (system == EnSystem.SignalR && cache.Source == ENSource.Android_HesabYar && _group == null)
						{
							result = await ret.AddErrorAsync("InstallmentGroup in Null !!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 225);
						}
						else
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 227);
							if (!ret.HasError)
							{
								if (system != EnSystem.Transfer && (ReceptionGuid ?? Guid.Empty) == Guid.Empty && cache.Source == ENSource.Accounting && _group != null)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateSumAghsatAsync(ContextGuid, _group.TafsilGuid, _price, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 230);
								}
								goto end_IL_021a;
							}
							result = ret;
						}
						goto end_IL_0135;
						IL_05f9:
						if (system == EnSystem.SignalR && cache.Source == ENSource.Accounting)
						{
							ServerStatus = ServerStatusForClient.None;
						}
						if (system == EnSystem.Transfer || _group == null)
						{
							goto IL_0887;
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateServerStatusAsync(ContextGuid, _group.TafsilGuid, ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 219);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Account.UpdateServerStatusAsync(_group.TafsilGuid, ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 220);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 221), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 221);
						if (!ret.HasError)
						{
							goto IL_0887;
						}
						result = ret;
						goto end_IL_0135;
						end_IL_021a:;
					}
					goto end_IL_011f;
					end_IL_0135:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 234);
					ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 235);
					goto end_IL_011f;
				}
				return result;
				end_IL_011f:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 240), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 240);
				}
				if (!ret.HasError && sendToServer)
				{
					if (await4Send)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 243);
					}
					else
					{
						Task.Run(() => Send2ServerAsync(token, system, priority));
					}
				}
				if (ClsCache.GetCache(ContextGuid).BufferEnable && cache.Source == ENSource.Accounting && system != EnSystem.Transfer)
				{
					Task.Run(() => SendEntitiesToServerAsync(token));
				}
				if (!ret.HasError && system == EnSystem.UserInterface)
				{
					ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.AghsatM.RaiseGhestSaved(this, EventArgs.Empty);
				}
			}
			return ret;
		}
	}

	private async Task SendEntitiesToServerAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs"))
		{
			_ = 1;
			try
			{
				await Task.Delay(500);
				await Task.WhenAll<ReturnedSaveFuncInfo>(KolBusiness.ResendNotSents(ContextGuid, token, "", 5), MoeinBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), TafsilBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), AccountBusiness.ResendNotSentsAsync(ContextGuid, EnSystem.None, token, "", 10));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 265);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 13;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoCommit)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 276), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 276);
				}
				transactionHasBeenCreated = true;
				await GetGroupAsync(token, tranName);
				if ((ReceptionGuid ?? Guid.Empty) != Guid.Empty)
				{
					ReceptionBusiness receptionBusiness = await ReceptionBusiness.GetAsync(ContextGuid, ReceptionGuid.Value, system, null, fromDatabase: true, token, tranName);
					if (receptionBusiness != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.RemoveAsync(system, awaitOnSemaphore: true, 10, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 283);
					}
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0c94;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMRepository.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 286);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0c94;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await TafsilBusiness.UpdateSumAghsatAsync(ContextGuid, _group.TafsilGuid, -_price, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 288);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0c94;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 290), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 290);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0c94;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _group.UpdateSumAsync(token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 292);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0c94;
				}
				AghsatMFilter filter = new AghsatMFilter
				{
					GroupGuid = _group.Guid
				};
				if (!((await GetAllAsync(ContextGuid, filter, token, tranName))?.Any() ?? false))
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _group.RemoveAsync(system, sendToServer: true, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 296);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0c94;
				}
				if (SanadNo.GetValueOrDefault() != 0)
				{
					if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, Document.AghsatM, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 302);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 303), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 303);
					}
					if (ret.HasError)
					{
						result = ret;
						goto IL_0c94;
					}
					SanadBusiness sanadBusiness = await GenerateSanadAsync(null, system, token, tranName);
					if (sanadBusiness != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, tryCount, tranName, token, new Tuple<Document, Guid>(Document.AghsatM, Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 308);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 313);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 314);
			}
		}
		finally
		{
			if (autoCommit && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 319), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 319);
			}
		}
		return ret;
		IL_0c94:
		return result;
	}

	public static Task<IEnumerable<AghsatMSummaryViewModel>> GetSummaryAsync(string contextGuid, DateTime? date1 = null, DateTime? date2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetSummaryAsync(date1, date2, token, tranName);
	}

	public static Task<int> NextId2Async(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.NextId2Async(token, tranName);
	}

	public static Task<List<AghsatMBusiness>> GetAllNotPayedAsync(string contextGuid, Guid? hGuid = null, HesabType? hesabType = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAllNotPayedAsync(hGuid, hesabType, token, tranName);
	}

	public static Task<IEnumerable<AghsatMBusiness>> GetAllAsync(string contextGuid, AghsatMFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAllAsync(token, filter, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, Priority priority, List<AghsatMBusiness> Lst, bool sendtoServer, bool awit4Send = false, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(Lst?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (AghsatMBusiness item in Lst)
			{
				current = checked(current + 1);
				if (item == null)
				{
					throw new NullReferenceException("list contains null items!");
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 343);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, priority, sendtoServer, awaitOnSemaphore: true, awit4Send, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 344);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(Lst.Count(), current, "شماره".GetTranslate() + ":" + $"{item.Id2} - {item.Description}", token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 357);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 358);
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
					percentCompleted(new PercenteCompletedEventArgs(Lst?.Count() ?? 0, Lst?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, List<AghsatMBusiness> list, CancellationToken token, Priority priority = Priority.High, bool sendtoServer = true, bool awit4Send = false, string tranName = "")
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
			ReturnedSaveFuncInfo result;
			try
			{
				if (!(list?.Any() ?? false))
				{
					result = res;
					goto end_IL_0079;
				}
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "AghsatMBusiness/SaveRangeAsync", 6000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 384), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 384);
						}
						transactionHasBeenCreated = true;
						foreach (AghsatMBusiness item in list)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, priority, sendtoServer, awaitOnSemaphore: false, awit4Send, 1, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 388);
							if (!res.HasError)
							{
								continue;
							}
							result = res;
							goto end_IL_0079;
						}
						goto end_IL_0135;
					}
					result = res.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 382);
					goto end_IL_0079;
					end_IL_0135:;
				}
				goto end_IL_0065;
				end_IL_0079:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 394);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 395);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 400), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 400);
			}
		}
		return res;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMRepository;
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 413), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 413);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AghsatMRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 415);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 418);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 421);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 422);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 427), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 427);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs"))
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
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 441);
				}
				List<WebAghsatAshkhas> list = new List<WebAghsatAshkhas>();
				List<WebAghsatAshkhas> list2 = list;
				list2.Add(await AghsatMMapper.Instance.MapAsync(ContextGuid, this, new WebAghsatAshkhas(), "", token));
				List<Packet> paks = WebAghsatAshkhas.GenerateSaveRequest(list, 0, "ارسال از کلاینت به سرور".GetTranslate());
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 447);
				}
				using AghsatMWaiter waiter = new AghsatMWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("AghsatMBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 451);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 453);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 455);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 458);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 459);
			}
			return ret;
		}
	}

	internal Task<string> GetUiDescriptionAsync()
	{
		try
		{
			return AghsatStatus switch
			{
				EnAghsatMStatus.Recieved => Task.FromResult("اقساط مشتری دریافت شده".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
				EnAghsatMStatus.NotRecieved => Task.FromResult("اقساط مشتری دریافت نشده".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
				_ => throw new NotImplementedException($"chequeSh status is not valid. {Status}"), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUiDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 476);
			throw;
		}
	}

	public static Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(string contextGuid, BedehiTalabReportFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAllReportAsync(filter, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, List<Guid> eConnectionData, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs"))
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
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "AghsatMBusiness/DeleteRangeAsync", 6000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 493), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 493);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.DeleteRangeAsync(eConnectionData, 1, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 495);
							goto end_IL_012c;
						}
						result = res.AddErrorTokenIsCancelled("", "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 492);
						goto end_IL_0095;
						end_IL_012c:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 499);
					res.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 500);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 505), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 505);
				}
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<AghsatMBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<AghsatMBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs"))
		{
			int line = 0;
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.GhestM;
			try
			{
				if (uiUpdater != null)
				{
					line = 1;
					uiUpdater.ShortMessage = "در حال ارسال اقساط اشخاص".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 530);
				}
				line = 2;
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
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 533);
				}
				line = 3;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 535);
				}
				line = 4;
				if (ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.AghsatMRepository is null");
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 540);
				}
				line = 5;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 6;
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 7;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 548);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("اقساط اشخاص سینک هستند تعداد اقساط اشخاص سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 549);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				line = 8;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						line = 9;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 558);
						}
						current = checked(current + 1);
						line = 10;
						AghsatMBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						line = 11;
						if (obj == null)
						{
							continue;
						}
						line = 12;
						if (uiUpdater != null)
						{
							line = 13;
							uiUpdater.ShortMessage = "قسط اشخاص".GetTranslate() + " " + $"{obj.FullName}-{obj._price:N0}";
							uiUpdater.Count = current;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 571);
						}
						line = 14;
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: AghsatM \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 576);
							continue;
						}
						line = 15;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 583);
						}
						line = 16;
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						line = 17;
						if (tasks.Count >= maxParalelTask)
						{
							line = 18;
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							line = 19;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 591);
							line = 20;
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 595);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 597);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 599);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 602);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 603);
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

	public static Task<int> GetCountByHGuidAsync(string contextGuid, Guid tafsilGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetCountByHGuidAsync(tafsilGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> CalculateReceptionAsync(string contextGuid, Guid receptionGuid, Guid qestGuid, decimal? receptionRemain = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatMBusiness qest = await GetAsync(contextGuid, qestGuid, token, tranName);
			if (qest == null)
			{
				return res;
			}
			decimal qestPrice = qest.TotalPrice;
			ReceptionBusiness reception = await ReceptionBusiness.GetAsync(contextGuid, receptionGuid, EnSystem.UserInterface, null, fromDatabase: true, token, tranName);
			if (reception == null)
			{
				return res;
			}
			decimal receptionSum = reception.Sum;
			if (receptionRemain.GetValueOrDefault() > 0m)
			{
				receptionSum = receptionRemain.Value;
			}
			List<AghsatMBusiness> lstaghsat = (await GetAllNotPayedAsync(contextGuid, (await qest.GetGroupAsync(token, tranName)).TafsilGuid, null, token, tranName))?.Where((AghsatMBusiness q) => q.Guid != qestGuid)?.ToList();
			Guid guid = qest.Guid;
			decimal newPrice = default(decimal);
			if (receptionSum - qestPrice > 0m)
			{
				while (receptionSum > newPrice)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtAsync(contextGuid, guid, reception.Guid, token, tranName), ReturnedState.None, "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 646);
					receptionSum -= (await GetAsync(contextGuid, guid, token, tranName)).TotalPrice;
					AghsatMBusiness aghsatMBusiness = lstaghsat?.Take(1).FirstOrDefault();
					if (aghsatMBusiness == null)
					{
						guid = Guid.Empty;
						break;
					}
					lstaghsat.Remove(aghsatMBusiness);
					newPrice = aghsatMBusiness.TotalPrice;
					guid = aghsatMBusiness.Guid;
				}
				if ((receptionSum > 0m) & (guid != Guid.Empty))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtGuidAsync(contextGuid, guid, reception.Guid, receptionSum, token, tranName), ReturnedState.None, "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 663);
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await AddNewGhestAsync(contextGuid, guid, newPrice - receptionSum, reception?.Date, token, tranName), ReturnedState.None, "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 664);
				}
			}
			else if (receptionSum - qestPrice < 0m)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ElamPardakhtGuidAsync(contextGuid, qest.Guid, reception.Guid, receptionSum, token, tranName), ReturnedState.None, "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 669);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await AddNewGhestAsync(contextGuid, qest.Guid, qestPrice - receptionSum, reception?.Date, token, tranName), ReturnedState.None, "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 670);
			}
			else
			{
				qest.ReceptionGuid = reception.Guid;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await qest.SaveAsync(EnSystem.UserInterface, default(CancellationToken), Priority.High, sendToServer: true, awaitOnSemaphore: true, await4Send: false, 10), ReturnedState.None, "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 675);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 680);
			res.AddReturnedValue(ex, "", "CalculateReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 681);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ElamPardakhtAsync(string contextGuid, Guid guid, Guid recGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatMBusiness cls = await GetAsync(contextGuid, guid, token, tranName);
			cls.ReceptionGuid = (await ReceptionBusiness.GetAsync(contextGuid, recGuid, EnSystem.UserInterface, null, fromDatabase: true, token, tranName)).Guid;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await cls.SaveAsync(EnSystem.UserInterface, token, Priority.High, sendToServer: true, awaitOnSemaphore: true, await4Send: false, 10, tranName), ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 693);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 697);
			res.AddReturnedValue(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 698);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> ElamPardakhtGuidAsync(string contextGuid, Guid guid, Guid recGuid, decimal r, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatMBusiness obj = await GetAsync(contextGuid, guid, token, tranName);
			obj.ReceptionGuid = recGuid;
			obj.Price = r;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(EnSystem.UserInterface, token, Priority.High, sendToServer: true, awaitOnSemaphore: true, await4Send: false, 10, tranName), ReturnedState.None, "ElamPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 710);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ElamPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 714);
			res.AddReturnedValue(ex, "", "ElamPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 715);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> AddNewGhestAsync(string contextGuid, Guid guid, decimal p, DateTime? receptionDate, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AghsatMBusiness cls = await GetAsync(contextGuid, guid, token, tranName);
			AghsatMBusiness aghsatMBusiness = new AghsatMBusiness(contextGuid)
			{
				Date = ((!receptionDate.HasValue) ? DateTime.Now : receptionDate.Value.AddSeconds(1.0)),
				DateSarResid = cls.DateSarResid,
				Description = (cls.Description ?? "").GetTranslate() + "باقیمانده قسط شماره ی ".GetTranslate() + $"{cls.Id2}"
			};
			AghsatMBusiness aghsatMBusiness2 = aghsatMBusiness;
			aghsatMBusiness2.Id2 = await NextId2Async(contextGuid, token, tranName);
			aghsatMBusiness.GuidGroup = cls.GuidGroup;
			aghsatMBusiness.Mounth = cls.Mounth;
			aghsatMBusiness.Price = p;
			aghsatMBusiness.Guid = Guid.NewGuid();
			AghsatMBusiness aghsatMBusiness3 = aghsatMBusiness;
			aghsatMBusiness3.Mounth = cls.Mounth;
			aghsatMBusiness3.TypePardakht = cls.TypePardakht;
			if (receptionDate.HasValue)
			{
				aghsatMBusiness3.DateSarResid = new DateTime(aghsatMBusiness3.DateSarResid.Year, aghsatMBusiness3.DateSarResid.Month, aghsatMBusiness3.DateSarResid.Day, receptionDate.Value.Hour, receptionDate.Value.Minute, receptionDate.Value.Second).AddSeconds(1.0);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await aghsatMBusiness3.SaveAsync(EnSystem.UserInterface, token, Priority.High, sendToServer: true, awaitOnSemaphore: true, await4Send: false, 10, tranName), ReturnedState.None, "AddNewGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 743);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddNewGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 747);
			res.AddReturnedValue(ex, "", "AddNewGhestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 748);
		}
		return res;
	}

	public static Task<IEnumerable<AghsatMDelayModel>> GetAllDelayedInstallmentsAsync(string contextGuid, AghsatMDelayedConfig config, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.GetAllDelayedInstallmentsAsync(config, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAggregatedAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "AghsatMBusiness/SaveAggregatedAsync", 6000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 763);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 765), ReturnedState.None, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 765);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await _group.SaveAsync(system, dis.Token, Priority.High, sendToServer: true, await4Send: false, awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 768);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(system, dis.Token, Priority.High, sendToServer: true, awaitOnSemaphore: false, await4Send: false, 10, tranName), ReturnedState.None, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 770);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 772), ReturnedState.None, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 772);
								if (!res.HasError)
								{
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await _group.UpdateSumAsync(token, tranName), ReturnedState.None, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 774);
									goto end_IL_0124;
								}
								result = res;
							}
						}
					}
					goto end_IL_007d;
					end_IL_0124:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 778);
				res.AddReturnedValue(ex, "", "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 779);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 784), ReturnedState.None, "SaveAggregatedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 784);
			}
		}
		return res;
	}

	private async Task<string> GetSanadDescriptionAsync(AghsatMDelayModel model, CancellationToken token, string tranName)
	{
		try
		{
			string text = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Installments.VajhEltezam.VajhEltezamSanadDescription;
			if (text.Contains("[نام طرف حساب]"))
			{
				text = text.Replace("[نام طرف حساب]", model?.TafsilName ?? "");
			}
			if (text.Contains("[شماره قسط]"))
			{
				text = text.Replace("[شماره قسط]", Id2.ToString());
			}
			if (text.Contains("[تاریخ سررسید]"))
			{
				text = text.Replace("[تاریخ سررسید]", DateSarResidSh);
			}
			if (text.Contains("[مبلغ قسط]"))
			{
				text = text.Replace("[مبلغ قسط]", _price.NpToString("N0", 0));
			}
			if (text.Contains("[تعداد روز تاخیر]"))
			{
				text = text.Replace("[تعداد روز تاخیر]", (model?.DelayDayCount ?? 0).ToString());
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 803);
			throw;
		}
	}

	private async Task<SanadBusiness> GenerateSanadAsync(AghsatMDelayModel model, EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs"))
		{
			_ = 13;
			try
			{
				await GetGroupAsync(token, tranName);
				string description = await GetSanadDescriptionAsync(model, token, tranName);
				if (SanadNo.GetValueOrDefault() <= 0)
				{
					SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
				}
				int oldSanadNumber = (await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName))?.SanadNo.GetValueOrDefault() ?? 0;
				Guid oldGuid = Guid.NewGuid();
				SanadBusiness sanad2 = await SanadBusiness.GetAsync(ContextGuid, SanadNo.Value, token, tranName);
				if (sanad2 != null)
				{
					oldGuid = sanad2.Guid;
				}
				sanad2 = new SanadBusiness(ContextGuid, oldSanadNumber)
				{
					Description = description,
					DateM = DateTime.Now,
					UserGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid),
					Guid = oldGuid,
					SanadType = SanadType.Automatic
				};
				Sanad_DetBusiness debit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AghsatM, sanad2.NextOrdinal())
				{
					Guid = Guid.NewGuid(),
					Credit = 0m,
					Debit = VajhEltezam,
					MasterGuid = sanad2.Guid,
					RowDesc = description
				};
				Sanad_DetBusiness sanad_DetBusiness = debit;
				await sanad_DetBusiness.SetMoeinGuidAsync(await _group.GetMoeinGuidAsync(system, token, tranName), token, tranName);
				await debit.SetTafsilGuidAsync(_group.TafsilGuid, tranName, token);
				await sanad2.AddToListSanadAsync(debit, system, token);
				Sanad_DetBusiness credit = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AghsatM, sanad2.NextOrdinal())
				{
					Guid = Guid.NewGuid(),
					Credit = VajhEltezam,
					Debit = 0m,
					MasterGuid = sanad2.Guid,
					RowDesc = description
				};
				await credit.SetMoeinGuidAsync((await GetCreditMoeinGuidAsync(token)).Value, token, tranName);
				sanad_DetBusiness = credit;
				await sanad_DetBusiness.SetTafsilGuidAsync(await GetCreditTafsilGuidAsync(token), tranName, token);
				await sanad2.AddToListSanadAsync(credit, system, token);
				return sanad2;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 857);
				throw;
			}
		}
	}

	private async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(AghsatMDelayModel model, EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs"))
		{
			_ = 7;
			try
			{
				if (system == EnSystem.Transfer)
				{
					return null;
				}
				string description = await GetSanadDescriptionAsync(model, token, tranName);
				await GetGroupAsync(token, tranName);
				List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid);
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.DebitMoeinGuid = await _group.GetMoeinGuidAsync(system, token, tranName);
				viewSanadDetailCoverBusiness.DebitTafsilGuid = _group.TafsilGuid;
				viewSanadDetailCoverBusiness.Date = DateTime.Now;
				viewSanadDetailCoverBusiness.AsGuid = Guid;
				viewSanadDetailCoverBusiness.AsType = DocumentDet.AghsatM;
				viewSanadDetailCoverBusiness.AsTypeDocument = Document.AghsatM;
				viewSanadDetailCoverBusiness.Debit = VajhEltezam;
				viewSanadDetailCoverBusiness.Credit = 0m;
				viewSanadDetailCoverBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				viewSanadDetailCoverBusiness.SanadNo = SanadNo.GetValueOrDefault();
				viewSanadDetailCoverBusiness.SanadSt = SanadSt.Movaghat;
				viewSanadDetailCoverBusiness.Id2 = Id2;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = await GetCreditMoeinGuidAsync(token);
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = await GetCreditTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.SanadDesc = description;
				viewSanadDetailCoverBusiness.RDesc = description;
				viewSanadDetailCoverBusiness.DateMoaser = DateSarResid;
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
				viewSanadDetailCoverBusiness6.DebitMoeinGuid = await _group.GetMoeinGuidAsync(system, token, tranName);
				viewSanadDetailCoverBusiness5.DebitTafsilGuid = _group.TafsilGuid;
				viewSanadDetailCoverBusiness5.Date = DateTime.Now;
				viewSanadDetailCoverBusiness5.AsGuid = Guid;
				viewSanadDetailCoverBusiness5.AsType = DocumentDet.AghsatM;
				viewSanadDetailCoverBusiness5.AsTypeDocument = Document.AghsatM;
				viewSanadDetailCoverBusiness5.Debit = 0m;
				viewSanadDetailCoverBusiness5.Credit = VajhEltezam;
				viewSanadDetailCoverBusiness5.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				viewSanadDetailCoverBusiness5.SanadNo = SanadNo.GetValueOrDefault();
				viewSanadDetailCoverBusiness5.SanadSt = SanadSt.Movaghat;
				viewSanadDetailCoverBusiness5.Id2 = Id2;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness7 = viewSanadDetailCoverBusiness5;
				viewSanadDetailCoverBusiness7.CreditMoeinGuid = await GetCreditMoeinGuidAsync(token);
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness8 = viewSanadDetailCoverBusiness5;
				viewSanadDetailCoverBusiness8.CreditTafsilGuid = await GetCreditTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness5.SanadDesc = description;
				viewSanadDetailCoverBusiness5.RDesc = description;
				viewSanadDetailCoverBusiness5.DateMoaser = DateSarResid;
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
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 948);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SavePenaltySanadAsync(string contextGuid, EnSystem system, List<AghsatMDelayModel> list, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 8;
			ReturnedSaveFuncInfo result;
			try
			{
				if (!(list?.Any() ?? false))
				{
					result = res;
					goto end_IL_00aa;
				}
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "AghsatMBusiness/SavePenaltySanadAsync", 6000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 964), ReturnedState.None, "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 964);
						}
						transactionHasBeenCreated = true;
						foreach (AghsatMDelayModel item in list)
						{
							if ((item?.Guid ?? Guid.Empty) == Guid.Empty)
							{
								continue;
							}
							AghsatMBusiness installment = await GetAsync(contextGuid, item.Guid, token, tranName);
							if (installment == null)
							{
								continue;
							}
							installment.VajhEltezam = item.PenaltyPrice;
							installment.VajhEltezamModified = DateTime.Now;
							installment.Modified = DateTime.Now;
							installment.ServerStatus = ServerStatusForClient.None;
							SanadBusiness sanad = await installment.GenerateSanadAsync(item, system, token, tranName);
							if (sanad == null)
							{
								continue;
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await sanad.CheckValidationAsync(system, true, tranName, false, token, new Tuple<Document, Guid>(Document.AghsatM, installment.Guid)), ReturnedState.None, "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 980);
							if (res.HasError)
							{
								continue;
							}
							if (installment.SanadNo <= 0)
							{
								AghsatMBusiness aghsatMBusiness = installment;
								aghsatMBusiness.SanadNo = await SanadBusiness.NextIdAsync(contextGuid, token, tranName);
							}
							sanad.SanadNo = installment.SanadNo.Value;
							List<ViewSanadDetailCoverBusiness> lst = await installment.GetSanadCoverListAsync(item, system, token, tranName);
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.AghsatM, installment.Guid)), ReturnedState.None, "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 986);
							if (res.HasError)
							{
								continue;
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.SaveAsync(installment, 1, token, tranName), ReturnedState.None, "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 989);
							if (!res.HasError)
							{
								Task.Run(() => installment.Send2ServerAsync(token, system, Priority.High));
							}
						}
						goto end_IL_016d;
					}
					result = res.AddErrorTokenIsCancelled("", "", "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 962);
					goto end_IL_00aa;
					end_IL_016d:;
				}
				goto end_IL_0096;
				end_IL_00aa:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 996);
				res.AddReturnedValue(ex, "", "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 997);
				goto end_IL_0096;
			}
			return result;
			end_IL_0096:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1002), ReturnedState.None, "SavePenaltySanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1002);
			}
			if (!res.HasError)
			{
				ClsCache.GetCache(contextGuid).BusinessAggregator.Events.AghsatM.RaiseGhestSaved(null, EventArgs.Empty);
			}
		}
		return res;
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[تعداد]".GetTranslate()),
			new SmsTemplateRecord("[پردازنده]".GetTranslate()),
			new SmsTemplateRecord("[شماره قسط]".GetTranslate()),
			new SmsTemplateRecord("[تاریخ قسط]".GetTranslate()),
			new SmsTemplateRecord("[مبلغ قسط]".GetTranslate())
		};
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateReceptionInfoAsync(string contextGuid, Guid? receptionGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "AghsatMBusiness/UpdateReceptionInfoAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1027), ReturnedState.None, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1027);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.AghsatMRepository.UpdateReceptionInfoAsync(receptionGuid, token, tranName), ReturnedState.None, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1029);
						goto end_IL_010d;
					}
					result = res.AddErrorTokenIsCancelled("", "", "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1025);
					goto end_IL_0076;
					end_IL_010d:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1033);
				res.AddReturnedValue(ex, "", "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1034);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1039), ReturnedState.None, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Aghsat\\AghsatMBusiness.cs", 1039);
			}
		}
		return res;
	}
}
