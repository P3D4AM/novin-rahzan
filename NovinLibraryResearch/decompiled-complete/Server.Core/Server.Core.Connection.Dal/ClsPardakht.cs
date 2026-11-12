using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsPardakht : IEntity<WebPardakht>
{
	private class NestedPardakht
	{
		internal static readonly ClsPardakht PardakhtInstance;

		static NestedPardakht()
		{
			PardakhtInstance = new ClsPardakht();
		}
	}

	public static ClsPardakht PardakhtInstance => NestedPardakht.PardakhtInstance;

	private ClsPardakht()
	{
	}

	public Task<List<WebPardakht>> ApplyAccessAsync(IEnumerable<WebPardakht> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebPardakht>>(null);
		}
		return Task.FromResult(lst.ToList());
	}

	internal async Task PardakhtInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebPardakht> e)
	{
		using (new JaegerMonitor("PardakhtInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs"))
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			List<WebPardakht> requests = new List<WebPardakht>();
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Pardakht, 1, isReceivedPacket: true);
				if (e.ConnectionData == null)
				{
					return;
				}
				if (e.Connection?.DepId <= 0)
				{
					throw new Exception("DepId is not valid" + e.ToString());
				}
				foreach (WebPardakht item in e.ConnectionData)
				{
					ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
					try
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Pardakht.FixAsync(e.Connection, item), ReturnedState.None, "PardakhtInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 52);
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Pardakht.SaveAsync(e.Connection.DepId, e.Connection.HashId, item, e.Connection.UserId, e.Connection.Ip, e.Connection.Source, 10), ReturnedState.None, "PardakhtInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 53);
						if (!res.HasError)
						{
							requests.Add(item);
						}
					}
					catch (OperationCanceledException)
					{
						res.AddErrorTokenIsCancelled("", "", "PardakhtInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 58);
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, item.ToString(), "PardakhtInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 61);
						res.AddReturnedValue(ex2, "", "PardakhtInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 62);
					}
					responses.Add(new SaveResponse
					{
						Date = DateTime.Now,
						ReferencedId = item.ReferencedId,
						HostRefrenced = item.HostRefrenced,
						Guid = item.Guid,
						Message = res.Messages,
						SaveResultStatus = !res.HasError,
						Shortmessage = res.Messages,
						RequestRefrencedId = e.PacketReferenceId
					});
				}
				List<Packet> packs = WebPardakht.GenerateSaveResponse(responses);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(packs, Priority.Medium);
				if (requests.Count > 0)
				{
					ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebPardakht.GenerateSaveRequest(requests), Priority.High);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "PardakhtInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 83);
			}
		}
	}

	internal async Task PardakhtInstance_OnReadRequest(object sender, ReadOperationEventArgs e)
	{
		using (new JaegerMonitor("PardakhtInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Pardakht, 5, isReceivedPacket: true);
				if (e.Top > 200)
				{
					e.Top = 100;
				}
				List<WebPardakht> items = await Server.Core.Assistance.Initializer.Dal.Pardakht.GetAllAsync(e.Connection.DepId, e.Status, e.ChangeReadST, e.Top, e.Modified, e.Connection.cancellationToken);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebPardakht.GenerateReadResponse(items), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PardakhtInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 96);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnsent(ClsConnectionData connection, string connectionUniqueId, short maxParallelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs"))
		{
			int line = 0;
			try
			{
				line = 2;
				int millisecondsDelay = 180000;
				CancellationTokenSource tokenTimeOut = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(millisecondsDelay).Token);
				tokenTimeOut.CancelAfter(millisecondsDelay);
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Pardakht, connection.cancellationToken);
				line = 3;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 158);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("برگه پرداختی برای ارسال به حسابداری موجود نیست", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 159);
				}
				List<Task<ReturnedSaveFuncInfo>> taskList = new List<Task<ReturnedSaveFuncInfo>>();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال برگه پرداخت({item}) به حسابداری.", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 163);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 164);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in Pardakht !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 167);
						return ret;
					}
					WebPardakht pardakht = await Server.Core.Assistance.Initializer.Dal.Pardakht.GetAsync(item, connection.DepId);
					if (pardakht != null && !pardakht.CheckValidation().HasError)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SendPardakhtTafsilSendAsync(pardakht, connection, maxParallelTasks, tokenTimeOut.Token), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 173);
						if (tokenTimeOut.IsCancellationRequested)
						{
							WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in Pardakht !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 176);
							return ret;
						}
						taskList.Add(SendPardakhtAsync(pardakht, connection));
						if (taskList.Count >= maxParallelTasks)
						{
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(taskList);
							taskList.Remove(task);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 185);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(taskList), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 188);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 190);
			}
			catch (Exception ex)
			{
				if (connection?.cancellationToken.IsCancellationRequested ?? true)
				{
					Serilog.Log.Warning(ex, $"line:{line}");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"line:{line}", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 196);
				}
				ret.AddReturnedValue(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 197);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendPardakhtAsync(WebPardakht pardakht, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using DeliveryWaiter waiter = new DeliveryWaiter(connection.Context, pardakht.Guid, EntityPart.Pardakht, connection.cancellationToken);
				connection.SendPacketAsync(WebPardakht.GenerateSaveRequest(new List<WebPardakht> { pardakht }), Priority.Medium);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 211);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 215);
				ret.AddReturnedValue(ex, "", "SendPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 216);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendPardakhtTafsilSendAsync(WebPardakht pardakht, ClsConnectionData connection, short maxParallelTasks, CancellationToken timeOutToken)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<Guid?> list = new List<Guid?> { pardakht.TafsilGuid };
			if ((pardakht?.Naghd?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebPardakhtNaghd item in pardakht.Naghd)
				{
					list.Add(item.TafsilGuid);
				}
			}
			if ((pardakht?.Bank?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebPardakhtBank item2 in pardakht.Bank)
				{
					list.Add(item2.TafsilGuid);
				}
			}
			if ((pardakht?.CheckSh?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebPardakhtCheckSh item3 in pardakht.CheckSh)
				{
					list.Add(item3.A2re_TafsilGuid);
					list.Add(item3.BankTafsilGuid);
					list.Add(item3.DebitTafsilGuid);
				}
			}
			if ((pardakht?.Enteghal?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebSanad_Det item4 in pardakht.Enteghal)
				{
					list.Add(item4.TafsilGuid);
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsTafsil.SendTafsilIfNotDeliveredToAccAsync(connection, maxParallelTasks, list, timeOutToken), ReturnedState.None, "SendPardakhtTafsilSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 251);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendPardakhtTafsilSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 255);
			res.AddReturnedValue(ex, "", "SendPardakhtTafsilSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPardakht.cs", 256);
		}
		return res;
	}
}
