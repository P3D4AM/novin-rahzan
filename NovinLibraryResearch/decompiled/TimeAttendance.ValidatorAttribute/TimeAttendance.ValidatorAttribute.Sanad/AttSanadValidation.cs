using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRSanad;

namespace TimeAttendance.ValidatorAttribute.Sanad;

public class AttSanadValidation
{
	public static async Task<DialogResult> CheckValidationAsync(string contextGuid, AttSanad pObj, bool isShowMessage, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ReturnedSaveFuncInfo val = await new AttSanadValidator(pObj).ValidateAsync(token);
			DialogResult result;
			if (isShowMessage && (val.HasError || val.HasInformation || val.HasWarning))
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadValidation.cs");
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadValidation.cs", 27);
		}
		return (DialogResult)7;
	}
}
