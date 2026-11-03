using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business;

public class ExecuteQueryBusiness : IHasContext
{
	public string ContextGuid { get; set; }

	public ExecuteQueryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ExecuteQueryResponseEventArgs> GetQueryResponseAsync(string contextGuid, ExecuteQueryRequestEventArgs queryRequest, CancellationToken token, int timeOutMiliSeconds = 30000)
	{
		try
		{
			ClsCache cache = ClsCache.GetCache(contextGuid);
			WebContextHolder context = cache.Context;
			using ExecuteQueryWaiter waiter = new ExecuteQueryWaiter(context, queryRequest.PacketReferenceId, token, timeOutMiliSeconds);
			cache.SendData(WebExecuteQuery.GenerateQueryRequest(queryRequest, queryRequest.PacketReferenceId), Priority.High);
			return await waiter.WaiteForExecuteResponseAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetQueryResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteQueryBusiness.cs", 30);
			return new ExecuteQueryResponseEventArgs
			{
				Result = false,
				ResultMessage = "خطا در ارسال کوئری به سرور".GetTranslate() + "\r\n " + ex.Message,
				PacketReferenceId = queryRequest.PacketReferenceId
			};
		}
	}
}
