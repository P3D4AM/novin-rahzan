using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Message;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;
using WebSmsStandard;

namespace Server.Core.Connection.Dal;

public class ClsFileInfo : IEntity<WebFileInfo>
{
	private class NestedFileInFo
	{
		internal static readonly ClsFileInfo FileInfoInstance;

		static NestedFileInFo()
		{
			FileInfoInstance = new ClsFileInfo();
		}
	}

	private static int counter = 0;

	private static List<string> _commitedFiles = new List<string>();

	public static ClsFileInfo FileInfoInstance => NestedFileInFo.FileInfoInstance;

	private ClsFileInfo()
	{
	}

	public Task<List<WebFileInfo>> ApplyAccessAsync(IEnumerable<WebFileInfo> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebFileInfo p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return Task.FromResult(lst?.ToList());
	}

	public async Task WebFileInfo_OnSaveRequest(object sender, ConnectionEventArgs<WebFileInfo> e)
	{
		using (new JaegerMonitor("WebFileInfo_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 1, isReceivedPacket: true);
				new List<string>(e.ConnectionData.Count);
				new List<bool>(e.ConnectionData.Count);
				new List<string>(e.ConnectionData.Count);
				List<SaveResponse> responses = await Server.Core.Assistance.Initializer.Dal.FileInfo.SaveAsync(e.Connection.DepId, e.ConnectionData, e.Connection.Source, e.Connection.cancellationToken, 10);
				Task.Run(() => UpdateEntitiesAsync(e.ConnectionData, e.Connection));
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				if (!e.Connection.cancellationToken.IsCancellationRequested && connection != null)
				{
					await connection.SendPacketAsync(WebFileInfo.GenerateSaveResponse(responses, ""), Priority.Low);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "WebFileInfo_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 70);
			}
		}
	}

	private async Task UpdateEntitiesAsync(List<WebFileInfo> files, ConnectionData connection)
	{
		_ = 5;
		try
		{
			if (!(files?.Any() ?? false))
			{
				return;
			}
			foreach (WebFileInfo file in files)
			{
				if (file == null)
				{
					continue;
				}
				switch (file.Part)
				{
				case FileManagement_Parts.PicPrd:
				case FileManagement_Parts.PicPrdOthers:
				{
					WebProduct webProduct = (WebProduct)(await Server.Core.Assistance.Initializer.Dal.Product.GetAsync(connection.DepId, file.EntityGuid));
					if (webProduct != null)
					{
						webProduct.Modified = DateTime.Now;
						await Server.Core.Assistance.Initializer.Dal.Product.SaveAsync(connection.DepId, connection.HashId, new List<WebProduct> { webProduct }, connection.Source, default(CancellationToken));
					}
					break;
				}
				case FileManagement_Parts.PicCustomer:
				case FileManagement_Parts.PicSign:
				{
					WebCus webCus = await Server.Core.Assistance.Initializer.Dal.Cus.GetAsync(connection.DepId, file.EntityGuid);
					if (webCus != null)
					{
						webCus.Modified = DateTime.Now;
						await Server.Core.Assistance.Initializer.Dal.Cus.SaveAsync("clsFileInfo", connection.DepId, connection.HashId, new List<WebCus> { webCus }, connection.Source, default(CancellationToken));
					}
					break;
				}
				case FileManagement_Parts.PicPrdGroup:
				{
					WebPrdGroup webPrdGroup = await Server.Core.Assistance.Initializer.Dal.ProductGroup.LoadDataAsync(connection.DepId, file.EntityGuid, default(CancellationToken), 10);
					if (webPrdGroup != null)
					{
						webPrdGroup.Modified = DateTime.Now;
						await Server.Core.Assistance.Initializer.Dal.ProductGroup.SaveAsync(connection.DepId, connection.HashId, connection.Source, new List<WebPrdGroup> { webPrdGroup }, default(CancellationToken), 10);
					}
					break;
				}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateEntitiesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 115);
		}
	}

	public async Task FileInstance_OnWriteRequest(object sender, ConnectionEventArgs<WebFileInfo> e)
	{
		using (new JaegerMonitor("FileInstance_OnWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 1, isReceivedPacket: true);
				List<string> filenames = new List<string>(e.ConnectionData.Count);
				List<bool> successes = new List<bool>(e.ConnectionData.Count);
				List<string> messages = new List<string>(e.ConnectionData.Count);
				int line = 1;
				foreach (WebFileInfo connectionDatum in e.ConnectionData)
				{
					try
					{
						line = 5;
						WebErrorLog.Instance.Debug($"HubConnection.FileInstance_OnWriteRequest counter is:{++counter}", "", "FileInstance_OnWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 134);
						line = 6;
						filenames.Add(connectionDatum.FileName);
						line = 7;
						if (!(connectionDatum.FileContent?.Any() ?? false))
						{
							connectionDatum.FileContent = connectionDatum.Content;
						}
						ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await Server.Core.Assistance.Initializer.Dal.FileInfo.FileCreateAsync(FileManagement_Parts.Temp, e.Connection.Source, connectionDatum.FileName, connectionDatum.FileContent, e.Connection.DepId, 10);
						UpdateEntitiesAsync(e.ConnectionData, e.Connection);
						line = 10;
						if (returnedSaveFuncInfoWithValue.HasError)
						{
							line = 11;
							successes.Add(item: false);
							messages.Add("");
						}
						else
						{
							line = 12;
							successes.Add(item: true);
							messages.Add(returnedSaveFuncInfoWithValue.value);
						}
					}
					catch (OperationCanceledException)
					{
						messages.Add("توکن کنسل شد");
					}
					catch (Exception ex2)
					{
						successes.Add(item: false);
						messages.Add(ex2.Message);
						WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//e.connetionData:{Json.ToStringJson(e?.ConnectionData)}", "FileInstance_OnWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 162);
					}
				}
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebFileInfo.GenerateWriteResponse(filenames, successes, messages, ""), Priority.Low);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "FileInstance_OnWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 168);
			}
		}
	}

	public async Task FileInstance_OnDeleteRequest(object sender, ConnectionEventArgs<WebFileInfo> e)
	{
		using (new JaegerMonitor("FileInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 9, isReceivedPacket: true);
				if (e?.ConnectionData == null)
				{
					return;
				}
				List<WebFileInfo> connectionData = e.ConnectionData;
				if ((connectionData != null && connectionData.Count <= 0) || e.Connection.DepId <= 0)
				{
					return;
				}
				Task.Run(() => UpdateEntitiesAsync(e.ConnectionData, e.Connection));
				List<ClsConnectionData> connectionsData = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId);
				if (connectionsData == null || connectionsData.Count <= 0)
				{
					return;
				}
				foreach (ClsConnectionData item in connectionsData)
				{
					if (item.HashId != e.Connection.HashId || item.Source != ENSource.WebsiteWordPress)
					{
						await item.SendPacketAsync(WebFileInfo.GenerateReadResponse(e.ConnectionData), Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "FileInstance_OnDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 196);
			}
		}
	}

	internal async Task FileInstance_OnCommitRequest(object sender, FileCommitRequestEventArgs e)
	{
		using (new JaegerMonitor("FileInstance_OnCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs"))
		{
			Packet packet = null;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.File, 3, isReceivedPacket: true);
				e.FileName = e.FileName.FixString();
				Stopwatch watch = new Stopwatch();
				watch.Start();
				WebFileInfo fileInfo = await Server.Core.Assistance.Initializer.Dal.FileInfo.CommitAsync(e, 10);
				watch.Stop();
				if (fileInfo == null)
				{
					return;
				}
				packet = WebFileInfo.GenerateCommitResponse(e.Crc, success: true, "File Created successfuly", e.FileName, fileInfo.Url, e.PacketReferenceId, e.FileGuid);
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p.IsAccountingFamily && p.Source != ENSource.WebsiteWordPress && p.HashId != e.Connection.HashId && p.Source != ENSource.Android_HesabYar && p.Source != ENSource.Android_SefareshGir);
				if (enumerable != null && enumerable.Any() && e.Connection.Source != ENSource.NPBackupService)
				{
					foreach (ClsConnectionData item in enumerable)
					{
						await item.SendPacketAsync(WebFileInfo.GenerateReadResponse(new List<WebFileInfo> { fileInfo }), Priority.High);
					}
				}
				if (e.Connection.Source == ENSource.NPBackupService && !_commitedFiles.Contains(e.FileName))
				{
					_commitedFiles.Add(e.FileName);
					SendSmsAsync(e, fileInfo.Url);
					SendToTelegramAsync(e, fileInfo.Url, e.Connection.UserId);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "FileInstance_OnCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 240);
				packet = WebFileInfo.GenerateCommitResponse(0L, success: false, "File Created fail", e.FileName, "", e.PacketReferenceId, e.FileGuid);
			}
			finally
			{
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(packet, Priority.High);
			}
		}
	}

	private async Task SendToTelegramAsync(FileCommitRequestEventArgs e, string url, int userId)
	{
		_ = 2;
		try
		{
			WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(userId);
			WebDepartment webDepartment = await Server.Core.Assistance.Initializer.Dal.Department.LoadDataAsync(user.DepId);
			List<WebTafsilNumbers> list = new List<WebTafsilNumbers>();
			if (!string.IsNullOrEmpty(webDepartment.Mob))
			{
				list.AddRange(from p in webDepartment.Mob.SplitString()
					select new WebTafsilNumbers(p));
			}
			if (!string.IsNullOrEmpty(user.Mob))
			{
				list.AddRange(from p in user.Mob.SplitString()
					select new WebTafsilNumbers(p));
			}
			List<long> list2 = (from p in list
				where p.IsValid && p.Mobile
				select p.CalcedNumber).Distinct().ToList();
			if (!list2.Any())
			{
				list2.Add(9158980915L);
			}
			string caption = $"{DateTime.Now.MiladiToCalture()}   {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}\r\n" + "✅پشتیبان شما با موفقیت به سرور های نوین پرداز ارسال شد.\r\n\r\nنام فایل:\r\n" + e.FileName + "\r\n\r\nلینک دانلود:\r\n\ud83c\udf10" + url + "\r\n\r\n\ud83d\udca5\ud83d\udca5شرکت مبتکران نوین پرداز سپنتا\ud83d\udca5\ud83d\udca5\r\nپیشرو در ارائه خدمات نوین \r\n\ud83e\udd17با ما همراه باشید\ud83e\udd17\r\n\r\n\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\ud83d\udc47\r\nبا عضویت در کانال تلگرام شرکت از آخرین اخبار و اطلاعیه های ما مطلع شوید\r\n@Novin_p\r\n\r\nشعار ما\r\n\ud83c\uddee\ud83c\uddf7تولید ملی\ud83c\uddee\ud83c\uddf7\r\n\ud83c\uddee\ud83c\uddf7افتخار ملی\ud83c\uddee\ud83c\uddf7\r\n\ud83d\udcaa\ud83d\udcaaحمایت از جوان ایرانی\ud83d\udcaa\ud83d\udcaa";
			WebMessage message = WebMessage.Factory(DateTime.Now, caption, 10, ENPlatFormType.Telegram);
			message.Sender = 9381908737L;
			List<WebMessageDetail> messageDetails = new List<WebMessageDetail> { WebMessageDetail.Factory(url, enWebMesssageDetailType.Url) };
			message.MessageRecivers = new List<WebMessageReciver>();
			message.MessageDetails = messageDetails;
			list2.ForEach(delegate(long m)
			{
				message.MessageRecivers.Add(WebMessageReciver.Factory(m.ToString(), EnMessageSendType.TelegramDirect));
			});
			await Server.Core.Assistance.Initializer.Dal.Message.InsertAsync(message, 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToTelegramAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 291);
		}
	}

	private async Task SendSmsAsync(FileCommitRequestEventArgs e, string url, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs"))
		{
			int line = 0;
			short depId = (e?.Connection?.DepId).GetValueOrDefault(-1);
			try
			{
				if ((e?.Connection?.DepId).GetValueOrDefault(-1) <= 0)
				{
					throw new Exception("DepId is zero!");
				}
				line = 1;
				WebUser user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(e.Connection.UserId);
				string UserFullName = user?.UserFullName ?? "";
				line = 2;
				if (!string.IsNullOrEmpty(user.PanelUName) && !string.IsNullOrEmpty(user.PanelPass) && !string.IsNullOrEmpty(user.PanelDomain) && !string.IsNullOrEmpty(user.SMSLine) && !string.IsNullOrEmpty(user.Mob))
				{
					line = 3;
					line = 4;
					string message = "مشترک گرامی\r\n" + user.UserFullName + "\r\nفایل بکاپ شما\r\n" + e.FileName + "\r\nدر سرور نوین پرداز ذخیره شد\r\n" + url;
					line = 5;
					long[] array = await SmsSender.SendSmsAsync(user.PanelUName, user.PanelPass, user.PanelDomain, message, user.Mob, user.SMSLine, token);
					line = 60;
					if ((array == null && (array == null || array.Length == 0)) || (array?.FirstOrDefault() ?? (-1)) <= 0)
					{
						line = 61;
						WebErrorLog.Instance.Warning("userFullName:" + UserFullName + "\r\n" + $"DepId:{depId}\r\n" + "smsDendError:" + (array?.FirstOrDefault().ToString() ?? "Null") + "(" + (array?.FirstOrDefault() ?? 0).GetUserFriendlyTextForSendSmsErrorCode() + ")\r\nPanelDomain:" + user.PanelDomain + "\r\nMob:" + user.Mob + "\r\nSMSLine:" + user.SMSLine + "\r\nPanelUName:" + user.PanelUName + "\r\nPanelPass:" + user.PanelPass + "FileName:" + e.FileName, "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 336);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 349);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			if (connection.ClientVersion >= 10)
			{
				return await Sync2Client_AccV6OrGrather_Async(connection, ConnectionUniqueId, 10);
			}
			SyncInformation syncInfo = await connection.GetSyncInfoAsync();
			line = 1;
			if (connection.Source == ENSource.Accounting && syncInfo != null)
			{
				syncInfo.FileInfo = DateTime.Now.AddYears(-10).AddDays(-10.0);
			}
			line = 5;
			if (syncInfo == null)
			{
				ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 370);
				return ret;
			}
			line = 6;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 374);
			}
			line = 7;
			await Task.Delay(10, connection.cancellationToken);
			line = 8;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 378);
			}
			line = 9;
			DateTime syncing = DateTime.Now;
			line = 10;
			List<WebFileInfo> lst = await Server.Core.Assistance.Initializer.Dal.FileInfo.GetAllAsync(connection.DepId, connection.Source, syncInfo?.FileInfo ?? new DateTime(1900, 1, 1), connection.cancellationToken, loadFileContents: false);
			if (connection.Source == ENSource.Android_SefareshGir)
			{
				List<WebFileInfo> list = await Server.Core.Assistance.Initializer.Dal.FileInfo.GetCustomerPicsAsync(connection.DepId, connection.CustomerTafsilGuid, syncInfo?.FileInfo ?? new DateTime(1900, 1, 1), connection.cancellationToken, loadFileContents: false);
				if (list != null && list != null && list.Count > 0)
				{
					lst?.AddRange(list);
				}
			}
			line = 11;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 396);
			}
			line = 12;
			line = 16;
			lst = (await ApplyAccessAsync(lst, connection))?.ToList();
			line = 17;
			if (lst?.Any() ?? false)
			{
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.FileInformations, 300000, connection.cancellationToken);
				line = 18;
				await connection.SendPacketAsync(WebFileInfo.GenerateReadResponse(lst, 0, isSync: true), Priority.Low);
				line = 19;
				bool flag = lst?.Any() ?? false;
				if (flag)
				{
					flag = !(await waiter.WaitForSyncDonePacket());
				}
				if (flag)
				{
					ret.AddReturnedValue(ReturnedState.Error, "!await waiter.WaitForSyncDonePacket()", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 418);
					return ret;
				}
				line = 21;
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.FileInformations, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 423);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 426);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}\r\n" + $"Source:{connection?.Source ?? ENSource.None}\r\n" + $"depID:{connection?.DepId ?? (-1)}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 429);
			ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 432);
		}
		finally
		{
			ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client_AccV6OrGrather_Async(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2Client_AccV6OrGrather_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				connection?.cancellationToken.ThrowIfCancellationRequested();
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client_AccV6OrGrather_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 447);
				}
				List<int> totalPartitions = new List<int>();
				List<int> list = await Server.Core.Assistance.Initializer.Dal.FileInfo.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					List<Task> tasks = new List<Task>();
					foreach (int item in list)
					{
						tasks.Add(SyncPartitionAsync(item, connection));
						if (tasks.Count >= maxParalel)
						{
							List<Task> list2 = tasks;
							list2.Remove(await Task.WhenAny(tasks));
						}
					}
					await Task.WhenAll(tasks);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client_AccV6OrGrather_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 465);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Sync2Client_AccV6OrGrather_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 468);
				ret.AddReturnedValue(ex2, "", "Sync2Client_AccV6OrGrather_Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 469);
			}
			finally
			{
				if (!ret.HasError)
				{
					Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				}
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(int partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (connection.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			try
			{
				bool loadFileContents = connection.ClientVersion >= 10 && connection.Source != ENSource.Android_HesabYar;
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 490);
				}
				List<WebFileInfo> list = await Server.Core.Assistance.Initializer.Dal.FileInfo.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken, loadFileContents);
				if (list != null && list.Any())
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 505);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.FileInformations, 300000, connection.cancellationToken);
					await connection.SendPacketAsync(WebFileInfo.GenerateReadResponse(list, partition, isSync: true), Priority.Low);
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 510);
						return ret;
					}
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.FileInformations, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 514);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 517);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 520);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsFileInfo.cs", 521);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
