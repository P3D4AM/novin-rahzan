using System;
using System.Collections.Concurrent;
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

public class ClsCusTitle : IEntity<WebCusTitle>
{
	private class NestedCusTitle
	{
		internal static readonly ClsCusTitle CusTitleInstance;

		static NestedCusTitle()
		{
			CusTitleInstance = new ClsCusTitle();
		}
	}

	private static ConcurrentDictionary<short, DateTime> dic_isModifiedDepartmentData = new ConcurrentDictionary<short, DateTime>();

	public static ClsCusTitle CusTitleInstance => NestedCusTitle.CusTitleInstance;

	private ClsCusTitle()
	{
	}

	public async Task<List<WebCusTitle>> ApplyAccessAsync(IEnumerable<WebCusTitle> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebCusTitle p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task CusTitleInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCusTitle> e)
	{
		using (new JaegerMonitor("CusTitleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.CustomerTitle, 1, isReceivedPacket: true);
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.CusTitle.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					result = false;
					WebErrorLog.Instance.StartLog(ex2, "", "CusTitleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 69);
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection != null)
					{
						await connection.SendPacketAsync(WebCusTitle.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebCusTitle> truSavedInRet = e.ConnectionData.Where((WebCusTitle p) => trueSaved.Contains(p.Guid));
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				if ((e == null || e.Connection.Source != ENSource.Accounting) && (truSavedInRet?.Any() ?? false))
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 8)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebCusTitle.GenerateSaveRequest(truSavedInRet), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting && p.ClientVersion > 8);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData Connection in enumerable)
				{
					List<WebCusTitle> dataForSend = await ApplyAccessAsync(truSavedInRet, Connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => Connection.SendPacketAsync(WebCusTitle.GenerateReadResponse(dataForSend, isSync: false), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "CusTitleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 115);
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
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs"))
		{
			int line = 0;
			try
			{
				if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 130);
				}
				line = 1;
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				line = 2;
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 134);
					return ret;
				}
				line = 3;
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				line = 4;
				DateTime customerTitleSyncing = DateTime.Now;
				line = 5;
				IEnumerable<WebCusTitle> lst = await Server.Core.Assistance.Initializer.Dal.CusTitle.GetAll(connection.DepId, syncInformation.CustomerTitle, connection.cancellationToken);
				line = 6;
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 143);
					return ret;
				}
				line = 7;
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 152);
					return ret;
				}
				line = 8;
				lst = await ApplyAccessAsync(lst, connection);
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 157);
					return ret;
				}
				line = 10;
				if (lst != null && lst.Any())
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.CustomerTitle, 300000, connection.cancellationToken);
					connection.SendPacketAsync(WebCusTitle.GenerateReadResponse(lst, isSync: true), Priority.Medium);
					line = 15;
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "GenerateReadResponse waiter.WaitForSyncDonePacket", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 166);
						return ret;
					}
					line = 20;
					if (connection?.IsDisposed ?? true)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 170);
						return ret;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.CustomerTitle, customerTitleSyncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 171);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 176);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 177);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
				sb.AppendLine($"CusTitleSyncResult:{!ret.HasError}");
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendTitleAsync(WebCusTitle title, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using CusTitleWaiter waiter = new CusTitleWaiter(connection.Context, title.Guid, connection.cancellationToken);
				connection.SendPacketAsync(WebCusTitle.GenerateSaveRequest(new List<WebCusTitle> { title }), Priority.Medium);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("ClsCusTitle.SendTitleAsync"), ReturnedState.None, "SendTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 195);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 197);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 200);
				ret.AddReturnedValue(ex, "", "SendTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 201);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId, short maxParalelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs"))
		{
			_ = 4;
			try
			{
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"Source({connection.Source})!=accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 211);
				}
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.CustomerTitle, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 213);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					ret.AddInformation($"sending CusTitle({item}) to accounting connecion", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 219);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 220);
					}
					WebCusTitle webCusTitle = await Server.Core.Assistance.Initializer.Dal.CusTitle.GetAsync(connection.DepId, item);
					if (webCusTitle != null)
					{
						tasks.Add(SendTitleAsync(webCusTitle, connection));
						if (tasks.Count >= maxParalelTasks)
						{
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
							tasks.Remove(task);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 228);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 231);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 235);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 240);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusTitle.cs", 241);
			}
			return ret;
		}
	}
}
