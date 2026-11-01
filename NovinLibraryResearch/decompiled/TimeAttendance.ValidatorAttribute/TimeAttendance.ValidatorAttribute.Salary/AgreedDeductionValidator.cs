using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendance.ValidatorAttribute.Salary;

internal class AgreedDeductionValidator : IHasContext
{
	private AgreedDeduction _obj;

	public string ContextGuid { get; set; }

	public AgreedDeductionValidator(string contextGuid, AgreedDeduction pObj)
	{
		ContextGuid = contextGuid;
		_obj = pObj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			AgreedDeduction obj = _obj;
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
				errorsInfo.AddReturnedValue((ReturnedState)2, "لطفا کد شخص را وارد کنید.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 32);
			}
			AgreedDeduction obj2 = _obj;
			int num2;
			if (obj2 == null)
			{
				num2 = 0;
			}
			else
			{
				_ = obj2.HesabId;
				num2 = 1;
			}
			if (num2 != 0 && _obj.HesabId <= 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "لطفا نام حساب را وارد کنید.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 34);
			}
			AgreedDeduction obj3 = _obj;
			int num3;
			if (obj3 == null)
			{
				num3 = 0;
			}
			else
			{
				_ = obj3.AmountPayable;
				num3 = 1;
			}
			if (num3 != 0 && _obj.AmountPayable <= 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ قابل پرداخت باید بزرگتر از صفر باشد.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 36);
			}
			AgreedDeduction obj4 = _obj;
			int num4;
			if (obj4 == null)
			{
				num4 = 0;
			}
			else
			{
				_ = obj4.MonthlyInstallment;
				num4 = 1;
			}
			if (num4 != 0 && _obj.MonthlyInstallment <= 0m)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ قسط ماهیانه باید بزرگتر از صفر باشد.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 38);
			}
			AgreedDeduction obj5 = _obj;
			int num5;
			if (obj5 == null)
			{
				num5 = 0;
			}
			else
			{
				_ = obj5.MonthlyInstallment;
				num5 = 1;
			}
			if (num5 != 0 && _obj.MonthlyInstallment > _obj.AmountPayable)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "مبلغ قسط ماهیانه نباید بیشتر از مبلغ قابل پرداخت باشد.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 40);
			}
			bool flag = _obj != null;
			if (flag)
			{
				flag = !(await CheckFishNoAsync(_obj, token));
			}
			if (flag)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "حساب انتخاب شده در قرارداد شخص وجود دارد و شما نمیتوانید از آن استفاده نمائید.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 42);
			}
			AgreedDeduction obj6 = _obj;
			int num6;
			if (obj6 == null)
			{
				num6 = 0;
			}
			else
			{
				_ = obj6.DateCreatedM;
				num6 = 1;
			}
			if (num6 != 0 && _obj.DateCreatedM <= new DateTime(2010L))
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "تاریخ نامعتبر است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 44);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Salary\\AgreedDeductionValidator.cs", 48);
		}
		return errorsInfo;
	}

	private async Task<bool> CheckFishNoAsync(AgreedDeduction pagd, CancellationToken token)
	{
		try
		{
			GharardadNew gharardadNew = await GharardadNew.GetAsync(ContextGuid, pagd.PersonId, pagd.DateCreatedM, token);
			if (gharardadNew == null)
			{
				return true;
			}
			return !gharardadNew.ListGharardadDet.Any((GharardadDetNew q) => q.HesabId == pagd.HesabId);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
