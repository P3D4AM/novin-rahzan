using System;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRShift;

namespace TimeAttendance.ValidatorAttribute.Salary.FLRShift;

public class ShiftCodeingCheckValidation
{
	public static DialogResult CheckValidation(string contextGuid, ShiftCodeing pObj, bool isShowMessage)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DialogResult val = (DialogResult)7;
			ReturnedSaveFuncInfo val2 = new ShiftCodeingValidator(pObj).Validate();
			if ((val2.HasError || val2.HasInformation || val2.HasWarning) && isShowMessage)
			{
				FrmShowErrorMessage val3 = new FrmShowErrorMessage(contextGuid, val2, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingCheckValidation.cs");
				((Form)val3).ShowDialog();
				return ((Form)val3).DialogResult;
			}
			return (DialogResult)6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingCheckValidation.cs", 45);
		}
		return (DialogResult)7;
	}
}
