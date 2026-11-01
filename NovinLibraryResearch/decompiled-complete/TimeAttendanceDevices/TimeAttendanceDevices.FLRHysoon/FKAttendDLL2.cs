using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using PacketParser.Entities;

namespace TimeAttendanceDevices.FLRHysoon;

public class FKAttendDLL2
{
	public static int nCommHandleIndex;

	public const short MAX_BELLCOUNT_DAY = 24;

	public const short MAX_PASSCTRLGROUP_COUNT = 50;

	public const short MAX_PASSCTRL_COUNT = 7;

	public const short MAX_USERPASSINFO_COUNT = 3;

	public const short MAX_GROUPPASSINFO_COUNT = 3;

	public const short MAX_GROUPMATCHINFO_COUNT = 10;

	public const short NEWS_EXTEND = 2;

	public const short NEWS_STANDARD = 1;

	public const short SIZE_BELLINFO = 72;

	public const short SIZE_PASSTIME = 4;

	public const short SIZE_PASSCRLTIME = 28;

	public const short SIZE_USERPASSINFO = 3;

	public const short SIZE_GROUPPASSINFO = 3;

	public const short SIZE_GROUPMATCHINFO = 20;

	public const short MAX_SHIFT_COUNT = 24;

	public const short MAX_POST_COUNT = 16;

	public const short NAME_BYTE_COUNT = 128;

	public const short MAX_DAY_IN_MONTH = 31;

	public const short SIZE_POST_SHIFT_INFO_V2 = 2476;

	public const short VER2_POST_SHIFT_INFO = 2;

	public const short SIZE_USER_INFO_V2 = 184;

	public const short VER2_USER_INFO = 2;

	public const int SIZE_EXT_CMD_CODE = 56;

	public const int TIME_SLOT_COUNT = 6;

	public const int TIME_ZONE_COUNT = 255;

	public const int SIZE_TIME_ZONE_STRUCT = 32;

	public const int SIZE_USER_WEEK_PASS_TIME_STRUCT = 16;

