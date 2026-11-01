using System;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendance.ValidatorAttribute.Company;

public class CompanyValidation
{
	public static DialogResult CheckValidation(string contextGuid, TimeAttendanceDAL.Dal.Other.Company pObj, bool isShowMessage)
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
			ReturnedSaveFuncInfo val2 = new CompanyValidator(pObj).Validate();
			if (val2.HasError || val2.HasInformation || val2.HasWarning)
			{
				FrmShowErrorMessage val3 = new FrmShowErrorMessage(contextGuid, val2, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Company\\CompanyValidation.cs");
				((Form)val3).ShowDialog();
				return ((Form)val3).DialogResult;
			}
			return (DialogResult)6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Company\\CompanyValidation.cs", 43);
		}
		return (DialogResult)7;
	}
}
