using System;
using System.Collections.Concurrent;
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

public class ClsHyperLink : IEntity<WebHyperLink>
{
	private class NestedHyperLink
	{
		internal static readonly ClsHyperLink HyperLinkInstance;

		static NestedHyperLink()
		{
			HyperLinkInstance = new ClsHyperLink();
		}
	}

	private static ConcurrentDictionary<short, DateTime> dic_isModifiedDepartmentData = new ConcurrentDictionary<short, DateTime>();

	public static ClsHyperLink HyperLinkInstance => NestedHyperLink.HyperLinkInstance;

	private ClsHyperLink()
	{
	}

	public static void UpdateDictionary(short DepId)
	{
		dic_isModifiedDepartmentData.TryRemove(DepId, out var _);
		dic_isModifiedDepartmentData.TryAdd(DepId, DateTime.Now);
	}

	public Task<List<WebHyperLink>> ApplyAccessAsync(IEnumerable<WebHyperLink> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebHyperLink>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task HyperLinkInstance_OnReadRequest(object sender, HyperLinkReadEventArgs e)
	{
		using (new JaegerMonitor("HyperLinkInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.HyperLink, 5, isReceivedPacket: true);
				IEnumerable<WebHyperLink> items = await Server.Core.Assistance.Initializer.Dal.HyperLink.GetAllAsync(e.Connection.DepId, e.Modified, default(CancellationToken));
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebHyperLink.GenerateReadResponse(items), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "HyperLinkInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 52);
			}
		}
	}

	internal async Task HyperLinkInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebHyperLink> e)
	{
		using (new JaegerMonitor("HyperLinkInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs"))
		{
			List<SaveResponse> list = new List<SaveResponse>();
			List<WebHyperLink> list2 = new List<WebHyperLink>();
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.HyperLink, 1, isReceivedPacket: true);
				if (e.ConnectionData == null)
				{
					return;
				}
				if (e.Connection?.DepId <= 0)
				{
					throw new Exception("DepId is not valid" + e.ToString());
				}
				foreach (WebHyperLink connectionDatum in e.ConnectionData)
				{
					try
					{
						Server.Core.Assistance.Initializer.Dal.HyperLink.Save(e.Connection.DepId, new List<WebHyperLink> { connectionDatum });
						list2.Add(connectionDatum);
						SaveResponse item = new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = connectionDatum.Id,
							Guid = connectionDatum.Guid,
							Message = "",
							SaveResultStatus = true,
							Shortmessage = ""
						};
						list.Add(item);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "HyperLinkInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 90);
						list.Add(new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = 0L,
							Guid = connectionDatum.Guid,
							Message = ex.Message,
							Shortmessage = ex.Message,
							SaveResultStatus = false
						});
					}
				}
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebHyperLink.GenerateSaveResponse(list), Priority.Medium);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "HyperLinkInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 106);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor($"sync2client_{connection.Source}", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 127);
				}
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 128);
					return ret;
				}
				line = 1;
				DateTime syncing = DateTime.Now;
				line = 200;
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 132);
				}
				line = 201;
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				IEnumerable<WebHyperLink> lst = await Server.Core.Assistance.Initializer.Dal.HyperLink.GetAllAsync(connection?.DepId ?? (-1), syncInformation?.HyperLink ?? DateTime.Now.AddYears(-10), connection?.cancellationToken ?? new CancellationToken(canceled: true));
				line = 3;
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 138);
					return ret;
				}
				line = 4;
				line = 6;
				lst = await ApplyAccessAsync(lst, connection);
				line = 7;
				if (lst?.Any() ?? false)
				{
					line = 9;
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.HyperLink, 300000, connection.cancellationToken);
					Task.Run(() => ConnectionManager.Instance.GetConnection(ConnectionUniqueId)?.SendPacketAsync(WebHyperLink.GenerateReadResponse(lst, isSync: true), Priority.Low));
					line = 11;
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "!await waiter.WaitForSyncDonePacket()", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 162);
						return ret;
					}
					line = 12;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.HyperLink, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 166);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 169);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "Line:" + line, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 172);
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHyperLink.cs", 173);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
