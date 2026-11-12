using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Accounting.TaxService;
using PacketParser.EventArgs;
using PacketParser.EventArgs.TaxService;
using PacketParser.LogPacket;
using PacketParser.Records.TaxService;
using PacketParser.TaxService;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsOrder : IEntity<WebOrder>
{
	public ITaxConnectionManager TaxConnectionManager { get; private set; }

	public ClsOrder(ITaxConnectionManager taxManager)
	{
		TaxConnectionManager = taxManager;
	}

	public Task<List<WebOrder>> ApplyAccessAsync(IEnumerable<WebOrder> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebOrder>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task FactorInstance_OnReadRequest(object sender, ReadOperationEventArgs e)
	{
		using (new JaegerMonitor("FactorInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.PishFactor, 5, isReceivedPacket: true);
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				List<WebOrder> items = await Server.Core.Assistance.Initializer.Dal.Order.GetNotDeliveredAsync(e.Connection.DepId, e.Status.First(), e.ChangeReadST, e.Top, cancellationTokenSource.Token);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebOrder.GenerateReadResponse(items), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FactorInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 42);
			}
		}
	}

	internal async Task FactorInstance_OnSaveTempRequest(object sender, ConnectionEventArgs<WebOrder> e)
	{
		using (new JaegerMonitor("FactorInstance_OnSaveTempRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs"))
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			List<WebOrder> requests = new List<WebOrder>();
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.PishFactor, 3, isReceivedPacket: true);
				if (e?.ConnectionData == null || e?.Connection == null || e.Connection.DepId <= 0)
				{
					return;
				}
				foreach (WebOrder item in e.ConnectionData)
				{
					if (item == null)
					{
						continue;
					}
					WebCashier webCashier = await ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).GetCashierInfoAsync();
					if (webCashier != null)
					{
						item.UserAccGuid = webCashier.Guid;
					}
					try
					{
						Task.Run(() => Server.Core.Assistance.Initializer.Dal.Order.SaveTempAsync(e.Connection, e.Connection.CustomerTafsilGuid, item));
						requests.Add(item);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "FactorInstance_OnSaveTempRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 66);
					}
					SaveResponse item2 = new SaveResponse
					{
						Date = DateTime.Now,
						ReferencedId = item.ReferencedId,
						Guid = item.Guid,
						Message = item.DeliveryMessage,
						SaveResultStatus = true,
						Shortmessage = item.DeliveryShortMessage
					};
					responses.Add(item2);
				}
				if (requests.Count > 0)
				{
					List<Packet> packs = WebOrder.GenerateSaveTempRequest(requests);
					ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(packs, Priority.High);
				}
				if (responses.Count > 0)
				{
					List<Packet> packs2 = WebOrder.GenerateSaveTempResponse(responses);
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(packs2, Priority.High);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "FactorInstance_OnSaveTempRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 93);
			}
		}
	}

	internal async Task FactorInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebOrder> e)
	{
		using (new JaegerMonitor("FactorInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs"))
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			int line = 0;
			try
			{
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(e.Connection.UserId);
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.PishFactor, 1, isReceivedPacket: true);
				if (e?.ConnectionData == null || e?.Connection == null)
				{
					return;
				}
				if ((e != null && e.Connection.HashId <= 0) || e.Connection.DepId <= 0)
				{
					return;
				}
				if (e.Connection.Source == ENSource.WebsiteWordPress)
				{
					new WebTelegramMessage(user.WordPressTransferDataLogger).Sent2web("در خواست ذخیره فاکتور وردپرس\r\n" + $"Guid:{e?.ConnectionData?.SingleOrDefault()?.Guid}\r\n" + $"Id:{e?.ConnectionData?.SingleOrDefault()?.Id}\r\n" + $"Date:{e?.ConnectionData?.SingleOrDefault()?.Date}\r\n" + $"Sum:{e?.ConnectionData?.SingleOrDefault()?.Sum}\r\n" + $"DepId:{e.Connection.DepId}\r\n" + $"IsConnectionDisposed:{e.Connection.IsDisposed}\r\n" + $"ConnectionTokeIsCanceled:{e.Connection.cancellationToken.IsCancellationRequested}", null, "FactorInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 113);
				}
				foreach (WebOrder item in e.ConnectionData)
				{
					if (item == null)
					{
						continue;
					}
					try
					{
						if (item.UserAccGuid == Guid.Empty)
						{
							item.UserAccGuid = e.Connection.UserAccGuid;
						}
						await Server.Core.Assistance.Initializer.Dal.Order.FixAsync(e.Connection.DepId, item, 0, 0);
						if (e.Connection.Source == ENSource.Android_SefareshGir || e.Connection.Source == ENSource.WebSite)
						{
							throw new NotImplementedException();
						}
						if (e.Connection.cancellationToken.IsCancellationRequested)
						{
							return;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = await Server.Core.Assistance.Initializer.Dal.Order.SaveAsync(e.Connection.Context, e.Connection, item, e.Connection.CustomerTafsilGuid != Guid.Empty);
						if (!returnedSaveFuncInfo.HasError)
						{
							ClsConnectionData accConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
							if (!(accConnection?.IsDisposed ?? true))
							{
								int timeOut = 180000;
								Task.Run(() => SendOrderAsync(item, accConnection, timeOut, Priority.High, "FactorOnSaveRequest"));
							}
						}
						if (e.Connection.cancellationToken.IsCancellationRequested)
						{
							return;
						}
						if (e.Connection.Source == ENSource.WebsiteWordPress)
						{
							new WebTelegramMessage(user.WordPressTransferDataLogger).Sent2web("Order saved.\r\n" + $"save has error:{returnedSaveFuncInfo.HasError}.\r\n" + "error message:" + returnedSaveFuncInfo.GetErrorMessage() + "\r\n" + $"Guid:{item.Guid}\r\n" + $"Id:{item.Id}\r\n" + $"Date:{item.Date}\r\n" + $"Sum:{item.Sum}\r\n" + $"DepId:{e.Connection.DepId}\r\n" + $"IsConnectionDisposed:{e.Connection.IsDisposed}\r\n" + $"ConnectionTokeIsCanceled:{e.Connection.cancellationToken.IsCancellationRequested}", null, "FactorInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 161);
						}
						responses.Add(new SaveResponse
						{
							Date = DateTime.Now,
							ReferencedId = item.ReferencedId,
							HostRefrenced = item.HostRefrenced,
							Guid = item.Guid,
							Message = item.DeliveryMessage,
							SaveResultStatus = item.DeliveryStatus.GetValueOrDefault(),
							Shortmessage = item.DeliveryShortMessage
						});
					}
					catch (OperationCanceledException)
					{
						return;
					}
					catch (Exception ex3)
					{
						Exception ex4 = ex3;
						Exception ex = ex4;
						responses = e.ConnectionData.Select((WebOrder p) => new SaveResponse
						{
							Guid = p.Guid,
							Date = DateTime.Now,
							HostRefrenced = "",
							Message = ex.Message,
							PartitionId = 0,
							ReferencedId = 0L,
							RequestPart = 0,
							RequestRefrencedId = p.Guid,
							SaveResultStatus = false,
							Shortmessage = ex.Message
						})?.ToList();
						WebErrorLog.Instance.StartLog(ex, "", "FactorInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 199);
						if (e.Connection.Source == ENSource.WebsiteWordPress)
						{
							new WebTelegramMessage(user.WordPressTransferDataLogger).Sent2web("Order save error." + ex.Message, null, "FactorInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 201);
						}
					}
				}
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				if ((responses?.Any() ?? false) && connection != null && !connection.IsDisposed && !connection.cancellationToken.IsCancellationRequested)
				{
					await connection.SendPacketAsync(WebOrder.GenerateSaveResponse(responses), Priority.High);
					if (e.Connection.Source == ENSource.WebsiteWordPress)
					{
						new WebTelegramMessage(user.WordPressTransferDataLogger).Sent2web("Order save response sent to client\r\n" + $"Guid:{e?.ConnectionData?.SingleOrDefault()?.Guid}\r\n" + $"Id:{e?.ConnectionData?.SingleOrDefault()?.Id}\r\n" + $"Date:{e?.ConnectionData?.SingleOrDefault()?.Date}\r\n" + $"Sum:{e?.ConnectionData?.SingleOrDefault()?.Sum}\r\n" + $"DepId:{e.Connection.DepId}\r\n" + $"IsConnectionDisposed:{e.Connection.IsDisposed}\r\n" + $"ConnectionTokeIsCanceled:{e.Connection.cancellationToken.IsCancellationRequested}", null, "FactorInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 214);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex6)
			{
				WebErrorLog.Instance.StartLog(ex6, string.Format("Line:{0}//e:{1}", line, e?.ToString() ?? "null"), "FactorInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 225);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnsent(ClsConnectionData connection, short maxParallelTask = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 300000))
		{
			int line = 0;
			try
			{
				if (connection.IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 235);
				}
				if (connection.Source != ENSource.Accounting)
				{
					throw new InvalidOperationException("این کد  فقط برای حسابداری باید اجرا شود");
				}
				WebUser user = await connection.GetUserAsync();
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Order, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 241);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("فاکتوری در سرور برای ارسال به حسابداری وجود ندارد.", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 243);
				}
				int timeOut = 30000;
				ReturnedSaveFuncInfo returnedSaveFuncInfo2;
				foreach (Guid guid in list)
				{
					ret.AddInformation($"در حال ارسال فاکتور ({guid}) به حسابداری", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 247);
					if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested || connection.DepId <= 0)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 248);
					}
					WebOrder order = await Server.Core.Assistance.Initializer.Dal.Order.LoadDataAsync(connection.DepId, guid);
					if (order == null)
					{
						await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.DeleteAsync(connection.DepId, EntityPart.Order, guid);
						if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested || connection.DepId <= 0)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 253);
						}
						WebTelegramMessage.GetNp_ErrorHanlerUnhandled_bot().Sent2web("فاکتوری با این Guid در دیتابیس پیدا نکردم به همین دلیل از جدول دلیوری ها این شناسه رو هم حذف میکنم\r\n" + $"#depId{connection?.DepId ?? (-1)}\r\n" + "SendFactor to Accounting returned because order not found!\r\n" + $"Guid:{guid}", null, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 254);
						continue;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = order.CheckValidation();
					if (returnedSaveFuncInfo.HasError)
					{
						WebTelegramMessage.GetNp_ErrorHanlerUnhandled_bot().Sent2web("این یک خطای مدیریت نشده نیست\r\nیک فاکتور با نقض در اطلاعات ولیدیشین در هاست ذخیره شده که حالا میخواد برای حسابداری اش ارسال بشه.\r\n" + $"#depId{connection?.DepId ?? (-1)}\r\n" + "Source:" + (connection?.Source.GetDisplay()?.FixForTelegram() ?? "#SourceIsNull") + "\r\nUser:" + user?.Name?.FixForTelegram() + "\r\nUsername:" + connection?.Username?.FixForTelegram() + "\r\n" + $"orderGuid:{order?.Guid}\r\n" + returnedSaveFuncInfo.GetErrorMessage(), null, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 264);
					}
					else
					{
						tasks.Add(Task.Run(() => SendOrderAsync(order, connection, timeOut, Priority.High, "RenedNotSent")));
						await Task.Delay(1000);
						if (tasks.Count > maxParallelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo2 = ret;
							returnedSaveFuncInfo2.AddReturnedValue(await t, ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 280);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo2 = ret;
				returnedSaveFuncInfo2.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 284);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 288);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 293);
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 294);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> WaitForTafsilAndProductsDelivery(WebOrder order, int timeOutMiliSecond, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		Stopwatch sw = Stopwatch.StartNew();
		string logInfo = "";
		try
		{
			logInfo = $"#DepId_{connection.DepId}\r\n" + $"#OrderGuid : {order.Guid}\r\n" + "Order: " + Json.ToStringJson(order, indented: true);
			CancellationTokenSource tokenTimeOut = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(timeOutMiliSecond).Token);
			tokenTimeOut.CancelAfter(timeOutMiliSecond);
			List<Guid?> guids = new List<Guid?> { order.TafsilGuid, order.B1TafsilGuid, order.B2TafsilGuid, order.B3TafsilGuid, order.B4TafsilGuid };
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			if (connection.ClientVersion >= 10)
			{
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsTafsil.SendTafsilIfNotDeliveredToAccAsync(connection, 10, guids, tokenTimeOut.Token), ReturnedState.None, "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 321);
			}
			if (tokenTimeOut.IsCancellationRequested)
			{
				WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in Order !!!\r\n" + logInfo, "", "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 324);
				return ret;
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCustomer.SendCustomerIfNotDeliveredToAccAsync(connection, 10, guids, tokenTimeOut.Token), ReturnedState.None, "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 327);
			if (tokenTimeOut.IsCancellationRequested)
			{
				WebErrorLog.Instance.StartLog("*** TimeOut Token is Cancell in Order ***\r\n" + logInfo, "", "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 330);
				return ret;
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsProduct.SendProductIfNotDeliveredToAccAsync(connection, 10, order.ListKala?.Select((WebOrderDetail q) => q.ProductGuid)?.ToList()), ReturnedState.None, "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 333);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "desc:\r\n" + logInfo, "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 337);
			ret.AddReturnedValue(ex, "", "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 338);
		}
		finally
		{
			if (sw.ElapsedMilliseconds > 60000)
			{
				WebErrorLog.Instance.StartLog($"Wait for order delivery is more than one minutes: {sw.ElapsedMilliseconds}(ms)\r\n{logInfo}", "", "WaitForTafsilAndProductsDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 343);
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SendOrderAsync(WebOrder order, ClsConnectionData connection, int timeOutMiliSecond, Priority priority, string callerName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		StringBuilder sb = new StringBuilder(100000);
		Guid.NewGuid();
		try
		{
			sb.AppendLine("CalleName:" + callerName?.FixForTelegram());
			sb.AppendLine($"DepId:{connection?.DepId ?? (-1)}");
			sb.AppendLine($"IsDisposed:{connection?.IsDisposed ?? true}");
			sb.AppendLine($"OrderGuid:{order?.Guid ?? Guid.Empty}");
			line = 1000;
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 361);
			}
			line = 1001;
			line = 1002;
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 370);
			}
			line = 1003;
			ret.throwExceptionIfError();
			line = 1004;
			if (ret.HasError || connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				sb.AppendLine("❌❌❌❌ has error=True");
				line = 3;
				return ret.AddError("timedOut or cancellationToken is canceeled for send order!", "", "SendOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 381);
			}
			line = 4;
			sb.AppendLine("before init waiter".FixForTelegram());
			using DeliveryWaiter waiter = new DeliveryWaiter(connection.Context, order.Guid, EntityPart.Order, connection.cancellationToken);
			line = 5;
			sb.AppendLine("before send".FixForTelegram());
			sb.AppendLine("sendPacketPriority:" + priority.GetDisplay());
			sb.AppendLine($"WaitForDelivery:{connection.WaitForDelivery}");
			List<Packet> pack = WebOrder.GenerateSaveRequest(new List<WebOrder> { order });
			if (!pack.Any())
			{
				throw new NullReferenceException("Order.GenerateSaveRequest returnedNull!!!!");
			}
			await WaitForTafsilAndProductsDelivery(order, 300000, connection);
			Task.Run(() => connection.SendPacketAsync(pack, priority, 0, connection.cancellationToken));
			line = 6;
			sb.AppendLine("after send".FixForTelegram());
			ReturnedSaveFuncInfoWithValue<WebDelivery> returnedSaveFuncInfoWithValue = await waiter.WaitForDelivery(timeOutMiliSecond);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SendOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 406);
			sb.AppendLine($"waiterResult has error:{returnedSaveFuncInfoWithValue.HasError}");
		}
		catch (Exception ex)
		{
			sb.AppendLine("❌❌❌❌ Exception:" + ex.Message);
			ClsConnectionData clsConnectionData = connection;
			if (clsConnectionData == null || !clsConnectionData.IsDisposed)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//Version:{connection?.ClientVersion ?? (-1)}//DepId:{connection?.DepId ?? (-1)}", "SendOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 415);
				ret.AddReturnedValue(ex, "", "SendOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 417);
			}
		}
		finally
		{
			sb.AppendLine($"FinalHasErro:{ret.HasError}");
		}
		return ret;
	}

	internal async Task OrderInstance_OnSendToTaxServiceRequestAsync(object sender, ConnectionEventArgs<Tuple<string, WebOrder>> orderTupple)
	{
		using (new JaegerMonitor("OrderInstance_OnSendToTaxServiceRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs"))
		{
			_ = 5;
			try
			{
				string clientId = orderTupple?.ConnectionData?.FirstOrDefault()?.Item1 ?? "";
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataByTaxClientIdAsync(orderTupple.Connection.DepId, clientId);
				WebOrder order = orderTupple?.ConnectionData?.FirstOrDefault()?.Item2;
				if (orderTupple.Connection.IsDisposed || orderTupple.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				ReturnedSaveFuncInfoWithValue<ITaxApiConnection> taxConnection = await TaxConnectionManager.GetOrHandleConnectionAsync(user, orderTupple.ConnectionUniqueId, handleLoginResponse: false);
				if (orderTupple.Connection.IsDisposed || orderTupple.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				TaxOrder item = new TaxOrder
				{
					ClientId = clientId,
					OrderGuid = order.Guid
				};
				if (orderTupple.Connection.IsDisposed || orderTupple.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await Server.Core.Assistance.Initializer.Dal.TaxSrvice.InsertAsync(orderTupple.Connection.DepId, orderTupple.Connection.UserId, item, orderTupple.Connection.cancellationToken);
				if (!orderTupple.Connection.IsDisposed && !orderTupple.Connection.cancellationToken.IsCancellationRequested && !returnedSaveFuncInfoWithValue.HasError && returnedSaveFuncInfoWithValue.value > 0 && !taxConnection.HasError)
				{
					ReturnedSaveFuncInfoWithValue<TaxInvoiceResultEventArgs> data = await taxConnection.value.SendAsync(order, returnedSaveFuncInfoWithValue.value);
					if (data?.value != null)
					{
						item = new TaxOrder
						{
							ClientId = clientId,
							RefrencedId = data.value.TaxRefrenceNumber,
							UId = data.value.Uid,
							OrderGuid = order.Guid,
							Status = EnTaxResult.Sent,
							InternaleSerial = data.value.InternaleSerial
						};
						await Server.Core.Assistance.Initializer.Dal.TaxSrvice.UpdateAsync(orderTupple.Connection.DepId, orderTupple.Connection.UserId, item, orderTupple.Connection.cancellationToken);
					}
					await ConnectionManager.Instance.GetConnection(orderTupple.ConnectionUniqueId).SendPacketAsync(WebOrder.GenerateSendToTaxServiceResponse(data.value), Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "OrderInstance_OnSendToTaxServiceRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 466);
			}
		}
	}

	internal async Task OrderInstance_OnTaxServiceTracykingRequestAsync(object sender, ConnectionEventArgs<Tuple<string, List<InvoiceTrackingEventArgs>>> orderTupple)
	{
		using (new JaegerMonitor("OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs"))
		{
			_ = 5;
			try
			{
				string clientId = orderTupple?.ConnectionData?.FirstOrDefault()?.Item1 ?? "";
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataByTaxClientIdAsync(orderTupple.Connection.DepId, clientId);
				List<InvoiceTrackingEventArgs> trackingList = orderTupple?.ConnectionData?.FirstOrDefault()?.Item2;
				ReturnedSaveFuncInfoWithValue<ITaxApiConnection> taxConnection = await TaxConnectionManager.GetOrHandleConnectionAsync(user, orderTupple.Connection.Guid, handleLoginResponse: false);
				taxConnection.throwExceptionIfError();
				List<TaxInvoiceTrackingEventArgs> data = new List<TaxInvoiceTrackingEventArgs>();
				foreach (InvoiceTrackingEventArgs item in trackingList)
				{
					try
					{
						ReturnedSaveFuncInfoWithValue<TaxInvoiceTrackingEventArgs> returnedSaveFuncInfoWithValue = await taxConnection.value.TrackingOrderAsync(item, orderTupple.Connection);
						if (!returnedSaveFuncInfoWithValue.HasError)
						{
							data.Add(returnedSaveFuncInfoWithValue.value);
						}
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 490);
					}
				}
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				if (data?.Any() ?? false)
				{
					foreach (TaxInvoiceTrackingEventArgs item2 in data)
					{
						TaxOrder taxOrder = new TaxOrder
						{
							ClientId = clientId,
							RefrencedId = item2.ReferenceNumber,
							UId = item2.Uid,
							OrderGuid = item2.OrderGuid
						};
						if ((item2?.Data?.Error?.Any()).GetValueOrDefault())
						{
							taxOrder.Status = EnTaxResult.Failed;
							taxOrder.Message = string.Join("\r\n", item2.Data.Error.Select((InvoiceTrackingResultModel q) => q.Message).ToList());
						}
						else
						{
							taxOrder.Status = EnTaxResult.Accepted;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						await returnedSaveFuncInfo.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.TaxSrvice.UpdateAsync(orderTupple.Connection.DepId, orderTupple.Connection.UserId, taxOrder, orderTupple.Connection.cancellationToken), ReturnedState.None, "OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 510);
					}
				}
				await ConnectionManager.Instance.GetConnection(orderTupple.ConnectionUniqueId).SendPacketAsync(WebOrder.GenerateTaxTrackingResponse(data.ToList()), Priority.High);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 516);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<TaxInvoiceTrackingEventArgs>>> OrderInstance_OnTaxServiceTracykingRequestAsync(ConnectionEventArgs<Tuple<string, List<InvoiceTrackingEventArgs>>> orderTupple)
	{
		using (new JaegerMonitor("OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<TaxInvoiceTrackingEventArgs>> response = new ReturnedSaveFuncInfoWithValue<List<TaxInvoiceTrackingEventArgs>>();
			try
			{
				string clientId = orderTupple?.ConnectionData?.FirstOrDefault()?.Item1 ?? "";
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataByTaxClientIdAsync(orderTupple.Connection.DepId, clientId);
				List<InvoiceTrackingEventArgs> trackingList = orderTupple?.ConnectionData?.FirstOrDefault()?.Item2;
				ReturnedSaveFuncInfoWithValue<ITaxApiConnection> taxConnection = await TaxConnectionManager.GetOrHandleConnectionAsync(user, orderTupple.Connection.Guid, handleLoginResponse: false);
				taxConnection.throwExceptionIfError();
				List<TaxInvoiceTrackingEventArgs> data = new List<TaxInvoiceTrackingEventArgs>();
				foreach (InvoiceTrackingEventArgs item in trackingList)
				{
					try
					{
						data.Add((await taxConnection.value.TrackingOrderAsync(item, orderTupple.Connection)).value);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 540);
					}
				}
				response.value = data;
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				if (data?.Any() ?? false)
				{
					foreach (TaxInvoiceTrackingEventArgs item2 in data)
					{
						TaxOrder taxOrder = new TaxOrder
						{
							ClientId = clientId,
							RefrencedId = item2.ReferenceNumber,
							UId = item2.Uid,
							OrderGuid = item2.OrderGuid
						};
						if ((item2?.Data?.Error?.Any()).GetValueOrDefault())
						{
							taxOrder.Status = EnTaxResult.Failed;
							taxOrder.Message = string.Join("\r\n", item2.Data.Error.Select((InvoiceTrackingResultModel q) => q.Message).ToList());
						}
						else
						{
							taxOrder.Status = EnTaxResult.Accepted;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						await returnedSaveFuncInfo.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.TaxSrvice.UpdateAsync(orderTupple.Connection.DepId, orderTupple.Connection.UserId, taxOrder, orderTupple.Connection.cancellationToken), ReturnedState.None, "OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 562);
					}
				}
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "OrderInstance_OnTaxServiceTracykingRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 566);
			}
			return response;
		}
	}

	internal async Task OrderInstance_OnTaxEconomicCodeStateRequestAsync(object sender, ConnectionEventArgs<TaxEconomicCodeRecord> data)
	{
		using (new JaegerMonitor("OrderInstance_OnTaxEconomicCodeStateRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs"))
		{
			_ = 3;
			try
			{
				string clientId = data?.ConnectionData?.SingleOrDefault()?.TaxClientId ?? "";
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataByTaxClientIdAsync(data.Connection.DepId, clientId);
				TaxEconomicCodeRecord item = await (await TaxConnectionManager.GetOrHandleConnectionAsync(user, data.ConnectionUniqueId, handleLoginResponse: false)).value.GetEconomicCodeInformationAsync(clientId);
				await ConnectionManager.Instance.GetConnection(data.ConnectionUniqueId).SendPacketAsync(WebOrder.GenerateTaxEconomicCodeStateResponse(item), Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "OrderInstance_OnTaxEconomicCodeStateRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsOrder.cs", 581);
			}
		}
	}
}
