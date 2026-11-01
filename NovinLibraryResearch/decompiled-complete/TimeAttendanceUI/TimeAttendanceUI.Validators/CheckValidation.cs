using System;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace TimeAttendanceUI.Validators;

internal class CheckValidation
{
	public static DialogResult Validtor(string contextGuid, ReturnedSaveFuncInfo pObj, bool isShowMessage)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DialogResult val = (DialogResult)7;
			if (!pObj.HasError && !pObj.HasWarning)
			{
				val = (DialogResult)6;
			}
			if ((int)val == 6 || !isShowMessage)
			{
				return val;
			}
			FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, pObj, "خطا", (MessageBoxButtons)3, false, (LanguageTypes)0, "Validtor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Validators\\CheckValidation.cs");
			((Form)val2).ShowDialog();
			return ((Form)val2).DialogResult;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validtor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Validators\\CheckValidation.cs", 25);
		}
		return (DialogResult)7;
	}
}
