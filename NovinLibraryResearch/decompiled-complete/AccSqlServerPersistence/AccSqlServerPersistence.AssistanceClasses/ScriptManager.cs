using System;
using System.Threading.Tasks;
using AccSqlServerPersistence.Properties;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class ScriptManager
{
	public static async Task<ReturnedSaveFuncInfo> RunAccountingScriptAsync(string contextGuid, string connectionString)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_Acc_sp", checkCrcBeforeExecution: false, ENSource.Accounting, Resources.Acc_sp, connectionString), ReturnedState.None, "RunAccountingScriptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\ScriptManager.cs", 14);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_FixAccountingData", checkCrcBeforeExecution: false, ENSource.Accounting, Resources.FixAccountingData, connectionString), ReturnedState.None, "RunAccountingScriptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\ScriptManager.cs", 15);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "acc_DropConstraintForClientsOfAccounting", checkCrcBeforeExecution: false, ENSource.Accounting, Resources.DropConstraintForClientsOfAccounting, connectionString), ReturnedState.None, "RunAccountingScriptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\ScriptManager.cs", 16);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RunAccountingScriptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\ScriptManager.cs", 20);
			res.AddReturnedValue(ex, "", "RunAccountingScriptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\ScriptManager.cs", 21);
		}
		return res;
	}
}
