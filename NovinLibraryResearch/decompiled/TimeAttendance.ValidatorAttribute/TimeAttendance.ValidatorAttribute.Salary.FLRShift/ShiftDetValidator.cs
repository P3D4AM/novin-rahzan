using System;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRShift;

namespace TimeAttendance.ValidatorAttribute.Salary.FLRShift;

internal class ShiftDetValidator
{
	private ShiftDet _obj;

	public ShiftDetValidator(ShiftDet pObj)
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
			if (_obj == null)
			{
				val.AddReturnedValue((ReturnedState)2, "شیفت انتخاب نشده است", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftDetValidator.cs", 33);
				return val;
			}
			if (_obj.DateStartM > _obj.DateFinishM)
			{
				val.AddReturnedValue((ReturnedState)2, "تاریخ پایان قبل از تاریخ شروع تعیین شده است", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftDetValidator.cs", 37);
			}
			if (string.IsNullOrEmpty(_obj?.Description))
			{
				val.AddReturnedValue((ReturnedState)2, "برای این نوع شیفت یک عنوان و شرحی انتخاب نمائید.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftDetValidator.cs", 39);
			}
			if (string.IsNullOrEmpty(_obj?.ShiftCodingId.ToString()))
			{
				val.AddReturnedValue((ReturnedState)2, "نوع تردد شیفت را انتخاب نمائید.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftDetValidator.cs", 41);
			}
			if (_obj.ShiftCodeing == null)
			{
				val.AddReturnedValue((ReturnedState)2, "نوع تردد شیفت را انتخاب نمائید.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftDetValidator.cs", 43);
			}
			if (!BeValidWeek(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, ". لطفا حداقل یکی از روزهای هفته را انتخاب کنید", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftDetValidator.cs", 45);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftDetValidator.cs", 50);
		}
		return val;
	}

	private bool BeValidWeek(ShiftDet objShiftDet)
	{
		if (!objShiftDet.Sat && !objShiftDet.Sun && !objShiftDet.Mon && !objShiftDet.Tue && !objShiftDet.Wed && !objShiftDet.Tur)
		{
			return objShiftDet.Fri;
		}
		return true;
	}
}