	public const string msErrorNoDevice = "No Device";

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ConnectComm(int anMachineNo, int anComPort, int anBaudRate, string astrTelNumber, int anWaitDialTime, int anLicense, int anComTimeOut);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ConnectNet(int anMachineNo, string astrIpAddress, int anNetPort, int anTimeOut, int anProtocolType, int anNetPassword, int anLicense);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ConnectUSB(int anMachineNo, int anLicense);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern void FK_DisConnect(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetLastError(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_EnableDevice(int anHandleIndex, byte anEnableFlag);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern void FK_PowerOnAllDevice(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_PowerOffDevice(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetDeviceStatus(int anHandleIndex, int anStatusIndex, ref int apnValue);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetDeviceTime(int anHandleIndex, ref DateTime apnDateTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetDeviceTime(int anHandleIndex, DateTime anDateTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetDeviceInfo(int anHandleIndex, int anInfoIndex, ref int apnValue);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetDeviceInfo(int anHandleIndex, int anInfoIndex, int anValue);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetProductData(int anHandleIndex, int anDataIndex, [MarshalAs(UnmanagedType.LPStr)] ref string apstrValue);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_LoadSuperLogData(int anHandleIndex, int anReadMark);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBLoadSuperLogDataFromFile(int anHandleIndex, string astrFilePath);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetSuperLogData(int anHandleIndex, ref int apnSEnrollNumber, ref int apnGEnrollNumber, ref int apnMachinePrivilege, ref int apnBackupNumber, ref DateTime apnDateTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_EmptySuperLogData(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_LoadGeneralLogData(int anHandleIndex, int anReadMark);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBLoadGeneralLogDataFromFile(int anHandleIndex, string astrFilePath);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetGeneralLogData(int anHandleIndex, ref int apnEnrollNumber, ref int apnVerifyMode, ref int apnInOutMode, ref DateTime apnDateTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_EmptyGeneralLogData(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_EnableUser(int anHandleIndex, int anEnrollNumber, int anBackupNumber, int anEnableFlag);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ModifyPrivilege(int anHandleIndex, int anEnrollNumber, int anBackupNumber, int anMachinePrivilege);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_BenumbAllManager(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ReadAllUserID(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetAllUserID(int anHandleIndex, ref int apnEnrollNumber, ref int apnBackupNumber, ref int apnMachinePrivilege, ref int apnEnable);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetEnrollData(int anHandleIndex, int anEnrollNumber, int anBackupNumber, ref int apnMachinePrivilege, byte[] apnEnrollData, ref int apnPassword);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern long GetEnrollDataWithString(long anEnrollNumber, long anBackupNumber, ref long apnMachinePrivilege, char[] apstrEnrollData);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_PutEnrollData(int anHandleIndex, int anEnrollNumber, int anBackupNumber, int anMachinePrivilege, byte[] apnEnrollData, int anPassword);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SaveEnrollData(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_DeleteEnrollData(int anHandleIndex, int anEnrollNumber, int anBackupNumber);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_EmptyEnrollData(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ClearKeeperData(int anHandleIndex);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBReadAllEnrollDataFromFile(int anHandleIndex, string astrFilePath);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetUSBModel(int anHandleIndex, int anModel);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetUDiskFileFKModel(int anHandleIndex, string strFKModel);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBReadAllEnrollDataCount(int anHandleIndex, int apnValue);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBGetOneEnrollData(int anHandleIndex, ref int apnEnrollNumber, ref int apnBackupNumber, ref int apnMachinePrivilege, byte[] apnEnrollData, ref int apnPassword, ref int apnEnableFlag, [MarshalAs(UnmanagedType.LPStr)] ref string apstrEnrollName);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBSetOneEnrollData(int anHandleIndex, int anEnrollNumber, int anBackupNumber, int anMachinePrivilege, byte[] apnEnrollData, int anPassword, int anEnableFlag, string astrEnrollName);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBWriteAllEnrollDataToFile(int anHandleIndex, string astrFilePath);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBReadAllEnrollDataFromFile_Color(int anHandleIndex, string astrFilePath);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBWriteAllEnrollDataToFile_Color(int anHandleIndex, string astrFilePath, long anNewsKind);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBGetOneEnrollData_Color(int anHandleIndex, ref int apnEnrollNumber, ref int apnBackupNumber, ref int apnMachinePrivilege, byte[] apnEnrollData, ref int apnPassWord, ref int apnEnableFlag, [MarshalAs(UnmanagedType.LPStr)] ref string apstrEnrollName, int anNewsKind);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_USBSetOneEnrollData_Color(int anHandleIndex, int anEnrollNumber, int anBackupNumber, int anMachinePrivilege, byte[] apnEnrollData, int anPassWord, int anEnableFlag, string astrEnrollName, int anNewsKind);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetUserName(int anHandleIndex, int anEnrollNumber, [MarshalAs(UnmanagedType.LPStr)] ref string apstrUserName);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetUserName(int anHandleIndex, int anEnrollNumber, string astrUserName);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetNewsMessage(int anHandleIndex, int anNewsId, [MarshalAs(UnmanagedType.LPStr)] ref string apstrNews);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetNewsMessage(int anHandleIndex, int anNewsId, string astrNews);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetUserNewsID(int anHandleIndex, int anEnrollNumber, ref int apnNewsId);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetUserNewsID(int anHandleIndex, int anEnrollNumber, int anNewsId);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetBellTime(int anHandleIndex, ref int apnBellCount, byte[] apnBellInfo);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetBellTime(int anHandleIndex, int anBellCount, byte[] apnBellInfo);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetDoorStatus(int anHandleIndex, ref int apnStatusVal);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetDoorStatus(int anHandleIndex, int anStatusVal);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetPassTime(int anHandleIndex, int anPassTimeID, byte[] apnPassTime, int anPassTimeSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetPassTime(int anHandleIndex, int anPassTimeID, byte[] apnPassTime, int anPassTimeSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetUserPassTime(int anHandleIndex, int anEnrollNumber, ref int apnGroupID, byte[] apnUserPassTimeInfo, int anUserPassTimeInfoSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetUserPassTime(int anHandleIndex, int anEnrollNumber, int anGroupID, byte[] apnUserPassTimeInfo, int anUserPassTimeInfoSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetGroupPassTime(int anHandleIndex, int anGroupID, byte[] apnGroupPassTimeInfo, int anGroupPassTimeInfoSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetGroupPassTime(int anHandleIndex, int anGroupID, byte[] apnGroupPassTimeInfo, int anGroupPassTimeInfoSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetGroupMatch(int nHandleIndex, byte[] apnGroupMatchInfo, int anGroupMatchInfoSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetGroupMatch(int nHandleIndex, byte[] apnGroupMatchInfo, int anGroupMatchInfoSize);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetAdjustInfo(int nHandleIndex, ref int apnAdjustedState, ref int apnAdjustedMonth, ref int apnAdjustedDay, ref int apnAdjustedHour, ref int apnAdjustedMinute, ref int apnRestoredState, ref int apnRestoredMonth, ref int apnRestoredDay, ref int apnRestoredHour, ref int apnRestoredMinte);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetAdjustInfo(int anHandleIndex, int anAdjustedState, int anAdjustedMonth, int anAdjustedDay, int anAdjustedHour, int anAdjustedMinute, int anRestoredState, int anRestoredMonth, int anRestoredDay, int anRestoredHour, int anRestoredMinte);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetAccessTime(int nHandleIndex, int anEnrollNumber, ref int apnAccessTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetAccessTime(int nHandleIndex, int anEnrollNumber, int anAccessTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetFontName(int nHandleIndex, string aStrFontName, int anFontType);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetRealTimeInfo(int anHandleIndex, byte[] apnRealTimeInfo);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetRealTimeInfo(int anHandleIndex, byte[] apnRealTimeInfo);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetServerNetInfo(int anHandleIndex, [MarshalAs(UnmanagedType.LPStr)] ref string apstrServerIPAddress, ref int apnServerPort, ref int apnServerRequest);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetServerNetInfo(int nHandleIndex, string astrServerIPAddress, int anServerPort, int anServerRequest);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetPostShiftInfo(int nHandleIndex, byte[] abytPostShiftInfo, ref int apPostShiftInfoLen);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetPostShiftInfo(int nHandleIndex, byte[] abytPostShiftInfo, int anPostShiftInfoLen);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetUserInfoEx(int nHandleIndex, int anEnrollNumber, byte[] abytUserInfo, ref int apUserInfoLen);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetUserInfoEx(int nHandleIndex, int anEnrollNumber, byte[] abytUserInfo, int anUserInfoLen);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetEnrollPhoto(int nHandleIndex, int anEnrollNumber, byte[] abytPhotoImage, ref int anPhotoLength);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_SetEnrollPhoto(int nHandleIndex, int anEnrollNumber, byte[] abytPhotoImage, int anPhotoLength);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_DeleteEnrollPhoto(int nHandleIndex, int anEnrollNumber);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_GetLogPhoto(int nHandleIndex, int anEnrollNumber, int anYear, int anMonth, int anDay, int anHour, int anMinute, int anSec, byte[] abytPhotoImage, ref int anPhotoLength);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_IsSupportedEnrollData(int nHandleIndex, int anBackupNumber, ref int apnSupportFlag);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_HS_GetTimeZone(int anHandleIndex, byte[] abytOneTimeZone);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_HS_SetTimeZone(int anHandleIndex, byte[] abytOneTimeZone);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_HS_GetUserWeekPassTime(int anHandleIndex, byte[] abytUserWeekPassTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_HS_SetUserWeekPassTime(int anHandleIndex, byte[] abytUserWeekPassTime);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ExecCommand(int nHandleIndex, string astrCommand, [MarshalAs(UnmanagedType.LPStr)] ref string astrResult);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern int FK_ExtCommand(int anHandleIndex, byte[] abytCmdStruct);

	[DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
	public static extern long SetDeviceTime(DateTime anDateTime);

	public static string ReturnResultPrint(long anResultCode)
	{
		long num = anResultCode - -15;
		if ((ulong)num <= 16uL)
		{
			switch (num)
			{
			case 16L:
				return "Successful!";
			case 15L:
				return "No support";
			case 14L:
				return "Unknown error";
			case 13L:
				return "No Open Comm";
			case 12L:
				return "Write Error";
			case 11L:
				return "Read Error";
			case 10L:
				return "Parameter Error";
			case 9L:
				return "execution of command failed";
			case 8L:
				return "End of data";
			case 7L:
				return "Nonexistence data";
			case 6L:
				return "Memory Allocating Error";
			case 5L:
				return "License Error";
			case 4L:
				return "full enrolldata & can`t put enrolldata";
			case 3L:
				return "this Id is already  existed.";
			case 1L:
				return "full manager & can`t put manager.";
			case 0L:
				return "mistake fp data version.";
			}
		}
		return "Unknown error";
	}

	public static object ConvertByteArrayToStructure(string contextGuid, byte[] aByteArray, Type aType)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int num = Marshal.SizeOf(aType);
			if (aByteArray.Length < num)
			{
				return null;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(aByteArray, 0, intPtr, num);
			object? result = Marshal.PtrToStructure(intPtr, aType);
			Marshal.FreeHGlobal(intPtr);
			return result;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Fail to convert byte array to structure");
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConvertByteArrayToStructure", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\FKAttendDLL2.cs", 669);
			return null;
		}
	}

	public static void ConvertStructureToByteArray(string contextGuid, object aStruct, byte[] aByteArray)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			IntPtr zero = IntPtr.Zero;
			int num = Marshal.SizeOf(aStruct);
			if (aByteArray.Length >= num)
			{
				zero = Marshal.AllocHGlobal(num);
				Marshal.StructureToPtr(aStruct, zero, fDeleteOld: false);
				Marshal.Copy(zero, aByteArray, 0, num);
				Marshal.FreeHGlobal(zero);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Fail to convert structure to byte array");
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConvertStructureToByteArray", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\FKAttendDLL2.cs", 691);
		}
	}

	public static void StringToByteArrayUtf16(string aString, byte[] aByteArray)
	{
		Array.Clear(aByteArray, 0, aByteArray.Length);
		byte[] bytes = Encoding.GetEncoding("utf-16").GetBytes(aString);
		int num = bytes.Length;
		if (num > aByteArray.Length - 2)
		{
			num = aByteArray.Length - 2;
		}
		Array.Copy(bytes, aByteArray, num);
	}

	public static string ByteArrayUtf16ToString(string contextGuid, byte[] aByteArray)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		try
		{
			text = Encoding.GetEncoding("utf-16").GetString(aByteArray, 0, aByteArray.Length);
			text = text.TrimEnd(new char[1]);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return text;
	}

	public static string ByteArrayUtf8ToString(string contextGuid, byte[] aByteArray)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		try
		{
			text = Encoding.GetEncoding("utf-8").GetString(aByteArray, 0, aByteArray.Length);
			text = text.TrimEnd(new char[1]);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return text;
	}

	public static bool IsNumeric(string str)
	{
		try
		{
			Convert.ToInt32(str);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static string StringToUTF16HexString(string astrVal)
	{
		return HexEncoding.ToString(Encoding.GetEncoding("utf-16").GetBytes(astrVal));
	}

	public static string UTF16HexStringToString(string astrUtf16Hex)
	{
		byte[] array = null;
		int discarded = 0;
		array = HexEncoding.GetBytes(astrUtf16Hex, out discarded);
		return Encoding.GetEncoding("utf-16").GetString(array);
	}

	public static int GetInt(string asVal)
	{
		try
		{
			return Convert.ToInt32(asVal);
		}
		catch
		{
			return 0;
		}
	}
}
