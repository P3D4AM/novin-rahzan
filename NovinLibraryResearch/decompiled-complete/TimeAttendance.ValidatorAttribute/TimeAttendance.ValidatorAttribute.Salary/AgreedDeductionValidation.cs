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

public class AgreedDeductionValidation
{
	public static async Task<DialogResult> CheckValidationAsync(string contextGuid, AgreedDeduction pObj, bool isShowMessage, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ReturnedSaveFuncInfo val = await new AgreedDeductionValidator(contextGuid, pObj).ValidateAsync(token);
			DialogResult result;
			if ((val.HasError || val.HasInformation || val.HasWarning) && isShowMessage)
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidation.cs");
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidation.cs", 30);
		}
		return (DialogResult)7;
	}
}
