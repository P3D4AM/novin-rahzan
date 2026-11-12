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

namespace Server.Core.Connection.Dal.Cheque;

public class ChequeShOperation : IEntity<WebChequeShOperation>
{
	private class NestedChequeShOperation
	{
		internal static readonly ChequeShOperation ChequeShOperationInstance;

		static NestedChequeShOperation()
		{
			ChequeShOperationInstance = new ChequeShOperation();
		}
	}

	public static ChequeShOperation ChequeShOperationInstance => NestedChequeShOperation.ChequeShOperationInstance;

	public async Task WebChequeShOperation_OnSaveRequest(object sender, ConnectionEventArgs<WebChequeShOperation> e)
	{
		using (new JaegerMonitor("WebChequeShOperation_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.CheckMOperation, 1, isReceivedPacket: true);
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
					ret = await Server.Core.Assistance.Initializer.Dal.ChequeShOperation.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					WebErrorLog.Instance.StartLog(ex2, "", "WebChequeShOperation_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 51);
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				line = 150;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					line = 151;
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 152;
					if (!(connection?.IsDisposed ?? true))
					{
						await connection.SendPacketAsync(WebChequeShOperation.GenerateSaveResponse(ret), Priority.High);
					}
				}
				line = 16;
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				line = 17;
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				line = 18;
				IEnumerable<WebChequeShOperation> enumerable = e.ConnectionData.Where((WebChequeShOperation p) => trueSaved.Contains(p.Guid));
				line = 19;
				if (enumerable == null || (enumerable != null && enumerable.Count() <= 0))
				{
					return;
				}
				line = 20;
				if ((e == null || e.Connection.Source != ENSource.Accounting) && enumerable != null && enumerable.Any())
				{
					line = 21;
					ClsConnectionData accountingConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accountingConnection != null && accountingConnection?.ClientVersion >= 10)
					{
						accountingConnection?.SendPacketAsync(WebChequeShOperation.GenerateSaveRequest(e.ConnectionData, Guid.NewGuid()), Priority.High);
					}
				}
				line = 23;
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
				WebErrorLog.Instance.StartLog(ex4, $"Line:{line}", "WebChequeShOperation_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 116);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnsent(ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs"))
		{
			_ = 2;
			try
			{
				int millisecondsDelay = 180000;
				CancellationTokenSource tokenTimeOut = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(millisecondsDelay).Token);
				tokenTimeOut.CancelAfter(millisecondsDelay);
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.ChequeShOperation, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 128);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("عملیات چک پرداختنی برای ارسال به حسابداری موجود نیست", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 129);
				}
				List<WebChequeShOperation> chequeList = new List<WebChequeShOperation>();
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال عملیات چک پرداختنی({item}) به حسابداری.", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 133);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 134);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in ChequeShOperation !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 137);
						return ret;
					}
					WebChequeShOperation webChequeShOperation = await Server.Core.Assistance.Initializer.Dal.ChequeShOperation.GetAsync(item, connection.DepId);
					if (webChequeShOperation != null)
					{
						chequeList.Add(webChequeShOperation);
						if (tokenTimeOut.IsCancellationRequested)
						{
							WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in ChequeShOperation !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 145);
							return ret;
						}
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendAsync(chequeList, connection), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 149);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 151);
			}
			catch (Exception ex)
			{
				if (connection?.cancellationToken.IsCancellationRequested ?? true)
				{
					Serilog.Log.Warning(ex, "");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 157);
				}
				ret.AddReturnedValue(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 158);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendAsync(List<WebChequeShOperation> chequeList, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using DeliveryBatchWaiter waiter = new DeliveryBatchWaiter(connection.Context, Guid.NewGuid(), chequeList.Select((WebChequeShOperation q) => q.Guid)?.ToList(), EntityPart.ChequeShOperation, connection.cancellationToken);
				connection.SendPacketAsync(WebChequeShOperation.GenerateSaveRequest(chequeList, waiter.PacketRefrencedId), Priority.High);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 170);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.ChequeShOperation, DateTime.Now, connection), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 171);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 175);
				ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeShOperation.cs", 176);
			}
			return ret;
		}
	}

	public Task<List<WebChequeShOperation>> ApplyAccessAsync(IEnumerable<WebChequeShOperation> lst, IClsConnectionData connection)
	{
		return Task.FromResult(lst?.ToList());
	}
}
