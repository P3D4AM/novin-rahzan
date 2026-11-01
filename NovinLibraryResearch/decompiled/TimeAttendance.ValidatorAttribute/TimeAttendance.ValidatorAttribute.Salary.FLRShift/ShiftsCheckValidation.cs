using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRShift;

namespace TimeAttendance.ValidatorAttribute.Salary.FLRShift;

public class ShiftsCheckValidation
{
	public static async Task<DialogResult> CheckValidationAsync(string contextGuid, Shifts pObj, bool isShowMessage, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ReturnedSaveFuncInfo val = await new ShiftsValidator(contextGuid, pObj).ValidateAsync(token);
			DialogResult result;
			if (isShowMessage && (val.HasError || val.HasInformation || val.HasWarning))
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsCheckValidation.cs");
				((Form)val2).ShowDialog();
				result = ((Form)val2).DialogResult;
			}
			else
			{
				result = (DialogResult)6;
			}
			return result;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsCheckValidation.cs", 30);
		}
		return (DialogResult)7;
	}
}
