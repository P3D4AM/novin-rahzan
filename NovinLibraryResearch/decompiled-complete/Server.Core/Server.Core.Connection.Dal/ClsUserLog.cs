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
using PacketParser.TransferLayer.Waiters.Accounting;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsUserLog : IEntity<WebCashierLog>
{
	private class NestedVisit
	{
		internal static readonly ClsUserLog UserLogInstance;

		static NestedVisit()
		{
			UserLogInstance = new ClsUserLog();
		}
	}

	public static ClsUserLog UserLogInstance => NestedVisit.UserLogInstance;

	public async Task<List<WebCashierLog>> ApplyAccessAsync(IEnumerable<WebCashierLog> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs"))
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
					lst = lst?.Where((WebCashierLog p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return lst?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 50);
				return null;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnSent(ClsConnectionData connection, string connectionUniqueId, short maxParallelTasks = 5)
	{
		using (new JaegerMonitor("SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1;
				int millisecondsDelay = 180000;
				CancellationTokenSource tokenTimeOut = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(millisecondsDelay).Token);
				tokenTimeOut.CancelAfter(millisecondsDelay);
				line = 5;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.UserLog, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 69);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				List<Task<ReturnedSaveFuncInfo>> taskList = new List<Task<ReturnedSaveFuncInfo>>();
				line = 10;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 76);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						return ret;
					}
					WebCashierLog webCashierLog = await Server.Core.Assistance.Initializer.Dal.UserLog.GetAsync(item, connection.DepId);
					if (webCashierLog != null)
					{
						line = 20;
						taskList.Add(SendLogAsync(webCashierLog, connection));
						if (taskList.Count >= maxParallelTasks)
						{
							line = 30;
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(taskList);
							taskList.Remove(task);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 88);
						}
						line = 35;
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(taskList), ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 92);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 96);
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				ret.AddReturnedValue(ex2, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 102);
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 103);
			}
			return ret;
		}
	}

	public async Task UserLogInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCashierLog> e)
	{
		using (new JaegerMonitor("UserLogInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.UserLog, 1, isReceivedPacket: true);
				line = 1;
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				line = 3;
				bool result = true;
				try
				{
					line = 4;
					ret = await Server.Core.Assistance.Initializer.Dal.UserLog.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					line = 5;
					foreach (SaveResponse item in ret)
					{
						line = 6;
						item.PartitionId = e.PartitionId;
						line = 7;
					}
					line = 8;
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
					WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "UserLogInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 140);
				}
				line = 10;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					line = 11;
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 12;
					if (connection != null)
					{
						line = 13;
						await connection.SendPacketAsync(WebCashierLog.GenerateSaveResponse(ret), Priority.High);
						line = 14;
					}
				}
				line = 15;
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				line = 16;
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				line = 17;
				List<WebCashierLog> list = e.ConnectionData.Where((WebCashierLog p) => trueSaved.Contains(p.Guid)).ToList();
				line = 18;
				if (list == null || !list.Any())
				{
					return;
				}
				line = 19;
				if (e.Connection.Source != ENSource.Accounting && list.Any())
				{
					line = 20;
					ClsConnectionData accountingConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					line = 21;
					if (accountingConnection != null && accountingConnection?.ClientVersion > 8)
					{
						line = 22;
						SendLogAsync(e.ConnectionData.Single(), accountingConnection);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"#DepId_{(e?.Connection?.DepId).GetValueOrDefault(-1)}\r\n" + $"#Source_{(e?.Connection?.Source).GetValueOrDefault()}\r\n" + $"#Line_{line}", "UserLogInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 182);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendLogAsync(WebCashierLog log, ClsConnectionData connection)
	{
		BaseLocks baseLocks = new BaseLocks();
		using (new JaegerMonitor("SendLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(30000).Token).Token;
			SemaphoreSlim semaphore = await baseLocks.GetSemaphoreAsync(log.Guid.ToString());
			try
			{
				await semaphore.WaitAsync(token);
				if (connection.Source != ENSource.Accounting)
				{
					throw new NotImplementedException();
				}
				if (log.ServerStatus == ServerStatusForClient.Delivered)
				{
					return ret;
				}
				using CashierLogWaiter waiter = new CashierLogWaiter(connection.Context, log.Guid, connection.cancellationToken);
				connection.SendPacketAsync(WebCashierLog.GenerateSaveRequest(new List<WebCashierLog> { log }), Priority.Medium);
				ret.AddReturnedValue(await waiter.WaitForSaveResponse("به نظرم این کد جواب نمبگبره\r\n" + $"#DepId_{connection.DepId}\r\n" + "DepName:" + connection.DepName + "\r\n" + $"source:{connection.Source}\r\n" + $"11112222333_waiting for CashierLog with Guid:{log.Guid}"), ReturnedState.None, "SendLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 209);
				if (ret.HasError)
				{
					return ret;
				}
				log.ServerStatus = ServerStatusForClient.Delivered;
				await Server.Core.Assistance.Initializer.Dal.UserLog.SaveAsync(connection.DepId, connection.HashId, connection.Source, new List<WebCashierLog> { log }, connection.cancellationToken);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 216);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 219);
				ret.AddReturnedValue(ex, "", "SendLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserLog.cs", 220);
			}
			finally
			{
				semaphore.Release();
			}
			return ret;
		}
	}
}
