using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Department;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Department;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Department;

namespace EntityCache.Business.Department;

public class LkInfoBusiness : ILkInfo, IHasModified, IHasStatus, IDisposable, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private CusCompactBusiness customerInfo;

	public AccountingPackage AccountingPackage { get; set; }

	public ENCode TypeAccounting { get; set; }

	public HozoorPackage HozoorType { get; set; }

	public long LKSerial { get; set; }

	public DateTime ExpireDate { get; set; }

	public Guid TafsilGuid { get; set; }

	public DateTime StartDate { get; set; }

	public string Description { get; set; }

	public bool Tolid { get; set; }

	public bool BarCode { get; set; }

	public bool Scale { get; set; }

	public bool SpecialAccountingPackage { get; set; }

	public bool IsOffline { get; set; }

	public TypeHozor Hozor_Hoghogh { get; set; }

	public short ScaleCount { get; set; }

	public short MaxActivationDays { get; set; }

	public short Net_Accounting { get; set; }

	public short Net_Hozor { get; set; }

	public short Net_Scales { get; set; }

	public bool CallerID { get; set; }

	public DateTime? FailedDate { get; set; }

	public string FailedDescription { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public int RemainingDays => Calendar.RemainingDays(ExpireDate);

	public int UsedDays => Calendar.UsedDays(ExpireDate);

	public DateTime? DisableActivationCodeDate { get; set; }

	public EnLK_SpecialFacilities SpecialFacilities { get; set; }

	public string URLTamdid => $"http://npsms.ir/active.aspx?LK={LKSerial}";

	[XmlIgnore]
	[JsonIgnore]
	public bool IsFailed => !string.IsNullOrEmpty(FailedDescription);

	[XmlIgnore]
	[JsonIgnore]
	public string StrHozor_Hoghogh
	{
		get
		{
			return Hozor_Hoghogh.GetDisplay();
		}
		set
		{
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public string StrTypeAccounting
	{
		get
		{
			return TypeAccounting.GetDisplay();
		}
		set
		{
		}
	}

	public string ExpireDateSh => ExpireDate.MiladiToCalture();

	public string StartDateSh => StartDate.MiladiToCalture();

	[XmlIgnore]
	[JsonIgnore]
	public string TypeLock
	{
		get
		{
			string text = "";
			try
			{
				if (TypeAccounting != 0)
				{
					text = text + "حسابداری سطح ".GetTranslate() + ":" + TypeAccounting.GetDisplay().GetTranslate();
				}
				if (Hozor_Hoghogh != 0)
				{
					text = text + " سطح حضور و غیاب".GetTranslate() + ": " + Hozor_Hoghogh.GetDisplay().GetTranslate();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TypeLock", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 90);
			}
			return text;
		}
	}

	public LkInfoMeta MetaData { get; set; }

	public Guid Guid { get; set; }

	public byte[] Image
	{
		get
		{
			try
			{
				if (IsOffline)
				{
					return ImageResourceManager.IsOffline;
				}
				return ImageResourceManager.IsOnline;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Image", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 105);
			}
			return ImageResourceManager.ServerStatus_None;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public bool HasTax => (SpecialFacilities & EnLK_SpecialFacilities.Tax) == EnLK_SpecialFacilities.Tax;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasAghsat => (SpecialFacilities & EnLK_SpecialFacilities.Aghsat) == EnLK_SpecialFacilities.Aghsat;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasCheque => (SpecialFacilities & EnLK_SpecialFacilities.Cheque) == EnLK_SpecialFacilities.Cheque;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasCustomerClub => (SpecialFacilities & EnLK_SpecialFacilities.CustomerClub) == EnLK_SpecialFacilities.CustomerClub;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasProductSerial => (SpecialFacilities & EnLK_SpecialFacilities.ProductSerial) == EnLK_SpecialFacilities.ProductSerial;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasAdvancedFinance => (SpecialFacilities & EnLK_SpecialFacilities.AdvancedFinance) == EnLK_SpecialFacilities.AdvancedFinance;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasVoiceCall => (SpecialFacilities & EnLK_SpecialFacilities.VoiceCall) == EnLK_SpecialFacilities.VoiceCall;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasDepot => (SpecialFacilities & EnLK_SpecialFacilities.Depot) == EnLK_SpecialFacilities.Depot;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasMultiCurrency => (SpecialFacilities & EnLK_SpecialFacilities.MultiCurrency) == EnLK_SpecialFacilities.MultiCurrency;

	[XmlIgnore]
	[JsonIgnore]
	public bool HasService => (SpecialFacilities & EnLK_SpecialFacilities.Service) == EnLK_SpecialFacilities.Service;

	public LkInfoBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public void SetCustomerInfo(CusCompactBusiness value)
	{
		customerInfo = value;
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs");
	}

	public async Task<string> GetTafsilNameAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs"))?.Name;
	}

	public async Task<CusCompactBusiness> GetCustomerInfoAsync(EnSystem system, string tranName, CancellationToken token)
	{
		try
		{
			if (customerInfo == null)
			{
				customerInfo = await CusCompactBusiness.GetAsync(ContextGuid, system, TafsilGuid, null, tranName, token);
			}
			return customerInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCustomerInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 138);
			return null;
		}
	}

	public async Task<string> GetCustomerFullNameAsync(EnSystem system, string tranName, CancellationToken token)
	{
		return (await GetCustomerInfoAsync(system, tranName, token))?.FullName ?? "";
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool flag = string.IsNullOrEmpty(tranName);
		if (flag)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool flag2 = false;
		try
		{
			throw new NotImplementedException();
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 158);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 161);
			ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 162);
		}
		finally
		{
			if (flag && flag2)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 167), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 167);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(string formOrWay, CancellationToken token, EnSystem system, Priority priority = Priority.High)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "LkInfoBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 181);
				}
				List<WebLkInfo> list = new List<WebLkInfo>();
				List<WebLkInfo> list2 = list;
				list2.Add(await ToWebItemAsync());
				List<Packet> lstPacket = WebLkInfo.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 183);
				}
				using LkInfoWaiter waiter = new LkInfoWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), LKSerial, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ret.AddReturnedValue(await waiter.WaitForSaveResponse(), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 187);
				if (!ret.HasError)
				{
					ServerStatus = ServerStatusForClient.Delivered;
				}
				DeliveryDate = DateTime.Now;
				DeliveryMessage = ret.ErrorMessage;
				DeliveryShortMessage = ret.ErrorMessage;
				Modified = DateTime.Now;
				await SaveAsync(formOrWay, EnSystem.SignalR, CashierBusiness.GetCurrentCashierGuid(ContextGuid), sendToServer: false, Priority.Low, awaitOnSemaphore: true, await4send: false, 10, "", token);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 197);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 200);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 201);
			}
			return ret;
		}
	}

	public async Task<WebLkInfo> ToWebItemAsync()
	{
		using (new JaegerMonitor("ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs"))
		{
			try
			{
				return await LkInfoMapper.Instance.MapAsync(ContextGuid, this, new WebLkInfo());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 217);
				return null;
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public static Task<LkInfoBusiness> GetAsync(string contextGuid, long Serial, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.LkInfo.GetAsync(Serial, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string formOrWay, EnSystem system, Guid cashierGuid, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool await4send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		WebLkInfo oldLkInfo = null;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "LkInfoBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 238);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).DptDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 239), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 239);
						}
						LkInfoBusiness lkInfoBusiness = await GetAsync(ContextGuid, LKSerial);
						if (system != EnSystem.SignalR && lkInfoBusiness != null)
						{
							oldLkInfo = await lkInfoBusiness.ToWebItemAsync();
						}
						sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
						transactionHasBeenCreated = true;
						if (!ret.HasError)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.LkInfo.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 246);
							goto end_IL_0163;
						}
						result = ret;
					}
					goto end_IL_009e;
					end_IL_0163:;
				}
				goto end_IL_008a;
				end_IL_009e:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 248);
				goto end_IL_008a;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 251);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 252);
				goto end_IL_008a;
			}
			return result;
			end_IL_008a:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 257), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 257);
			}
			if (!ret.HasError)
			{
				if (system != EnSystem.SignalR)
				{
					SaveLogAsync(ContextGuid, system, cashierGuid, this, oldLkInfo, formOrWay);
				}
				if (sendToServer)
				{
					if (await4send)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(formOrWay, token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 267);
					}
					else
					{
						Send2ServerAsync(formOrWay, token, system, priority);
					}
				}
			}
		}
		return ret;
	}

	private static async Task SaveLogAsync(string contextGuid, EnSystem system, Guid cashierGuid, LkInfoBusiness lkInfoBusiness, WebLkInfo oldLkInfo, string formOrWay, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			WebLkInfo newLkInfo = await lkInfoBusiness.ToWebItemAsync();
			string changes = GetChanges(newLkInfo, oldLkInfo, formOrWay);
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CashiersLogBusiness log = new CashiersLogBusiness(contextGuid)
			{
				CashierGuid = cashierGuid,
				Action = ENAction.Edit,
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = contextGuid,
				DateM = DateTime.Now,
				Title = "ویرایش قفل".GetTranslate() + " " + $" {newLkInfo.LKSerial} \r\n {changes}",
				System = system,
				Status = true,
				ServerStatus = ServerStatusForClient.None,
				SanadType = EnLog.LkInfo,
				SanadGuid = Guid.Empty,
				ApplicationVersion = cache.ApplicationVersion,
				Source = cache.Source
			};
			if (oldLkInfo != null)
			{
				byte[] data = Json.ToBytesJson(oldLkInfo);
				CashiersLogBusiness cashiersLogBusiness = log;
				cashiersLogBusiness.NewSerializedData = await GZip.Compress(data);
			}
			if (newLkInfo != null)
			{
				byte[] data2 = Json.ToBytesJson(newLkInfo);
				CashiersLogBusiness cashiersLogBusiness = log;
				cashiersLogBusiness.OldSerializedData = await GZip.Compress(data2);
			}
			await log.SaveAsync(system, sendToServer: true, awaitOnSemaphore: true, token, 10);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 310);
		}
	}

	private static string GetChanges(WebLkInfo newLkInfo, WebLkInfo oldLkInfo, string formOrWay)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		try
		{
			if (!string.IsNullOrEmpty(formOrWay))
			{
				stringBuilder.Append("روش".GetTranslate() + ": " + formOrWay);
			}
			if (oldLkInfo == null)
			{
				stringBuilder.AppendLine("تعریف قفل".GetTranslate() + $" {newLkInfo.LKSerial}");
				if (oldLkInfo == null)
				{
					return stringBuilder.ToString();
				}
				if (newLkInfo.Tolid && !oldLkInfo.Tolid)
				{
					stringBuilder.AppendLine("تولید فعال شد".GetTranslate());
				}
				if (!newLkInfo.Tolid && oldLkInfo.Tolid)
				{
					stringBuilder.AppendLine("تولید غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialTax && !oldLkInfo.HasSpecialTax)
				{
					stringBuilder.AppendLine("سامانه مودیان فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialTax && oldLkInfo.HasSpecialTax)
				{
					stringBuilder.AppendLine("سامانه مودیان غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialProductSerial && !oldLkInfo.HasSpecialProductSerial)
				{
					stringBuilder.AppendLine("کالا سریال دار فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialProductSerial && oldLkInfo.HasSpecialProductSerial)
				{
					stringBuilder.AppendLine("کالا سریال دار غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialInstallment && !oldLkInfo.HasSpecialInstallment)
				{
					stringBuilder.AppendLine("اقساط فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialInstallment && oldLkInfo.HasSpecialInstallment)
				{
					stringBuilder.AppendLine("اقساط غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialMultiCurrency && !oldLkInfo.HasSpecialMultiCurrency)
				{
					stringBuilder.AppendLine("چند ارزی فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialMultiCurrency && oldLkInfo.HasSpecialMultiCurrency)
				{
					stringBuilder.AppendLine("چند ارزی غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialCustomerClub && !oldLkInfo.HasSpecialCustomerClub)
				{
					stringBuilder.AppendLine(" باشگاه مشتریان فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialCustomerClub && oldLkInfo.HasSpecialCustomerClub)
				{
					stringBuilder.AppendLine("باشگاه مشتریان غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialCheque_Khazane && !oldLkInfo.HasSpecialCheque_Khazane)
				{
					stringBuilder.AppendLine(" چک و خزانه فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialCheque_Khazane && oldLkInfo.HasSpecialCheque_Khazane)
				{
					stringBuilder.AppendLine("چک و خزانه غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialProductDepot_Pishrafteh && !oldLkInfo.HasSpecialProductDepot_Pishrafteh)
				{
					stringBuilder.AppendLine(" انبار پیشرفته فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialProductDepot_Pishrafteh && oldLkInfo.HasSpecialProductDepot_Pishrafteh)
				{
					stringBuilder.AppendLine("انبار پیشرفته غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialService && !oldLkInfo.HasSpecialService)
				{
					stringBuilder.AppendLine(" سرویس خدمات فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialService && oldLkInfo.HasSpecialService)
				{
					stringBuilder.AppendLine("سرویس خدمات غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialVoiceCall && !oldLkInfo.HasSpecialVoiceCall)
				{
					stringBuilder.AppendLine(" فراخوان صوتی فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialVoiceCall && oldLkInfo.HasSpecialVoiceCall)
				{
					stringBuilder.AppendLine("فراخوان صوتی غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.HasSpecialAccounting && !oldLkInfo.HasSpecialAccounting)
				{
					stringBuilder.AppendLine(" مالی پیشرفته فعال شد".GetTranslate());
				}
				if (!newLkInfo.HasSpecialAccounting && oldLkInfo.HasSpecialAccounting)
				{
					stringBuilder.AppendLine("مالی پیشرفته غیر فعال شد".GetTranslate());
				}
				if (newLkInfo.BarCode)
				{
					stringBuilder.AppendLine("بارکد فعال".GetTranslate());
				}
				if (newLkInfo.Scale)
				{
					stringBuilder.AppendLine("ترازو فعال".GetTranslate());
				}
				if (newLkInfo.CallerID)
				{
					stringBuilder.AppendLine("کالر آی دی فعال".GetTranslate());
				}
				if (newLkInfo.IsFailed)
				{
					stringBuilder.AppendLine("وضعیت مسدود شده".GetTranslate());
				}
				if (newLkInfo.TypeAccounting != 0)
				{
					stringBuilder.AppendLine("حسابداری".GetTranslate() + ":" + newLkInfo.TypeAccounting.GetDisplay().GetTranslate());
				}
				if (newLkInfo.HozoorType != 0)
				{
					stringBuilder.AppendLine("پیکج حضورغیاب".GetTranslate() + ":" + newLkInfo.HozoorType.GetDisplay().GetTranslate());
				}
				if (newLkInfo.Hozor_Hoghogh != 0)
				{
					stringBuilder.AppendLine("حضورغیاب".GetTranslate() + ":" + newLkInfo.Hozor_Hoghogh.GetDisplay().GetTranslate());
				}
				if (newLkInfo.Net_Accounting != 0)
				{
					stringBuilder.AppendLine("کلاینت شبکه حسابداری".GetTranslate() + ":" + $"{newLkInfo.Net_Accounting}");
				}
				if (newLkInfo.Net_Hozor != 0)
				{
					stringBuilder.AppendLine("کلاینت شبکه حضورغیاب".GetTranslate() + ":" + $"{newLkInfo.Net_Hozor}");
				}
				if (newLkInfo.Net_Scales != 0)
				{
					stringBuilder.AppendLine("تعداد کلاینت شبکه ترازو".GetTranslate() + ":" + $"{newLkInfo.Net_Scales}");
				}
				if (newLkInfo.ScaleCount != 0)
				{
					stringBuilder.AppendLine("کلاینت ترازو فعال".GetTranslate() + ":" + $"{newLkInfo.ScaleCount}");
				}
				stringBuilder.AppendLine("تاریخ انقضاء".GetTranslate() + ": " + newLkInfo.ExpireDateSh);
				return stringBuilder.ToString();
			}
			stringBuilder.AppendLine("ویرایش قفل".GetTranslate() + $" {newLkInfo.LKSerial}");
			if (newLkInfo.Tolid != oldLkInfo.Tolid)
			{
				if (oldLkInfo.Tolid)
				{
					stringBuilder.AppendLine("حذف زیر بسته تولید".GetTranslate());
				}
				else
				{
					stringBuilder.AppendLine("افزودن زیر بسته تولید".GetTranslate());
				}
			}
			if (newLkInfo.BarCode != oldLkInfo.BarCode)
			{
				if (oldLkInfo.BarCode)
				{
					stringBuilder.AppendLine("حذف زیر بسته بارکد".GetTranslate());
				}
				else
				{
					stringBuilder.AppendLine("افزودن زیر بسته بارکد".GetTranslate());
				}
			}
			if (newLkInfo.CallerID != oldLkInfo.CallerID)
			{
				if (oldLkInfo.CallerID)
				{
					stringBuilder.AppendLine("حذف زیر بسته کالر آی دی".GetTranslate());
				}
				else
				{
					stringBuilder.AppendLine("افزودن زیر بسته کالر آی دی".GetTranslate());
				}
			}
			if (newLkInfo.Scale != oldLkInfo.Scale)
			{
				if (oldLkInfo.Scale)
				{
					stringBuilder.AppendLine("حذف زیر بسته ترازو".GetTranslate());
				}
				else
				{
					stringBuilder.AppendLine("افزودن زیر بسته ترازو".GetTranslate());
				}
			}
			if (newLkInfo.IsFailed != oldLkInfo.IsFailed)
			{
				if (oldLkInfo.IsFailed)
				{
					stringBuilder.AppendLine("فعال کردن قفل مسدود شده".GetTranslate());
				}
				else
				{
					stringBuilder.AppendLine("مسدود کردن قفل به علت".GetTranslate() + newLkInfo.FailedDescription);
				}
			}
			if (newLkInfo.ExpireDateSh != oldLkInfo.ExpireDateSh)
			{
				stringBuilder.AppendLine("تغییر تاریخ انقضاء از".GetTranslate() + " " + oldLkInfo.ExpireDateSh + " به".GetTranslate() + " " + newLkInfo.ExpireDateSh);
			}
			if (newLkInfo.TypeAccounting != oldLkInfo.TypeAccounting)
			{
				stringBuilder.AppendLine("تغییر نسخه حسابداری از ".GetTranslate() + oldLkInfo.TypeAccounting.GetDisplay().GetTranslate() + " به".GetTranslate() + "  " + newLkInfo.TypeAccounting.GetDisplay());
			}
			if (newLkInfo.HozoorType != oldLkInfo.HozoorType)
			{
				stringBuilder.AppendLine("تغییر پکیج حضور غیاب از".GetTranslate() + (oldLkInfo.HozoorType.GetDisplay() ?? "").GetTranslate() + " به".GetTranslate() + newLkInfo.HozoorType.GetDisplay().GetTranslate());
			}
			if (newLkInfo.Hozor_Hoghogh != oldLkInfo.Hozor_Hoghogh)
			{
				stringBuilder.AppendLine("تغییر نسخه حضور غیاب از".GetTranslate() + oldLkInfo.Hozor_Hoghogh.GetDisplay().GetTranslate() + " به".GetTranslate() + newLkInfo.Hozor_Hoghogh.GetDisplay().GetTranslate());
			}
			if (newLkInfo.Net_Accounting != oldLkInfo.Net_Accounting)
			{
				stringBuilder.AppendLine("تغییر در تعداد کلاینت شبکه حسابداری".GetTranslate() + $"{oldLkInfo.Net_Accounting}".GetTranslate() + " به".GetTranslate() + $"{newLkInfo.Net_Accounting}".GetTranslate());
			}
			if (newLkInfo.Net_Hozor != oldLkInfo.Net_Hozor)
			{
				stringBuilder.AppendLine("تغییر در تعداد کلاینت شبکه حضور".GetTranslate() + $" {oldLkInfo.Net_Hozor}".GetTranslate() + " به".GetTranslate() + $"  {newLkInfo.Net_Hozor}".GetTranslate());
			}
			if (newLkInfo.Net_Scales != oldLkInfo.Net_Scales)
			{
				stringBuilder.AppendLine("تغییر در تعداد کلاینت شبکه ترازو ".GetTranslate() + $"{oldLkInfo.Net_Scales}".GetTranslate() + " به ".GetTranslate() + $" {newLkInfo.Net_Scales}".GetTranslate());
			}
			if (newLkInfo.ScaleCount != oldLkInfo.ScaleCount)
			{
				stringBuilder.AppendLine("تغییر در تعداد ترازو".GetTranslate() + $" {oldLkInfo.ScaleCount}".GetTranslate() + " به".GetTranslate() + $"  {newLkInfo.ScaleCount}".GetTranslate());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetChanges", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 422);
			stringBuilder.AppendLine(ex.Message);
		}
		return stringBuilder.ToString();
	}

	public static async Task<List<LkInfoBusiness>> GetAllAsync(string contextGuid, CancellationToken token, LkModel filters = null, string tranName = "")
	{
		List<LkInfoBusiness> ret = null;
		try
		{
			token.ThrowIfCancellationRequested();
			if (filters == null)
			{
				filters = new LkModel
				{
					BeginExpireDate = DateTime.Now.AddYears(-15)
				};
			}
			token.ThrowIfCancellationRequested();
			ret = await ClsCache.GetCache(contextGuid).CacheRepository.LkInfo.GetAllAsync(filters, token, tranName);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 442);
		}
		return ret;
	}

	public static async Task<List<LkInfoBusiness>> SearchAsync(IEnumerable<LkInfoBusiness> lstSource, string searchString, CancellationToken token)
	{
		IEnumerable<LkInfoBusiness> TempLst = lstSource;
		try
		{
			if (TempLst?.Any() ?? false)
			{
				if (searchString.ParseToLong(0L) <= 0)
				{
					await Task.WhenAll(TempLst.Select((LkInfoBusiness p) => p.GetCustomerInfoAsync(EnSystem.UserInterface, "", token)));
					TempLst = TempLst.Where((LkInfoBusiness p) => p.customerInfo != null && p.customerInfo.FullName.Contains(searchString));
				}
				else
				{
					TempLst = TempLst.Where((LkInfoBusiness p) => p.LKSerial.ToString().Contains(searchString));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 460);
		}
		return TempLst?.ToList();
	}

	public static ReturnedSaveFuncInfo Check(string softSerail, ILkInfo LK)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (LK != null && LK.IsFailed)
			{
				returnedSaveFuncInfo.AddError("مشترک گرامی،".GetTranslate() + "\r\n" + "قفل شما با سریال ".GetTranslate() + LK.LKSerial + "\r\n" + "بعلت ".GetTranslate() + LK.FailedDescription + "\r\n" + "1جدید!_مسدود میباشد.".GetTranslate(), "", "Check", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 470);
			}
			if (LK != null && LK.TafsilGuid == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("قفل با شماره سریال".GetTranslate() + LK.LKSerial + "\r\n" + "در سیستم تعریف نشده است.".GetTranslate() + "\r\nTafsilGuid is Empty!", "", "Check", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 478);
			}
			else if (LK != null && LK.IsFailed)
			{
				returnedSaveFuncInfo.AddError(LK.FailedDescription, "", "Check", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 483);
			}
			else if (softSerail.Length < 10)
			{
				returnedSaveFuncInfo.AddError("سریال نرم افزار وارد شده معتبر نمیباشد".GetTranslate(), "", "Check", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 485);
			}
			if (LK == null)
			{
				returnedSaveFuncInfo.AddError("قفل در سیستم تعریف نشده است".GetTranslate(), "", "Check", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 487);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Check", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 489);
		}
		return returnedSaveFuncInfo;
	}

	public static async Task<List<LkInfoBusiness>> GetLKOnList(string contextGuid, long selectedLkLkSerial)
	{
		List<LkInfoBusiness> Lstlk = new List<LkInfoBusiness>();
		try
		{
			LkInfoBusiness lkInfoBusiness = await GetAsync(contextGuid, selectedLkLkSerial);
			if (lkInfoBusiness != null)
			{
				Lstlk.Add(lkInfoBusiness);
			}
			else
			{
				Lstlk = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLKOnList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 520);
			Lstlk = null;
		}
		return Lstlk;
	}

	public static Task<int> GetAllGuidsDontLock(string contextGuid)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.LkInfo.GetAllGuidsDontLock();
	}

	public static Task<int> GetAllGuidsHasLock(string contextGuid)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.LkInfo.GetAllGuidsHasLock();
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		EnSystem system = EnSystem.SignalR;
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.LkInfo;
			try
			{
				if (!ClsCache.GetCache(contextGuid).Configs.Any((EnCacheConfig p) => p == EnCacheConfig.Department))
				{
					return ret;
				}
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال قفل ها".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.None;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 548);
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
					return ret.AddError(system, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 549);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 551);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.LkInfo == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ILKInfo is null");
				}
				List<long> list = (await ClsCache.GetCache(contextGuid).CacheRepository.LkInfo.GetAllNotSentAsync(tranName)) ?? new List<long>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 558);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("قفل ها سینک هستند تعداد قفل های سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 559);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 560);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (long item in list)
					{
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 568);
						}
						LkInfoBusiness lkInfoBusiness = await GetAsync(contextGuid, item, token, tranName);
						if (string.IsNullOrEmpty(lkInfoBusiness.ContextGuid))
						{
							throw new NullReferenceException("ContextGuid is null or empty!!");
						}
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = $"قفل جاری: {lkInfoBusiness?.LKSerial}";
							uiUpdater.Count = current;
						}
						if (lkInfoBusiness.ServerStatus == ServerStatusForClient.Delivered || lkInfoBusiness.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: LkInfo \r\n" + $"LkSerial:{lkInfoBusiness?.LKSerial ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 579);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 584);
						}
						tasks.Add(lkInfoBusiness.Send2ServerAsync("ارسال قفل های ارسال نشده به هاست".GetTranslate(), token, EnSystem.SignalR));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 589);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 593);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 595);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 598);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoBusiness.cs", 599);
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
}
