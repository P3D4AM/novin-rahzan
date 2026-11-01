using System;
using System.Collections.Concurrent;
using LK;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace TimeAttendanceDAL.Utility.Settings;

public class LkOperations : ICheckActivation, IHasContext
{
	private static ConcurrentDictionary<string, LkOperations> _dic = new ConcurrentDictionary<string, LkOperations>();

	public static bool _isFreeVersion;

	public static bool _isTrialVersion;

	public string ContextGuid { get; set; }

	public LkOperations(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static LkOperations GetLkOperations(string contextGuid)
	{
		_dic.TryGetValue(contextGuid, out var value);
		if (value == null)
		{
			value = new LkOperations(contextGuid);
			_dic.TryAdd(contextGuid, value);
		}
		return value;
	}

	public ReturnedSaveFuncInfoWithValue<HozoorPackage> GetHozorType()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		ReturnedSaveFuncInfoWithValue<HozoorPackage> val = new ReturnedSaveFuncInfoWithValue<HozoorPackage>
		{
			value = (HozoorPackage)0
		};
		try
		{
			if (_isFreeVersion)
			{
				return val;
			}
			ReturnedSaveFuncInfoWithValue<HozoorPackage> hozoorType = CheckActivation.GetHozoorType((Package)2);
			val.AddReturnedValue((ReturnedSaveFuncInfo)(object)hozoorType, (ReturnedState)0, "GetHozorType", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 53);
			val.value = hozoorType.value;
		}
		catch (Exception ex)
		{
			val.value = (HozoorPackage)0;
			val.AddReturnedValue(ex, "", "GetHozorType", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 59);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetHozorType", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 60);
		}
		return val;
	}

	public ReturnedSaveFuncInfoWithValue<Level_Hozor> CheckActivation_Hozor()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		ReturnedSaveFuncInfoWithValue<Level_Hozor> val = new ReturnedSaveFuncInfoWithValue<Level_Hozor>
		{
			value = (Level_Hozor)1
		};
		if (_isFreeVersion)
		{
			return val;
		}
		try
		{
			ReturnedSaveFuncInfoWithValue<Level_Hozor> val2 = CheckActivation.CheckActivation_Hozor(ContextGuid, MainFormOwner.owner, true, ref _isTrialVersion);
			val.value = val2.value;
			val.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 76);
		}
		catch (Exception ex)
		{
			Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, ex.Message, "", 3);
		}
		return val;
	}

	public string LkSerial()
	{
		if (_isFreeVersion)
		{
			return "1234567890";
		}
		try
		{
			WebErrorLog.ErrorLogInstance.LKSerial = LK.GetSerialNumber(MainFormOwner.owner, (Package)2);
			return LK.GetSerialNumber(MainFormOwner.owner, (Package)2).ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LkSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 95);
		}
		return "";
	}

	public ReturnedSaveFuncInfoWithValue<string> SoftwareSerial()
	{
		ReturnedSaveFuncInfoWithValue<string> val = new ReturnedSaveFuncInfoWithValue<string>();
		if (_isFreeVersion)
		{
			return val;
		}
		try
		{
			ReturnedSaveFuncInfoWithValue<string> softwareSerial_Hozor = CheckActivation.GetSoftwareSerial_Hozor();
			val.AddReturnedValue((ReturnedSaveFuncInfo)(object)softwareSerial_Hozor, (ReturnedState)0, "SoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 107);
			val.value = softwareSerial_Hozor.value;
			return val;
		}
		catch (Exception ex)
		{
			val.value = "";
			val.AddReturnedValue(ex, "", "SoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 114);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SoftwareSerial", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Settings\\LkOperations.cs", 115);
			return val;
		}
	}
}
