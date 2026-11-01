using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.View.Salary;

namespace TimeAttendance.ValidatorAttribute.Insurance;

public class InsuranceCheckValidation
{
	public static DialogResult CheckValidation(string contextGuid, List<TimeAttendanceDAL.View.Salary.Insurance> pObj, bool isShowMessage)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReturnedSaveFuncInfo val = new InsuranceValidator(pObj).Validate();
			if (val.HasError || val.HasInformation || val.HasWarning)
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Insurance\\InsuranceCheckValidation.cs");
				((Form)val2).ShowDialog();
				return ((Form)val2).DialogResult;
			}
			return (DialogResult)6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Insurance\\InsuranceCheckValidation.cs", 28);
		}
		return (DialogResult)7;
	}
}
