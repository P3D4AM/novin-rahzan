using System;
using System.Windows.Forms;
using FormCustom;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRGharardad;

namespace TimeAttendance.ValidatorAttribute.FLRGharardad;

public class GharardadDetCheckValidation
{
	public static DialogResult CheckValidation(string contextGuid, GharardadDetNew pObj, int pLastPrivilage)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReturnedSaveFuncInfo val = new GharardadDetValidator(pObj, pLastPrivilage).Validate();
			if (val.HasError || val.HasInformation || val.HasWarning)
			{
				FrmShowErrorMessage val2 = new FrmShowErrorMessage(contextGuid, val, "", (MessageBoxButtons)3, false, (LanguageTypes)0, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\GharardadDetCheckValidation.cs");
				((Form)val2).ShowDialog();
				return ((Form)val2).DialogResult;
			}
			return (DialogResult)6;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\GharardadDetCheckValidation.cs", 28);
		}
		return (DialogResult)7;
	}
}
