using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRSanad;

namespace TimeAttendance.ValidatorAttribute.Sanad;

internal class AttSanadValidator
{
	private AttSanad _obj;

	public AttSanadValidator(AttSanad pObj)
	{
		_obj = pObj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			List<ATT_SanadDet> obj = await (_obj?.GetLstSanadDetsAsync(token));
			if (obj.Count <= 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "هیچ داده ای برای ثبت وجود ندارد.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadValidator.cs", 25);
			}
			foreach (ATT_SanadDet item in obj)
			{
				AttSanadDetValidator attSanadDetValidator = new AttSanadDetValidator(item);
				errorsInfo.AddReturnedValue(attSanadDetValidator.Validate(), (ReturnedState)0, "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadValidator.cs", 30);
			}
			if (await (_obj?.GetSumDebitAsync(token)) != await (_obj?.GetSumCreditAsync(token)))
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "سند حسابداری تراز نیست لطفا ابتدا سند را تراز نمایید.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadValidator.cs", 33);
			}
			AttSanad obj2 = _obj;
			if (obj2 != null && obj2.No <= 0)
			{
				errorsInfo.AddReturnedValue((ReturnedState)2, "شماره سند باید بزرگتر از صفر باشد.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadValidator.cs", 35);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Sanad\\AttSanadValidator.cs", 40);
		}
		return errorsInfo;
	}
}
