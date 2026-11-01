using System;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendance.ValidatorAttribute.FLRGharardad;

internal class GharardadDetValidator
{
	private string _messageError = "";

	private GharardadDetNew _obj;

	private int _pLastPrivilage;

	public GharardadDetValidator(GharardadDetNew pObj, int pLastPrivilage)
	{
		_obj = pObj;
		_pLastPrivilage = pLastPrivilage;
	}

	private bool BeAValidFormula(string formula)
	{
		ExpressionClass expressionClass = new ExpressionClass(formula);
		_messageError = expressionClass.ErrorMsg;
		return expressionClass.Result;
	}

	public ReturnedSaveFuncInfo Validate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (_obj?.Name != null && string.IsNullOrEmpty(_obj.Name))
			{
				val.AddReturnedValue((ReturnedState)2, "لطفا نام حساب را وارد کنید.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\GharardadDetValidator.cs", 55);
			}
			GharardadDetNew obj = _obj;
			if (obj != null)
			{
				_ = obj.Privilege;
				if (true && _obj?.Privilege > _pLastPrivilage)
				{
					val.AddReturnedValue((ReturnedState)2, "فرمول جاری نمیتواند از فرمول های هم سطح خود استفاده کند.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\GharardadDetValidator.cs", 57);
				}
			}
			GharardadDetNew obj2 = _obj;
			if (obj2 != null)
			{
				_ = obj2.TypeAccount;
				if (true && ((int?)_obj?.TypeAccount).GetValueOrDefault() != 1 && ((int?)_obj?.TypeAccount).GetValueOrDefault() != 2)
				{
					val.AddReturnedValue((ReturnedState)2, "مزایا یا کسورات را انتخاب کنید.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\GharardadDetValidator.cs", 59);
				}
			}
			if (_obj?.Formula != null && !BeAValidFormula(_obj?.Formula))
			{
				val.AddReturnedValue((ReturnedState)2, "فرمول نامعتبر است", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\GharardadDetValidator.cs", 61);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\GharardadDetValidator.cs", 65);
		}
		return val;
	}
}
