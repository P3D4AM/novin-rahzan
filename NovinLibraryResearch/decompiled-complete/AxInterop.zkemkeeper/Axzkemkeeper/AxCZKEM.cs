using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using zkemkeeper;

namespace Axzkemkeeper;

[DefaultEvent("OnAttTransaction")]
[DesignTimeVisible(true)]
[Clsid("{00853a19-bd51-419b-9269-2dabe57eb61f}")]
public class AxCZKEM : AxHost
{
	private IZKEM ocx;

	private AxCZKEMEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DispId(1)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool ReadMark
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ReadMark", (ActiveXInvokeKind)1);
			}
			return ocx.ReadMark;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ReadMark", (ActiveXInvokeKind)2);
			}
			ocx.ReadMark = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(2)]
	public virtual int CommPort
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CommPort", (ActiveXInvokeKind)1);
			}
			return ocx.CommPort;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CommPort", (ActiveXInvokeKind)2);
			}
			ocx.CommPort = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(64)]
	public virtual int ConvertBIG5
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ConvertBIG5", (ActiveXInvokeKind)1);
			}
			return ocx.ConvertBIG5;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ConvertBIG5", (ActiveXInvokeKind)2);
			}
			ocx.ConvertBIG5 = value;
		}
	}

	[DispId(76)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int BASE64
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BASE64", (ActiveXInvokeKind)1);
			}
			return ocx.BASE64;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BASE64", (ActiveXInvokeKind)2);
			}
			ocx.BASE64 = value;
		}
	}

	[DispId(78)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual uint PIN2
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PIN2", (ActiveXInvokeKind)1);
			}
			return ocx.PIN2;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PIN2", (ActiveXInvokeKind)2);
			}
			ocx.PIN2 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(79)]
	public virtual int AccGroup
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AccGroup", (ActiveXInvokeKind)1);
			}
			return ocx.AccGroup;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AccGroup", (ActiveXInvokeKind)2);
			}
			ocx.AccGroup = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(102)]
	public virtual int PINWidth
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PINWidth", (ActiveXInvokeKind)1);
			}
			return ocx.PINWidth;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(123)]
	public virtual int MachineNumber
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MachineNumber", (ActiveXInvokeKind)1);
			}
			return ocx.MachineNumber;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MachineNumber", (ActiveXInvokeKind)2);
			}
			ocx.MachineNumber = value;
		}
	}

	[DispId(191)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int SSRPin
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("SSRPin", (ActiveXInvokeKind)1);
			}
			return ocx.SSRPin;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(241)]
	public virtual int PullMode
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PullMode", (ActiveXInvokeKind)1);
			}
			return ocx.PullMode;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PullMode", (ActiveXInvokeKind)2);
			}
			ocx.PullMode = value;
		}
	}

	public event _IZKEMEvents_OnAttTransactionEventHandler OnAttTransaction;

	public event _IZKEMEvents_OnKeyPressEventHandler OnKeyPress;

	public event _IZKEMEvents_OnEnrollFingerEventHandler OnEnrollFinger;

	public event _IZKEMEvents_OnNewUserEventHandler OnNewUser;

	public event _IZKEMEvents_OnEMDataEventHandler OnEMData;

	public event EventHandler OnConnected;

	public event EventHandler OnDisConnected;

	public event EventHandler OnFinger;

	public event _IZKEMEvents_OnVerifyEventHandler OnVerify;

	public event _IZKEMEvents_OnFingerFeatureEventHandler OnFingerFeature;

	public event _IZKEMEvents_OnHIDNumEventHandler OnHIDNum;

	public event _IZKEMEvents_OnDoorEventHandler OnDoor;

	public event _IZKEMEvents_OnAlarmEventHandler OnAlarm;

	public event _IZKEMEvents_OnWriteCardEventHandler OnWriteCard;

	public event _IZKEMEvents_OnEmptyCardEventHandler OnEmptyCard;

	public event _IZKEMEvents_OnDeleteTemplateEventHandler OnDeleteTemplate;

	public event _IZKEMEvents_OnAttTransactionExEventHandler OnAttTransactionEx;

	public event _IZKEMEvents_OnEnrollFingerExEventHandler OnEnrollFingerEx;

	public AxCZKEM()
		: base("00853a19-bd51-419b-9269-2dabe57eb61f")
	{
	}

	public virtual bool GetHIDEventCardNumAsStr(out string strHIDEventCardNum)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetHIDEventCardNumAsStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetHIDEventCardNumAsStr(out strHIDEventCardNum);
	}

	public virtual bool GetStrCardNumber(out string aCardNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetStrCardNumber", (ActiveXInvokeKind)0);
		}
		return ocx.GetStrCardNumber(out aCardNumber);
	}

	public virtual bool SetStrCardNumber(string aCardNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetStrCardNumber", (ActiveXInvokeKind)0);
		}
		return ocx.SetStrCardNumber(aCardNumber);
	}

	public virtual bool RegEvent(int dwMachineNumber, int eventMask)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("RegEvent", (ActiveXInvokeKind)0);
		}
		return ocx.RegEvent(dwMachineNumber, eventMask);
	}

	public virtual bool CancelBatchUpdate(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CancelBatchUpdate", (ActiveXInvokeKind)0);
		}
		return ocx.CancelBatchUpdate(dwMachineNumber);
	}

	public virtual bool SetSysOption(int dwMachineNumber, string option, string value)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetSysOption", (ActiveXInvokeKind)0);
		}
		return ocx.SetSysOption(dwMachineNumber, option, value);
	}

	public virtual bool Connect_Modem(int comPort, int machineNumber, int baudRate, string telephone)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Connect_Modem", (ActiveXInvokeKind)0);
		}
		return ocx.Connect_Modem(comPort, machineNumber, baudRate, telephone);
	}

	public virtual bool UseGroupTimeZone()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("UseGroupTimeZone", (ActiveXInvokeKind)0);
		}
		return ocx.UseGroupTimeZone();
	}

	public virtual bool SetHoliday(int dwMachineNumber, string holiday)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetHoliday", (ActiveXInvokeKind)0);
		}
		return ocx.SetHoliday(dwMachineNumber, holiday);
	}

	public virtual bool GetHoliday(int dwMachineNumber, ref string holiday)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetHoliday", (ActiveXInvokeKind)0);
		}
		return ocx.GetHoliday(dwMachineNumber, ref holiday);
	}

	public virtual bool SetDaylight(int dwMachineNumber, int support, string beginTime, string endTime)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDaylight", (ActiveXInvokeKind)0);
		}
		return ocx.SetDaylight(dwMachineNumber, support, beginTime, endTime);
	}

	public virtual bool GetDaylight(int dwMachineNumber, ref int support, ref string beginTime, ref string endTime)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDaylight", (ActiveXInvokeKind)0);
		}
		return ocx.GetDaylight(dwMachineNumber, ref support, ref beginTime, ref endTime);
	}

	public virtual bool SSR_SetUnLockGroup(int dwMachineNumber, int combNo, int group1, int group2, int group3, int group4, int group5)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetUnLockGroup", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetUnLockGroup(dwMachineNumber, combNo, group1, group2, group3, group4, group5);
	}

	public virtual bool SSR_GetUnLockGroup(int dwMachineNumber, int combNo, ref int group1, ref int group2, ref int group3, ref int group4, ref int group5)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetUnLockGroup", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetUnLockGroup(dwMachineNumber, combNo, ref group1, ref group2, ref group3, ref group4, ref group5);
	}

	public virtual bool SSR_SetGroupTZ(int dwMachineNumber, int groupNo, int tz1, int tz2, int tz3, int vaildHoliday, int verifyStyle)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetGroupTZ", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetGroupTZ(dwMachineNumber, groupNo, tz1, tz2, tz3, vaildHoliday, verifyStyle);
	}

	public virtual bool SSR_GetGroupTZ(int dwMachineNumber, int groupNo, ref int tz1, ref int tz2, ref int tz3, ref int vaildHoliday, ref int verifyStyle)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetGroupTZ", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetGroupTZ(dwMachineNumber, groupNo, ref tz1, ref tz2, ref tz3, ref vaildHoliday, ref verifyStyle);
	}

	public virtual bool SSR_GetHoliday(int dwMachineNumber, int holidayID, ref int beginMonth, ref int beginDay, ref int endMonth, ref int endDay, ref int timeZoneID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetHoliday", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetHoliday(dwMachineNumber, holidayID, ref beginMonth, ref beginDay, ref endMonth, ref endDay, ref timeZoneID);
	}

	public virtual bool SSR_SetHoliday(int dwMachineNumber, int holidayID, int beginMonth, int beginDay, int endMonth, int endDay, int timeZoneID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetHoliday", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetHoliday(dwMachineNumber, holidayID, beginMonth, beginDay, endMonth, endDay, timeZoneID);
	}

	public virtual bool GetPlatform(int dwMachineNumber, ref string platform)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPlatform", (ActiveXInvokeKind)0);
		}
		return ocx.GetPlatform(dwMachineNumber, ref platform);
	}

	public virtual bool SSR_SetUserSMS(int dwMachineNumber, string dwEnrollNumber, int sMSID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetUserSMS", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetUserSMS(dwMachineNumber, dwEnrollNumber, sMSID);
	}

	public virtual bool SSR_DeleteUserSMS(int dwMachineNumber, string dwEnrollNumber, int sMSID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_DeleteUserSMS", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_DeleteUserSMS(dwMachineNumber, dwEnrollNumber, sMSID);
	}

	public virtual bool IsTFTMachine(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("IsTFTMachine", (ActiveXInvokeKind)0);
		}
		return ocx.IsTFTMachine(dwMachineNumber);
	}

	public virtual bool SSR_EnableUser(int dwMachineNumber, string dwEnrollNumber, bool bFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_EnableUser", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_EnableUser(dwMachineNumber, dwEnrollNumber, bFlag);
	}

	public virtual bool SendCMDMsg(int dwMachineNumber, int param1, int param2)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SendCMDMsg", (ActiveXInvokeKind)0);
		}
		return ocx.SendCMDMsg(dwMachineNumber, param1, param2);
	}

	public virtual bool SendFile(int dwMachineNumber, string fileName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SendFile", (ActiveXInvokeKind)0);
		}
		return ocx.SendFile(dwMachineNumber, fileName);
	}

	public virtual bool SetLanguageByID(int dwMachineNumber, int languageID, string language)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetLanguageByID", (ActiveXInvokeKind)0);
		}
		return ocx.SetLanguageByID(dwMachineNumber, languageID, language);
	}

	public virtual bool ReadFile(int dwMachineNumber, string fileName, string filePath)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadFile", (ActiveXInvokeKind)0);
		}
		return ocx.ReadFile(dwMachineNumber, fileName, filePath);
	}

	public virtual bool SetLastCount(int count)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetLastCount", (ActiveXInvokeKind)0);
		}
		return ocx.SetLastCount(count);
	}

	public virtual bool SetCustomizeAttState(int dwMachineNumber, int stateID, int newState)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetCustomizeAttState", (ActiveXInvokeKind)0);
		}
		return ocx.SetCustomizeAttState(dwMachineNumber, stateID, newState);
	}

	public virtual bool DelCustomizeAttState(int dwMachineNumber, int stateID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DelCustomizeAttState", (ActiveXInvokeKind)0);
		}
		return ocx.DelCustomizeAttState(dwMachineNumber, stateID);
	}

	public virtual bool EnableCustomizeAttState(int dwMachineNumber, int stateID, int enable)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EnableCustomizeAttState", (ActiveXInvokeKind)0);
		}
		return ocx.EnableCustomizeAttState(dwMachineNumber, stateID, enable);
	}

	public virtual bool SetCustomizeVoice(int dwMachineNumber, int voiceID, string fileName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetCustomizeVoice", (ActiveXInvokeKind)0);
		}
		return ocx.SetCustomizeVoice(dwMachineNumber, voiceID, fileName);
	}

	public virtual bool DelCustomizeVoice(int dwMachineNumber, int voiceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DelCustomizeVoice", (ActiveXInvokeKind)0);
		}
		return ocx.DelCustomizeVoice(dwMachineNumber, voiceID);
	}

	public virtual bool EnableCustomizeVoice(int dwMachineNumber, int voiceID, int enable)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EnableCustomizeVoice", (ActiveXInvokeKind)0);
		}
		return ocx.EnableCustomizeVoice(dwMachineNumber, voiceID, enable);
	}

	public virtual bool StartEnrollEx(string userID, int fingerID, int flag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("StartEnrollEx", (ActiveXInvokeKind)0);
		}
		return ocx.StartEnrollEx(userID, fingerID, flag);
	}

	public virtual bool SSR_SetUserTmpExt(int dwMachineNumber, int isDeleted, string dwEnrollNumber, int dwFingerIndex, ref byte tmpData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetUserTmpExt", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetUserTmpExt(dwMachineNumber, isDeleted, dwEnrollNumber, dwFingerIndex, ref tmpData);
	}

	public virtual bool SSR_DelUserTmpExt(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_DelUserTmpExt", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_DelUserTmpExt(dwMachineNumber, dwEnrollNumber, dwFingerIndex);
	}

	public virtual bool SSR_DeleteEnrollDataExt(int dwMachineNumber, string dwEnrollNumber, int dwBackupNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_DeleteEnrollDataExt", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_DeleteEnrollDataExt(dwMachineNumber, dwEnrollNumber, dwBackupNumber);
	}

	public virtual bool SSR_GetWorkCode(int aWorkCode, out string name)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetWorkCode(aWorkCode, out name);
	}

	public virtual bool SSR_SetWorkCode(int aWorkCode, string name)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetWorkCode(aWorkCode, name);
	}

	public virtual bool SSR_DeleteWorkCode(int pIN)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_DeleteWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_DeleteWorkCode(pIN);
	}

	public virtual bool SSR_ClearWorkCode()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_ClearWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_ClearWorkCode();
	}

	public virtual bool SSR_GetSuperLogData(int machineNumber, out int number, out string admin, out string user, out int manipulation, out string time, out int params1, out int params2, out int params3)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetSuperLogData", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetSuperLogData(machineNumber, out number, out admin, out user, out manipulation, out time, out params1, out params2, out params3);
	}

	public virtual bool SSR_SetShortkey(int shortKeyID, int shortKeyFun, int stateCode, string stateName, int stateAutoChange, string stateAutoChangeTime)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetShortkey", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetShortkey(shortKeyID, shortKeyFun, stateCode, stateName, stateAutoChange, stateAutoChangeTime);
	}

	public virtual bool SSR_GetShortkey(int shortKeyID, ref int shortKeyFun, ref int stateCode, ref string stateName, ref int autoChange, ref string autoChangeTime)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetShortkey", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetShortkey(shortKeyID, ref shortKeyFun, ref stateCode, ref stateName, ref autoChange, ref autoChangeTime);
	}

	public virtual bool Connect_USB(int machineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Connect_USB", (ActiveXInvokeKind)0);
		}
		return ocx.Connect_USB(machineNumber);
	}

	public virtual bool GetSuperLogData2(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int params4, ref int params1, ref int params2, ref int dwManipulation, ref int params3, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSuperLogData2", (ActiveXInvokeKind)0);
		}
		return ocx.GetSuperLogData2(dwMachineNumber, ref dwTMachineNumber, ref dwSEnrollNumber, ref params4, ref params1, ref params2, ref dwManipulation, ref params3, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecs);
	}

	public virtual bool GetUserFace(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, ref byte tmpData, ref int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserFace", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserFace(dwMachineNumber, dwEnrollNumber, dwFaceIndex, ref tmpData, ref tmpLength);
	}

	public virtual bool SetUserFace(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, ref byte tmpData, int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserFace", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserFace(dwMachineNumber, dwEnrollNumber, dwFaceIndex, ref tmpData, tmpLength);
	}

	public virtual bool DelUserFace(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DelUserFace", (ActiveXInvokeKind)0);
		}
		return ocx.DelUserFace(dwMachineNumber, dwEnrollNumber, dwFaceIndex);
	}

	public virtual bool GetUserFaceStr(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, ref string tmpData, ref int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserFaceStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserFaceStr(dwMachineNumber, dwEnrollNumber, dwFaceIndex, ref tmpData, ref tmpLength);
	}

	public virtual bool SetUserFaceStr(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, string tmpData, int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserFaceStr", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserFaceStr(dwMachineNumber, dwEnrollNumber, dwFaceIndex, tmpData, tmpLength);
	}

	public virtual bool GetUserTmpEx(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out int flag, out byte tmpData, out int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserTmpEx", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserTmpEx(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out flag, out tmpData, out tmpLength);
	}

	public virtual bool GetUserTmpExStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out int flag, out string tmpData, out int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserTmpExStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserTmpExStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out flag, out tmpData, out tmpLength);
	}

	public virtual bool SetUserTmpEx(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, int flag, ref byte tmpData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserTmpEx", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserTmpEx(dwMachineNumber, dwEnrollNumber, dwFingerIndex, flag, ref tmpData);
	}

	public virtual bool SetUserTmpExStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, int flag, string tmpData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserTmpExStr", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserTmpExStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, flag, tmpData);
	}

	public virtual bool MergeTemplate(IntPtr templates, int fingerCount, ref byte templateDest, ref int fingerSize)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("MergeTemplate", (ActiveXInvokeKind)0);
		}
		return ocx.MergeTemplate(templates, fingerCount, ref templateDest, ref fingerSize);
	}

	public virtual bool SplitTemplate(ref byte template, IntPtr templates, ref int fingerCount, ref int fingerSize)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SplitTemplate", (ActiveXInvokeKind)0);
		}
		return ocx.SplitTemplate(ref template, templates, ref fingerCount, ref fingerSize);
	}

	public virtual bool ReadUserAllTemplate(int dwMachineNumber, string dwEnrollNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadUserAllTemplate", (ActiveXInvokeKind)0);
		}
		return ocx.ReadUserAllTemplate(dwMachineNumber, dwEnrollNumber);
	}

	public virtual bool UpdateFile(string fileName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("UpdateFile", (ActiveXInvokeKind)0);
		}
		return ocx.UpdateFile(fileName);
	}

	public virtual bool ReadLastestLogData(int dwMachineNumber, int newLog, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadLastestLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadLastestLogData(dwMachineNumber, newLog, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
	}

	public virtual bool SetOptionCommPwd(int dwMachineNumber, string commKey)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetOptionCommPwd", (ActiveXInvokeKind)0);
		}
		return ocx.SetOptionCommPwd(dwMachineNumber, commKey);
	}

	public virtual bool ReadSuperLogDataEx(int dwMachineNumber, int dwYear_S, int dwMonth_S, int dwDay_S, int dwHour_S, int dwMinute_S, int dwSecond_S, int dwYear_E, int dwMonth_E, int dwDay_E, int dwHour_E, int dwMinute_E, int dwSecond_E, int dwLogIndex)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadSuperLogDataEx", (ActiveXInvokeKind)0);
		}
		return ocx.ReadSuperLogDataEx(dwMachineNumber, dwYear_S, dwMonth_S, dwDay_S, dwHour_S, dwMinute_S, dwSecond_S, dwYear_E, dwMonth_E, dwDay_E, dwHour_E, dwMinute_E, dwSecond_E, dwLogIndex);
	}

	public virtual bool GetSuperLogDataEx(int dwMachineNumber, ref string enrollNumber, ref int params4, ref int params1, ref int params2, ref int dwManipulation, ref int params3, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSuperLogDataEx", (ActiveXInvokeKind)0);
		}
		return ocx.GetSuperLogDataEx(dwMachineNumber, ref enrollNumber, ref params4, ref params1, ref params2, ref dwManipulation, ref params3, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
	}

	public virtual bool GetPhotoByName(int dwMachineNumber, string photoName, out byte photoData, out int photoLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPhotoByName", (ActiveXInvokeKind)0);
		}
		return ocx.GetPhotoByName(dwMachineNumber, photoName, out photoData, out photoLength);
	}

	public virtual bool GetPhotoNamesByTime(int dwMachineNumber, int iFlag, string sTime, string eTime, out string allPhotoName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPhotoNamesByTime", (ActiveXInvokeKind)0);
		}
		return ocx.GetPhotoNamesByTime(dwMachineNumber, iFlag, sTime, eTime, out allPhotoName);
	}

	public virtual bool ClearPhotoByTime(int dwMachineNumber, int iFlag, string sTime, string eTime)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearPhotoByTime", (ActiveXInvokeKind)0);
		}
		return ocx.ClearPhotoByTime(dwMachineNumber, iFlag, sTime, eTime);
	}

	public virtual bool GetPhotoCount(int dwMachineNumber, out int count, int iFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPhotoCount", (ActiveXInvokeKind)0);
		}
		return ocx.GetPhotoCount(dwMachineNumber, out count, iFlag);
	}

	public virtual bool ClearDataEx(int dwMachineNumber, string tableName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearDataEx", (ActiveXInvokeKind)0);
		}
		return ocx.ClearDataEx(dwMachineNumber, tableName);
	}

	public virtual bool GetDataFileEx(int dwMachineNumber, string sourceFileName, string destFileName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDataFileEx", (ActiveXInvokeKind)0);
		}
		return ocx.GetDataFileEx(dwMachineNumber, sourceFileName, destFileName);
	}

	public virtual bool SSR_SetDeviceData(int dwMachineNumber, string tableName, string datas, string options)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetDeviceData", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetDeviceData(dwMachineNumber, tableName, datas, options);
	}

	public virtual bool SSR_GetDeviceData(int dwMachineNumber, out string buffer, int bufferSize, string tableName, string filedNames, string filter, string options)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetDeviceData", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetDeviceData(dwMachineNumber, out buffer, bufferSize, tableName, filedNames, filter, options);
	}

	public virtual bool GetUserInfoByPIN2(int dwMachineNumber, ref string name, ref string password, ref int privilege, ref bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserInfoByPIN2", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserInfoByPIN2(dwMachineNumber, ref name, ref password, ref privilege, ref enabled);
	}

	public virtual bool GetUserInfoByCard(int dwMachineNumber, ref string name, ref string password, ref int privilege, ref bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserInfoByCard", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserInfoByCard(dwMachineNumber, ref name, ref password, ref privilege, ref enabled);
	}

	public virtual int get_CardNumber(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_CardNumber", (ActiveXInvokeKind)0);
		}
		return ocx.get_CardNumber(index);
	}

	public virtual void set_CardNumber(int index, int pVal)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_CardNumber", (ActiveXInvokeKind)0);
		}
		ocx.set_CardNumber(index, pVal);
	}

	public virtual bool CaptureImage(bool fullImage, ref int width, ref int height, ref byte image, string imageFile)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CaptureImage", (ActiveXInvokeKind)0);
		}
		return ocx.CaptureImage(fullImage, ref width, ref height, ref image, imageFile);
	}

	public virtual bool UpdateFirmware(string firmwareFile)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("UpdateFirmware", (ActiveXInvokeKind)0);
		}
		return ocx.UpdateFirmware(firmwareFile);
	}

	public virtual bool StartEnroll(int userID, int fingerID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("StartEnroll", (ActiveXInvokeKind)0);
		}
		return ocx.StartEnroll(userID, fingerID);
	}

	public virtual bool StartVerify(int userID, int fingerID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("StartVerify", (ActiveXInvokeKind)0);
		}
		return ocx.StartVerify(userID, fingerID);
	}

	public virtual bool StartIdentify()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("StartIdentify", (ActiveXInvokeKind)0);
		}
		return ocx.StartIdentify();
	}

	public virtual bool CancelOperation()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CancelOperation", (ActiveXInvokeKind)0);
		}
		return ocx.CancelOperation();
	}

	public virtual bool QueryState(ref int state)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("QueryState", (ActiveXInvokeKind)0);
		}
		return ocx.QueryState(ref state);
	}

	public virtual bool BackupData(string dataFile)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("BackupData", (ActiveXInvokeKind)0);
		}
		return ocx.BackupData(dataFile);
	}

	public virtual bool RestoreData(string dataFile)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("RestoreData", (ActiveXInvokeKind)0);
		}
		return ocx.RestoreData(dataFile);
	}

	public virtual bool WriteLCD(int row, int col, string text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteLCD", (ActiveXInvokeKind)0);
		}
		return ocx.WriteLCD(row, col, text);
	}

	public virtual bool ClearLCD()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearLCD", (ActiveXInvokeKind)0);
		}
		return ocx.ClearLCD();
	}

	public virtual bool Beep(int delayMS)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Beep", (ActiveXInvokeKind)0);
		}
		return ocx.Beep(delayMS);
	}

	public virtual bool PlayVoice(int position, int length)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("PlayVoice", (ActiveXInvokeKind)0);
		}
		return ocx.PlayVoice(position, length);
	}

	public virtual bool PlayVoiceByIndex(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("PlayVoiceByIndex", (ActiveXInvokeKind)0);
		}
		return ocx.PlayVoiceByIndex(index);
	}

	public virtual bool EnableClock(int enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EnableClock", (ActiveXInvokeKind)0);
		}
		return ocx.EnableClock(enabled);
	}

	public virtual bool GetUserIDByPIN2(int pIN2, ref int userID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserIDByPIN2", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserIDByPIN2(pIN2, ref userID);
	}

	public virtual bool GetPIN2(int userID, ref int pIN2)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPIN2", (ActiveXInvokeKind)0);
		}
		return ocx.GetPIN2(userID, ref pIN2);
	}

	public virtual bool FPTempConvertNew(ref byte tmpData1, ref byte tmpData2, ref int size)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("FPTempConvertNew", (ActiveXInvokeKind)0);
		}
		return ocx.FPTempConvertNew(ref tmpData1, ref tmpData2, ref size);
	}

	public virtual bool FPTempConvertNewStr(string tmpData1, ref string tmpData2, ref int size)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("FPTempConvertNewStr", (ActiveXInvokeKind)0);
		}
		return ocx.FPTempConvertNewStr(tmpData1, ref tmpData2, ref size);
	}

	public virtual bool ReadAllTemplate(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAllTemplate", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAllTemplate(dwMachineNumber);
	}

	public virtual bool DisableDeviceWithTimeOut(int dwMachineNumber, int timeOutSec)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DisableDeviceWithTimeOut", (ActiveXInvokeKind)0);
		}
		return ocx.DisableDeviceWithTimeOut(dwMachineNumber, timeOutSec);
	}

	public virtual bool SetDeviceTime2(int dwMachineNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceTime2", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceTime2(dwMachineNumber, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
	}

	public virtual bool ClearSLog(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearSLog", (ActiveXInvokeKind)0);
		}
		return ocx.ClearSLog(dwMachineNumber);
	}

	public virtual bool RestartDevice(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("RestartDevice", (ActiveXInvokeKind)0);
		}
		return ocx.RestartDevice(dwMachineNumber);
	}

	public virtual bool GetDeviceMAC(int dwMachineNumber, ref string sMAC)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceMAC", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceMAC(dwMachineNumber, ref sMAC);
	}

	public virtual bool SetDeviceMAC(int dwMachineNumber, string sMAC)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceMAC", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceMAC(dwMachineNumber, sMAC);
	}

	public virtual bool GetWiegandFmt(int dwMachineNumber, ref string sWiegandFmt)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetWiegandFmt", (ActiveXInvokeKind)0);
		}
		return ocx.GetWiegandFmt(dwMachineNumber, ref sWiegandFmt);
	}

	public virtual bool SetWiegandFmt(int dwMachineNumber, string sWiegandFmt)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetWiegandFmt", (ActiveXInvokeKind)0);
		}
		return ocx.SetWiegandFmt(dwMachineNumber, sWiegandFmt);
	}

	public virtual bool ClearSMS(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearSMS", (ActiveXInvokeKind)0);
		}
		return ocx.ClearSMS(dwMachineNumber);
	}

	public virtual bool GetSMS(int dwMachineNumber, int iD, ref int tag, ref int validMinutes, ref string startTime, ref string content)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSMS", (ActiveXInvokeKind)0);
		}
		return ocx.GetSMS(dwMachineNumber, iD, ref tag, ref validMinutes, ref startTime, ref content);
	}

	public virtual bool SetSMS(int dwMachineNumber, int iD, int tag, int validMinutes, string startTime, string content)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetSMS", (ActiveXInvokeKind)0);
		}
		return ocx.SetSMS(dwMachineNumber, iD, tag, validMinutes, startTime, content);
	}

	public virtual bool DeleteSMS(int dwMachineNumber, int iD)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeleteSMS", (ActiveXInvokeKind)0);
		}
		return ocx.DeleteSMS(dwMachineNumber, iD);
	}

	public virtual bool SetUserSMS(int dwMachineNumber, int dwEnrollNumber, int sMSID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserSMS", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserSMS(dwMachineNumber, dwEnrollNumber, sMSID);
	}

	public virtual bool DeleteUserSMS(int dwMachineNumber, int dwEnrollNumber, int sMSID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeleteUserSMS", (ActiveXInvokeKind)0);
		}
		return ocx.DeleteUserSMS(dwMachineNumber, dwEnrollNumber, sMSID);
	}

	public virtual bool GetCardFun(int dwMachineNumber, ref int cardFun)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetCardFun", (ActiveXInvokeKind)0);
		}
		return ocx.GetCardFun(dwMachineNumber, ref cardFun);
	}

	public virtual bool ClearUserSMS(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearUserSMS", (ActiveXInvokeKind)0);
		}
		return ocx.ClearUserSMS(dwMachineNumber);
	}

	public virtual bool SetDeviceCommPwd(int dwMachineNumber, int commKey)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceCommPwd", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceCommPwd(dwMachineNumber, commKey);
	}

	public virtual bool GetDoorState(int machineNumber, ref int state)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDoorState", (ActiveXInvokeKind)0);
		}
		return ocx.GetDoorState(machineNumber, ref state);
	}

	public virtual bool GetVendor(ref string strVendor)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetVendor", (ActiveXInvokeKind)0);
		}
		return ocx.GetVendor(ref strVendor);
	}

	public virtual bool GetSensorSN(int dwMachineNumber, ref string sensorSN)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSensorSN", (ActiveXInvokeKind)0);
		}
		return ocx.GetSensorSN(dwMachineNumber, ref sensorSN);
	}

	public virtual bool ReadCustData(int dwMachineNumber, ref string custData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadCustData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadCustData(dwMachineNumber, ref custData);
	}

	public virtual bool WriteCustData(int dwMachineNumber, string custData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteCustData", (ActiveXInvokeKind)0);
		}
		return ocx.WriteCustData(dwMachineNumber, custData);
	}

	public virtual bool BeginBatchUpdate(int dwMachineNumber, int updateFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("BeginBatchUpdate", (ActiveXInvokeKind)0);
		}
		return ocx.BeginBatchUpdate(dwMachineNumber, updateFlag);
	}

	public virtual bool BatchUpdate(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("BatchUpdate", (ActiveXInvokeKind)0);
		}
		return ocx.BatchUpdate(dwMachineNumber);
	}

	public virtual bool ClearData(int dwMachineNumber, int dataFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearData", (ActiveXInvokeKind)0);
		}
		return ocx.ClearData(dwMachineNumber, dataFlag);
	}

	public virtual bool GetDataFile(int dwMachineNumber, int dataFlag, string fileName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDataFile", (ActiveXInvokeKind)0);
		}
		return ocx.GetDataFile(dwMachineNumber, dataFlag, fileName);
	}

	public virtual bool WriteCard(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex1, ref byte tmpData1, int dwFingerIndex2, ref byte tmpData2, int dwFingerIndex3, ref byte tmpData3, int dwFingerIndex4, ref byte tmpData4)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteCard", (ActiveXInvokeKind)0);
		}
		return ocx.WriteCard(dwMachineNumber, dwEnrollNumber, dwFingerIndex1, ref tmpData1, dwFingerIndex2, ref tmpData2, dwFingerIndex3, ref tmpData3, dwFingerIndex4, ref tmpData4);
	}

	public virtual bool GetGeneralExtLogData(int dwMachineNumber, ref int dwEnrollNumber, ref int dwVerifyMode, ref int dwInOutMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond, ref int dwWorkCode, ref int dwReserved)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetGeneralExtLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetGeneralExtLogData(dwMachineNumber, ref dwEnrollNumber, ref dwVerifyMode, ref dwInOutMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond, ref dwWorkCode, ref dwReserved);
	}

	public virtual bool EmptyCard(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EmptyCard", (ActiveXInvokeKind)0);
		}
		return ocx.EmptyCard(dwMachineNumber);
	}

	public virtual bool GetDeviceStrInfo(int dwMachineNumber, int dwInfo, out string value)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceStrInfo", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceStrInfo(dwMachineNumber, dwInfo, out value);
	}

	public virtual bool GetSysOption(int dwMachineNumber, string option, out string value)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSysOption", (ActiveXInvokeKind)0);
		}
		return ocx.GetSysOption(dwMachineNumber, option, out value);
	}

	public virtual bool SetUserInfoEx(int dwMachineNumber, int dwEnrollNumber, int verifyStyle, ref byte reserved)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserInfoEx", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserInfoEx(dwMachineNumber, dwEnrollNumber, verifyStyle, ref reserved);
	}

	public virtual bool GetUserInfoEx(int dwMachineNumber, int dwEnrollNumber, out int verifyStyle, out byte reserved)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserInfoEx", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserInfoEx(dwMachineNumber, dwEnrollNumber, out verifyStyle, out reserved);
	}

	public virtual bool DeleteUserInfoEx(int dwMachineNumber, int dwEnrollNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeleteUserInfoEx", (ActiveXInvokeKind)0);
		}
		return ocx.DeleteUserInfoEx(dwMachineNumber, dwEnrollNumber);
	}

	public virtual bool SSR_GetGeneralLogData(int dwMachineNumber, out string dwEnrollNumber, out int dwVerifyMode, out int dwInOutMode, out int dwYear, out int dwMonth, out int dwDay, out int dwHour, out int dwMinute, out int dwSecond, ref int dwWorkCode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetGeneralLogData", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode);
	}

	public virtual bool SSR_GetAllUserInfo(int dwMachineNumber, out string dwEnrollNumber, out string name, out string password, out int privilege, out bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetAllUserInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetAllUserInfo(dwMachineNumber, out dwEnrollNumber, out name, out password, out privilege, out enabled);
	}

	public virtual bool SSR_GetUserInfo(int dwMachineNumber, string dwEnrollNumber, out string name, out string password, out int privilege, out bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetUserInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetUserInfo(dwMachineNumber, dwEnrollNumber, out name, out password, out privilege, out enabled);
	}

	public virtual bool SSR_GetUserTmp(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out byte tmpData, out int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetUserTmp", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out tmpData, out tmpLength);
	}

	public virtual bool SSR_GetUserTmpStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out string tmpData, out int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_GetUserTmpStr", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_GetUserTmpStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out tmpData, out tmpLength);
	}

	public virtual bool SSR_DeleteEnrollData(int dwMachineNumber, string dwEnrollNumber, int dwBackupNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_DeleteEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_DeleteEnrollData(dwMachineNumber, dwEnrollNumber, dwBackupNumber);
	}

	public virtual bool SSR_SetUserInfo(int dwMachineNumber, string dwEnrollNumber, string name, string password, int privilege, bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetUserInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetUserInfo(dwMachineNumber, dwEnrollNumber, name, password, privilege, enabled);
	}

	public virtual bool SSR_SetUserTmp(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, ref byte tmpData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetUserTmp", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex, ref tmpData);
	}

	public virtual bool SSR_SetUserTmpStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, string tmpData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_SetUserTmpStr", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_SetUserTmpStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, tmpData);
	}

	public virtual bool SSR_DelUserTmp(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_DelUserTmp", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_DelUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex);
	}

	public virtual string get_STR_CardNumber(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_STR_CardNumber", (ActiveXInvokeKind)0);
		}
		return ocx.get_STR_CardNumber(index);
	}

	public virtual void set_STR_CardNumber(int index, string pVal)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_STR_CardNumber", (ActiveXInvokeKind)0);
		}
		ocx.set_STR_CardNumber(index, pVal);
	}

	public virtual bool SetWorkCode(int workCodeID, int aWorkCode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.SetWorkCode(workCodeID, aWorkCode);
	}

	public virtual bool GetWorkCode(int workCodeID, out int aWorkCode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.GetWorkCode(workCodeID, out aWorkCode);
	}

	public virtual bool DeleteWorkCode(int workCodeID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeleteWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.DeleteWorkCode(workCodeID);
	}

	public virtual bool ClearWorkCode()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearWorkCode", (ActiveXInvokeKind)0);
		}
		return ocx.ClearWorkCode();
	}

	public virtual bool ReadAttRule(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAttRule", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAttRule(dwMachineNumber);
	}

	public virtual bool ReadDPTInfo(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadDPTInfo", (ActiveXInvokeKind)0);
		}
		return ocx.ReadDPTInfo(dwMachineNumber);
	}

	public virtual bool SaveTheDataToFile(int dwMachineNumber, string theFilePath, int fileFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SaveTheDataToFile", (ActiveXInvokeKind)0);
		}
		return ocx.SaveTheDataToFile(dwMachineNumber, theFilePath, fileFlag);
	}

	public virtual bool ReadTurnInfo(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadTurnInfo", (ActiveXInvokeKind)0);
		}
		return ocx.ReadTurnInfo(dwMachineNumber);
	}

	public virtual bool SSR_OutPutHTMLRep(int dwMachineNumber, string dwEnrollNumber, string attFile, string userFile, string deptFile, string timeClassFile, string attruleFile, int bYear, int bMonth, int bDay, int bHour, int bMinute, int bSecond, int eYear, int eMonth, int eDay, int eHour, int eMinute, int eSecond, string tempPath, string outFileName, int hTMLFlag, int resv1, string resv2)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SSR_OutPutHTMLRep", (ActiveXInvokeKind)0);
		}
		return ocx.SSR_OutPutHTMLRep(dwMachineNumber, dwEnrollNumber, attFile, userFile, deptFile, timeClassFile, attruleFile, bYear, bMonth, bDay, bHour, bMinute, bSecond, eYear, eMonth, eDay, eHour, eMinute, eSecond, tempPath, outFileName, hTMLFlag, resv1, resv2);
	}

	public virtual bool ReadAOptions(string aOption, out string aValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAOptions", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAOptions(aOption, out aValue);
	}

	public virtual bool ReadRTLog(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadRTLog", (ActiveXInvokeKind)0);
		}
		return ocx.ReadRTLog(dwMachineNumber);
	}

	public virtual bool GetRTLog(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetRTLog", (ActiveXInvokeKind)0);
		}
		return ocx.GetRTLog(dwMachineNumber);
	}

	public virtual bool ClearAdministrators(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearAdministrators", (ActiveXInvokeKind)0);
		}
		return ocx.ClearAdministrators(dwMachineNumber);
	}

	public virtual bool DeleteEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeleteEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.DeleteEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber);
	}

	public virtual bool ReadSuperLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadSuperLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadSuperLogData(dwMachineNumber);
	}

	public virtual bool ReadAllSLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAllSLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAllSLogData(dwMachineNumber);
	}

	public virtual bool ReadGeneralLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadGeneralLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadGeneralLogData(dwMachineNumber);
	}

	public virtual bool ReadAllGLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAllGLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAllGLogData(dwMachineNumber);
	}

	public virtual bool EnableUser(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, bool bFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EnableUser", (ActiveXInvokeKind)0);
		}
		return ocx.EnableUser(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, bFlag);
	}

	public virtual bool EnableDevice(int dwMachineNumber, bool bFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EnableDevice", (ActiveXInvokeKind)0);
		}
		return ocx.EnableDevice(dwMachineNumber, bFlag);
	}

	public virtual bool GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceStatus", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceStatus(dwMachineNumber, dwStatus, ref dwValue);
	}

	public virtual bool GetDeviceInfo(int dwMachineNumber, int dwInfo, ref int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceInfo", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceInfo(dwMachineNumber, dwInfo, ref dwValue);
	}

	public virtual bool SetDeviceInfo(int dwMachineNumber, int dwInfo, int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceInfo(dwMachineNumber, dwInfo, dwValue);
	}

	public virtual bool SetDeviceTime(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceTime", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceTime(dwMachineNumber);
	}

	public virtual void PowerOnAllDevice()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("PowerOnAllDevice", (ActiveXInvokeKind)0);
		}
		ocx.PowerOnAllDevice();
	}

	public virtual bool PowerOffDevice(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("PowerOffDevice", (ActiveXInvokeKind)0);
		}
		return ocx.PowerOffDevice(dwMachineNumber);
	}

	public virtual bool ModifyPrivilege(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ModifyPrivilege", (ActiveXInvokeKind)0);
		}
		return ocx.ModifyPrivilege(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege);
	}

	public virtual void GetLastError(ref int dwErrorCode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetLastError", (ActiveXInvokeKind)0);
		}
		ocx.GetLastError(ref dwErrorCode);
	}

	public virtual bool GetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnrollData, ref int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.GetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, ref dwMachinePrivilege, ref dwEnrollData, ref dwPassWord);
	}

	public virtual bool SetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege, ref int dwEnrollData, int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.SetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege, ref dwEnrollData, dwPassWord);
	}

	public virtual bool GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceTime", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceTime(dwMachineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
	}

	public virtual bool GetGeneralLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwInOutMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetGeneralLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetGeneralLogData(dwMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwInOutMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute);
	}

	public virtual bool GetSuperLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int params4, ref int params1, ref int params2, ref int dwManipulation, ref int params3, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSuperLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetSuperLogData(dwMachineNumber, ref dwTMachineNumber, ref dwSEnrollNumber, ref params4, ref params1, ref params2, ref dwManipulation, ref params3, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute);
	}

	public virtual bool GetAllSLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int params4, ref int params1, ref int params2, ref int dwManipulation, ref int params3, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetAllSLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetAllSLogData(dwMachineNumber, ref dwTMachineNumber, ref dwSEnrollNumber, ref params4, ref params1, ref params2, ref dwManipulation, ref params3, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute);
	}

	public virtual bool GetAllGLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwInOutMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetAllGLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetAllGLogData(dwMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwInOutMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute);
	}

	public virtual void ConvertPassword(int dwSrcPSW, ref int dwDestPSW, int dwLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ConvertPassword", (ActiveXInvokeKind)0);
		}
		ocx.ConvertPassword(dwSrcPSW, ref dwDestPSW, dwLength);
	}

	public virtual bool ReadAllUserID(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAllUserID", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAllUserID(dwMachineNumber);
	}

	public virtual bool GetAllUserID(int dwMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnable)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetAllUserID", (ActiveXInvokeKind)0);
		}
		return ocx.GetAllUserID(dwMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwBackupNumber, ref dwMachinePrivilege, ref dwEnable);
	}

	public virtual bool GetSerialNumber(int dwMachineNumber, out string dwSerialNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSerialNumber", (ActiveXInvokeKind)0);
		}
		return ocx.GetSerialNumber(dwMachineNumber, out dwSerialNumber);
	}

	public virtual bool ClearKeeperData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearKeeperData", (ActiveXInvokeKind)0);
		}
		return ocx.ClearKeeperData(dwMachineNumber);
	}

	public virtual int GetBackupNumber(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetBackupNumber", (ActiveXInvokeKind)0);
		}
		return ocx.GetBackupNumber(dwMachineNumber);
	}

	public virtual bool GetProductCode(int dwMachineNumber, out string lpszProductCode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetProductCode", (ActiveXInvokeKind)0);
		}
		return ocx.GetProductCode(dwMachineNumber, out lpszProductCode);
	}

	public virtual bool GetFirmwareVersion(int dwMachineNumber, ref string strVersion)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetFirmwareVersion", (ActiveXInvokeKind)0);
		}
		return ocx.GetFirmwareVersion(dwMachineNumber, ref strVersion);
	}

	public virtual bool GetSDKVersion(ref string strVersion)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSDKVersion", (ActiveXInvokeKind)0);
		}
		return ocx.GetSDKVersion(ref strVersion);
	}

	public virtual bool ClearGLog(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearGLog", (ActiveXInvokeKind)0);
		}
		return ocx.ClearGLog(dwMachineNumber);
	}

	public virtual int GetFPTempLength(ref byte dwEnrollData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetFPTempLength", (ActiveXInvokeKind)0);
		}
		return ocx.GetFPTempLength(ref dwEnrollData);
	}

	public virtual bool Connect_Com(int comPort, int machineNumber, int baudRate)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Connect_Com", (ActiveXInvokeKind)0);
		}
		return ocx.Connect_Com(comPort, machineNumber, baudRate);
	}

	public virtual bool Connect_Net(string iPAdd, int port)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Connect_Net", (ActiveXInvokeKind)0);
		}
		return ocx.Connect_Net(iPAdd, port);
	}

	public virtual void Disconnect()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Disconnect", (ActiveXInvokeKind)0);
		}
		ocx.Disconnect();
	}

	public virtual bool SetUserInfo(int dwMachineNumber, int dwEnrollNumber, string name, string password, int privilege, bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserInfo(dwMachineNumber, dwEnrollNumber, name, password, privilege, enabled);
	}

	public virtual bool GetUserInfo(int dwMachineNumber, int dwEnrollNumber, ref string name, ref string password, ref int privilege, ref bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserInfo", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserInfo(dwMachineNumber, dwEnrollNumber, ref name, ref password, ref privilege, ref enabled);
	}

	public virtual bool SetDeviceIP(int dwMachineNumber, string iPAddr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceIP", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceIP(dwMachineNumber, iPAddr);
	}

	public virtual bool GetDeviceIP(int dwMachineNumber, ref string iPAddr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceIP", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceIP(dwMachineNumber, ref iPAddr);
	}

	public virtual bool GetUserTmp(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, ref byte tmpData, ref int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserTmp", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex, ref tmpData, ref tmpLength);
	}

	public virtual bool SetUserTmp(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, ref byte tmpData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserTmp", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex, ref tmpData);
	}

	public virtual bool GetAllUserInfo(int dwMachineNumber, ref int dwEnrollNumber, ref string name, ref string password, ref int privilege, ref bool enabled)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetAllUserInfo", (ActiveXInvokeKind)0);
		}
		return ocx.GetAllUserInfo(dwMachineNumber, ref dwEnrollNumber, ref name, ref password, ref privilege, ref enabled);
	}

	public virtual bool DelUserTmp(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DelUserTmp", (ActiveXInvokeKind)0);
		}
		return ocx.DelUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex);
	}

	public virtual bool RefreshData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("RefreshData", (ActiveXInvokeKind)0);
		}
		return ocx.RefreshData(dwMachineNumber);
	}

	public virtual bool FPTempConvert(ref byte tmpData1, ref byte tmpData2, ref int size)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("FPTempConvert", (ActiveXInvokeKind)0);
		}
		return ocx.FPTempConvert(ref tmpData1, ref tmpData2, ref size);
	}

	public virtual bool SetCommPassword(int commKey)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetCommPassword", (ActiveXInvokeKind)0);
		}
		return ocx.SetCommPassword(commKey);
	}

	public virtual bool GetUserGroup(int dwMachineNumber, int dwEnrollNumber, ref int userGrp)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserGroup", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserGroup(dwMachineNumber, dwEnrollNumber, ref userGrp);
	}

	public virtual bool SetUserGroup(int dwMachineNumber, int dwEnrollNumber, int userGrp)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserGroup", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserGroup(dwMachineNumber, dwEnrollNumber, userGrp);
	}

	public virtual bool GetTZInfo(int dwMachineNumber, int tZIndex, ref string tZ)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetTZInfo", (ActiveXInvokeKind)0);
		}
		return ocx.GetTZInfo(dwMachineNumber, tZIndex, ref tZ);
	}

	public virtual bool SetTZInfo(int dwMachineNumber, int tZIndex, string tZ)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetTZInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SetTZInfo(dwMachineNumber, tZIndex, tZ);
	}

	public virtual bool GetUnlockGroups(int dwMachineNumber, ref string grps)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUnlockGroups", (ActiveXInvokeKind)0);
		}
		return ocx.GetUnlockGroups(dwMachineNumber, ref grps);
	}

	public virtual bool SetUnlockGroups(int dwMachineNumber, string grps)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUnlockGroups", (ActiveXInvokeKind)0);
		}
		return ocx.SetUnlockGroups(dwMachineNumber, grps);
	}

	public virtual bool GetGroupTZs(int dwMachineNumber, int groupIndex, ref int tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetGroupTZs", (ActiveXInvokeKind)0);
		}
		return ocx.GetGroupTZs(dwMachineNumber, groupIndex, ref tZs);
	}

	public virtual bool SetGroupTZs(int dwMachineNumber, int groupIndex, ref int tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetGroupTZs", (ActiveXInvokeKind)0);
		}
		return ocx.SetGroupTZs(dwMachineNumber, groupIndex, ref tZs);
	}

	public virtual bool GetUserTZs(int dwMachineNumber, int dwEnrollNumber, ref int tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserTZs", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserTZs(dwMachineNumber, dwEnrollNumber, ref tZs);
	}

	public virtual bool SetUserTZs(int dwMachineNumber, int dwEnrollNumber, ref int tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserTZs", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserTZs(dwMachineNumber, dwEnrollNumber, ref tZs);
	}

	public virtual bool ACUnlock(int dwMachineNumber, int delay)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ACUnlock", (ActiveXInvokeKind)0);
		}
		return ocx.ACUnlock(dwMachineNumber, delay);
	}

	public virtual bool GetACFun(ref int aCFun)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetACFun", (ActiveXInvokeKind)0);
		}
		return ocx.GetACFun(ref aCFun);
	}

	public virtual bool GetGeneralLogDataStr(int dwMachineNumber, ref int dwEnrollNumber, ref int dwVerifyMode, ref int dwInOutMode, ref string timeStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetGeneralLogDataStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetGeneralLogDataStr(dwMachineNumber, ref dwEnrollNumber, ref dwVerifyMode, ref dwInOutMode, ref timeStr);
	}

	public virtual bool GetUserTmpStr(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, ref string tmpData, ref int tmpLength)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserTmpStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserTmpStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, ref tmpData, ref tmpLength);
	}

	public virtual bool SetUserTmpStr(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, string tmpData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserTmpStr", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserTmpStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, tmpData);
	}

	public virtual bool GetEnrollDataStr(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref string dwEnrollData, ref int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetEnrollDataStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetEnrollDataStr(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, ref dwMachinePrivilege, ref dwEnrollData, ref dwPassWord);
	}

	public virtual bool SetEnrollDataStr(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege, string dwEnrollData, int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetEnrollDataStr", (ActiveXInvokeKind)0);
		}
		return ocx.SetEnrollDataStr(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege, dwEnrollData, dwPassWord);
	}

	public virtual bool GetGroupTZStr(int dwMachineNumber, int groupIndex, ref string tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetGroupTZStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetGroupTZStr(dwMachineNumber, groupIndex, ref tZs);
	}

	public virtual bool SetGroupTZStr(int dwMachineNumber, int groupIndex, string tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetGroupTZStr", (ActiveXInvokeKind)0);
		}
		return ocx.SetGroupTZStr(dwMachineNumber, groupIndex, tZs);
	}

	public virtual bool GetUserTZStr(int dwMachineNumber, int dwEnrollNumber, ref string tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserTZStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserTZStr(dwMachineNumber, dwEnrollNumber, ref tZs);
	}

	public virtual bool SetUserTZStr(int dwMachineNumber, int dwEnrollNumber, string tZs)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserTZStr", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserTZStr(dwMachineNumber, dwEnrollNumber, tZs);
	}

	public virtual bool FPTempConvertStr(string tmpData1, ref string tmpData2, ref int size)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("FPTempConvertStr", (ActiveXInvokeKind)0);
		}
		return ocx.FPTempConvertStr(tmpData1, ref tmpData2, ref size);
	}

	public virtual int GetFPTempLengthStr(string dwEnrollData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetFPTempLengthStr", (ActiveXInvokeKind)0);
		}
		return ocx.GetFPTempLengthStr(dwEnrollData);
	}

	public virtual int get_AccTimeZones(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_AccTimeZones", (ActiveXInvokeKind)0);
		}
		return ocx.get_AccTimeZones(index);
	}

	public virtual void set_AccTimeZones(int index, int pVal)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_AccTimeZones", (ActiveXInvokeKind)0);
		}
		ocx.set_AccTimeZones(index, pVal);
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxCZKEMEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(_IZKEMEvents));
		}
		catch (Exception)
		{
		}
	}

	protected override void DetachSink()
	{
		try
		{
			cookie.Disconnect();
		}
		catch (Exception)
		{
		}
	}

	protected override void AttachInterfaces()
	{
		try
		{
			ocx = (IZKEM)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnOnAttTransaction(object sender, _IZKEMEvents_OnAttTransactionEvent e)
	{
		if (this.OnAttTransaction != null)
		{
			this.OnAttTransaction(sender, e);
		}
	}

	internal void RaiseOnOnKeyPress(object sender, _IZKEMEvents_OnKeyPressEvent e)
	{
		if (this.OnKeyPress != null)
		{
			this.OnKeyPress(sender, e);
		}
	}

	internal void RaiseOnOnEnrollFinger(object sender, _IZKEMEvents_OnEnrollFingerEvent e)
	{
		if (this.OnEnrollFinger != null)
		{
			this.OnEnrollFinger(sender, e);
		}
	}

	internal void RaiseOnOnNewUser(object sender, _IZKEMEvents_OnNewUserEvent e)
	{
		if (this.OnNewUser != null)
		{
			this.OnNewUser(sender, e);
		}
	}

	internal void RaiseOnOnEMData(object sender, _IZKEMEvents_OnEMDataEvent e)
	{
		if (this.OnEMData != null)
		{
			this.OnEMData(sender, e);
		}
	}

	internal void RaiseOnOnConnected(object sender, EventArgs e)
	{
		if (this.OnConnected != null)
		{
			this.OnConnected(sender, e);
		}
	}

	internal void RaiseOnOnDisConnected(object sender, EventArgs e)
	{
		if (this.OnDisConnected != null)
		{
			this.OnDisConnected(sender, e);
		}
	}

	internal void RaiseOnOnFinger(object sender, EventArgs e)
	{
		if (this.OnFinger != null)
		{
			this.OnFinger(sender, e);
		}
	}

	internal void RaiseOnOnVerify(object sender, _IZKEMEvents_OnVerifyEvent e)
	{
		if (this.OnVerify != null)
		{
			this.OnVerify(sender, e);
		}
	}

	internal void RaiseOnOnFingerFeature(object sender, _IZKEMEvents_OnFingerFeatureEvent e)
	{
		if (this.OnFingerFeature != null)
		{
			this.OnFingerFeature(sender, e);
		}
	}

	internal void RaiseOnOnHIDNum(object sender, _IZKEMEvents_OnHIDNumEvent e)
	{
		if (this.OnHIDNum != null)
		{
			this.OnHIDNum(sender, e);
		}
	}

	internal void RaiseOnOnDoor(object sender, _IZKEMEvents_OnDoorEvent e)
	{
		if (this.OnDoor != null)
		{
			this.OnDoor(sender, e);
		}
	}

	internal void RaiseOnOnAlarm(object sender, _IZKEMEvents_OnAlarmEvent e)
	{
		if (this.OnAlarm != null)
		{
			this.OnAlarm(sender, e);
		}
	}

	internal void RaiseOnOnWriteCard(object sender, _IZKEMEvents_OnWriteCardEvent e)
	{
		if (this.OnWriteCard != null)
		{
			this.OnWriteCard(sender, e);
		}
	}

	internal void RaiseOnOnEmptyCard(object sender, _IZKEMEvents_OnEmptyCardEvent e)
	{
		if (this.OnEmptyCard != null)
		{
			this.OnEmptyCard(sender, e);
		}
	}

	internal void RaiseOnOnDeleteTemplate(object sender, _IZKEMEvents_OnDeleteTemplateEvent e)
	{
		if (this.OnDeleteTemplate != null)
		{
			this.OnDeleteTemplate(sender, e);
		}
	}

	internal void RaiseOnOnAttTransactionEx(object sender, _IZKEMEvents_OnAttTransactionExEvent e)
	{
		if (this.OnAttTransactionEx != null)
		{
			this.OnAttTransactionEx(sender, e);
		}
	}

	internal void RaiseOnOnEnrollFingerEx(object sender, _IZKEMEvents_OnEnrollFingerExEvent e)
	{
		if (this.OnEnrollFingerEx != null)
		{
			this.OnEnrollFingerEx(sender, e);
		}
	}
}
