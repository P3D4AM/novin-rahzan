using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendance.ValidatorAttribute.Vacation;

public class VacationValidator : IHasContext
{
	private TaradodFullDet _getTaradodFullDet;

	private Morakhasidaily _obj;

	private StatuseTaradodEnumType _statuseTaradod;

	public string ContextGuid { get; set; }

	public VacationValidator(string contextGuid, Morakhasidaily pObj, StatuseTaradodEnumType statuseTaradod)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		_obj = pObj;
		_statuseTaradod = statuseTaradod;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		try
		{
			ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
			if (_obj == null)
			{
				throw new ArgumentNullException(_obj.GetType().Name);
			}
			if (_obj.TypeId <= 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "[نوع مرخصی مشخص نشده است]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationValidator.cs", 52);
			}
			if (await IsExistAsync(_obj, token))
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "[قبلا در این روز برای این شخص مرخصی ثبت شده است]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationValidator.cs", 55);
			}
			Morakhasidaily obj = _obj;
			int num;
			if (obj == null)
			{
				num = 0;
			}
			else
			{
				_ = obj.PersonId;
				num = 1;
			}
			if (num != 0 && _obj.PersonId <= 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "[نام شخص انتخاب نشده است]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationValidator.cs", 58);
			}
			if ((int)_statuseTaradod == 11)
			{
				errorsInfo.AddReturnedValue((ReturnedState)4, "[این شخص در این روز استراحت دارد]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationValidator.cs", 61);
			}
			if ((int)_statuseTaradod == 8)
			{
				errorsInfo.AddReturnedValue((ReturnedState)4, "[این شخص در این روز شیفت ندارد]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationValidator.cs", 64);
			}
			if ((int)_statuseTaradod == 2)
			{
				errorsInfo.AddReturnedValue((ReturnedState)4, "[این شخص در این روز تردد ناقص دارد]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationValidator.cs", 67);
			}
			return errorsInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task<bool> IsExistAsync(Morakhasidaily morakhasi, CancellationToken token)
	{
		bool result = false;
		try
		{
			Morakhasidaily morakhasidaily = await Morakhasidaily.GetAsync(ContextGuid, morakhasi.DateFrom, morakhasi.PersonId, token);
			if (morakhasidaily != null && morakhasi.Id == 0 && morakhasidaily.Id > 0 && morakhasidaily.PersonId == morakhasi.PersonId && morakhasidaily.DateFrom == morakhasi.DateFrom)
			{
				result = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Vacation\\VacationValidator.cs", 91);
		}
		return result;
	}
}
