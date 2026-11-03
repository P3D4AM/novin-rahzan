using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class SyncDoneWaiter : IDisposable
{
	private TaskCompletionSource<bool> TaskCompletion = new TaskCompletionSource<bool>();

	public long HashId { get; private set; }

	public int PartitionId { get; private set; }

	public EntityPart EntityPart { get; private set; }

	public int TimeOut { get; private set; }

	public CancellationToken Token { get; private set; }

	public string ContextGuid { get; private set; }

	public string Message { get; private set; }

	public SyncDoneWaiter(string contextGuid, long hashId, int partitionId, EntityPart entityPart, int timeOut = 60000, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ContextGuid = contextGuid;
			HashId = hashId;
			PartitionId = partitionId;
			TimeOut = timeOut;
			Token = token;
			EntityPart = entityPart;
			WebContextHolder.GetWebContextHolder(ContextGuid).Sync.OnSyncDone += SyncInstance_OnSyncDone;
			WebContextHolder.GetWebContextHolder(ContextGuid).Sync.OnSyncResetDone += SyncInstance_OnSyncResetDone;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SyncDoneWaiter.cs", 34);
		}
	}

	private Task SyncInstance_OnSyncDone(object sender, SyncDoneEventArgs e)
	{
		if (e == null || e.Connection == null || e.SyncPart != EntityPart || e.Connection.HashId != HashId)
		{
			return Task.CompletedTask;
		}
		try
		{
			if (EntityPart == EntityPart.CustomerGroup || EntityPart == EntityPart.CustomerTitle || EntityPart == EntityPart.TafsilGroup || EntityPart == EntityPart.ProductGroup || EntityPart == EntityPart.Depot || EntityPart == EntityPart.ProductVahed)
			{
				Message = e.Message;
				TaskCompletion?.TrySetResult(e.Result);
			}
			if (e.PartitionId == PartitionId)
			{
				Message = e.Message;
				TaskCompletion?.TrySetResult(e.Result);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSyncDone", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SyncDoneWaiter.cs", 60);
		}
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		try
		{
			TaskCompletion?.TrySetCanceled();
			TaskCompletion = null;
			WebContextHolder.GetWebContextHolder(ContextGuid).Sync.OnSyncDone -= SyncInstance_OnSyncDone;
			WebContextHolder.GetWebContextHolder(ContextGuid).Sync.OnSyncResetDone -= SyncInstance_OnSyncResetDone;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SyncDoneWaiter.cs", 73);
		}
	}

	public async Task<bool> WaitForSyncDonePacket()
	{
		_ = 1;
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				TaskCompletion.Task,
				Task.Delay(TimeOut, Token)
			}) == TaskCompletion.Task)
			{
				return await TaskCompletion.Task;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaitForSyncDonePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SyncDoneWaiter.cs", 88);
			return false;
		}
	}

	private Task SyncInstance_OnSyncResetDone(object sender, SyncResetDoneEventArgs e)
	{
		try
		{
			if (e == null)
			{
				goto IL_00a1;
			}
			ConnectionData connection = e.Connection;
			if (connection == null)
			{
				goto IL_00a1;
			}
			_ = connection.Source;
			if (false || e?.SyncPart != EntityPart || e?.Connection?.HashId != HashId)
			{
				goto IL_00a1;
			}
			if (e.SyncPart == EntityPart && e.Connection.HashId == HashId)
			{
				TaskCompletion.TrySetResult(result: true);
			}
			goto end_IL_0000;
			IL_00a1:
			return Task.CompletedTask;
			end_IL_0000:;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSyncResetDone", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SyncDoneWaiter.cs", 106);
		}
		return Task.CompletedTask;
	}
}
