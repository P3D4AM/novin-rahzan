using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendance.ValidatorAttribute.Payamak;

public class PayamakValidation
{
	public static async Task<DialogResult> CheckValidationAsync(string contextGuid, TimeAttendanceDAL.Settings.Payamak pObj, bool checkPayamakWeb, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!((AttDepartmentAdminSms)pObj).EnablePayamak)
			{
				return (DialogResult)6;
			}
			ReturnedSaveFuncInfo val = await new PayamakValidator(contextGuid, pObj, checkPayamakWeb).ValidateAsync(token);
			DialogResult result;
			if (val.HasError || val.HasInformation || val.HasWarning)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "بعضی از تنظیمات ارسال پیامک نادرست است", "", 5);
				result = (DialogResult)4;
			}
			else
			{
				result = (DialogResult)6;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidation.cs", 29);
		}
		return (DialogResult)7;
	}
}
