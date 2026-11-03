using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class GetDeleteResponsePacket : IDisposable, IHasContext
{
	private TaskCompletionSource<bool> TaskCompletion = new TaskCompletionSource<bool>();

	private long HashId { get; set; }

	private int PartitionId { get; set; }

	private int TimeOut { get; set; }

	private CancellationToken Token { get; set; }

	private EntityPart EntityPart { get; set; }

	public string ContextGuid { get; set; }

	public GetDeleteResponsePacket(long hashId, int partitionId, int timeOut, CancellationToken token, EntityPart entityPart, string contextGuid)
	{
		HashId = hashId;
		PartitionId = partitionId;
		TimeOut = timeOut;
		Token = token;
		EntityPart = entityPart;
		ContextGuid = contextGuid;
	}

	public async Task<bool> WaitForDeleteDonePacket()
	{
		try
		{
			WebContextHolder.GetWebContextHolder(ContextGuid).Sync.OnDeleteResponse += SyncInstance_OnDeleteResponse;
			TaskCompletion = new TaskCompletionSource<bool>();
			if (await Task.WhenAny(new Task[2]
			{
				TaskCompletion.Task,
				Task.Delay(TimeOut, Token)
			}) == TaskCompletion.Task)
			{
				return true;
			}
			return false;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForDeleteDonePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GetDeleteResponsePacket.cs", 51);
			return false;
		}
	}

	private Task SyncInstance_OnDeleteResponse(object sender, DeleteResponseEventArgs e)
	{
		if (e != null)
		{
			ConnectionData connection = e.Connection;
			if (connection != null)
			{
				_ = connection.Source;
				if (0 == 0 && e?.SyncPart == EntityPart && e?.PartitionId == PartitionId && e?.Connection?.HashId == HashId)
				{
					try
					{
						TaskCompletion?.TrySetResult(result: true);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnDeleteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GetDeleteResponsePacket.cs", 63);
					}
					return Task.CompletedTask;
				}
			}
		}
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GetDeleteResponsePacket.cs"))
		{
			try
			{
				TaskCompletion = null;
				WebContextHolder.GetWebContextHolder(ContextGuid).Sync.OnDeleteResponse -= SyncInstance_OnDeleteResponse;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GetDeleteResponsePacket.cs", 79);
			}
		}
	}
}
