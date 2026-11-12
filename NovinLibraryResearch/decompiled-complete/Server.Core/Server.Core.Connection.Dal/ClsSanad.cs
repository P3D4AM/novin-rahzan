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
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsSanad : IEntity<WebSanad>
{
	private class NestedSanad
	{
		internal static readonly ClsSanad SanadInstance;

		static NestedSanad()
		{
			SanadInstance = new ClsSanad();
		}
	}

	public static ClsSanad SanadInstance => NestedSanad.SanadInstance;

	public Task<List<WebSanad>> ApplyAccessAsync(IEnumerable<WebSanad> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebSanad>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task SanadInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebSanad> e)
	{
		using (new JaegerMonitor("SanadInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Sanad, 1, isReceivedPacket: true);
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.Sanad.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					WebErrorLog.Instance.StartLog(ex2, "", "SanadInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 51);
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (!(connection?.IsDisposed ?? true))
					{
						await connection.SendPacketAsync(WebSanad.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebSanad> enumerable = e.ConnectionData.Where((WebSanad p) => trueSaved.Contains(p.Guid));
				if (enumerable == null || (enumerable != null && enumerable.Count() <= 0))
				{
					return;
				}
				if ((e == null || e.Connection.Source != ENSource.Accounting) && enumerable != null && enumerable.Any())
				{
					ClsConnectionData accountingConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accountingConnection != null && accountingConnection?.ClientVersion >= 10)
					{
						accountingConnection?.SendPacketAsync(WebSanad.GenerateSaveRequest(e.ConnectionData, Guid.NewGuid()), Priority.High);
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				throw new InvalidOperationException();
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "SanadInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 81);
			}
		}
	}

	internal async Task ReceptionInstance_OnReadRequest(object sender, ReadOperationEventArgs e)
	{
		using (new JaegerMonitor("ReceptionInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 5, isReceivedPacket: true);
				IEnumerable<WebSanad> items = await Server.Core.Assistance.Initializer.Dal.Sanad.GetAllAsync(e.Connection.DepId, e.Status.First(), e.ChangeReadST, e.Top, default(CancellationToken));
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebSanad.GenerateReadResponse(items), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReceptionInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 93);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnsent(ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs"))
		{
			_ = 2;
			try
			{
				if (connection.IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 102);
				}
				int millisecondsDelay = 180000;
				CancellationTokenSource tokenTimeOut = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(millisecondsDelay).Token);
				tokenTimeOut.CancelAfter(millisecondsDelay);
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Sanad, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 107);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("سند برای ارسال به حسابداری موجود نیست", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 108);
				}
				List<WebSanad> sanadList = new List<WebSanad>();
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال سند {item}) به حسابداری.", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 112);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 113);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in Sanad !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 116);
						return ret;
					}
					WebSanad webSanad = await Server.Core.Assistance.Initializer.Dal.Sanad.LoadDataAsync(connection.DepId, item);
					if (webSanad != null)
					{
						sanadList.Add(webSanad);
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendAsync(sanadList, connection), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 123);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 125);
			}
			catch (Exception ex)
			{
				if (connection?.cancellationToken.IsCancellationRequested ?? true)
				{
					Serilog.Log.Warning(ex, "");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 131);
				}
				ret.AddReturnedValue(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 132);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendAsync(List<WebSanad> sanadList, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using DeliveryBatchWaiter waiter = new DeliveryBatchWaiter(connection.Context, Guid.NewGuid(), sanadList.Select((WebSanad q) => q.Guid).ToList(), EntityPart.Sanad, connection.cancellationToken);
				connection.SendPacketAsync(WebSanad.GenerateSaveRequest(sanadList, waiter.PacketRefrencedId), Priority.High);
				ret.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 145);
				if (ret.HasError)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.Sanad, DateTime.Now, connection), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 147);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 151);
				ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSanad.cs", 152);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendSanadTafsilSendAsync(WebSanad Sanad, ClsConnectionData connection, short maxParallelTasks, CancellationToken timeOutToken)
	{
		return new ReturnedSaveFuncInfo();
	}
}
