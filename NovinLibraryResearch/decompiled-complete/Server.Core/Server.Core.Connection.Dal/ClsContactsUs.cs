using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsContactsUs : IEntity<WebContactUs>
{
	private class NestedContactsUs
	{
		internal static readonly ClsContactsUs ContactsUsInstance;

		static NestedContactsUs()
		{
			ContactsUsInstance = new ClsContactsUs();
		}
	}

	public static ClsContactsUs ContactsUsInstance => NestedContactsUs.ContactsUsInstance;

	private ClsContactsUs()
	{
	}

	public async Task<List<WebContactUs>> ApplyAccessAsync(IEnumerable<WebContactUs> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebContactUs p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task ContactUsInstance_OnSaveRequestAsync(object sender, ConnectionEventArgs<WebContactUs> e)
	{
		int line = 0;
		using (new JaegerMonitor("ContactUsInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs"))
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			new List<WebContactUs>();
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.ContactUs, 1, isReceivedPacket: true);
				if (e.ConnectionData == null || e.Connection == null)
				{
					return;
				}
				if (e.Connection?.DepId <= 0)
				{
					throw new Exception("DepId is not valid" + e.ToString());
				}
				line = 1;
				List<SaveResponse> collection = await Server.Core.Assistance.Initializer.Dal.ContactUs.SaveAsync(e.Connection.DepId, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken, e.Connection.Source);
				line = 2;
				responses.AddRange(collection);
				line = 3;
				if (responses != null && responses.Count > 0)
				{
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebContactUs.GenerateSaveResponse(responses), Priority.High);
				}
				line = 4;
				ConnectionEventArgs<WebContactUs> connectionEventArgs = e;
				if (connectionEventArgs == null || connectionEventArgs.Connection.Source != ENSource.Accounting)
				{
					ConnectionEventArgs<WebContactUs> connectionEventArgs2 = e;
					if (connectionEventArgs2 != null && connectionEventArgs2.ConnectionData?.Count > 0)
					{
						ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebContactUs.GenerateSaveRequest(e.ConnectionData), Priority.Medium);
					}
				}
				line = 50;
				List<WebContactUs> connectionData = e.ConnectionData;
				if (connectionData == null || connectionData.Count <= 0)
				{
					return;
				}
				line = 51;
				List<ClsConnectionData> connectionsData = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId);
				line = 52;
				connectionsData = connectionsData?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && ((p != null && p.Source == ENSource.Android_HesabYar) || (p != null && p.Source == ENSource.IOS_HesabYar) || (p != null && p.Source == ENSource.Android_SefareshGir) || (p != null && p.Source == ENSource.Baje) || (p != null && p.Source == ENSource.AccountingPortal) || (p != null && p.Source == ENSource.WebsiteWordPress) || (p != null && p.Source == ENSource.WebSite))).ToList();
				line = 60;
				if (connectionsData != null)
				{
					foreach (ClsConnectionData Connection in connectionsData)
					{
						if (Connection != null && !Connection.IsDisposed)
						{
							List<WebContactUs> list = await ApplyAccessAsync(e.ConnectionData, Connection);
							if (list != null && list.Any() && Connection != null && !Connection.IsDisposed)
							{
								await Connection.SendPacketAsync(WebContactUs.GenerateReadResponse(list), Priority.Medium);
							}
						}
					}
				}
				line = 6;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "ContactUsInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 111);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 125);
				}
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				if (syncInformation == null)
				{
					throw new OperationCanceledException();
				}
				DateTime syncing = DateTime.Now;
				IEnumerable<WebContactUs> lst = await Server.Core.Assistance.Initializer.Dal.ContactUs.GetAllAsync(connection.DepId, syncInformation.ContactUs, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 131);
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 135);
				}
				lst = await ApplyAccessAsync(lst, connection);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 147);
				}
				if (connection == null || connection.IsDisposed)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection == null || connection.IsDisposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 150);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 153);
				}
				if (lst != null && lst.Any())
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.ContactUs, 300000, connection.cancellationToken);
					connection.SendPacketAsync(WebContactUs.GenerateReadResponse(lst, isSync: true), Priority.Low);
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "!await waiter.WaitForSyncDonePacket()//ClsContacUs.Sync2Client", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 160);
						return ret;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.ContactUs, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 164);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 167);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 170);
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsContactsUs.cs", 171);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}
}
