using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendance.ValidatorAttribute.Salary.Codeing;

internal class CodeingHesabValidator : IHasContext
{
	private FishHesab _obj;

	public string ContextGuid { get; set; }

	private async Task<bool> BeAValidFormulaAsync(FishHesab objFishHesab, CancellationToken token)
	{
		try
		{
			return await FishHesab.GetCountByCodeMAsync(ContextGuid, objFishHesab.Id, objFishHesab.CodeMoein, objFishHesab.CodeTafsil, objFishHesab.Type, token) <= 0;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public CodeingHesabValidator(string contextGuid, FishHesab pObj)
	{
		ContextGuid = contextGuid;
		_obj = pObj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (_obj?.Name != null && string.IsNullOrEmpty(_obj.Name))
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "لطفا نام حساب را وارد کنید.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\CodeingHesabValidator.cs", 43);
			}
			FishHesab obj = _obj;
			int num;
			if (obj == null)
			{
				num = 0;
			}
			else
			{
				_ = obj.CodeMoein;
				num = 1;
			}
			if (num != 0 && _obj.CodeMoein == 0L)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "کد معین نامعتبر است", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\CodeingHesabValidator.cs", 45);
			}
			FishHesab obj2 = _obj;
			int num2;
			if (obj2 == null)
			{
				num2 = 0;
			}
			else
			{
				_ = obj2.CodeTafsil;
				num2 = 1;
			}
			if (num2 != 0 && _obj.CodeTafsil == 0L)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "کد تفصیل نامعتبر است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\CodeingHesabValidator.cs", 47);
			}
			FishHesab obj3 = _obj;
			int num3;
			if (obj3 == null)
			{
				num3 = 0;
			}
			else
			{
				_ = obj3.Type;
				num3 = 1;
			}
			if (num3 != 0 && _obj.Type != 1 && _obj.Type != 2)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مزایا یا کسورات را انتخاب کنید.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\CodeingHesabValidator.cs", 49);
			}
			bool flag = _obj != null;
			if (flag)
			{
				flag = !(await BeAValidFormulaAsync(_obj, token));
			}
			if (flag)
			{
				errorsInfo.AddReturnedValue((ReturnedState)4, "حساب های دیگری با این کد معین و کد تفصیل وجود دارد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\CodeingHesabValidator.cs", 51);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\Codeing\\CodeingHesabValidator.cs", 56);
		}
		return errorsInfo;
	}
}
