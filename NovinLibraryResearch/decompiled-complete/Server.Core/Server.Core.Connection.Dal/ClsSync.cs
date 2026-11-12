using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsSync
{
	private static ConcurrentDictionary<long, Task> _bufferLoops = new ConcurrentDictionary<long, Task>();

	private static SemaphoreSlim _sendSyncSemaphore = new SemaphoreSlim(1, 1);

	public static async Task SendSyncDoneResponseAsync(CancellationToken token, ClsConnectionData connection = null, StringBuilder sb = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection != null && !connection.IsDisposed && !connection.cancellationToken.IsCancellationRequested)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendSyncDoneResponseAsync_(token, connection), ReturnedState.None, "SendSyncDoneResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 26);
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SendSyncDoneResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 28);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendSyncDoneResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 31);
			ret.AddReturnedValue(ex, "", "SendSyncDoneResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 32);
			sb?.AppendLine($"SyncDoneResponseResult:{!ret.HasError}");
		}
	}

	private static async Task<ReturnedSaveFuncInfo> SendSyncDoneResponseAsync_(CancellationToken token, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		bool tokenHasBeenTaken = false;
		try
		{
			await _sendSyncSemaphore.WaitAsync();
			tokenHasBeenTaken = true;
			line = 100;
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddError("connection is null or disposed!", "", "SendSyncDoneResponseAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 48);
			}
			line = 101;
			connection.SyncDoneBuffer.Post(DateTime.Now);
			line = 2;
			if (!_bufferLoops.TryGetValue(connection.HashId, out var _))
			{
				line = 3;
				_bufferLoops.TryAdd(connection.HashId, Task.Run(() => SendSyncDoneResponseAsync_(connection.SyncDoneBuffer, connection)));
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"1111/Line:{line}", "SendSyncDoneResponseAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 61);
			ret.AddReturnedValue(ex2, "", "SendSyncDoneResponseAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 62);
		}
		finally
		{
			if (tokenHasBeenTaken)
			{
				_sendSyncSemaphore?.Release();
			}
		}
		return ret;
	}

	private static async Task SendSyncDoneResponseAsync_(BufferBlock<DateTime> buffer, ClsConnectionData connection)
	{
		long hashId = connection?.HashId ?? (-1);
		int line = 0;
		try
		{
			line = 1;
			while (!(connection?.IsDisposed ?? true))
			{
				line = 2;
				await buffer.ReceiveAsync(connection.cancellationToken);
				buffer.TryReceiveAll(out IList<DateTime> _);
				await Task.Delay(5000);
				line = 3;
				if (connection?.IsDisposed ?? true)
				{
					break;
				}
				line = 4;
				if (!((DateTime.Now - connection.SyncDoneTime).TotalSeconds < 5.0))
				{
					line = 5;
					connection.SyncDoneTime = DateTime.Now;
					line = 6;
					Sync sync = (await Server.Core.Assistance.Initializer.Dal.Sync.LoadDataAsync(connection.HashId, connection.cancellationToken))?.value;
					line = 7;
					if (connection?.IsDisposed ?? true)
					{
						break;
					}
					line = 8;
					if (sync != null && connection.Source != ENSource.WebsiteWordPress)
					{
						line = 9;
						connection.SendPacketAsync(Sync.GenerateSyncDoneResponse(sync.SyncInfo), Priority.High);
						line = 10;
					}
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (InvalidOperationException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, $"xxx=>Line:{line}", "SendSyncDoneResponseAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 106);
		}
		finally
		{
			_bufferLoops?.TryRemove(hashId, out var _);
		}
	}

	private static void SetLastSync(Sync syncInfo, int hashId, ENSource source)
	{
		using (new JaegerMonitor("SetLastSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs"))
		{
			try
			{
				if (syncInfo != null)
				{
					List<DateTime> list = new List<DateTime>();
					list.Add(syncInfo.SyncInfo.Customer);
					list.Add(syncInfo.SyncInfo.CustomerGroup);
					list.Add(syncInfo.SyncInfo.CustomerTitle);
					list.Add(syncInfo.SyncInfo.ProductGroup);
					list.Add(syncInfo.SyncInfo.ProductVahed);
					switch (source)
					{
					case ENSource.Baje:
					case ENSource.Android_HesabYar:
					case ENSource.IOS_HesabYar:
					case ENSource.CrmPortal:
					case ENSource.AccountingPortal:
						list.Add(syncInfo.SyncInfo.AghsatAshkhas);
						list.Add(syncInfo.SyncInfo.AghsatShakhsi);
						list.Add(syncInfo.SyncInfo.CallingLog);
						list.Add(syncInfo.SyncInfo.Depot);
						list.Add(syncInfo.SyncInfo.HesabTree);
						list.Add(syncInfo.SyncInfo.LastPrice);
						list.Add(syncInfo.SyncInfo.Delivery);
						break;
					case ENSource.Android_SefareshGir:
						list.Add(syncInfo.SyncInfo.Product);
						list.Add(syncInfo.SyncInfo.News);
						list.Add(syncInfo.SyncInfo.HyperLink);
						list.Add(syncInfo.SyncInfo.Delivery);
						break;
					case ENSource.WebSite:
						list.Add(syncInfo.SyncInfo.Product);
						list.Add(syncInfo.SyncInfo.News);
						list.Add(syncInfo.SyncInfo.HyperLink);
						list.Add(syncInfo.SyncInfo.Delivery);
						list.Add(syncInfo.SyncInfo.ContactUs);
						break;
					}
					DateTime minSync = list.Min();
					Server.Core.Assistance.Initializer.Dal.Sync.UpdateLastSyncAsync(hashId, minSync, syncInfo.GetToken());
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetLastSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSync.cs", 155);
			}
		}
	}
}
