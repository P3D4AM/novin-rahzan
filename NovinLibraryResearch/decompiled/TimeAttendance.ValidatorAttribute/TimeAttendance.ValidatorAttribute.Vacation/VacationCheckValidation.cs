using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendance.ValidatorAttribute.Vacation;

public class VacationCheckValidation
{
	public static async Task<DialogResult> CheckValidationAsync(string contextGuid, Morakhasidaily pObj, StatuseTaradodEnumType statuseTaradod, bool isShowMessage, CancellationToken token)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReturnedSaveFuncInfo val = await new VacationValidator(contextGuid, pObj, statuseTaradod).ValidateAsync(token);
			DialogResult result;
			if (isShowMessage && (val.HasError || val.HasInformation || val.HasWarning))
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationCheckValidation.cs");
				((Form)val2).ShowDialog();
				result = ((Form)val2).DialogResult;
			}
			else
			{
				result = (DialogResult)6;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationCheckValidation.cs", 32);
		}
		return (DialogResult)7;
	}
}
