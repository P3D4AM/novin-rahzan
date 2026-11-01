using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendance.ValidatorAttribute.Salary;

public class PayRollValidator : IHasContext
{
	private Fish _obj;

	public string ContextGuid { get; set; }

	public PayRollValidator(string contextGuid, Fish pObj)
	{
		ContextGuid = contextGuid;
		_obj = pObj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			Fish obj = _obj;
			int num;
			if (obj == null)
			{
				num = 0;
			}
			else
			{
				_ = obj.FishNo;
				num = 1;
			}
			if (num != 0 && _obj.FishNo == 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "وارد کردن شماره فیش الزامی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 33);
			}
			bool flag = _obj != null;
			if (flag)
			{
				flag = !(await CheckFishNoAsync(_obj, token));
			}
			if (flag)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "شماره فیش وارد شده تکراری می باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 35);
			}
			if (_obj != null && _obj.CountNaghes > 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)4, "برای این شخص در این بازه زمانی تردد ناقص وجود دارد، آیا مایل به ثبت فیش هستید؟", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 37);
			}
			if (_obj != null && _obj.SumKhales < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ فیش حقوقی نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 39);
			}
			flag = _obj != null;
			if (flag)
			{
				flag = !(await BeVallidMaliatAsync(_obj, token));
			}
			if (flag)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "گزینه مالیات را انتخاب کرده اید ولی جدول مالیاتی برای این سال تهیه نشده است", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 41);
			}
			if (_obj != null && _obj.KasrMin < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "یکی از روزها کسر کار نامعتبر است (احتمالا\u064b ) مثبت شده است", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 43);
			}
			if (_obj != null && _obj.KasrPrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ کسر کار نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 45);
			}
			if (_obj != null && _obj.CountKarKerd < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "تعداد روزهای کارکرد نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 47);
			}
			if (_obj != null && _obj.KarkerdPrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ کارکرد نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 49);
			}
			if (_obj != null && _obj.EzafeMin < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع اضافه کاری نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 51);
			}
			if (_obj != null && _obj.EzafePrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ اضافه کاری نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 53);
			}
			if (_obj != null && _obj.ShabMin < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع شب کاری نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 55);
			}
			if (_obj != null && _obj.ShabPrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ شب کاری نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 57);
			}
			if (_obj != null && _obj.NobatMin1 < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع نوبت کاری صبح نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 59);
			}
			if (_obj != null && _obj.NobatPrice1 < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ نوبت کاری صبح نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 61);
			}
			if (_obj != null && _obj.NobatMin2 < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع نوبت کاری عصر نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 63);
			}
			if (_obj != null && _obj.NobatPrice2 < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ نوبت کاری عصر نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 65);
			}
			if (_obj != null && _obj.NobatMin3 < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع نوبت کاری شب نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 67);
			}
			if (_obj != null && _obj.NobatPrice3 < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ نوبت کاری شب نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 69);
			}
			if (_obj != null && _obj.JomeMin < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع جمعه کاری نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 73);
			}
			if (_obj != null && _obj.JomePrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ جمعه کاری نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 75);
			}
			if (_obj != null && _obj.VacationMin < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع مرخصی نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 77);
			}
			if (_obj != null && _obj.VacationPrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ مرخصی نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 79);
			}
			if (_obj != null && _obj.GheybatRoze < 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع غیبت روزانه نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 81);
			}
			if (_obj != null && _obj.GheybatPrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ غیبت روزانه نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 83);
			}
			if (_obj != null && _obj.MissionRoz < 0.0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "جمع ماموریت روزانه نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 85);
			}
			if (_obj != null && _obj.MissionPrice < 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ ماموریت روزانه نباید کمتر از صفر باشد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 87);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\PayRollValidator.cs", 91);
		}
		return errorsInfo;
	}

	private async Task<bool> CheckFishNoAsync(Fish pFish, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return await Fish.CheckFishNoAsync(ContextGuid, pFish.FishNo, pFish.Id, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task<bool> BeVallidMaliatAsync(Fish pgh, CancellationToken token)
	{
		try
		{
			bool result = true;
			if (pgh.ListFishDet.Any((FishDetJson q) => q.HesabId == 6))
			{
				int yearOfDate = LanguageAndCalendar.Instance.CaltureCalender.GetYearOfDate(Calendar.MiladiToCalture(pgh.DateStartM));
				if (await TaxTable.RecordCountAsync(ContextGuid, (short)yearOfDate, token) <= 0)
				{
					result = false;
				}
			}
			return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
