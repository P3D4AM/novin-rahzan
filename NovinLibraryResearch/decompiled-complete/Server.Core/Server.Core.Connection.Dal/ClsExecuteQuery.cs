using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace Server.Core.Connection.Dal;

public class ClsExecuteQuery
{
	private class NestedExecuteQuery
	{
		internal static readonly ClsExecuteQuery Instance;

		static NestedExecuteQuery()
		{
			Instance = new ClsExecuteQuery();
		}
	}

	public static ClsExecuteQuery QueryInstance => NestedExecuteQuery.Instance;

	public async Task WebExecuteQuery_OnExecuteQueryRequest(object sender, ConnectionEventArgs<ExecuteQueryRequestEventArgs> e)
	{
		using (new JaegerMonitor("WebExecuteQuery_OnExecuteQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsExecuteQuery.cs"))
		{
			_ = 2;
			try
			{
				if (e.Connection.Source == ENSource.Accounting)
				{
					throw new NotImplementedException();
				}
				ClsConnectionData accountingConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
				if (accountingConnection == null)
				{
					await SendResponse(e.ConnectionUniqueId, new ExecuteQueryResponseEventArgs
					{
						ResultMessage = "نرم افزار حسابداری آفلاین است",
						Result = false,
						PacketReferenceId = e.PacketReferenceId
					});
					return;
				}
				List<Packet> packs = WebExecuteQuery.GenerateQueryRequest(e.ConnectionData.Single(), e.PacketReferenceId);
				using ExecuteQueryWaiter waiter = new ExecuteQueryWaiter(e.Connection.Context, e.PacketReferenceId, e.Connection.cancellationToken);
				accountingConnection.SendPacketAsync(packs, Priority.High);
				ExecuteQueryResponseEventArgs executeQueryResponseEventArgs = await waiter.WaiteForExecuteResponseAsync();
				if (executeQueryResponseEventArgs == null)
				{
					executeQueryResponseEventArgs = new ExecuteQueryResponseEventArgs
					{
						PacketReferenceId = e.PacketReferenceId,
						ResultMessage = "پاسخی از سمت حسابداری دریافت نشد.",
						Result = false
					};
				}
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.Connection.Guid);
				if ((e?.Connection?.IsDisposed).GetValueOrDefault(true) || e.Connection.cancellationToken.IsCancellationRequested || (connection?.IsDisposed ?? true))
				{
					return;
				}
				List<Packet> packs2 = WebExecuteQuery.GenerateQueryResponse(executeQueryResponseEventArgs);
				await connection.SendPacketAsync(packs2, Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebExecuteQuery_OnExecuteQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsExecuteQuery.cs", 50);
			}
		}
	}

	private async Task SendResponse(string connectionUniqueId, ExecuteQueryResponseEventArgs response)
	{
		try
		{
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(connectionUniqueId);
			if (!(connection?.IsDisposed ?? true))
			{
				await connection.SendPacketAsync(WebExecuteQuery.GenerateQueryResponse(response), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsExecuteQuery.cs", 61);
		}
	}
}
