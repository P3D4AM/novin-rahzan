using System;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRSanad;

namespace TimeAttendance.ValidatorAttribute.Sanad;

public class AttSanadDetValidation
{
	public static DialogResult CheckValidation(string contextGuid, ATT_SanadDet pObj, bool isShowMessage)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DialogResult val = (DialogResult)7;
			ReturnedSaveFuncInfo val2 = new AttSanadDetValidator(pObj).Validate();
			if (isShowMessage && (val2.HasError || val2.HasInformation || val2.HasWarning))
			{
				FrmShowErrorMessage val3 = new FrmShowErrorMessage(contextGuid, val2, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadDetValidation.cs");
				((Form)val3).ShowDialog();
				return ((Form)val3).DialogResult;
			}
			return (DialogResult)6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadDetValidation.cs", 47);
		}
		return (DialogResult)7;
	}
}
