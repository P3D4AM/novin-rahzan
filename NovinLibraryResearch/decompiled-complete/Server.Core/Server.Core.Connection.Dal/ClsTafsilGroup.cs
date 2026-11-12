using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsTafsilGroup : IEntity<WebTafsilGroup>
{
	private class NestedTafsilLevel
	{
		internal static readonly ClsTafsilGroup TafsilGroupInstance;

		static NestedTafsilLevel()
		{
			TafsilGroupInstance = new ClsTafsilGroup();
		}
	}

	public static ClsTafsilGroup TafsilGroupInstance => NestedTafsilLevel.TafsilGroupInstance;

	private ClsTafsilGroup()
	{
	}

	public async Task<List<WebTafsilGroup>> ApplyAccessAsync(IEnumerable<WebTafsilGroup> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebTafsilGroup p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task TafsilLevelInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebTafsilGroup> e)
	{
		using (new JaegerMonitor("TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.TafsilGroup, 1, isReceivedPacket: true);
				ConnectionEventArgs<WebTafsilGroup> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					if (e.Connection.Source != ENSource.Accounting)
					{
						if (e.Connection.DepId == 3)
						{
							return;
						}
						WebUser webUser = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(e.Connection.UserId);
						throw new Exception(string.Format("این بابا({0})/({1}) داره میاد و گروه تفصیلی میفرسته به هاست.", e.Connection.UserId, webUser?.Name?.FixForTelegram() ?? "user is null!!"));
					}
					ret = await Server.Core.Assistance.Initializer.Dal.DalTafsilGroup.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex2)
				{
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					WebErrorLog.Instance.StartLog(ex2, "", "TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 69);
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebTafsilGroup.GenerateSaveResponse(ret), Priority.High);
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (trueSaved == null)
				{
					return;
				}
				HashSet<Guid> hashSet = trueSaved;
				if (hashSet != null && hashSet.Count() <= 0)
				{
					return;
				}
				List<WebTafsilGroup> truSavedInRet = e.ConnectionData.Where((WebTafsilGroup p) => trueSaved.Contains(p.Guid)).ToList();
				if (truSavedInRet == null || truSavedInRet.Count() <= 0)
				{
					return;
				}
				ConnectionEventArgs<WebTafsilGroup> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting) && truSavedInRet.Count() > 0)
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 8)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebTafsilGroup.GenerateSaveRequest(truSavedInRet), Priority.Medium));
					}
				}
				if (truSavedInRet.Count() <= 0)
				{
					return;
				}
				List<ClsConnectionData> list = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting) && p?.ClientVersion > 8).ToList();
				if (list == null)
				{
					return;
				}
				foreach (ClsConnectionData item2 in list)
				{
					if (item2 != null && !item2.IsDisposed && item2 != null)
					{
						ClsConnectionData clsConnectionData = item2;
						clsConnectionData.SendPacketAsync(WebTafsilGroup.GenerateReadResponse(await ApplyAccessAsync(truSavedInRet, item2)), Priority.Medium);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "TafsilLevelInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 110);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 300000))
		{
			int line = 1;
			try
			{
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 121);
					return ret;
				}
				DateTime modified = await Server.Core.Assistance.Initializer.Dal.Sync.GetLastSyncTime(connection.HashId, EntityPart.TafsilGroup, 0, connection.cancellationToken);
				line = 3;
				DateTime syncing = DateTime.Now;
				line = 40;
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("Connection Is Disposed!", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 127);
				}
				line = 41;
				IEnumerable<WebTafsilGroup> lst = await Server.Core.Assistance.Initializer.Dal.DalTafsilGroup.GetAllAsync(connection.DepId, modified, connection.cancellationToken);
				line = 5;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 131);
				}
				line = 6;
				line = 8;
				lst = await ApplyAccessAsync(lst, connection);
				line = 9;
				line = 11;
				if (lst?.Any() ?? false)
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.TafsilGroup, 300000, connection.cancellationToken);
					line = 12;
					Task.Run(() => connection.SendPacketAsync(WebTafsilGroup.GenerateReadResponse(lst, isSync: true), Priority.Low));
					line = 13;
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						return ret.AddError("waiter.WaitForSyncDonePacket timeOut!", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 147);
					}
					line = 14;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.TafsilGroup, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 149);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 152);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 155);
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 156);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"TafsilGroupSyncResult:{!ret.HasError}");
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1;
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"Source({connection.Source}) is != accouting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 174);
				}
				line = 2;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.TafsilGroup, connection.cancellationToken);
				line = 3;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 178);
				}
				line = 40;
				if (list != null && list.Any())
				{
					line = 41;
					foreach (Guid item in list)
					{
						line = 5;
						ret.AddInformation($"sending tafsilGroup({item}) to accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 186);
						line = 6;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 188);
						}
						line = 7;
						WebTafsilGroup webTafsilGroup = await Server.Core.Assistance.Initializer.Dal.DalTafsilGroup.GetAsync(connection.DepId, item);
						line = 8;
						if (webTafsilGroup != null)
						{
							line = 9;
							await connection.SendPacketAsync(WebTafsilGroup.GenerateSaveRequest(new List<WebTafsilGroup> { webTafsilGroup }), Priority.Medium);
						}
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 202);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 207);
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsilGroup.cs", 208);
			}
			return ret;
		}
	}
}
