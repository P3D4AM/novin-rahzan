using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.View.Salary;

namespace TimeAttendance.ValidatorAttribute.Insurance;

internal class InsuranceValidator
{
	private List<TimeAttendanceDAL.View.Salary.Insurance> _obj;

	private List<TimeAttendanceDAL.View.Salary.Insurance> EmptyWorkShopCodeList(List<TimeAttendanceDAL.View.Salary.Insurance> insurance)
	{
		List<TimeAttendanceDAL.View.Salary.Insurance> result = new List<TimeAttendanceDAL.View.Salary.Insurance>();
		try
		{
			result = insurance.Where((TimeAttendanceDAL.View.Salary.Insurance p) => p.WorkShopCode == 0).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EmptyWorkShopCodeList", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Insurance\\InsuranceValidator.cs", 21);
		}
		return result;
	}

	private List<TimeAttendanceDAL.View.Salary.Insurance> EmptyInsuranceNoList(List<TimeAttendanceDAL.View.Salary.Insurance> insurance)
	{
		List<TimeAttendanceDAL.View.Salary.Insurance> result = new List<TimeAttendanceDAL.View.Salary.Insurance>();
		try
		{
			result = insurance.Where((TimeAttendanceDAL.View.Salary.Insurance p) => string.IsNullOrEmpty(p.InsuranceNo)).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EmptyInsuranceNoList", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Insurance\\InsuranceValidator.cs", 34);
		}
		return result;
	}

	public InsuranceValidator(List<TimeAttendanceDAL.View.Salary.Insurance> pObj)
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
			if (_obj != null)
			{
				List<TimeAttendanceDAL.View.Salary.Insurance> list = EmptyWorkShopCodeList(_obj);
				if (list.Count > 0)
				{
					foreach (TimeAttendanceDAL.View.Salary.Insurance item in list)
					{
						val.AddReturnedValue((ReturnedState)2, "آقا/خانم  " + item.FullName + " برای ماه مورد نظر، کارکرد موظفی ندارد", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Insurance\\InsuranceValidator.cs", 52);
					}
				}
				List<TimeAttendanceDAL.View.Salary.Insurance> list2 = EmptyInsuranceNoList(_obj);
				if (list2.Count > 0)
				{
					foreach (TimeAttendanceDAL.View.Salary.Insurance item2 in list2)
					{
						val.AddReturnedValue((ReturnedState)2, "شماره بیمه " + item2.FullName + " در قرارداد وی ثبت نشده است", "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Insurance\\InsuranceValidator.cs", 56);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Validate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Insurance\\InsuranceValidator.cs", 61);
		}
		return val;
	}
}
