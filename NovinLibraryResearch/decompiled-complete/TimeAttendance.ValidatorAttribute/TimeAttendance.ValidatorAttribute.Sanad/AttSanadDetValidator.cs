using System;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRSanad;

namespace TimeAttendance.ValidatorAttribute.Sanad;

internal class AttSanadDetValidator
{
	private ATT_SanadDet _obj;

	public AttSanadDetValidator(ATT_SanadDet pObj)
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
			ATT_SanadDet obj = _obj;
			if (obj != null)
			{
				_ = obj.CodeMoein;
				if (true && _obj.CodeMoein == 0L)
				{
					val.AddReturnedValue((ReturnedState)2, "کد معین نمیتواند خالی باشد.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadDetValidator.cs", 22);
				}
			}
			ATT_SanadDet obj2 = _obj;
			if (obj2 != null)
			{
				_ = obj2.CodeTafsil;
				if (true && _obj.CodeTafsil == 0L)
				{
					val.AddReturnedValue((ReturnedState)2, "کد تفصیل نمیتواند خالی باشد.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadDetValidator.cs", 24);
				}
			}
			ATT_SanadDet obj3 = _obj;
			if (obj3 != null)
			{
				_ = obj3.Credit;
				if (true)
				{
					ATT_SanadDet obj4 = _obj;
					if (obj4 != null)
					{
						_ = obj4.Debit;
						if (true && _obj.Credit == 0m && _obj.Debit == 0m)
						{
							val.AddReturnedValue((ReturnedState)2, "مبلغ بدهکار و بستانکار نمیتوانند تواما\u064b صفر باشند.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadDetValidator.cs", 26);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadDetValidator.cs", 30);
		}
		return val;
	}
}
