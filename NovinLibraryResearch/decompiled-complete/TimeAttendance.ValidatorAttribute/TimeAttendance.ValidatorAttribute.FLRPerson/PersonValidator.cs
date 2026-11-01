using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendance.ValidatorAttribute.FLRPerson;

public class PersonValidator : IHasContext
{
	private Person _obj;

	public string ContextGuid { get; set; }

	private async Task<bool> BeAValidMoeinTafsilAsync(string postcode, CancellationToken token)
	{
		try
		{
			string[] array = postcode.Split(new char[1] { ':' });
			return await Person.GetCountByCodeMAsync(ContextGuid, int.Parse(array[0]), array[1], array[2], token) <= 0;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public PersonValidator(string contextGuid, Person pObj)
	{
		ContextGuid = contextGuid;
		_obj = pObj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			Person obj = _obj;
			if (((obj != null) ? ((PersonBase)obj).Family : null) != null && string.IsNullOrEmpty(((PersonBase)_obj).Family))
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "وارد کردن نام خانوادگی الزامی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRPerson\\PersonValidator.cs", 44);
			}
			Person obj2 = _obj;
			int num;
			if (obj2 == null)
			{
				num = 0;
			}
			else
			{
				_ = ((PersonBase)obj2).Id;
				num = 1;
			}
			bool flag = (byte)num != 0;
			if (flag)
			{
				flag = !(await BeAValidMoeinTafsilAsync(((PersonBase)_obj).Id + ":" + ((PersonBase)_obj).CodeMoein + ":" + ((PersonBase)_obj).CodeTafsil, token));
			}
			if (flag)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "کارمند دیگری با این کد معین و تفصیل وجود دارد", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRPerson\\PersonValidator.cs", 46);
			}
			Person obj3 = _obj;
			int num2;
			if (obj3 == null)
			{
				num2 = 0;
			}
			else
			{
				_ = ((PersonBase)obj3).CodeMoein;
				num2 = 1;
			}
			if (num2 != 0 && ((PersonBase)_obj).CodeMoein == 0L)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "کد معین نامعتبر است", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRPerson\\PersonValidator.cs", 49);
			}
			Person obj4 = _obj;
			int num3;
			if (obj4 == null)
			{
				num3 = 0;
			}
			else
			{
				_ = ((PersonBase)obj4).CodeTafsil;
				num3 = 1;
			}
			if (num3 != 0 && ((PersonBase)_obj).CodeTafsil == 0L)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "کد تفضیل نامعتبر است", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRPerson\\PersonValidator.cs", 52);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRPerson\\PersonValidator.cs", 56);
		}
		return errorsInfo;
	}
}
