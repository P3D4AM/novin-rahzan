using System;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendance.ValidatorAttribute.Salary.Codeing;

internal class TaxTableValidator
{
	private TaxTable _obj;

	public TaxTableValidator(TaxTable pObj)
	{
		_obj = pObj;
	}

	public ReturnedSaveFuncInfo Validate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (_obj?.StartDateM > _obj?.EndDateM)
			{
				val.AddReturnedValue((ReturnedState)2, "تاریخ پایان باید از تاریخ شرئع بیشتر باشد.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\TaxTableValidator.cs", 22);
			}
			if (_obj?.FromPrice > _obj?.ToPrice)
			{
				val.AddReturnedValue((ReturnedState)2, "مبلغ اول باید کوچکتر از مبلغ دوم باشد", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\TaxTableValidator.cs", 24);
			}
			TaxTable obj = _obj;
			if (obj != null && obj.FromPrice <= default(decimal))
			{
				val.AddReturnedValue((ReturnedState)2, "مبالغ نباید کمتر یا مساوی صفر باشد.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\TaxTableValidator.cs", 26);
			}
			TaxTable obj2 = _obj;
			if (obj2 != null && obj2.InitialValue < default(decimal))
			{
				val.AddReturnedValue((ReturnedState)2, "مبلغ مقدار اولیه باید بزرگتر یا مساوی صفر باشد.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\TaxTableValidator.cs", 28);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\TaxTableValidator.cs", 32);
		}
		return val;
	}
}
