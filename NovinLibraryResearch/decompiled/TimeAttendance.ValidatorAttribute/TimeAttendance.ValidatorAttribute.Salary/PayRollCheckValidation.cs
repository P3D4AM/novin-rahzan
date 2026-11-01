using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendance.ValidatorAttribute.Salary;

public class PayRollCheckValidation
{
	public static async Task<DialogResult> CheckValidationAsync(string contextGuid, Fish pObj, bool isShowMessage, bool isShowButtons, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			await Task.Delay(100);
			if (token.IsCancellationRequested)
			{
				return (DialogResult)2;
			}
			ReturnedSaveFuncInfo val = await new PayRollValidator(contextGuid, pObj).ValidateAsync(token);
			DialogResult result;
			if (isShowMessage && (val.HasError || val.HasInformation || val.HasWarning))
			{
				MessageBoxButtons val2 = (MessageBoxButtons)2;
				if (!isShowButtons)
				{
					val2 = (MessageBoxButtons)0;
				}
				FrmShowErrorMessage val3 = new FrmShowErrorMessage(contextGuid, val, "", val2, false, (LanguageTypes)0, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollCheckValidation.cs");
				((Form)val3).ShowDialog();
				result = ((Form)val3).DialogResult;
			}
			else
			{
				result = (DialogResult)6;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollCheckValidation.cs", 41);
		}
		return (DialogResult)7;
	}
}
