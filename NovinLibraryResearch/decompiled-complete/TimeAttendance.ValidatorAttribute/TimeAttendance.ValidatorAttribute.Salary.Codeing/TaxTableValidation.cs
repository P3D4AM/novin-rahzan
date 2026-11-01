using System;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendance.ValidatorAttribute.Salary.Codeing;

public class TaxTableValidation
{
	public static DialogResult CheckValidation(string contextGuid, TaxTable pObj, bool isShowMessage)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReturnedSaveFuncInfo val = new TaxTableValidator(pObj).Validate();
			if ((val.HasError || val.HasInformation || val.HasWarning) && isShowMessage)
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\TaxTableValidation.cs");
				((Form)val2).ShowDialog();
				return ((Form)val2).DialogResult;
			}
			return (DialogResult)6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\TaxTableValidation.cs", 28);
		}
		return (DialogResult)7;
	}
}
