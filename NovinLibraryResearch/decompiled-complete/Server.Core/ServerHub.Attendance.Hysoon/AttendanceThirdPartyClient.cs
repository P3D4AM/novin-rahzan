using System;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace ServerHub.Attendance.Hysoon;

public static class AttendanceThirdPartyClient
{
	internal static async Task SendAttendanceLogAsync(WebAttendanceLogOnPerson attRecord, WebAttendanceDeviceInfo device, WebTelegramMessage bot, string endPoinUrl, short tryCount = 100)
	{
		StringBuilder sb = new StringBuilder(100000);
		try
		{
			attRecord.DeviceInfo = device;
			string value = Json.ToStringJson(attRecord, indented: true);
			sb.AppendLine(value);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = await HttpHelpers.PostToApi(attRecord, endPoinUrl, null, strongDeserialize: false, 15000, 3);
			if (returnedSaveFuncInfo.HasError)
			{
				sb.AppendLine("➡\ufe0f#SendResult_HasError:" + returnedSaveFuncInfo.GetErrorMessage());
			}
			else
			{
				sb.AppendLine("✅#SendSuccessFully");
			}
		}
		catch (Exception ex)
		{
			sb.AppendLine("❌#SendError");
			sb.AppendLine($"RemTryCount:{tryCount}");
			sb.AppendLine(ex.Message);
			WebErrorLog.Instance.StartLog(ex, "", "SendAttendanceLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Hysoon\\AttendanceThirdPartyClient.cs", 33);
			if (tryCount > 0)
			{
				await Task.Delay(30000);
				Task.Run(() => SendAttendanceLogAsync(attRecord, device, bot, endPoinUrl, --tryCount));
			}
		}
		finally
		{
			bot.Message = sb.ToString();
			bot.Sent2web("", null, "SendAttendanceLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Attendance\\Hysoon\\AttendanceThirdPartyClient.cs", 43);
		}
	}
}
