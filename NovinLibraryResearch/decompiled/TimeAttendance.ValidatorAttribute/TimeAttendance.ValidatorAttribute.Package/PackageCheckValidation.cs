using System;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.View.Shift;

namespace TimeAttendance.ValidatorAttribute.Package;

public class PackageCheckValidation
{
	public static DialogResult CheckValidation(string contextGuid, ShiftViewModel pObj, bool isShowMessage)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DialogResult val = (DialogResult)7;
			ReturnedSaveFuncInfo val2 = new PackageValidator(pObj).Validate();
			if (val2.HasError || val2.HasInformation || val2.HasWarning)
			{
				FrmShowErrorMessage val3 = new FrmShowErrorMessage(contextGuid, val2, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Package\\PackageCheckValidation.cs");
				((Form)val3).ShowDialog();
				return ((Form)val3).DialogResult;
			}
			return (DialogResult)6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Package\\PackageCheckValidation.cs", 46);
		}
		return (DialogResult)7;
	}
}
