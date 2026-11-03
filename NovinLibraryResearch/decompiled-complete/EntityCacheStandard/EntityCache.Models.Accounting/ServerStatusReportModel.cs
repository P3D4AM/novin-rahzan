using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Models.Accounting;

public class ServerStatusReportModel
{
	private static SemaphoreSlim databaseSemaphor = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public int Ordinal { get; set; }

	public DateTime? Date { get; set; }

	public string DateSh => Date.MiladiToCalture();

	public EntityPart Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public long Id2 { get; set; }

	public decimal TotalSum { get; set; }

	public string TafsilName { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public string ServerStatusForDisplay => ServerStatus.GetDisplay().GetTranslate();

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public bool IsChecked { get; set; }

	public static async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(string contextGuid, List<ServerStatusReportModel> list, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		try
		{
			_ = 2;
			try
			{
				if (!(list?.Any() ?? false))
				{
					return res;
				}
				using SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(true, "ServerStatusReportModel/UpdateServerStatusAsync", 60000, token);
				if (dis.Token.IsCancellationRequested)
				{
					res.AddErrorTokenIsCancelled("", "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 41);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autotran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 42), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 42);
				}
				trantionBeenCreate = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Reports.UpdateServerStatusAsync(list, token, tranName), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 44);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 48);
				res.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 49);
			}
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 54), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Models\\Accounting\\ServerStatusReportModel.cs", 54);
			}
		}
		return res;
	}
}
