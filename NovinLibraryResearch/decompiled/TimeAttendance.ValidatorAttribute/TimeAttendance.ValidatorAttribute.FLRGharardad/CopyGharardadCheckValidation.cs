using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.View.Gharardad;

namespace TimeAttendance.ValidatorAttribute.FLRGharardad;

public class CopyGharardadCheckValidation
{
	public static async Task<DialogResult> CheckValidationAsync(string contextGuid, ClsCopyGharardad pObj, CancellationToken token)
	{
		try
		{
			DialogResult xDialogResult = (DialogResult)7;
			ReturnedSaveFuncInfo val = await new CopyGharardadValidator(contextGuid, pObj).ValidateAsync(token);
			if (val.HasError || val.HasInformation || val.HasWarning)
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\CopyGharardadCheckValidation.cs");
				((Form)val2).ShowDialog();
				xDialogResult = ((Form)val2).DialogResult;
			}
			return xDialogResult;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\CopyGharardadCheckValidation.cs", 29);
		}
		return (DialogResult)7;
	}
}
