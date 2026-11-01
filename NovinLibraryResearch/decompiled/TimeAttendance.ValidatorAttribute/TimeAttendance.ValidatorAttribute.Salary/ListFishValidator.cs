using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Settings;

namespace TimeAttendance.ValidatorAttribute.Salary;

internal class ListFishValidator : IHasContext
{
	private List<Fish> _obj;

	public string ContextGuid { get; set; }

	public ListFishValidator(string contextGuid, List<Fish> pObj)
	{
		ContextGuid = contextGuid;
		_obj = pObj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (_obj != null && !BeValidDate(_obj))
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "فیش های حقوقی انتخاب شده در یک ماه نیستند", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\ListFishValidator.cs", 32);
			}
			if (_obj != null && !BePersonValid(_obj))
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "از یک شخص 2 تا فیش حقوقی انتخاب شده است", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\ListFishValidator.cs", 34);
			}
			bool flag = _obj != null;
			if (flag)
			{
				flag = !(await BeAccountingValidAsync(_obj, token));
			}
			if (flag)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "کدینگ حسابهای پیش فرض مقداردهی نشده است.(امکانات - تنظیمات نرم افزار - سند اتوامات حقوق)", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\ListFishValidator.cs", 36);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\ListFishValidator.cs", 40);
		}
		return errorsInfo;
	}

	private bool BeValidDate(List<Fish> objFishs)
	{
		string text = "";
		bool result = true;
		foreach (Fish objFish in objFishs)
		{
			if (string.IsNullOrEmpty(text))
			{
				text = Calendar.MiladiToCalture(objFish.DateM).Substring(0, 7);
			}
			else if (!(text == Calendar.MiladiToCalture(objFish.DateM).Substring(0, 7)))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private bool BePersonValid(List<Fish> objFishs)
	{
		return !(from l in objFishs
			group l by l.PersonId into cl
			select new
			{
				xPersonId = cl.First().PersonId,
				xCount = cl.Count()
			}).ToList().Any(q => q.xCount > 1);
	}

	private async Task<bool> BeAccountingValidAsync(List<Fish> objFishs, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return !(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).CodeingAccounting.IsEmptyAccountingCodeing();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
