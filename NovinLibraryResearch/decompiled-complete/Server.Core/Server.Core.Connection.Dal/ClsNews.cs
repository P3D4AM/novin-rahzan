using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsNews : IEntity<WebNews>
{
	private class NestedNews
	{
		internal static readonly ClsNews NewsInstance;

		static NestedNews()
		{
			NewsInstance = new ClsNews();
		}
	}

	public static ClsNews NewsInstance => NestedNews.NewsInstance;

	private ClsNews()
	{
	}

	public async Task<List<WebNews>> ApplyAccessAsync(IEnumerable<WebNews> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebNews p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task NewsInstance_OnReadRequest(object sender, NewsReadEventArgs e)
	{
		using (new JaegerMonitor("NewsInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.News, 5, isReceivedPacket: true);
				IEnumerable<WebNews> items = await Server.Core.Assistance.Initializer.Dal.News.GetAllAsync(e.Connection.DepId, e.Modified, e.Connection.cancellationToken);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebNews.GenerateReadResponse(items, isSync: false), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NewsInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 55);
			}
		}
	}

	internal async Task NewsInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebNews> e)
	{
		using (new JaegerMonitor("NewsInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs"))
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			List<WebNews> requests = new List<WebNews>();
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.News, 1, isReceivedPacket: true);
				if (e.ConnectionData == null)
				{
					return;
				}
				if (e.Connection?.DepId <= 0)
				{
					throw new Exception("DepId is not valid" + e.ToString());
				}
				foreach (WebNews item in e.ConnectionData)
				{
					try
					{
						await Server.Core.Assistance.Initializer.Dal.News.SaveAsync(e.Connection.DepId, new List<WebNews> { item });
						requests.Add(item);
						SaveResponse item2 = new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = item.Id,
							Guid = item.Guid,
							Message = "",
							SaveResultStatus = true,
							Shortmessage = ""
						};
						responses.Add(item2);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "NewsInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 93);
						responses.Add(new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = 0L,
							Guid = item.Guid,
							Message = ex.Message,
							Shortmessage = ex.Message,
							SaveResultStatus = false
						});
					}
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebNews.GenerateSaveResponse(responses), Priority.Medium);
				}
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "NewsInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 110);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor($"Sync2Client_{connection.Source}", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs"))
		{
			int line = 0;
			try
			{
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 129);
				}
				if (connection == null || (connection?.DepId ?? 0) <= 0)
				{
					return ret.AddError("connection or syncInfo or depId is null or empty", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 130);
				}
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				line = 2;
				DateTime syncing = DateTime.Now;
				line = 3;
				IEnumerable<WebNews> lst = await Server.Core.Assistance.Initializer.Dal.News.GetAllAsync(connection.DepId, syncInformation.News, connection.cancellationToken);
				line = 5;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 137);
				}
				line = 6;
				line = 8;
				lst = await ApplyAccessAsync(lst, connection);
				line = 9;
				line = 11;
				if (lst != null && lst.Any())
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 152);
					}
					line = 12;
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.News, 300000, connection.cancellationToken);
					line = 13;
					connection.SendPacketAsync(WebNews.GenerateReadResponse(lst, isSync: true), Priority.Low);
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "GetSyncDonePacket waiter timedOut", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 159);
						return ret;
					}
					line = 15;
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.News, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 164);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 167);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 170);
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsNews .cs", 171);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}
}
