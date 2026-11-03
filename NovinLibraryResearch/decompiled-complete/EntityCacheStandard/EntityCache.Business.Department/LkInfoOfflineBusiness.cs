using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Department;

public class LkInfoOfflineBusiness : IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public string ContextGuid { get; private set; }

	public LkInfoBusiness LkInfo { get; private set; }

	public List<FileInfoBusiness> FileInfo { get; set; }

	public bool IsOffline { get; set; }

	public Guid CashierGuid { get; private set; }

	public string Description { get; set; }

	private LkInfoOfflineBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<LkInfoOfflineBusiness> GetFactoryAsync(string contextGuid, LkInfoBusiness lk)
	{
		try
		{
			LkInfoOfflineBusiness lkInfoOfflineBusiness = new LkInfoOfflineBusiness(contextGuid)
			{
				LkInfo = lk
			};
			LkInfoOfflineBusiness lkInfoOfflineBusiness2 = lkInfoOfflineBusiness;
			lkInfoOfflineBusiness2.FileInfo = await FileInfoBusiness.GetAsync(contextGuid, lk.Guid, FileManagement_Parts.Lk);
			lkInfoOfflineBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
			return lkInfoOfflineBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 41);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, string version, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 6;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "LkInfoOfflineBusiness/SaveAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 54);
					}
					else
					{
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).DptDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 57), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 57);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 58), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 58);
							transactionHasBeenCreated = true;
						}
						LkInfo.IsOffline = IsOffline;
						LkInfo.Modified = DateTime.Now;
						LkInfo.ServerStatus = ServerStatusForClient.None;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await LkInfo.SaveAsync("قفل آفلاین شد".GetTranslate(), system, CashierGuid, sendToServer: true, Priority.Low, awaitOnSemaphore: true, await4send: false, 10, tranName, dis.Token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 65);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (FileInfo == null)
							{
								goto IL_0558;
							}
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await FileInfoBusiness.SaveRangeAsync(system, dis.Token, 0, FileInfo, send2Server: true, Priority.Low, 10), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 70);
							if (!ret.HasError)
							{
								goto IL_0558;
							}
							result = ret;
						}
					}
					goto end_IL_009c;
					IL_0558:
					CashierBusiness cashierBusiness = await CashierBusiness.GetAsync(ContextGuid, CashierGuid, dis.Token, tranName);
					CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
					cashiersLogBusiness.Action = ENAction.Add;
					cashiersLogBusiness.ApplicationVersion = version;
					cashiersLogBusiness.CashierGuid = CashierGuid;
					cashiersLogBusiness.Guid = Guid.NewGuid();
					cashiersLogBusiness.CashierName = cashierBusiness?.Name;
					cashiersLogBusiness.ChangeStatus = EntityChangeSt.New;
					cashiersLogBusiness.ContextGuid = ContextGuid;
					cashiersLogBusiness.DateM = DateTime.Now;
					cashiersLogBusiness.Level = ReturnedState.Information;
					cashiersLogBusiness.Modified = DateTime.Now;
					cashiersLogBusiness.SanadGuid = LkInfo.Guid;
					cashiersLogBusiness.SanadType = EnLog.LkInfo;
					cashiersLogBusiness.ServerStatus = ServerStatusForClient.None;
					cashiersLogBusiness.Source = ENSource.Activation;
					cashiersLogBusiness.Status = true;
					cashiersLogBusiness.System = system;
					cashiersLogBusiness.Title = "وضعیت افلاین قفل با شماره سریال".GetTranslate() + " " + $" {LkInfo.LKSerial} ".GetTranslate() + " " + "در تاریخ".GetTranslate() + " " + DateTime.Now.MiladiToCalture() + " " + " توسط".GetTranslate() + " " + (" " + cashierBusiness.Name).GetTranslate() + " " + " در حالت ".GetTranslate() + " " + $"{IsOffline}".GetTranslate() + " " + " قرار گرفت".GetTranslate() + " " + Description;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: true, dis.Token, 10, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 96);
				}
				goto end_IL_008b;
				end_IL_009c:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 98);
				goto end_IL_008b;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 101);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 102);
				goto end_IL_008b;
			}
			return result;
			end_IL_008b:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 108), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 108);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 109), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Department\\LkInfoOfflineBusiness.cs", 109);
			}
		}
		return ret;
	}
}
