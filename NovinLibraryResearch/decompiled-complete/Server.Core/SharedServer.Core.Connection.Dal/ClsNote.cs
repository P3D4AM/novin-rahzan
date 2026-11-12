using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Server.Core.Assistance;
using Server.Core.Connection;

namespace SharedServer.Core.Connection.Dal;

public class ClsNote
{
	private class NestedNote
	{
		internal static readonly ClsNote Instance;

		static NestedNote()
		{
			Instance = new ClsNote();
		}
	}

	public static ClsNote Instance => NestedNote.Instance;

	public Task<List<WebNote>> ApplyAccessAsync(IEnumerable<WebNote> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs"))
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
					lst = lst?.Where((WebNote p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return Task.FromResult(lst?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 40);
				return null;
			}
		}
	}

	internal async Task NoteInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebNote> e)
	{
		using (new JaegerMonitor("NoteInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Note, 1, isReceivedPacket: true);
				line = 1;
				ConnectionEventArgs<WebNote> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
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
					ret = await Initializer.Dal.Note.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					WebErrorLog.Instance.StartLog(ex2, "", "NoteInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 69);
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				line = 15;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection2?.IsDisposed ?? true)
					{
						return;
					}
					await connection2.SendPacketAsync(WebNote.GenerateSaveResponse(ret), Priority.High);
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
				List<WebNote> truSavedInRet = e.ConnectionData.Where((WebNote p) => trueSaved.Contains(p.Guid)).ToList();
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebNote> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting) && e.Connection.ClientVersion >= 15)
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 8)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebNote.GenerateSaveRequest(truSavedInRet), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.Source != ENSource.Accounting && p.IsAccountingFamily && p.ClientVersion >= 14);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (WebNote item2 in truSavedInRet)
				{
					item2.DeliveryDate = DateTime.Now;
					item2.ServerStatus = ServerStatusForClient.Delivered;
				}
				foreach (ClsConnectionData connection in enumerable)
				{
					if (connection?.IsDisposed ?? true)
					{
						continue;
					}
					List<WebNote> dataForSend = await ApplyAccessAsync(truSavedInRet, connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => connection.SendPacketAsync(WebNote.GenerateReadResponse(dataForSend, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"Line:{line}", "NoteInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 113);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnSent(ClsConnectionData connection, short maxParallelTax = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 300000))
		{
			_ = 3;
			try
			{
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				List<Guid> list = await Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Note, connection.cancellationToken);
				connection.cancellationToken.ThrowIfCancellationRequested();
				if (list == null || !list.Any())
				{
					return ret.AddInformation("یادداشتی در سرور برای ارسال وجود ندارد.", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 126);
				}
				int timeout = 180000;
				foreach (Guid guid in list)
				{
					if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested || connection.DepId <= 0)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 130);
					}
					WebNote note = await Initializer.Dal.Note.GetAsync(connection.DepId, guid);
					if (note == null)
					{
						await Initializer.Dal.DalEntitiesDeliveryStatus.DeleteAsync(connection.DepId, EntityPart.Note, guid);
						if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested || connection.DepId <= 0)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 135);
						}
					}
					else
					{
						tasks.Add(Task.Run(() => SendNoteAsync(note, connection, timeout, Priority.High)));
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 140);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 144);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 149);
				WebErrorLog.Instance.StartLog(ex, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 150);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendNoteAsync(WebNote note, ClsConnectionData connection, int timeOutMiliSecond, Priority priority)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendNoteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 160);
			}
			if (ret.HasError || connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret;
			}
			DeliveryWaiter deliveryWaiter = new DeliveryWaiter(connection.Context, note.Guid, EntityPart.Note, connection.cancellationToken);
			List<Packet> packs = WebNote.GenerateSaveRequest(new List<WebNote> { note });
			connection.SendPacketAsync(packs, priority, 0, connection.cancellationToken);
			ret.AddReturnedValue(await deliveryWaiter.WaitForDelivery(), ReturnedState.None, "SendNoteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 170);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendNoteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 174);
			ret.AddReturnedValue(ex, "", "SendNoteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNote.cs", 175);
		}
		return ret;
	}
}
