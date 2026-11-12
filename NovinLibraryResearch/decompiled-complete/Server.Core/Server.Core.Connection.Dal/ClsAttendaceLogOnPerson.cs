using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsAttendaceLogOnPerson : IEntity<WebAttendanceLogOnPerson>
{
	private class NestedAttendaceLogOnPerson
	{
		internal static readonly ClsAttendaceLogOnPerson AttendaceLogOnPersonInstance;

		static NestedAttendaceLogOnPerson()
		{
			AttendaceLogOnPersonInstance = new ClsAttendaceLogOnPerson();
		}
	}

	public static ClsAttendaceLogOnPerson ClsAttendaceLogOnPersonInstance => NestedAttendaceLogOnPerson.AttendaceLogOnPersonInstance;

	public async Task<List<WebAttendanceLogOnPerson>> ApplyAccessAsync(IEnumerable<WebAttendanceLogOnPerson> lst, IClsConnectionData connection)
	{
		if (!connection.IsAttendanceFamily)
		{
			return null;
		}
		return lst?.ToList();
	}

	public static string CreateMessageToSupporter(WebAttendanceLogOnPerson attRecord, WebAttendanceDeviceInfo device, ReturnedSaveFuncInfo ret)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		int num = 0;
		try
		{
			num = 1;
			foreach (string information in ret.InformationList)
			{
				stringBuilder.AppendLine(information);
			}
			num = 5;
			foreach (string error in ret.ErrorList)
			{
				stringBuilder.AppendLine(error);
			}
			num = 10;
			stringBuilder.AppendLine("DepId_" + (device?.DepId.ToString() ?? "#دپارتمان_تعریف_نشده_است"));
			num = 110;
			if (ret.HasError || (device?.Guid ?? Guid.Empty) == Guid.Empty)
			{
				num = 111;
				stringBuilder.AppendLine("❌ " + ret.GetErrorMessage() + " ❌");
				num = 112;
			}
			else
			{
				num = 113;
				stringBuilder.AppendLine("✅#ثبت_موفق_تردد");
				num = 114;
			}
			num = 20;
			stringBuilder.AppendLine("Model :" + (device?.TerminalTypeString ?? "UnKnow"));
			stringBuilder.AppendLine("------------------------------------------");
			stringBuilder.AppendLine("Device  #Serial is=> " + device?.SerialIdString);
			stringBuilder.AppendLine("DepartmantNmae :" + device?.DepName);
			num = 25;
			stringBuilder.AppendLine($"The #UserId => {attRecord.UserId}");
			stringBuilder.AppendLine("Action : #Action=>  " + attRecord.AttendanceStatus);
			stringBuilder.AppendLine($"Time In/Out is=> {attRecord.LogTime}");
			stringBuilder.AppendLine($"Action Code is=> {attRecord.ActionInt}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "CreateMessageToSupporter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 73);
			stringBuilder.AppendLine("Exception on CreateMessage for supporter:" + ex.Message);
		}
		return stringBuilder.ToString();
	}

	public static async Task<ReturnedSaveFuncInfo> Send2ClientOnlineAsync(WebAttendanceLogOnPerson attRecord, WebAttendanceDeviceInfo device)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetAllConnectionsData()?.Where((ClsConnectionData p) => p.DepId == device.DepId && p.IsAttendanceFamily);
			returnedSaveFuncInfo.AddInformation("لیست کانکشنهایی که داده برایشان ارسال شده است=>", "", "Send2ClientOnlineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 90);
			if (enumerable != null && enumerable.Any())
			{
				List<Packet> packs = WebAttendanceLogOnPerson.GenerateReadResponse(new List<WebAttendanceLogOnPerson> { attRecord }, device);
				foreach (ClsConnectionData item in enumerable)
				{
					if (!(item?.IsDisposed ?? true))
					{
						returnedSaveFuncInfo.AddInformation($"Department Id : {device.DepId}=>sending Data Attendance To connection By UserName :({item.Username}" + $"//IsOwnedConnection:{item.IsOwnedConnection}) ...", "", "Send2ClientOnlineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 98);
						item.SendPacketAsync(packs, Priority.High);
					}
				}
			}
			else
			{
				returnedSaveFuncInfo.AddInformation("there is no active connection for this department!", "", "Send2ClientOnlineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 104);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send2ClientOnlineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 108);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "Send2ClientOnlineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 109);
		}
		return returnedSaveFuncInfo;
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 300000))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 120);
					return ret;
				}
				await connection.GetSyncInfoAsync();
				List<int> totalPartitions = new List<int>();
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 123);
				}
				List<int> list = await Server.Core.Assistance.Initializer.Dal.AttendaceLogOnPersonRepository.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 125);
				}
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 134);
							return ret;
						}
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 137);
						}
						await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 139);
						}
						list = await Server.Core.Assistance.Initializer.Dal.AttendaceLogOnPersonRepository.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 141);
						}
						list = list?.Where((int x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 143);
						}
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 147);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 150);
				WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 151);
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

	private async Task<bool> SyncPartitionsAsync(List<int> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		_ = 1;
		try
		{
			new CancellationTokenSource();
			if (partitions != null)
			{
				if (partitions == null || partitions.Count > 0)
				{
					List<Task> tasks = new List<Task>();
					foreach (int partition2 in partitions)
					{
						short partition = (short)partition2;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						tasks.Add(SyncPartitionAsync(partition, connection, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs"));
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						if (tasks.Count >= maxParalel)
						{
							List<Task> list = tasks;
							list.Remove(await Task.WhenAny(tasks));
						}
					}
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return false;
					}
					await Task.WhenAll(tasks);
					return true;
				}
			}
			return true;
		}
		catch (OperationCanceledException)
		{
			return false;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 182);
			return false;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				DateTime syncing = DateTime.Now;
				line = 1;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 196);
				}
				line = 2;
				if (Server.Core.Assistance.Initializer.Dal == null)
				{
					await ret.AddReturnedValueAsync(ReturnedState.Error, "Server.Core.Assistance.Initializer.Dal is null", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 200);
					return ret;
				}
				IEnumerable<WebAttendanceLogOnPerson> lst = await (Server.Core.Assistance.Initializer.Dal.AttendaceLogOnPersonRepository?.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken));
				line = 3;
				IEnumerable<WebAttendanceLogOnPerson> enumerable = lst;
				if (enumerable == null || !enumerable.Any())
				{
					await ret.AddReturnedValueAsync(ReturnedState.Error, "lst is null or empty", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 207);
					return ret;
				}
				line = 4;
				lst = await ApplyAccessAsync(lst, connection);
				line = 5;
				IEnumerable<WebAttendanceLogOnPerson> enumerable2 = lst;
				if (enumerable2 == null || !enumerable2.Any())
				{
					await ret.AddReturnedValueAsync(ReturnedState.Error, "lst is null or empty", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 215);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 218);
				}
				line = 6;
				WebAttendanceDeviceInfo device = (await Server.Core.Assistance.Initializer.Dal.AttendanceDeviceInfoRepository.GetByGuidAsync(lst.FirstOrDefault().DeviceGuid)).value;
				line = 7;
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.AttendanceLogOnPersonel, 300000, connection.cancellationToken);
				line = 8;
				ReturnedSaveFuncInfoWithValue<List<WebAttendanceDeviceInfo>> returnedSaveFuncInfoWithValue = await Server.Core.Assistance.Initializer.Dal.AttendanceDeviceInfoRepository.GetAllAsync(connection.DepId);
				if ((returnedSaveFuncInfoWithValue?.value?.Any()).GetValueOrDefault())
				{
					foreach (WebAttendanceLogOnPerson item in lst)
					{
						WebAttendanceDeviceInfo webAttendanceDeviceInfo = returnedSaveFuncInfoWithValue.value.SingleOrDefault((WebAttendanceDeviceInfo q) => q.Guid == item.DeviceGuid);
						if (webAttendanceDeviceInfo != null)
						{
							item.DeviceInfo = webAttendanceDeviceInfo;
						}
					}
				}
				Task.Run(() => connection.SendPacketAsync(WebAttendanceLogOnPerson.GenerateReadResponse(lst, device, partition, isSync: true), Priority.Low));
				line = 9;
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					line = 10;
					await ret.AddReturnedValueAsync(ReturnedState.Error, "attendance logOnPerson WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 241);
					line = 11;
					return ret;
				}
				line = 12;
				if (lst?.Any() ?? false)
				{
					line = 13;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					await returnedSaveFuncInfo.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.AttendanceLogOnPersonel, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 249);
				}
			}
			catch (OperationCanceledException)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 252);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}" + "\r\nmemberName is " + memberName + " and sourceFilePath =" + sourceFilePath, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 255);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\ClsAttendaceLogOnPerson.cs", 256);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
