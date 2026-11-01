using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ClientHub.AccountingHubs;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Settings;

namespace TimeAttendance.ValidatorAttribute.Payamak;

public class PayamakValidator : IHasContext
{
	private TimeAttendanceDAL.Settings.Payamak _obj;

	private bool _checkPayamakWeb;

	private PayamakClientHubInstances _payamakService;

	public string ContextGuid { get; private set; }

	public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (!CheckContentManageEnter(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "بااینکه گزینه پیامک ورود برای مدیران فعال است. ولی متن آن خالی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 39);
			}
			if (!CheckContentManageExit(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "بااینکه گزینه پیامک خروج برای مدیران فعال است. ولی متن آن خالی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 42);
			}
			if (!CheckContentPersonelEnter(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "بااینکه گزینه پیامک ورود کارکنان / والدین فعال است. ولی متن آن خالی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 45);
			}
			if (!CheckContentPersonelExit(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "بااینکه گزینه پیامک خروج برای مدیران فعال است. ولی متن آن خالی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 48);
			}
			if (!CheckContent(_obj))
			{
				val.AddReturnedValue((ReturnedState)2, "متن پیامک برای تمامی گزینه ها خالی است.", "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 51);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 66);
		}
		return val;
	}

	private async Task<bool> CheckConnectionStringAsync(string pconnstr, CancellationToken token = default(CancellationToken))
	{
		bool result = true;
		try
		{
			if (string.IsNullOrEmpty(pconnstr))
			{
				return false;
			}
			SqlConnection cn = new SqlConnection(pconnstr);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				((DbConnection)(object)cn).Close();
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch
		{
			result = false;
		}
		return result;
	}

	private bool CheckContent(TimeAttendanceDAL.Settings.Payamak payamak)
	{
		try
		{
			return !string.IsNullOrEmpty(((AttDepartmentAdminSms)payamak).PayamakManageVorod.Trim()) || !string.IsNullOrEmpty(((AttDepartmentAdminSms)payamak).PayamakManageKhoroj.Trim()) || !string.IsNullOrEmpty(payamak.PayamakValedVorod.Trim()) || !string.IsNullOrEmpty(payamak.PayamakValedKhoroj.Trim()) || !string.IsNullOrEmpty(((AttDepartmentAdminSms)payamak).SmsAbsentManager.Trim()) || !string.IsNullOrEmpty(payamak.SmsAbsentPerson.Trim());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckContent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 103);
		}
		return false;
	}

	private bool CheckContentManageEnter(TimeAttendanceDAL.Settings.Payamak payamak)
	{
		bool result = false;
		try
		{
			result = !((AttDepartmentAdminSms)payamak).SendVorodManage || !string.IsNullOrEmpty(((AttDepartmentAdminSms)payamak).PayamakManageVorod);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckContentManageEnter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 117);
		}
		return result;
	}

	private bool CheckContentManageExit(TimeAttendanceDAL.Settings.Payamak payamak)
	{
		bool result = false;
		try
		{
			result = !((AttDepartmentAdminSms)payamak).SendKhorojManage || !string.IsNullOrEmpty(((AttDepartmentAdminSms)payamak).PayamakManageKhoroj);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckContentManageExit", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 130);
		}
		return result;
	}

	private bool CheckContentPersonelEnter(TimeAttendanceDAL.Settings.Payamak payamak)
	{
		bool result = false;
		try
		{
			result = !payamak.SendVorodValed || !string.IsNullOrEmpty(payamak.PayamakValedVorod);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckContentPersonelEnter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 144);
		}
		return result;
	}

	private bool CheckContentPersonelExit(TimeAttendanceDAL.Settings.Payamak payamak)
	{
		bool result = false;
		try
		{
			result = !payamak.SendKhorojValed || !string.IsNullOrEmpty(payamak.PayamakValedKhoroj);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckContentPersonelExit", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendance.ValidatorAttribute\\Payamak\\PayamakValidator.cs", 158);
		}
		return result;
	}

	public PayamakValidator(string contextGuid, TimeAttendanceDAL.Settings.Payamak pObj, bool checkPayamakWeb)
	{
		ContextGuid = contextGuid;
		_payamakService = PayamakClientHubInstances.GetInstance(contextGuid);
		_obj = pObj;
		_checkPayamakWeb = checkPayamakWeb;
	}
}
