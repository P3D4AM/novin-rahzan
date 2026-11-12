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

public class ChequeMOperation : IEntity<WebChequeMOperation>
{
	private class NestedChequeMOperation
	{
		internal static readonly ChequeMOperation ChequeMOperationInstance;

		static NestedChequeMOperation()
		{
			ChequeMOperationInstance = new ChequeMOperation();
		}
	}

	public static ChequeMOperation ChequeMOperationInstance => NestedChequeMOperation.ChequeMOperationInstance;

	public async Task WebChequeMOperation_OnSaveRequest(object sender, ConnectionEventArgs<WebChequeMOperation> e)
	{
		using (new JaegerMonitor("WebChequeMOperation_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs"))
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
					ret = await Server.Core.Assistance.Initializer.Dal.ChequeMOperation.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					WebErrorLog.Instance.StartLog(ex2, "", "WebChequeMOperation_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 51);
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
						await connection.SendPacketAsync(WebChequeMOperation.GenerateSaveResponse(ret), Priority.High);
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
				IEnumerable<WebChequeMOperation> enumerable = e.ConnectionData.Where((WebChequeMOperation p) => trueSaved.Contains(p.Guid));
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
						accountingConnection?.SendPacketAsync(WebChequeMOperation.GenerateSaveRequest(e.ConnectionData, Guid.NewGuid()), Priority.High);
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
				WebErrorLog.Instance.StartLog(ex4, $"Line:{line}", "WebChequeMOperation_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 116);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnsent(ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs"))
		{
			_ = 2;
			try
			{
				int millisecondsDelay = 180000;
				CancellationTokenSource tokenTimeOut = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(millisecondsDelay).Token);
				tokenTimeOut.CancelAfter(millisecondsDelay);
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.ChequeMOperation, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 128);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("عملیات چک دریافتنی برای ارسال به حسابداری موجود نیست", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 129);
				}
				List<WebChequeMOperation> lstChequeOperation = new List<WebChequeMOperation>();
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال عملیات چک دریافتنی({item}) به حسابداری.", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 133);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 134);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in ChequeMOperation !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 137);
						return ret;
					}
					WebChequeMOperation webChequeMOperation = await Server.Core.Assistance.Initializer.Dal.ChequeMOperation.GetAsync(item, connection.DepId);
					if (webChequeMOperation != null)
					{
						lstChequeOperation.Add(webChequeMOperation);
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendAsync(lstChequeOperation, connection), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 144);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 146);
			}
			catch (Exception ex)
			{
				if (connection?.cancellationToken.IsCancellationRequested ?? true)
				{
					Serilog.Log.Warning(ex, "");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 152);
				}
				ret.AddReturnedValue(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 153);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendAsync(List<WebChequeMOperation> chequeList, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using DeliveryBatchWaiter waiter = new DeliveryBatchWaiter(connection.Context, Guid.NewGuid(), chequeList.Select((WebChequeMOperation p) => p.Guid).ToList(), EntityPart.ChequeMOperation, connection.cancellationToken);
				connection.SendPacketAsync(WebChequeMOperation.GenerateSaveRequest(chequeList, waiter.PacketRefrencedId), Priority.High);
				ret.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 166);
				if (ret.HasError)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.ChequeMOperation, DateTime.Now, connection), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 168);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 172);
				ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Cheque\\ChequeMOperation.cs", 173);
			}
			return ret;
		}
	}

	public Task<List<WebChequeMOperation>> ApplyAccessAsync(IEnumerable<WebChequeMOperation> lst, IClsConnectionData connection)
	{
		return Task.FromResult(lst?.ToList());
	}
}
