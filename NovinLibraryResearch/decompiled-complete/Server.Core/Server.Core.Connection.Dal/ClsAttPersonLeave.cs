using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.EventArgs;
using PacketParser.Records.Attendance;
using PacketParser.TransferLayer.Waiters.Attendance.AttpersonLeave;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsAttPersonLeave
{
	private class NestedAttPersonLeave
	{
		internal static readonly ClsAttPersonLeave Instance;

		static NestedAttPersonLeave()
		{
			Instance = new ClsAttPersonLeave();
		}
	}

	public static ClsAttPersonLeave Instance => NestedAttPersonLeave.Instance;

	public Task<List<WebAttPersonLeave>> ApplyAccessAsync(IEnumerable<WebAttPersonLeave> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs"))
		{
			try
			{
				if (!connection.IsAttendanceFamily)
				{
					return null;
				}
				if (connection.Source != ENSource.TimeAttendance)
				{
					lst = lst?.Where((WebAttPersonLeave p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return Task.FromResult(lst?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 38);
				throw;
			}
		}
	}

	internal async Task AttPersonLeaveInstance_OnSaveRequestAsync(object sender, ConnectionEventArgs<WebAttPersonLeave> e)
	{
		using (new JaegerMonitor("AttPersonLeaveInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs"))
		{
			_ = 2;
			try
			{
				ConnectionEventArgs<WebAttPersonLeave> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken, e.Connection.Source);
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
					WebErrorLog.Instance.StartLog(ex2, "", "AttPersonLeaveInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 59);
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection2?.IsDisposed ?? true)
					{
						return;
					}
					await connection2.SendPacketAsync(WebAttPersonLeave.GenerateSaveResponse(ret, e.PacketReferenceId), Priority.High);
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (trueSaved == null)
				{
					return;
				}
				HashSet<Guid> hashSet = trueSaved;
				if (hashSet != null && hashSet.Count() <= 0)
				{
					return;
				}
				List<WebAttPersonLeave> truSavedInRet = e.ConnectionData.Where((WebAttPersonLeave p) => trueSaved.Contains(p.Guid)).ToList();
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebAttPersonLeave> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.TimeAttendance) && e.Connection.ClientVersion >= 15)
				{
					ClsConnectionData attconnection = ConnectionManager.Instance.GetAttendanceConnection(e.Connection.DepId);
					if (attconnection != null && attconnection?.ClientVersion > 8)
					{
						Task.Run(() => attconnection?.SendPacketAsync(WebAttPersonLeave.GenerateSaveRequest(truSavedInRet), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.TimeAttendance)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.Source != ENSource.TimeAttendance && p.IsAccountingFamily && p.ClientVersion >= 14);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (WebAttPersonLeave item2 in truSavedInRet)
				{
					item2.DeliveryDate = DateTime.Now;
					item2.ServerStatus = ServerStatusForClient.Delivered;
				}
				foreach (ClsConnectionData connection in enumerable)
				{
					if (connection?.IsDisposed ?? true)
					{
						continue;
					}
					List<WebAttPersonLeave> dataForSend = await ApplyAccessAsync(truSavedInRet, connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => connection.SendPacketAsync(WebAttPersonLeave.GenerateReadResponse(dataForSend, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "AttPersonLeaveInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 102);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string connectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs"))
		{
			if (connection == null)
			{
				throw new NullReferenceException("connection");
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			CancellationToken token = connection.cancellationToken;
			if (connection.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			int line = 0;
			try
			{
				if (token.IsCancellationRequested || connection.IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 114);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				SyncInformation obj = await connection.GetSyncInfoAsync();
				line = 100;
				if (obj == null)
				{
					line = 101;
					ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 121);
					line = 102;
					return ret;
				}
				line = 10001;
				if (connection.IsDisposed || token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 126);
				}
				List<short> totalPartitions = new List<short>();
				List<short> list = await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						if (connection.IsDisposed || token.IsCancellationRequested)
						{
							ret.AddError("connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 137);
							return ret;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SyncPartitionsAsync(list, connection, maxParalel), ReturnedState.None, "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 140);
						if (connection.IsDisposed || token.IsCancellationRequested)
						{
							ret.AddError("connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 143);
							return ret;
						}
						line = 22;
						if (connection.IsDisposed || token.IsCancellationRequested)
						{
							return ret;
						}
						list = await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
						line = 23;
						list = list?.Where((short x) => !totalPartitions.Contains(x)).ToList();
						line = 24;
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 155);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 158);
				ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 159);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(token, connection);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionsAsync(List<short> partitions, ClsConnectionData connection, short maxParalel)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(partitions?.Any() ?? false))
			{
				return ret;
			}
			List<Task> tasks = new List<Task>();
			foreach (short partition in partitions)
			{
				tasks.Add(SyncPartitionAsync(partition, connection));
				if (tasks.Count >= maxParalel)
				{
					List<Task> list = tasks;
					list.Remove(await Task.WhenAny(tasks));
				}
			}
			await Task.WhenAll(tasks);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 179);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 182);
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 183);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				DateTime syncing = DateTime.Now;
				line = 26;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 196);
				}
				List<WebAttPersonLeave> lst = await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken, loadGardeshHesab: true);
				line = 30;
				if (lst?.Any() ?? false)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					foreach (WebAttPersonLeave item in lst)
					{
						using AttPersonLeaveWaiters waiter = new AttPersonLeaveWaiters(WebContextHolder.GetWebContextHolder(connection.ContextGuid), item.Guid, connection.cancellationToken, 60000, "");
						connection.SendPacketAsync(WebAttPersonLeave.GenerateSaveRequest(lst, partition), Priority.Low);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse(""), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 205);
						if (ret.HasError)
						{
							ret.AddError("waiter time out", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 208);
							return ret;
						}
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 211);
						}
						line = 45;
						ret.throwExceptionIfError();
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.AttPersonLeave, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 215);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 218);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 221);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 222);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}

	public async Task SendRemindingSmsAsync()
	{
		_ = 10;
		try
		{
			CancellationToken token = new CancellationTokenSource().Token;
			while (!token.IsCancellationRequested)
			{
				await Task.Delay(60000);
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				DateTime dateM = DateTime.Now.Date;
				DateTime? dateTime = await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.GetRemindingSmsSettingAsync();
				if (dateTime.HasValue && dateTime.Value >= dateM)
				{
					await Task.Delay(3600000);
					continue;
				}
				List<AttPersonLeaveRemindingSmsRecord> list = await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.GetAllForSendRemindingSmsAsync(dateM);
				if (list == null || !list.Any())
				{
					await res.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.SaveRemindingSmsSettingAsync(dateM), ReturnedState.None, "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 251);
					await Task.Delay(3600000);
					continue;
				}
				List<IGrouping<short, AttPersonLeaveRemindingSmsRecord>> list2 = list?.GroupBy((AttPersonLeaveRemindingSmsRecord q) => q.DepId)?.ToList();
				foreach (IGrouping<short, AttPersonLeaveRemindingSmsRecord> item in list2)
				{
					if (item != null)
					{
						await res.AddReturnedValueAsync(await SendRemindingSmsAsync(item.Key, item.Select((AttPersonLeaveRemindingSmsRecord q) => q).ToList()), ReturnedState.None, "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 262);
					}
				}
				if (!res.HasError)
				{
					await res.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.AttPersonLeave.SaveRemindingSmsSettingAsync(dateM), ReturnedState.None, "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 269);
				}
				if (res.HasError)
				{
					WebErrorLog.Instance.StartLog(res.ErrorMessage, "", "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 272);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 275);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendRemindingSmsAsync(short depId, List<AttPersonLeaveRemindingSmsRecord> list)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			WebDepartment department = await Server.Core.Assistance.Initializer.Dal.Department.LoadDataAsync(depId);
			if (department == null)
			{
				return res;
			}
			WebUser user = await Server.Core.Assistance.Initializer.Dal.User.GetByUserOfDepartmentAsync(depId, ENSource.TimeAttendance);
			if (user == null)
			{
				return res;
			}
			WebLinkGenerator link = await Server.Core.Assistance.Initializer.Dal.LinkGenerator.GetAsync(depId);
			if (string.IsNullOrEmpty(link?.ShortUrl ?? ""))
			{
				link = new WebLinkGenerator(link?.Guid ?? Guid.Empty, depId, ENSource.TimeAttendance);
				link.GenerateUrl();
				await Server.Core.Assistance.Initializer.Dal.LinkGenerator.SaveAsync(link);
			}
			foreach (AttPersonLeaveRemindingSmsRecord item in list)
			{
				if (item == null)
				{
					continue;
				}
				if (item.Status == EnPersonLeaveStatus.Pending)
				{
					if (!string.IsNullOrEmpty(department?.Mob) && !string.IsNullOrEmpty(user.PanelUName) && !string.IsNullOrEmpty(user.PanelPass) && !string.IsNullOrEmpty(user.PanelDomain) && !string.IsNullOrEmpty(user.SMSLine))
					{
						string message2 = string.Format("با سلام \r\n تعداد {0} مرخصی بررسی نشده وجود دارد. \r\n لینک جهت بررسی: \r\n {1} \r\n پورتال حضور و غیاب نوین پرداز", item.Count, link?.ShortUrl ?? "---");
						Task.Run(() => Server.Core.Assistance.Initializer.Dal.Sms.SendSmsAsync(message2, department.Mob, user.PanelUName, user.PanelPass, user.PanelDomain, user.SMSLine, default(CancellationToken)));
					}
				}
				else
				{
					if (item.Status != EnPersonLeaveStatus.PendingForAdsmin)
					{
						continue;
					}
					if ((item.AdminGuid ?? Guid.Empty) == Guid.Empty)
					{
						continue;
					}
					ReturnedSaveFuncInfoWithValue<WebAttPerson> returnedSaveFuncInfoWithValue = await Server.Core.Assistance.Initializer.Dal.PersonReport.GetAsync(item.DepId, item.AdminGuid.Value);
					if (returnedSaveFuncInfoWithValue.HasError)
					{
						continue;
					}
					string mobil = ((!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.Mob1)) ? returnedSaveFuncInfoWithValue.value.Mob1 : returnedSaveFuncInfoWithValue.value.Mob2);
					if (!string.IsNullOrEmpty(mobil) && !string.IsNullOrEmpty(user.PanelUName) && !string.IsNullOrEmpty(user.PanelPass) && !string.IsNullOrEmpty(user.PanelDomain) && !string.IsNullOrEmpty(user.SMSLine))
					{
						string message = string.Format("با سلام \r\n تعداد {0} مرخصی بررسی نشده وجود دارد. \r\n لینک جهت بررسی: \r\n {1} \r\n پورتال حضور و غیاب نوین پرداز", item.Count, link?.OrginalUrl ?? "---");
						Task.Run(() => Server.Core.Assistance.Initializer.Dal.Sms.SendSmsAsync(message, mobil, user.PanelUName, user.PanelPass, user.PanelDomain, user.SMSLine, default(CancellationToken)));
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 319);
			res.AddReturnedValue(ex, "", "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\AttPersonLeave\\ClsAttPersonLeave.cs", 320);
		}
		return res;
	}
}
