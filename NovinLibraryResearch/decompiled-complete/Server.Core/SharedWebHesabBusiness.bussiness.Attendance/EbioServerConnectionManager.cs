using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance.Net;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Server.Core.Assistance;
using Server.Core.Connection;
using Server.Core.Connection.Dal;

namespace SharedWebHesabBusiness.bussiness.Attendance;

public class EbioServerConnectionManager
{
	public ConcurrentDictionary<string, DateTime> _lastPingDictionary = new ConcurrentDictionary<string, DateTime>();

	public static EbioServerConnectionManager instance { get; } = new EbioServerConnectionManager();


	public async Task EbioServer_PingLogAsync(object sender, PingEventLog e)
	{
		_ = 1;
		try
		{
			if (string.IsNullOrEmpty(e.serialNumber))
			{
				throw new ArgumentException("SerialNumber is not valid!");
			}
			if (_lastPingDictionary.TryGetValue(e.serialNumber, out var value))
			{
				if ((DateTime.Now - value).TotalSeconds < 30.0)
				{
					return;
				}
			}
			else
			{
				_lastPingDictionary.TryAdd(e.serialNumber, DateTime.Now);
			}
			await Server.Core.Assistance.Initializer.Dal.AttendanceDeviceInfoRepository.UpdateLastPing(e.serialNumber);
			await ((EbioTcpClientConnection)sender).SendReplyAsync("KeptAlive", e.transId);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EbioServer_PingLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 40);
		}
	}

	public async Task EbioServer_TimeLogAsync(object sender, TimeLogEventLog e)
	{
		WebAttendanceLogOnPerson attRecord = new WebAttendanceLogOnPerson();
		ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo> device = new ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo>();
		ReturnedSaveFuncInfoWithValue<Guid> ret = new ReturnedSaveFuncInfoWithValue<Guid>();
		Guid telegramMessageGuid = Guid.NewGuid();
		EbioTcpClientConnection tcpClient = (EbioTcpClientConnection)sender;
		using (new JaegerMonitor("EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs"))
		{
			_ = 6;
			try
			{
				WebTelegramMessage.GetAttendanceRealTimeMonitoring_bot().Sent2web(string.Format("یک تردد جدید برای سرور از نوع Ebio ارسال شد/Serial iS:{0} / ReciveTime :{1}", e.serialNumber ?? "Serial Not Valid", e.TimeLog), telegramMessageGuid, "EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 54);
				device = await Server.Core.Assistance.Initializer.Dal.AttendanceDeviceInfoRepository.GetOrSave(e.termType, e.serialNumber, e.termId, tcpClient?.Port.ToString());
				if ((await ret.AddReturnedValueAsync(device, ReturnedState.None, "EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 56)).HasError)
				{
					return;
				}
				(device?.value?.DepId).GetValueOrDefault(-1);
				attRecord = new WebAttendanceLogOnPerson
				{
					AntiPassv = e.antipassStatus,
					DoorId = e.doorID,
					AttendanceStatus = e.attendStatus,
					JobCode = e.jobCode,
					LogTime = e.TimeLog,
					Photo = e.photo,
					UserId = e.userID,
					VerifyMode = e.verifyMode,
					TransactionId = (int)e.transId,
					Modified = DateTime.Now,
					IpAddress = tcpClient.Ip.ToString(),
					PartitionId = 1,
					Guid = Guid.NewGuid(),
					DeviceGuid = device.value.Guid,
					TerminalID = device.value.TerminalId,
					ActionInt = WebAttendanceLogOnPerson.SetActionInt(e.attendStatus),
					DeviceInfo = (device.value ?? new WebAttendanceDeviceInfo())
				};
				if (!(await ret.AddReturnedValueAsync(attRecord.CheckValidtion(e.transId), ReturnedState.None, "EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 80)).HasError)
				{
					ReturnedSaveFuncInfoWithValue<Guid> returnedSaveFuncInfoWithValue = ret;
					await returnedSaveFuncInfoWithValue.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.AttendaceLogOnPersonRepository.SaveAsync(attRecord), ReturnedState.None, "EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 82);
					if (!ret.HasError)
					{
						Task<ReturnedSaveFuncInfo> send2ClientTask = ClsAttendaceLogOnPerson.Send2ClientOnlineAsync(attRecord, device.value);
						await Task.WhenAll(tcpClient.SendReplyAsync("UploadedLog", e.transId), send2ClientTask);
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await send2ClientTask, ReturnedState.None, "EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 90);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 95);
			}
			finally
			{
				WebTelegramMessage.GetAttendanceRealTimeMonitoring_bot().Sent2web(ClsAttendaceLogOnPerson.CreateMessageToSupporter(attRecord, device?.value, ret), telegramMessageGuid, "EbioServer_TimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 98);
			}
		}
	}

	public async Task EbioServer_AlarmLogAsync(object sender, AlarmEventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		EbioTcpClientConnection tcpClient = (EbioTcpClientConnection)sender;
		using (new JaegerMonitor("EbioServer_AlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo> device = new ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo>();
			new WebAttendanceLogOnAlarm();
			short depId = 0;
			try
			{
				_ = 5;
				try
				{
					device = await Server.Core.Assistance.Initializer.Dal.AttendanceDeviceInfoRepository.GetOrSave(e.termType, e.serialNumber, e.termId, tcpClient.Port.ToString());
					depId = (device?.value?.DepId).GetValueOrDefault(-1);
					await ret.AddReturnedValueAsync(device, ReturnedState.None, "EbioServer_AlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 116);
					if (ret.HasError)
					{
						return;
					}
					WebAttendanceLogOnAlarm attRecord = new WebAttendanceLogOnAlarm
					{
						DoorId = e.doorID,
						LogTime = e.TimeLog,
						AlarmType = e.alarmType,
						DeviceGuid = device.value.Guid,
						UserId = e.userID,
						Guid = Guid.NewGuid(),
						Modified = DateTime.Now,
						IpAddress = tcpClient.Ip.ToString(),
						PartitionId = 1,
						TransactionId = e.termId
					};
					if ((await ret.AddReturnedValueAsync(attRecord.CheckValidtion(e.termId), ReturnedState.None, "EbioServer_AlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 132)).HasError)
					{
						return;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					await returnedSaveFuncInfo.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.AttendaceLogOnAlarmRepository.SaveAsync(attRecord), ReturnedState.None, "EbioServer_AlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 134);
					if (ret.HasError)
					{
						return;
					}
					await tcpClient.SendReplyAsync("UploadedLog", e.termId);
					IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetAllConnectionsData()?.Where((ClsConnectionData p) => p.DepId == device.value.DepId);
					if (enumerable == null || !enumerable.Any())
					{
						return;
					}
					List<Packet> packs = WebAttendanceLogOnAlarm.GenerateReadResponse(new List<WebAttendanceLogOnAlarm> { attRecord });
					foreach (ClsConnectionData item in enumerable)
					{
						if (item != null && !item.IsDisposed && item.Source == ENSource.TimeAttendance)
						{
							item.SendPacketAsync(packs, Priority.High);
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "EbioServer_AlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 153);
				}
			}
			finally
			{
				StringBuilder sb = new StringBuilder(100000);
				sb.AppendLine($"#DepId_{depId}");
				if (ret.HasError)
				{
					StringBuilder stringBuilder = sb;
					stringBuilder.AppendLine("❌" + await ret.GetErrorMessageAsync() + "❌");
				}
				else
				{
					sb.AppendLine("✅ " + "ثبت موفق عملیات آلارم".FixForTelegram());
				}
				sb.AppendLine($"SerialNumber: {device?.value?.SerialId}");
				sb.AppendLine($"DepartmantId: {device?.value?.DepId}//{device?.value?.DepName?.FixForTelegram()}");
				sb.AppendLine("Action: " + "alarm".FixForTelegram());
				WebTelegramMessage.GetAttendanceRealTimeMonitoring_bot().Sent2web(sb.ToString(), null, "EbioServer_AlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 166);
			}
		}
	}

	public async Task EbioServer_AdminLogAsync(object sender, AdminLogEventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		EbioTcpClientConnection tcpClient = (EbioTcpClientConnection)sender;
		ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo> device = new ReturnedSaveFuncInfoWithValue<WebAttendanceDeviceInfo>();
		WebAttendanceLogOnAdmin attRecord = new WebAttendanceLogOnAdmin();
		short depId = 0;
		try
		{
			_ = 4;
			try
			{
				device = await Server.Core.Assistance.Initializer.Dal.AttendanceDeviceInfoRepository.GetOrSave(e.termType, e.serialNumber, e.termId, tcpClient?.Port.ToString());
				depId = (device?.value?.DepId).GetValueOrDefault(-1);
				if (!(await ret.AddReturnedValueAsync(device, ReturnedState.None, "EbioServer_AdminLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 180)).HasError)
				{
					attRecord = new WebAttendanceLogOnAdmin
					{
						LogTime = e.Logtime,
						Result = e.result,
						Action = e.action,
						AdminId = e.adminID,
						UserId = e.userID,
						Guid = Guid.NewGuid(),
						Modified = DateTime.Now,
						IpAddress = tcpClient.Ip.ToString(),
						PartitionId = 1,
						DeviceGuid = device.value.Guid,
						TransactionId = e.transId
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					await returnedSaveFuncInfo.AddReturnedValueAsync(await Server.Core.Assistance.Initializer.Dal.AttendaceLogOnAdminRepository.SaveAsync(attRecord), ReturnedState.None, "EbioServer_AdminLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 196);
					if (!ret.HasError)
					{
						await tcpClient.SendReplyAsync("UploadedLog", e.transId);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EbioServer_AdminLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 203);
			}
		}
		finally
		{
			StringBuilder sb = new StringBuilder(100000);
			sb.AppendLine($"#DepId_{depId}");
			if (ret.HasError)
			{
				StringBuilder stringBuilder = sb;
				stringBuilder.AppendLine("❌" + await ret.GetErrorMessageAsync() + "❌");
			}
			else
			{
				sb.AppendLine("✅ " + "ثبت موفق عملیات مدیریتی".FixForTelegram());
			}
			sb.AppendLine($"SerialNumber: {device?.value?.SerialId}");
			sb.AppendLine($"DepartmantId: {device?.value?.DepId}//{device?.value?.DepName?.FixForTelegram()}");
			sb.AppendLine("Action: " + attRecord?.Action?.FixForTelegram());
			WebTelegramMessage.GetAttendanceRealTimeMonitoring_bot().Sent2web(sb.ToString(), null, "EbioServer_AdminLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Ebio\\EbioServerConnectionManager.cs", 216);
		}
	}
}
