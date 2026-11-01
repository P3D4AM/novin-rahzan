using System;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRShift;

namespace TimeAttendance.ValidatorAttribute.Salary.FLRShift;

public class ShiftCodeingValidator
{
	private ShiftCodeing _obj;

	public ShiftCodeingValidator(ShiftCodeing pObj)
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
			if (_obj?.Description != null && string.IsNullOrEmpty(_obj.Description))
			{
				val.AddReturnedValue((ReturnedState)2, "[شرح نوع تردد نمیتواند خالی باشد.]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 24);
			}
			if (_obj != null && !BeValidTimeOnShiftNormal(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[در صورتیکه شیفت انتقال به روز بعد ندارد زمان خروج نباید کوچکتر از ورود باشد]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 27);
			}
			if (_obj != null && !BeValidTimeOnShiftNormal2(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[زمان شروع و پایان شیفت نباید یک زمان باشند]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 30);
			}
			if (_obj != null && !BeValidTimeOnShiftNormal3(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[در حالت عادی زمان خروج بایستی بزرگتر از ورود باشد در صورتیکه خروج در روز بعد است زمان خروج بایستی حداقل یک دقیقه کمتر از ورود باشد]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 33);
			}
			if (_obj != null && !BeValidTimeOnShiftShenavar(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[مدت زمان کاری شیفت شناور نامعتبر است.]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 36);
			}
			if (_obj != null && !BeValidMaxTimeCanExit(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[حداکثر زمان خروج نامعتبر است.]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 40);
			}
			if (_obj != null && !BeValidpeakOverTime(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[سقف اضافه کاری نامعتبر است.]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 43);
			}
			if (_obj != null && !BeValidpeakOverTime2(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[سقف اضافه کاری نباید از ساعت 24 تجاوز کند]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 46);
			}
			if (_obj != null && !BeValidLastExitTime(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[چون این شیفت انتقال به روز بعد دارد، حداکثر زمان خروج نباید صفر باشد]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 52);
			}
			if (_obj != null && !BeValidLastExitTime2(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "[در شیفت شناور حداکثر زمان خروج باید بزرگتر از مدت موظفی باشد]", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 55);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftCodeingValidator.cs", 59);
		}
		return val;
	}

	private bool BeValidLastExitTime(ShiftCodeing arg)
	{
		if (arg.NextDay)
		{
			return arg.MaxTimeCanExit > 0;
		}
		return true;
	}

	private bool BeValidLastExitTime2(ShiftCodeing arg)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		if (arg.NextDay && (int)arg.TypeShift == 2)
		{
			return arg.MaxTimeCanExit > arg.H2 * 60 + arg.M2;
		}
		return true;
	}

	private bool BeValidTimeOnShiftNormal(ShiftCodeing sc)
	{
		bool result = true;
		int num = sc.H1 * 60 + sc.M1;
		int num2 = sc.H2 * 60 + sc.M2;
		if (!sc.NextDay && num > num2)
		{
			result = false;
		}
		return result;
	}

	private bool BeValidTimeOnShiftNormal2(ShiftCodeing sc)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		bool result = true;
		int num = sc.H1 * 60 + sc.M1;
		int num2 = sc.H2 * 60 + sc.M2;
		if (num == num2 && (int)sc.TypeShift != 11)
		{
			result = false;
		}
		return result;
	}

	private bool BeValidTimeOnShiftNormal3(ShiftCodeing sc)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		bool result = true;
		int num = sc.H1 * 60 + sc.M1;
		int num2 = sc.H2 * 60 + sc.M2;
		if ((int)sc.TypeShift == 1 && sc.NextDay && num2 == num)
		{
			result = false;
		}
		return result;
	}

	private bool BeValidStartOnShiftNormal(ShiftCodeing sc)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)sc.TypeShift == 1)
		{
			if (sc.H1 == 0)
			{
				return sc.M1 != 0;
			}
			return true;
		}
		return true;
	}

	private bool BrValidLunch(ShiftCodeing sc)
	{
		if (sc.LunchTime > 0)
		{
			return sc.StartLunchTime >= 720;
		}
		return true;
	}

	private bool BeValidTimeOnShiftShenavar(ShiftCodeing sc)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		bool result = true;
		int num = sc.H1 * 60 + sc.M1;
		int num2 = sc.H2 * 60 + sc.M2;
		if ((int)sc.TypeShift == 2 && num > num2)
		{
			result = false;
		}
		return result;
	}

	private bool BeValidMaxTimeCanExit(ShiftCodeing sc)
	{
		return true;
	}

	private bool BeValidpeakOverTime(ShiftCodeing sc)
	{
		return true;
	}

	private bool BeValidpeakOverTime2(ShiftCodeing sc)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		int num = sc.H2 * 60 + sc.M2;
		int num2 = 1439;
		if ((int)sc.TypeShift != 1 || sc.PeakOvertime <= num2)
		{
			if ((int)sc.TypeShift == 2)
			{
				return sc.PeakOvertime <= num2 - num;
			}
			return true;
		}
		return false;
	}
}
