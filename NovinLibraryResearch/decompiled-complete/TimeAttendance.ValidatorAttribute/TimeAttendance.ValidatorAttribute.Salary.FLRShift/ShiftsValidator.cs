using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRShift;

namespace TimeAttendance.ValidatorAttribute.Salary.FLRShift;

internal class ShiftsValidator : IHasContext
{
	private Shifts _obj;

	public string ContextGuid { get; set; }

	public ShiftsValidator(string contextGuid, Shifts pObj)
	{
		ContextGuid = contextGuid;
		_obj = pObj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (_obj != null)
			{
				if (_obj?.Name != null && string.IsNullOrEmpty(_obj.Name))
				{
					errorsInfo.AddReturnedValue((ReturnedState)2, "لطفا نام شیفت را وارد کنید.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsValidator.cs", 30);
				}
				if (_obj?.Code != null && string.IsNullOrEmpty(_obj.Code))
				{
					errorsInfo.AddReturnedValue((ReturnedState)2, "وارد کردن کد شیفت الزامی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsValidator.cs", 32);
				}
				if (!(await BeValidCodeAsync(_obj, token)))
				{
					errorsInfo.AddReturnedValue((ReturnedState)2, "کد وارد شده تکراری می باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsValidator.cs", 34);
				}
				foreach (ShiftDet item in _obj.LstShiftDet)
				{
					ShiftDetValidator shiftDetValidator = new ShiftDetValidator(item);
					errorsInfo.AddReturnedValue(shiftDetValidator.Validate(), (ReturnedState)0, "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsValidator.cs", 38);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsValidator.cs", 44);
		}
		return errorsInfo;
	}

	private async Task<bool> BeValidCodeAsync(Shifts objFish, CancellationToken token)
	{
		try
		{
			int num = await Shifts.UniqeCodeAsync(ContextGuid, objFish.Code, token);
			return num == 0 || num == objFish.Id;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BeValidCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\FLRShift\\ShiftsValidator.cs", 58);
		}
		return false;
	}
}
