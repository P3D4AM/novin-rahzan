using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class GetQueryResponse
{
	public TaskCompletionSource<ExecuteQueryResponseEventArgs> taskCompletionSource = new TaskCompletionSource<ExecuteQueryResponseEventArgs>();

	private long _hashId { get; set; }

	private Guid _refId { get; set; }

	public GetQueryResponse(WebContextHolder context, long hashId, Guid refId)
	{
		_hashId = hashId;
		_refId = refId;
		context.WebExecuteQuery.OnExecuteQueryResponse -= QueryInstance_OnExecuteQueryResponse;
		context.WebExecuteQuery.OnExecuteQueryResponse += QueryInstance_OnExecuteQueryResponse;
	}

	public async Task<ExecuteQueryResponseEventArgs> WaitForQueryResponse(CancellationToken Token, int timeout = 30)
	{
		using (new JaegerMonitor("WaitForQueryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GetQueryResponse.cs"))
		{
			_ = 1;
			try
			{
				if (await Task.WhenAny(new Task[2]
				{
					taskCompletionSource.Task,
					Task.Delay(timeout, Token)
				}) == taskCompletionSource.Task)
				{
					return await taskCompletionSource.Task;
				}
				return null;
			}
			catch (TimeoutException ex)
			{
				throw ex;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "WaitForQueryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GetQueryResponse.cs", 47);
				return null;
			}
		}
	}

	private Task QueryInstance_OnExecuteQueryResponse(object sender, ConnectionEventArgs<ExecuteQueryResponseEventArgs> e)
	{
		try
		{
			if (e.Connection.HashId == _hashId && e.PacketReferenceId == _refId)
			{
				taskCompletionSource.TrySetResult(e.ConnectionData.Single());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "QueryInstance_OnExecuteQueryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GetQueryResponse.cs", 59);
		}
		return Task.CompletedTask;
	}
}
