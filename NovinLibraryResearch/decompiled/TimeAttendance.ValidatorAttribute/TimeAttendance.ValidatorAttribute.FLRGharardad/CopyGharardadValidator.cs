using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.View.Gharardad;

namespace TimeAttendance.ValidatorAttribute.FLRGharardad;

public class CopyGharardadValidator : IHasContext
{
	public ClsCopyGharardad _obj;

	public string ContextGuid { get; set; }

	public CopyGharardadValidator(string contextGuid, ClsCopyGharardad obj)
	{
		ContextGuid = contextGuid;
		_obj = obj;
	}

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (_obj != null)
			{
				if (string.IsNullOrEmpty(_obj.No.ToString()))
				{
					errorsInfo.AddReturnedValue((ReturnedState)2, "[لطفا شماره شروع قرارداد ها را وارد کنید]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\CopyGharardadValidator.cs", 32);
				}
				else
				{
					int no = _obj.No;
					if (no < await GharardadNew.NextGharardadAsync(ContextGuid, token))
					{
						errorsInfo.AddReturnedValue((ReturnedState)2, "[شماره شروع قرارداد ها از آخرین شماره قرارداد کوچکتر است]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\CopyGharardadValidator.cs", 34);
					}
				}
				string text = CheckGharardadsDate();
				if (text != "")
				{
					errorsInfo.AddReturnedValue((ReturnedState)4, "[تاریخ پایان قرارداد(های) ]" + text + " [از تاریخ شروع قرارداد جهت کپی بزرگتر است.لذا ابتدا آن قراردادها را ببندید]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\CopyGharardadValidator.cs", 41);
				}
				if (!IsValidGharardadLength())
				{
					errorsInfo.AddReturnedValue((ReturnedState)4, "[تاریخ پایان قراردادها حداقل باید یک ماه از تاریخ شروع بزرگتر باشد]", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\CopyGharardadValidator.cs", 46);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\FLRGharardad\\CopyGharardadValidator.cs", 53);
		}
		return errorsInfo;
	}

	private bool BeValidGharardad(ClsCopyGharardad pgh)
	{
		bool result = true;
		string text = "";
		foreach (GharardadNew lstGharardad in pgh.LstGharardads)
		{
			if (lstGharardad.DateFinishM >= pgh.DateStartM)
			{
				text = text + lstGharardad.No + ";";
			}
		}
		if (text != "")
		{
			result = false;
		}
		return result;
	}

	private bool BeVallidBetweenDate(ClsCopyGharardad pgh)
	{
		DateTime dateStartM = pgh.DateStartM;
		return !(pgh.DateFinishM < dateStartM.AddDays(29.0));
	}

	private string CheckGharardadsDate()
	{
		string text = "";
		foreach (GharardadNew lstGharardad in _obj.LstGharardads)
		{
			if (lstGharardad.DateFinishM >= _obj.DateStartM)
			{
				text = text + lstGharardad.No + ",";
			}
		}
		return text.TrimEnd(new char[1] { ',' });
	}

	private bool IsValidGharardadLength()
	{
		DateTime dateStartM = _obj.DateStartM;
		return !(_obj.DateFinishM < dateStartM.AddDays(29.0));
	}
}
