using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsKol : IEntity<WebKol>
{
	private class NestedKol
	{
		internal static ClsKol Instance { get; }

		static NestedKol()
		{
			Instance = new ClsKol();
		}
	}

	public static ClsKol KolInstance => NestedKol.Instance;

	private ClsKol()
	{
	}

	public async Task<List<WebKol>> ApplyAccessAsync(IEnumerable<WebKol> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs"))
		{
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return null;
				}
				if (connection.Source != ENSource.Accounting)
				{
					lst?.Count();
					lst = lst?.Where((WebKol p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return lst?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 42);
				return new List<WebKol>();
			}
		}
	}

	internal async Task KolInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebKol> e)
	{
		using (new JaegerMonitor("KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs"))
		{
			int line = 0;
			try
			{
				if (e == null)
				{
					throw new NullReferenceException("e");
				}
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Kol, 1, isReceivedPacket: true);
				line = 1;
				if (e.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				List<SaveResponse> ret = new List<SaveResponse>();
				line = 3;
				bool result = true;
				try
				{
					line = 4;
					ret = await Server.Core.Assistance.Initializer.Dal.Kol.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					line = 5;
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
					line = 6;
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 81);
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				line = 150;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					line = 151;
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 152;
					if (connection2 != null)
					{
						await connection2.SendPacketAsync(WebKol.GenerateSaveResponse(ret), Priority.High);
					}
					line = 155;
				}
				line = 156;
				HashSet<Guid> trueSaved = new HashSet<Guid>(from p in ret
					where p.SaveResultStatus
					select p.Guid);
				line = 160;
				if (!trueSaved.Any())
				{
					return;
				}
				line = 170;
				List<WebKol> truSavedInRet = e.ConnectionData.Where((WebKol p) => trueSaved.Contains(p.Guid)).ToList();
				line = 180;
				if (truSavedInRet.Any())
				{
					return;
				}
				line = 190;
				if (e.Connection.Source != ENSource.Accounting)
				{
					line = 200;
					ClsConnectionData accConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accConnection != null && accConnection.ClientVersion > 8)
					{
						Task.Run(() => accConnection.SendPacketAsync(WebKol.GenerateSaveRequest(truSavedInRet, 0), Priority.Medium));
					}
				}
				line = 300;
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				List<ClsConnectionData> list = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting && p.ClientVersion >= 10).ToList() ?? new List<ClsConnectionData>();
				if (list.Any())
				{
					return;
				}
				foreach (ClsConnectionData connection in list)
				{
					List<WebKol> dataForSend = await ApplyAccessAsync(truSavedInRet, connection);
					if (!dataForSend.Any())
					{
						Task.Run(() => connection.SendPacketAsync(WebKol.GenerateReadResponse(dataForSend, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"#Line_{line}", "KolInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 137);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string connectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs"))
		{
			if (connection == null)
			{
				throw new NullReferenceException("connection");
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			CancellationToken token = connection.cancellationToken;
			if (connection.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			int line = 0;
			try
			{
				if (token.IsCancellationRequested || connection.IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 150);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				SyncInformation obj = await connection.GetSyncInfoAsync();
				line = 100;
				if (obj == null)
				{
					line = 101;
					ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 157);
					line = 102;
					return ret;
				}
				line = 10001;
				if (connection.IsDisposed || token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 162);
				}
				if (!(await connection.GetDepartmentAsync()).SyncsKolMoeinTafsil)
				{
					line = 104;
					return ret.AddInformation($"دپارتمان({connection.DepName})({connection.DepId}) حساب کل را سینک نمیکند", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 167);
				}
				line = 20;
				List<short> totalPartitions = new List<short>();
				List<short> list = await Server.Core.Assistance.Initializer.Dal.Kol.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						if (connection.IsDisposed || token.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 181);
							return ret;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SyncPartitionsAsync(list, connection, maxParalel), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 184);
						if (connection.IsDisposed || token.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 187);
							return ret;
						}
						line = 22;
						if (connection.IsDisposed || token.IsCancellationRequested)
						{
							return ret;
						}
						list = await Server.Core.Assistance.Initializer.Dal.Kol.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
						line = 23;
						list = list?.Where((short x) => !totalPartitions.Contains(x)).ToList();
						line = 24;
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 199);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 202);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 203);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(token, connection));
				sb.AppendLine($"KolSyncResult:{!ret.HasError}");
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionsAsync(List<short> partitions, ClsConnectionData connection, short maxParalel)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(partitions?.Any() ?? false))
			{
				return ret;
			}
			List<Task> tasks = new List<Task>();
			foreach (short partition in partitions)
			{
				tasks.Add(SyncPartitionAsync(partition, connection));
				if (tasks.Count >= maxParalel)
				{
					List<Task> list = tasks;
					list.Remove(await Task.WhenAny(tasks));
				}
			}
			await Task.WhenAll(tasks);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 228);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 231);
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 232);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (connection.ClientVersion < 10)
				{
					throw new Exception($"sync kol with client lower than 10 / version:{connection.ClientVersion}");
				}
				line = 25;
				DateTime syncing = DateTime.Now;
				line = 26;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 249);
				}
				IEnumerable<WebKol> lst = await Server.Core.Assistance.Initializer.Dal.Kol.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken, loadGardeshHesab: true);
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Kol, 300000, connection.cancellationToken);
				line = 30;
				if (lst?.Any() ?? false)
				{
					Task.Run(() => connection.SendPacketAsync(WebKol.GenerateReadResponse(lst, isSync: true, partition), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "waiter time out", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 273);
						return ret;
					}
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 276);
					}
					line = 45;
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Kol, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 279);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 282);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 285);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 286);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		if (connection == null)
		{
			throw new NullReferenceException("connection");
		}
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			short depId = connection.DepId;
			try
			{
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"Connection({connection.Source})<>Source.Accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 301);
				}
				line = 1;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Kol, connection.cancellationToken);
				line = 2;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 305);
				}
				line = 30;
				foreach (Guid guid in list ?? new List<Guid>())
				{
					line = 4;
					ret.AddInformation($"sending kol({guid}) to accounting connection", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 310);
					line = 5;
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 312);
					}
					line = 6;
					WebKol webKol = await Server.Core.Assistance.Initializer.Dal.Kol.GetAsync(guid, connection.DepId, loadGardeshHesab: false);
					line = 7;
					if (webKol == null)
					{
						continue;
					}
					line = 8;
					Task.Run(() => connection.SendPacketAsync(WebKol.GenerateSaveRequest(new List<WebKol> { webKol }, 0), Priority.Medium));
					line = 9;
					using KolWaiter waiter = new KolWaiter(connection.Context, guid, connection.cancellationToken, 60000);
					line = 10;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("ClsKol.SendNotDeliveredAsync"), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 322);
					line = 110;
					if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 325);
					}
					line = 111;
					ret.throwExceptionIfError();
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 330);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 333);
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"#depId:{depId}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsKol.cs", 334);
			}
			return ret;
		}
	}
}
