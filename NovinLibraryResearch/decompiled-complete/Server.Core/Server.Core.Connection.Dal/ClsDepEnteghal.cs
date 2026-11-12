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

public class ClsDepEnteghal : IEntity<WebDepEnteghal>
{
	private class NestedDepEnteghal
	{
		internal static readonly ClsDepEnteghal DepEnteghalInstance;

		static NestedDepEnteghal()
		{
			DepEnteghalInstance = new ClsDepEnteghal();
		}
	}

	public static ClsDepEnteghal DepEnteghalInstance => NestedDepEnteghal.DepEnteghalInstance;

	private ClsDepEnteghal()
	{
	}

	public async Task<List<WebDepEnteghal>> ApplyAccessAsync(IEnumerable<WebDepEnteghal> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebDepEnteghal p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task DepEnteghalInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebDepEnteghal> e)
	{
		using (new JaegerMonitor("DepEnteghalInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs"))
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			List<WebDepEnteghal> requests = new List<WebDepEnteghal>();
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 1, isReceivedPacket: true);
				if (e.ConnectionData == null || e.Connection.DepId <= 0)
				{
					return;
				}
				foreach (WebDepEnteghal item in e.ConnectionData)
				{
					try
					{
						await Server.Core.Assistance.Initializer.Dal.DepEnteghal.FixAsync(e.Connection, item);
						if (e.Connection.Source == ENSource.Android_SefareshGir || e.Connection.Source == ENSource.WebSite)
						{
							item.DateM = DateTime.Now;
							if (item.Guid == Guid.Empty)
							{
								item.Guid = Guid.NewGuid();
							}
							item.IP = e.Connection.Ip;
						}
						ReturnedSaveFuncInfo saveResult = await Server.Core.Assistance.Initializer.Dal.DepEnteghal.SaveAsync(e.Connection.DepId, e.Connection.UserId, e.Connection.HashId, e.Connection.Source, item);
						if (saveResult.HasError)
						{
							responses = e?.ConnectionData?.Select((WebDepEnteghal x) => new SaveResponse
							{
								Date = DateTime.Now,
								ReferencedId = 0L,
								HostRefrenced = "",
								Guid = item.Guid,
								Message = saveResult.GetErrorMessage(),
								SaveResultStatus = false,
								Shortmessage = saveResult.GetErrorMessage()
							})?.ToList();
						}
						else
						{
							requests.Add(item);
						}
						responses.Add(new SaveResponse
						{
							Date = DateTime.Now,
							HostRefrenced = item.HostRefrenced,
							Guid = item.Guid,
							Message = item.DeliveryMessage,
							SaveResultStatus = item.DeliveryStatus.GetValueOrDefault(),
							Shortmessage = item.DeliveryShortMessage
						});
					}
					catch (Exception ex2)
					{
						Exception ex3 = ex2;
						Exception ex = ex3;
						responses = e?.ConnectionData?.Select((WebDepEnteghal x) => new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = 0L,
							HostRefrenced = "",
							Guid = item.Guid,
							Message = ex.Message,
							SaveResultStatus = false,
							Shortmessage = ex.Message
						})?.ToList();
						WebErrorLog.Instance.StartLog(ex, "", "DepEnteghalInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 108);
					}
				}
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebDepEnteghal.GenerateSaveResponse(responses), Priority.High);
				if (requests.Count > 0)
				{
					ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebDepEnteghal.GenerateSaveRequest(requests), Priority.High);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex5)
			{
				WebErrorLog.Instance.StartLog(ex5, "", "DepEnteghalInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 120);
			}
		}
	}

	internal async Task DepEnteghalInstance_OnReadRequest(object sender, ReadOperationEventArgs e)
	{
		using (new JaegerMonitor("DepEnteghalInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 5, isReceivedPacket: true);
				List<WebDepEnteghal> items = await Server.Core.Assistance.Initializer.Dal.DepEnteghal.GetAllAsync(e.Connection.DepId, e.Status, e.Top, default(CancellationToken));
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebDepEnteghal.GenerateReadResponse(items), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DepEnteghalInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 134);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnSent(ClsConnectionData connection, string ConnectionUniqueId, short maxParallelTasks = 5)
	{
		using (new JaegerMonitor("SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 300000))
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
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.DepEnteghal, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 193);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("برگه دریافت ارسال نشده برای حسابداری در هاست موجود نمیباشد.", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 195);
				}
				List<Task<ReturnedSaveFuncInfo>> taskList = new List<Task<ReturnedSaveFuncInfo>>();
				line = 10;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال برگه دریافت({item}) به حسابداری", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 200);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 201);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						return ret;
					}
					WebDepEnteghal webDepEnteghal = await Server.Core.Assistance.Initializer.Dal.DepEnteghal.GetAsync(item, connection.DepId);
					if (webDepEnteghal != null && !webDepEnteghal.CheckValidation().HasError)
					{
						line = 20;
						if (tokenTimeOut.IsCancellationRequested)
						{
							return ret;
						}
						line = 25;
						taskList.Add(SendDepEnteghalAsync(webDepEnteghal, connection));
						if (taskList.Count >= maxParallelTasks)
						{
							line = 30;
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(taskList);
							taskList.Remove(task);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 219);
						}
						line = 35;
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(taskList), ReturnedState.None, "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 223);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 227);
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				ret.AddReturnedValue(ex2, "", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 233);
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SendUnSent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 234);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendDepEnteghalAsync(WebDepEnteghal DepEnteghal, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendDepEnteghalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using DeliveryWaiter waiter = new DeliveryWaiter(connection.Context, DepEnteghal.Guid, EntityPart.DepEnteghal, connection.cancellationToken);
				connection.SendPacketAsync(WebDepEnteghal.GenerateSaveRequest(new List<WebDepEnteghal> { DepEnteghal }), Priority.Medium);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendDepEnteghalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 248);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendDepEnteghalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 252);
				ret.AddReturnedValue(ex, "", "SendDepEnteghalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepEnteghal.cs", 253);
			}
			return ret;
		}
	}
}
