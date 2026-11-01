using System;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendance.ValidatorAttribute.Company;

internal class CompanyValidator
{
	public TimeAttendanceDAL.Dal.Other.Company _ObjCompany;

	public CompanyValidator(TimeAttendanceDAL.Dal.Other.Company objCompany)
	{
		_ObjCompany = objCompany;
	}

	public ReturnedSaveFuncInfo Validate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (_ObjCompany?.Name != null && string.IsNullOrEmpty(_ObjCompany.Name))
			{
				val.AddReturnedValue((ReturnedState)2, "وارد کردن نام شرکت الزامی است", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Company\\CompanyValidator.cs", 22);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Company\\CompanyValidator.cs", 26);
		}
		return val;
	}
}
