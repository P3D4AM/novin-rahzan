using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.MetaData.CashierLog;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.TransferLayer.Waiters.Accounting;

namespace EntityCache.Business.Accounting;

public class CashiersLogBusiness : BaseBusiness<CashiersLogBusiness>, ICashierLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<CashiersLogBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid SanadGuid { get; set; }

	public EnLog SanadType { get; set; }

	[JsonIgnore]
	public string SanadTypeName => SanadType.GetDisplay();

	public string Title { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string DateSH => DateM.MiladiToCalture();

	[JsonIgnore]
	public string Time => $"{DateM.Hour}:{DateM.Minute}:{DateM.Second}";

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public byte[]? OldSerializedData { get; set; }

	public byte[]? NewSerializedData { get; set; }

	public ENAction Action { get; set; }

	[JsonIgnore]
	public string ActionName => Action.GetDisplay();

	public EnSystem System { get; set; }

	[JsonIgnore]
	public string SystemName => System.GetDisplay();

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public DateTime? DateUtc { get; set; }

	public short StatusId { get; set; }

	public ReturnedState Level { get; set; }

	public CashierLogMetaData MetaData { get; set; }

	public ENSource Source { get; set; }

	[JsonIgnore]
	public string SourceForDisplay => Source.GetDisplay();

	public string ApplicationVersion { get; set; }

	[JsonIgnore]
	public byte[] SourceImage
	{
		get
		{
			if (Source == ENSource.Baje)
			{
				return ImageResourceManager.SourceBaje;
			}
			if (Source == ENSource.Android_HesabYar)
			{
				return ImageResourceManager.SourceHesabYar;
			}
			if (Source == ENSource.CrmPortal)
			{
				return ImageResourceManager.SourceCrm;
			}
			if (Source == ENSource.Android_SefareshGir)
			{
				return ImageResourceManager.SourceSefareshGir;
			}
			if (Source == ENSource.WebsiteWordPress)
			{
				return ImageResourceManager.SourceWordPress;
			}
			if (Source == ENSource.Department)
			{
				return ImageResourceManager.SourceDepartment;
			}
			return ImageResourceManager.SourceAccounting;
		}
	}

	public CashiersLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static void Logger(string contextGuid, EnSystem system, bool sendToServer, EnLog type, ENAction action, object obj = null, object oldObject = null, string description = "", Guid? sanadGuid = null, CancellationToken token = default(CancellationToken))
	{
		Task.Run(() => LogerAsync(contextGuid, system, sendToServer, token, type, action, awaitOnSemaphore: true, obj, oldObject, description, sanadGuid, 10));
	}

	private static async Task<ReturnedSaveFuncInfo> LogerAsync(string contextGuid, EnSystem system, bool sendToServer, CancellationToken token, EnLog type, ENAction action, bool awaitOnSemaphore = true, object obj = null, object oldObject = null, string description = "", Guid? sanadGuid = null, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierLogBusiness/Loger", 60000, token);
			if (dis.Token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 91);
			}
			if (!(obj is OrderBusiness order))
			{
				if (!(obj is ReceptionBusiness reception))
				{
					if (!(obj is PardakhtBusiness pardakht))
					{
						if (!(obj is ChequeMOperationBusiness operation))
						{
							if (obj is ChequeShOperationBusiness operation2)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationLoggerAsync(contextGuid, system, sendToServer, token, action, operation2, description), ReturnedState.None, "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 110);
							}
							else
							{
								if (system == EnSystem.Transfer)
								{
									return res;
								}
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await OtherLogsAsync(contextGuid, system, sendToServer, token, type, action, description, sanadGuid, awaitOnSemaphore: false), ReturnedState.None, "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 114);
							}
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationLoggerAsync(contextGuid, system, sendToServer, token, action, operation, description), ReturnedState.None, "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 107);
						}
					}
					else
					{
						if (system == EnSystem.Transfer)
						{
							return res;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await PardakhtLogerAsync(contextGuid, system, sendToServer, token, type, action, pardakht, awaitOnSemaphore: false), ReturnedState.None, "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 104);
					}
				}
				else
				{
					if (system == EnSystem.Transfer)
					{
						return res;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ReceptionLogerAsync(contextGuid, system, sendToServer, token, type, action, reception, awaitOnSemaphore: false, tranName), ReturnedState.None, "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 100);
				}
			}
			else
			{
				if (system == EnSystem.Transfer)
				{
					return res;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await OrderLogerAsync(system, contextGuid, sendToServer, token, action, order, (oldObject == null) ? null : ((OrderBusiness)oldObject), tranName, awaitOnSemaphore: false), ReturnedState.None, "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 96);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 120);
			res.AddReturnedValue(ex, "", "LogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 121);
		}
		return res;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.CashiersLogRepository;
	}

	public static async Task<ReturnedSaveFuncInfo> OtherLogsAsync(string contextGuid, EnSystem system, bool sendToServer, CancellationToken token, EnLog type, ENAction action, string description, Guid? sanadGuid, bool awaitOnSemaphore = true, string tranName = "", TaskCompletionSource<bool>? masterOperarationResult = null)
	{
		int line = 0;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			if (cashier == null)
			{
				return res;
			}
			line = 1;
			using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierLog/OtherLogAsync", 60000, token);
			line = 2;
			if (dis.Token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 138);
			}
			line = 3;
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CashiersLogBusiness obj = new CashiersLogBusiness(contextGuid)
			{
				SanadGuid = (sanadGuid ?? Guid.Empty),
				Action = action,
				CashierGuid = cashier.Guid,
				DateM = DateTime.Now,
				SanadType = type,
				Title = description,
				System = system,
				ApplicationVersion = cache.ApplicationVersion,
				Source = cache.Source
			};
			line = 4;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, sendToServer, awaitOnSemaphore: false, token, 10, tranName, masterOperarationResult), ReturnedState.None, "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 154);
		}
		catch (OperationCanceledException exception)
		{
			res.AddReturnedValue(exception, "", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 156);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 159);
			res.AddReturnedValue(ex, "", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 160);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> OtherLogsAsync<T>(string contextGuid, EnSystem system, T entity, bool sendToServer, CancellationToken token, EnLog type, ENAction action, string description, Guid? sanadGuid, bool awaitOnSemaphore = true, string tranName = "", TaskCompletionSource<bool>? masterOperarationResult = null)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			if (cashier == null)
			{
				return res;
			}
			using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierLog/OtherLogAsync", 60000, token);
			if (dis.Token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 172);
			}
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CashiersLogBusiness log = new CashiersLogBusiness(contextGuid)
			{
				SanadGuid = (sanadGuid ?? Guid.Empty),
				Action = action,
				CashierGuid = cashier.Guid,
				DateM = DateTime.Now,
				SanadType = type,
				Title = description,
				System = system,
				ApplicationVersion = cache.ApplicationVersion,
				Source = cache.Source
			};
			if (entity != null)
			{
				byte[] array = null;
				if (entity is ProductBusiness source)
				{
					array = Json.ToBytesJson(await ProductMapper.Instance.MapAsync(contextGuid, source, new WebProduct(), tranName, token));
				}
				if (array != null)
				{
					CashiersLogBusiness cashiersLogBusiness = log;
					cashiersLogBusiness.OldSerializedData = await GZip.Compress(array);
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await log.SaveAsync(system, sendToServer, awaitOnSemaphore: false, token, 10, tranName, masterOperarationResult), ReturnedState.None, "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 196);
		}
		catch (OperationCanceledException exception)
		{
			res.AddReturnedValue(exception, "", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 198);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 201);
			res.AddReturnedValue(ex, "", "OtherLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 202);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> PardakhtLogerAsync(string contextGuid, EnSystem system, bool sendToServer, CancellationToken token, EnLog type, ENAction action, PardakhtBusiness pardakht, bool awaitOnSemaphore = true, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashier == null)
			{
				return res;
			}
			using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierLogBusiness/PardakhtLogAsync", 60000, token);
			if (dis.Token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "PardakhtLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 214);
			}
			TafsilBusiness tafsil = await pardakht.GetTafsilAsync(EnSystem.None, token, tranName);
			string translate = type.GetDisplay().GetTranslate();
			string translate2 = "شماره".GetTranslate();
			int num = await pardakht.GetId2Async(token, tranName);
			string title = "*" + translate + " ** " + translate2 + " " + $"({num})" + " " + " به مبلغ".GetTranslate() + " " + $"{pardakht.Sum:N0} ** " + "از".GetTranslate() + " " + (" " + tafsil.Name).GetTranslate();
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CashiersLogBusiness obj = new CashiersLogBusiness(contextGuid)
			{
				SanadGuid = pardakht.Guid,
				Action = action,
				CashierGuid = cashier.Guid,
				DateM = DateTime.Now,
				SanadType = EnLog.Pardakht,
				Title = title,
				System = system,
				ApplicationVersion = cache.ApplicationVersion,
				Source = cache.Source
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, sendToServer, awaitOnSemaphore: false, token, 10), ReturnedState.None, "PardakhtLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 230);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PardakhtLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 234);
			res.AddReturnedValue(ex, "", "PardakhtLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 235);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ChequeMOperationLoggerAsync(string contextGuid, EnSystem system, bool sendToServer, CancellationToken token, ENAction action, ChequeMOperationBusiness operation, string desc)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (operation == null)
		{
			throw new NullReferenceException("operation");
		}
		try
		{
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CashiersLogBusiness obj = new CashiersLogBusiness(contextGuid)
			{
				SanadGuid = operation.GuidCheque,
				Action = action,
				CashierGuid = operation.CashierGuid,
				DateM = operation.DateM,
				SanadType = EnLog.CheckSanad,
				Title = desc,
				System = system,
				ChangeStatus = EntityChangeSt.New,
				ApplicationVersion = cache.ApplicationVersion,
				Source = cache.Source
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, sendToServer, awaitOnSemaphore: true, token, 10), ReturnedState.None, "ChequeMOperationLoggerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 260);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "1111", "ChequeMOperationLoggerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 264);
			res.AddReturnedValue(ex, "", "ChequeMOperationLoggerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 265);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ChequeShOperationLoggerAsync(string contextGuid, EnSystem system, bool sendToServer, CancellationToken token, ENAction action, ChequeShOperationBusiness operation, string desc)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CashiersLogBusiness obj = new CashiersLogBusiness(contextGuid)
			{
				SanadGuid = operation.ChequeGuid,
				Action = action,
				CashierGuid = operation.CashierGuid,
				DateM = operation.DateM,
				SanadType = EnLog.CheckSanad,
				Title = desc,
				System = system,
				ChangeStatus = EntityChangeSt.New,
				ApplicationVersion = cache.ApplicationVersion,
				Source = cache.Source
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, sendToServer, awaitOnSemaphore: true, token, 10), ReturnedState.None, "ChequeShOperationLoggerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 288);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "22222", "ChequeShOperationLoggerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 292);
			res.AddReturnedValue(ex, "", "ChequeShOperationLoggerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 293);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ReceptionLogerAsync(string contextGuid, EnSystem system, bool sendToServer, CancellationToken token, EnLog type, ENAction action, ReceptionBusiness reception, bool awaitOnSemaphore = true, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashier == null)
			{
				return res;
			}
			line = 1;
			using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierLogBusiness/RecptionLogAsync", 60000, token);
			line = 2;
			if (dis.Token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "ReceptionLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 308);
			}
			line = 30;
			if (reception == null)
			{
				throw new NullReferenceException("Reception is null!!");
			}
			line = 31;
			string translate = type.GetDisplay().GetTranslate();
			string translate2 = "شماره".GetTranslate();
			int num = await reception.GetId2Async(token, tranName);
			string title = "*" + translate + " **" + translate2 + " " + $"({num})" + " " + "به مبلغ".GetTranslate() + $"{reception.Sum:N0} **" + " " + " از".GetTranslate() + " " + (" " + reception.GetTafsil(system, token, tranName).Name).GetTranslate();
			line = 4;
			ClsCache cache = ClsCache.GetCache(contextGuid);
			CashiersLogBusiness obj = new CashiersLogBusiness(contextGuid)
			{
				SanadGuid = reception.Guid,
				Action = action,
				CashierGuid = cashier.Guid,
				DateM = DateTime.Now,
				SanadType = EnLog.Reception,
				Title = title,
				System = system,
				ApplicationVersion = cache.ApplicationVersion,
				Source = cache.Source
			};
			line = 5;
			obj.SaveAsync(system, sendToServer, awaitOnSemaphore: false, token, 10);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ReceptionLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 332);
			res.AddReturnedValue(ex, "", "ReceptionLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 333);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> OrderLogerAsync(EnSystem system, string contextGuid, bool sendToServer, CancellationToken token, ENAction action, OrderBusiness order, OrderBusiness oldOrder = null, string tranName = "", bool awaitOnSemaphore = true)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			using (await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierLogBusiness/OrderLogerAsync", 60000, token))
			{
				TafsilBusiness tafsil = await order.GetTafsilAsync(system, tranName, token);
				string translate = order.FactorType.GetDisplay().GetTranslate();
				string translate2 = " شماره".GetTranslate();
				int num = await order.GetId2Async(system, tranName, token);
				string title = "*" + translate + " ** " + translate2 + " " + $"({num})" + " " + " به مبلغ".GetTranslate() + " " + $"{order.TotalSum:N0}" + " **" + " از".GetTranslate() + " " + (" " + tafsil?.Name).GetTranslate();
				ClsCache cache = ClsCache.GetCache(contextGuid);
				CashiersLogBusiness log = new CashiersLogBusiness(contextGuid)
				{
					SanadGuid = order.Guid,
					Action = action,
					CashierGuid = cashier.Guid,
					DateM = DateTime.Now,
					SanadType = order.GetLogType(),
					Title = title,
					System = system,
					ApplicationVersion = cache.ApplicationVersion,
					Source = cache.Source
				};
				if (order != null)
				{
					byte[] data = Json.ToBytesJson(await OrderMapper.Instance.MapAsync(contextGuid, order, new WebOrder(), tranName, token));
					CashiersLogBusiness cashiersLogBusiness = log;
					cashiersLogBusiness.NewSerializedData = await GZip.Compress(data);
				}
				if (oldOrder != null)
				{
					byte[] data2 = Json.ToBytesJson(await OrderMapper.Instance.MapAsync(contextGuid, oldOrder, new WebOrder(), tranName, token));
					CashiersLogBusiness cashiersLogBusiness = log;
					cashiersLogBusiness.OldSerializedData = await GZip.Compress(data2);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await log.SaveAsync(system, sendToServer, awaitOnSemaphore: false, token, 10), ReturnedState.None, "OrderLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 372);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OrderLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 376);
			res.AddReturnedValue(ex, "", "OrderLogerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 377);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "", TaskCompletionSource<bool>? masterOperarationResult = null)
	{
		TaskCompletionSource<bool> masterOperarationResult2 = masterOperarationResult;
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ClsCache cache = ClsCache.GetCache(ContextGuid) ?? throw new NullReferenceException("cache is null!!");
		if (cache.IsDisposed)
		{
			throw new ObjectDisposedException("cache is Disposed!");
		}
		try
		{
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CashierLogBusiness/SaveAsync", 60000, token))
				{
					if (autoTran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await cache.AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 392), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 392);
					}
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 393);
					}
					else
					{
						transactionHasBeenCreated = true;
						CashiersLogBusiness cashiersLogBusiness = await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName);
						if (cashiersLogBusiness == null || system != EnSystem.SignalR)
						{
							if (cashiersLogBusiness != null && system != EnSystem.SignalR)
							{
								throw new InvalidOperationException($"Log({Guid}) already exists on database. update log is not valid!");
							}
							sendToServer = sendToServer && cache.BufferEnable;
							System = system;
							if (string.IsNullOrEmpty(ApplicationVersion))
							{
								ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await cache.CacheRepository.CashiersLogRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 406);
							goto end_IL_01b0;
						}
						result = ret.AddInformation("لاگ قبلا ذخیره شده".GetTranslate(), "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 397);
					}
					goto end_IL_0107;
					end_IL_01b0:;
				}
				goto end_IL_00f6;
				end_IL_0107:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 410);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 411);
				goto end_IL_00f6;
			}
			return result;
			end_IL_00f6:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 416), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 416);
			}
			if (!ret.HasError && sendToServer && cache.Source != ENSource.Accounting)
			{
				Task.Run(() => Send2ServerAsync(token, system, masterOperarationResult2));
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, List<CashiersLogBusiness> list, bool sendToServer, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (CashiersLogBusiness item in list)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, awaitOnSemaphore: false, token, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 428);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 432);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 433);
		}
		return ret;
	}

	public static Task<List<CashiersLogBusiness>> GetAllAsync(string contextGuid, CashierLogFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CashiersLogRepository.GetAllAsync(filter, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		if (item == null)
		{
			throw new NullReferenceException("SaveResponse is null!!");
		}
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 448), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 448);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CashiersLogRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 450);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 452);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 455);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 456);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 461), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 461);
			}
		}
		return ret;
	}

	public static Task<CashiersLogBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.CashiersLogRepository.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<CashiersLogBusiness>(null);
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
				{
					return ret;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 477);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 479);
				}
				line = 50;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 481);
				}
				line = 51;
				if (ClsCache.GetCache(contextGuid).CacheRepository.CashiersLogRepository == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.CashiersLogRepository is null");
				}
				line = 52;
				if (ClsCache.GetCache(contextGuid) == null)
				{
					throw new NullReferenceException("clsCahce.GetCache(" + contextGuid + ") is null!!");
				}
				line = 53;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.CashiersLogRepository.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 490);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("لاگ کاربران سینک هستند تعداد لاگ سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 491);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 492);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 498);
						}
						current = checked(current + 1);
						CashiersLogBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: CashierLog \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 504);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 509);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR, null), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 514);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 518);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 519);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 522);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 525);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 526);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, TaskCompletionSource<bool>? masterOperarationResult, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs"))
		{
			_ = 4;
			try
			{
				if (masterOperarationResult != null && !(await masterOperarationResult.Task))
				{
					return ret.AddError("master operation failed!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 539);
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "CashierLogBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 546);
				}
				List<WebCashierLog> list = new List<WebCashierLog>();
				List<WebCashierLog> list2 = list;
				list2.Add(await CashierLogMapper.Instance.MapAsync(ContextGuid, this, new WebCashierLog(), "", token));
				List<Packet> paks = WebCashierLog.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 552);
				}
				using CashierLogWaiter waiter = new CashierLogWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("cashierLogWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 556);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 558);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 560);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 563);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 564);
			}
			return ret;
		}
	}

	public Document GetDocument()
	{
		return GetDocument(SanadType);
	}

	public static Document GetDocument(EnLog factorType)
	{
		try
		{
			return factorType switch
			{
				EnLog.OrderSell => Document.OrderSell, 
				EnLog.OrderBuy => Document.OrderBuy, 
				EnLog.OrderBackBuy => Document.OrderBackBuy, 
				EnLog.OrderBackSell => Document.OrderBackSell, 
				EnLog.PishFactor => Document.PishFactor, 
				EnLog.Zayeat => Document.Zayeat, 
				EnLog.AmaniVorod => Document.AmaniVorod, 
				EnLog.AmaniKhoroj => Document.AmaniKhoroj, 
				EnLog.OrderPreBuy => Document.OrderPreBuy, 
				_ => throw new NotImplementedException(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDocument", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\CashiersLogBusiness.cs", 589);
			throw;
		}
	}
}
