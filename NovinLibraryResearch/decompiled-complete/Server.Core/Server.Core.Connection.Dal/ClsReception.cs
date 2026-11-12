using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsReception : IEntity<WebReception>
{
	private class NestedReception
	{
		internal static readonly ClsReception ReceptionInstance;

		static NestedReception()
		{
			ReceptionInstance = new ClsReception();
		}
	}

	public static ClsReception ReceptionInstance => NestedReception.ReceptionInstance;

	private ClsReception()
	{
	}

	public Task<List<WebReception>> ApplyAccessAsync(IEnumerable<WebReception> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebReception>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task ReceptionInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebReception> e)
	{
		using (new JaegerMonitor("ReceptionInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs"))
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			List<WebReception> requests = new List<WebReception>();
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 1, isReceivedPacket: true);
				if (e.Connection.IsDisposed || e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				if (e.ConnectionData == null || e.Connection.DepId <= 0)
				{
					throw new InvalidOperationException("e.ConnectionData == null || e.Connection.DepId <= 0");
				}
				foreach (WebReception item in e.ConnectionData)
				{
					ReturnedSaveFuncInfo saveResult = new ReturnedSaveFuncInfo();
					try
					{
						await Server.Core.Assistance.Initializer.Dal.Reception.FixAsync(e.Connection, item);
						if (e.Connection.Source == ENSource.Android_SefareshGir || e.Connection.Source == ENSource.WebSite)
						{
							throw new NotImplementedException();
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = saveResult;
						returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Reception.SaveAsync(e.Connection.DepId, e.Connection.UserId, e.Connection.HashId, e.Connection.Source, item), ReturnedState.None, "ReceptionInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 103);
						if (!saveResult.HasError)
						{
							requests.Add(item);
						}
						responses.Add(new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = item.ReferencedId,
							HostRefrenced = item.HostRefrenced,
							Guid = item.Guid,
							Message = item.DeliveryMessage,
							SaveResultStatus = !saveResult.HasError,
							Shortmessage = item.DeliveryShortMessage
						});
					}
					catch (Exception ex2)
					{
						Exception ex3 = ex2;
						Exception ex = ex3;
						responses = e?.ConnectionData?.Select((WebReception x) => new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = 0L,
							HostRefrenced = "",
							Guid = item.Guid,
							Message = ex.Message,
							SaveResultStatus = false,
							Shortmessage = ex.Message
						})?.ToList();
						WebErrorLog.Instance.StartLog(ex, "", "ReceptionInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 129);
					}
				}
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebReception.GenerateSaveResponse(responses), Priority.High);
				if (requests.Count > 0)
				{
					ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebReception.GenerateSaveRequest(requests), Priority.High);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex5)
			{
				WebErrorLog.Instance.StartLog(ex5, "", "ReceptionInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 141);
			}
		}
	}

	internal async Task ReceptionInstance_OnReadRequest(object sender, ReadOperationEventArgs e)
	{
		using (new JaegerMonitor("ReceptionInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 5, isReceivedPacket: true);
				List<WebReception> items = await Server.Core.Assistance.Initializer.Dal.Reception.GetAllAsync(e.Connection.DepId, e.Status, e.ChangeReadST, e.Modified, e.Connection.cancellationToken, e.Top);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebReception.GenerateReadResponse(items), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReceptionInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 153);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnSent(ClsConnectionData connection, string ConnectionUniqueId, short maxParallelTasks = 5)
	{
		using (new JaegerMonitor("SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 300000))
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
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Reception, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 170);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("برگه دریافت ارسال نشده برای حسابداری در هاست موجود نمیباشد.", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 172);
				}
				List<Task<ReturnedSaveFuncInfo>> taskList = new List<Task<ReturnedSaveFuncInfo>>();
				line = 10;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال برگه دریافت({item}) به حسابداری", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 177);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 178);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						return ret;
					}
					WebReception reception = await Server.Core.Assistance.Initializer.Dal.Reception.GetAsync(item, connection.DepId);
					if (reception != null && !reception.CheckValidation().HasError)
					{
						line = 20;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SendReceptionTafsilSendAsync(reception, connection, maxParallelTasks, tokenTimeOut.Token), ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 185);
						if (tokenTimeOut.IsCancellationRequested)
						{
							return ret;
						}
						line = 25;
						taskList.Add(SendReceptionAsync(reception, connection));
						if (taskList.Count >= maxParallelTasks)
						{
							line = 30;
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(taskList);
							taskList.Remove(task);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 196);
						}
						line = 35;
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(taskList), ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 200);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 204);
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				ret.AddReturnedValue(ex2, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 210);
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 211);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendReceptionAsync(WebReception reception, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using DeliveryWaiter waiter = new DeliveryWaiter(connection.Context, reception.Guid, EntityPart.Reception, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebReception.GenerateSaveRequest(new List<WebReception> { reception }), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 225);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 229);
				ret.AddReturnedValue(ex, "", "SendReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 230);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendReceptionTafsilSendAsync(WebReception reception, ClsConnectionData connection, short maxParallelTasks, CancellationToken timeOutToken)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<Guid?> list = new List<Guid?> { reception.TafsilGuid };
			if ((reception?.ListNaghd?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebReceptionNaghd item in reception.ListNaghd)
				{
					list.Add(item.TafsilGuid);
				}
			}
			if ((reception?.ListBank?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebReceptionHavale item2 in reception.ListBank)
				{
					list.Add(item2.TafsilGuid);
				}
			}
			if ((reception?.ListCheck?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebReceptionCheck item3 in reception.ListCheck)
				{
					list.Add(item3.A2re_PardazandeTafsilGuid);
				}
			}
			if ((reception?.ListEnteghal?.Count).GetValueOrDefault() > 0)
			{
				foreach (WebSanad_Det item4 in reception.ListEnteghal)
				{
					list.Add(item4.TafsilGuid);
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsTafsil.SendTafsilIfNotDeliveredToAccAsync(connection, maxParallelTasks, list, timeOutToken), ReturnedState.None, "SendReceptionTafsilSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 261);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendReceptionTafsilSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 265);
			res.AddReturnedValue(ex, "", "SendReceptionTafsilSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsReception.cs", 266);
		}
		return res;
	}
}
