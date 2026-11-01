using System;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.View.Shift;

namespace TimeAttendance.ValidatorAttribute.Package;

public class PackageValidator
{
	private ShiftViewModel _obj;

	private bool BeValidDate(ShiftViewModel pcls)
	{
		return string.Compare(pcls.DateFinishSh, pcls.DateStartSh, StringComparison.Ordinal) >= 0;
	}

	public PackageValidator(ShiftViewModel pObj)
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
			if (_obj != null && !BeValidDate(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "تاریخ شروع از تاریخ خاتمه بزرگتر است.", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Package\\PackageValidator.cs", 28);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Package\\PackageValidator.cs", 32);
		}
		return val;
	}
}
