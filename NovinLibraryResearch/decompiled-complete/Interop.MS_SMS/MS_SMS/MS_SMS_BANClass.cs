using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using stdole;

namespace MS_SMS;

[ComImport]
[Guid("F69FC669-2E6D-4D10-B14F-65B5FBCD8481")]
[TypeLibType(34)]
[ClassInterface(0)]
[ComSourceInterfaces("MS_SMS.IMS_SMS_BANEvents\0\0")]
public class MS_SMS_BANClass : IMS_SMS_BAN, MS_SMS_BAN, IMS_SMS_BANEvents_Event
{
	[DispId(201)]
	public virtual extern bool Visible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		[param: In]
		set;
	}

	[DispId(202)]
	public virtual extern bool AutoScroll
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(202)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(202)]
		[param: In]
		set;
	}

	[DispId(203)]
	public virtual extern bool AutoSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(203)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(203)]
		[param: In]
		set;
	}

	[DispId(204)]
	public virtual extern TxActiveFormBorderStyle AxBorderStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(204)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(204)]
		[param: In]
		set;
	}

	[DispId(205)]
	public virtual extern int BorderWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(205)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(205)]
		[param: In]
		set;
	}

	[DispId(-518)]
	public virtual extern string Caption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-518)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-518)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[ComAliasName("stdole.OLE_COLOR")]
	[DispId(-501)]
	public virtual extern uint Color
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-501)]
		[return: ComAliasName("stdole.OLE_COLOR")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-501)]
		[param: In]
		[param: ComAliasName("stdole.OLE_COLOR")]
		set;
	}

	[ComAliasName("stdole.IFontDisp")]
	[DispId(-512)]
	public virtual extern IFontDisp Font
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-512)]
		[return: MarshalAs(UnmanagedType.Interface)]
		[return: ComAliasName("stdole.IFontDisp")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-512)]
		[param: In]
		[param: ComAliasName("stdole.IFontDisp")]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(206)]
	public virtual extern bool KeyPreview
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(206)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(206)]
		[param: In]
		set;
	}

	[DispId(207)]
	public virtual extern int PixelsPerInch
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(207)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(207)]
		[param: In]
		set;
	}

	[DispId(208)]
	public virtual extern TxPrintScale PrintScale
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(208)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(208)]
		[param: In]
		set;
	}

	[DispId(209)]
	public virtual extern bool Scaled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(209)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(209)]
		[param: In]
		set;
	}

	[DispId(210)]
	public virtual extern bool Active
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(210)]
		[TypeLibFunc(64)]
		get;
	}

	[DispId(211)]
	public virtual extern bool DropTarget
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(211)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(211)]
		[param: In]
		set;
	}

	[DispId(212)]
	public virtual extern string HelpFile
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(212)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(212)]
		[TypeLibFunc(64)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(213)]
	public virtual extern TxPopupMode PopupMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(213)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(213)]
		[param: In]
		set;
	}

	[DispId(214)]
	public virtual extern bool ScreenSnap
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(214)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(214)]
		[param: In]
		set;
	}

	[DispId(215)]
	public virtual extern int SnapBuffer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(215)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(215)]
		[TypeLibFunc(64)]
		[param: In]
		set;
	}

	[DispId(216)]
	public virtual extern bool DockSite
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(216)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(216)]
		[param: In]
		set;
	}

	[DispId(217)]
	public virtual extern bool DoubleBuffered
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(217)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(217)]
		[TypeLibFunc(64)]
		[param: In]
		set;
	}

	[DispId(218)]
	public virtual extern bool AlignDisabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(218)]
		get;
	}

	[DispId(219)]
	public virtual extern bool MouseInClient
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(219)]
		get;
	}

	[DispId(220)]
	public virtual extern int VisibleDockClientCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(220)]
		get;
	}

	[DispId(221)]
	public virtual extern bool ParentDoubleBuffered
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(221)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(221)]
		[param: In]
		set;
	}

	[DispId(222)]
	public virtual extern bool UseDockManager
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(222)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(222)]
		[param: In]
		set;
	}

	[DispId(-514)]
	public virtual extern bool Enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-514)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(-514)]
		[param: In]
		set;
	}

	[DispId(223)]
	public virtual extern int ExplicitLeft
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(223)]
		get;
	}

	[DispId(224)]
	public virtual extern int ExplicitTop
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(224)]
		get;
	}

	[DispId(225)]
	public virtual extern int ExplicitWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(225)]
		[TypeLibFunc(64)]
		get;
	}

	[DispId(226)]
	public virtual extern int ExplicitHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(226)]
		[TypeLibFunc(64)]
		get;
	}

	[DispId(227)]
	public virtual extern bool AlignWithMargins
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(227)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(227)]
		[param: In]
		set;
	}

	[DispId(228)]
	public virtual extern bool ParentCustomHint
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(228)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(228)]
		[param: In]
		set;
	}

	public virtual extern event IMS_SMS_BANEvents_OnActivateEventHandler OnActivate;

	public virtual extern event IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEventHandler OnAfterMonitorDpiChanged;

	public virtual extern event IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEventHandler OnBeforeMonitorDpiChanged;

	public virtual extern event IMS_SMS_BANEvents_OnClickEventHandler OnClick;

	public virtual extern event IMS_SMS_BANEvents_OnCreateEventHandler OnCreate;

	public virtual extern event IMS_SMS_BANEvents_OnDblClickEventHandler OnDblClick;

	public virtual extern event IMS_SMS_BANEvents_OnDestroyEventHandler OnDestroy;

	public virtual extern event IMS_SMS_BANEvents_OnDeactivateEventHandler OnDeactivate;

	public virtual extern event IMS_SMS_BANEvents_OnKeyPressEventHandler OnKeyPress;

	public virtual extern event IMS_SMS_BANEvents_OnMouseEnterEventHandler OnMouseEnter;

	public virtual extern event IMS_SMS_BANEvents_OnMouseLeaveEventHandler OnMouseLeave;

	public virtual extern event IMS_SMS_BANEvents_OnPaintEventHandler OnPaint;

	public virtual extern event IMS_SMS_BANEvents_OnSMSEventHandler OnSMS;

	public virtual extern event IMS_SMS_BANEvents_OnGSMEventEventHandler OnGSMEvent;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(-512)]
	public virtual extern void let_Font([In][ComAliasName("stdole.IFontDisp")][MarshalAs(UnmanagedType.Interface)] IFontDisp Value);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(229)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string SMS_Ver();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(230)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string SMS_Select_Device([In] short Indx);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(231)]
	public virtual extern short SMS_Device_Count();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(232)]
	public virtual extern bool SMS_Set_Vol([In][MarshalAs(UnmanagedType.BStr)] string Volume_1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(233)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string SMS_Dev_INFO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(234)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string SMS_S_N();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(235)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string SMS_Soft_Ver();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(236)]
	public virtual extern bool GSM_Test_Connect();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(237)]
	public virtual extern bool SMS_PDU_Mode([In] bool PDU);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(238)]
	public virtual extern bool SMS_Char_Mode([In] short Char_Mode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(239)]
	public virtual extern bool Send_Text_SMS([In] bool PDU, [In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_TXT);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(240)]
	public virtual extern bool SMS_Delete([In] int Indx, [In] int Del_Type);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(241)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_Text_SMS([In] int Indx, [In] int SMS_State);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(242)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_PDU_SMS([In] int Indx);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(243)]
	public virtual extern int Get_SMS_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(244)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_Product_Name();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(245)]
	public virtual extern bool Set_Pulse_Dialing();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(246)]
	public virtual extern bool Set_Tone_Dialing();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(247)]
	public virtual extern bool Disconnect_Voice_Call();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(249)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_SIM_PIN();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(248)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string Set_SIM_PIN([In][MarshalAs(UnmanagedType.BStr)] string SIM_PIN);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(250)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_GSM_Date_Time();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(251)]
	public virtual extern bool Set_GSM_Date_Time([In] short GSM_Year, [In] short GSM_Month, [In] short GSM_Day, [In] short GSM_Hour, [In] short GSM_Minute, [In] short GSM_Second);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(252)]
	public virtual extern bool Get_Battery([In][Out][MarshalAs(UnmanagedType.BStr)] ref string voltage, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Percent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(253)]
	public virtual extern bool GSM_Reset_Default();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(254)]
	public virtual extern bool GSM_Reset_Factory();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(255)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_Product_SN();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(256)]
	public virtual extern bool GSM_Dial_NO([In][MarshalAs(UnmanagedType.BStr)] string CountryCode, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Res_MSG);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(257)]
	public virtual extern bool GSM_Answer_Call();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(258)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_Signal();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(259)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_Service_Center();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(260)]
	public virtual extern bool Set_Service_Center([In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Service_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(261)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_CCDID();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(262)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_CIMI();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(263)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_GSM_Ver();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(264)]
	public virtual extern bool Send_SMS_PDU([In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(265)]
	public virtual extern bool GSM_Emerg_Rest();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(266)]
	public virtual extern bool R_SMS([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Date_Time, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(267)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_MSG([In][Out] ref int CMD, [In][Out] ref int Len);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(268)]
	public virtual extern bool GSM_Ready();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(269)]
	public virtual extern bool SMS_Battery([In][Out] ref int Percent, [In][Out] ref int voltage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(270)]
	public virtual extern bool SMS_Battery_Calib([In][MarshalAs(UnmanagedType.BStr)] string Pass);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(271)]
	public virtual extern bool SMS_Bat_charging([In][Out] ref short Status);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(272)]
	public virtual extern bool GSM_T([In] int Indx);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(273)]
	public virtual extern bool GSM_Set_Baud([In] int Baud);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(274)]
	public virtual extern bool SMS_Set_Baud([In] int Baud);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(275)]
	public virtual extern bool GSM_ON();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(276)]
	public virtual extern bool GSM_OFF();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(277)]
	public virtual extern bool GSM_Set_IP([In] int IP1, [In] int IP2, [In] int IP3, [In] int IP4, [In] int Port);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(278)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_Send_AT([In][MarshalAs(UnmanagedType.BStr)] string AT, [In][Out] ref int Response_Len);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(279)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_Response([In] int R_Wait, [In] int Indx, [In][Out] ref int Response_Len);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(280)]
	public virtual extern bool GSM_Set_Device_NO([In] short D_NO);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(281)]
	public virtual extern short GSM_Get_Device_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(282)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_SMS2([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Date_Time, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(283)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_SMS3([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Time_stamp, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Discharge, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_PID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_DCS, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_VP, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_ST, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Reference);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(284)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string Send_SMS_PDU2([In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(285)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_GetUSSD();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(286)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GSM_Send_USSD([In][MarshalAs(UnmanagedType.BStr)] string S_USSD);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(287)]
	public virtual extern bool HID_CMD([In] short S_CMD, [In][MarshalAs(UnmanagedType.BStr)] string S_Data);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(288)]
	public virtual extern void DeVice_Type([In] bool H_Device);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(289)]
	public virtual extern bool S_Ready_Modem();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(290)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_SMS_Ref();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(291)]
	public virtual extern bool R_GSM_POW();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(292)]
	public virtual extern void Ad_Pass([In][MarshalAs(UnmanagedType.BStr)] string Pass);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(293)]
	public virtual extern void Clear_Buf();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(294)]
	public virtual extern short Last_Buf_Index();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(295)]
	public virtual extern bool Set_Chage([In] short On_Off);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(296)]
	public virtual extern bool Insert_PIN([In][MarshalAs(UnmanagedType.BStr)] string I_PIN);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(297)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string Selected_Network();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(298)]
	public virtual extern void Res_AT_Delay([In] int S_Delay);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(299)]
	public virtual extern bool Change_SMS_Center([In][MarshalAs(UnmanagedType.BStr)] string SMS_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(300)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_SMS_Center();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(301)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_IMEI();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(302)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_SIM_ID();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(303)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_Natttional_Mobile_ID();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(304)]
	public virtual extern bool Get_SMS_Space([In][Out] ref int Free_Space, [In][Out] ref int Total_Size);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(305)]
	public virtual extern void Get_AT_Res([In][Out][MarshalAs(UnmanagedType.BStr)] ref string AT_Res);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(306)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string R_SMS4([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Time_stamp, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Discharge, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_PID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_DCS, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_VP, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_ST, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Reference, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string All_Part, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Part_NO);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(307)]
	public virtual extern bool Active_New_SMS_Event([In] bool NS_Active);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(308)]
	public virtual extern bool Recieved_New_SMS([In][Out] ref int Indx_From, [In][Out] ref int Indx_To);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(309)]
	public virtual extern void S_SMS_Delivery([In] short Deliver);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(310)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string S_SMS3([In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Text, [In] bool Request_Delivery_Report, [In] short Send_Interval, [In] short Retry_Times, [In] short SMS_Validity, [In] short SMS_Reference, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(311)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string Send_SMS_Error_Code();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(312)]
	public virtual extern void Add_Group_SMS_Buffer([In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Context);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(313)]
	public virtual extern void SMS_Buf_Start([In] bool Start);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(314)]
	public virtual extern void SMS_Buf_Clear([In] bool Stop);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(315)]
	public virtual extern int Get_SMS_Buf_Indx();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(316)]
	public virtual extern bool Get_SMS_Buf_Status([In] int index, [In][Out] ref bool Send_Done);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(317)]
	public virtual extern void Group_SMS_Setting([In] bool SMS_Delivery, [In] int SMS_Validity, [In] int SMS_NO_Change_Sim, [In] int Repeat_NO, [In] int Send_Interval, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(318)]
	public virtual extern void SMS_Buf_Pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(319)]
	public virtual extern int Get_GSM_Message([In][Out][MarshalAs(UnmanagedType.BStr)] ref string Message_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(320)]
	public virtual extern void Set_Timer_Interval([In] int Pass, [In] int Interval);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(323)]
	public virtual extern int Total_Oflline_SMS_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(324)]
	public virtual extern bool Clear_Oflline_SMS_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(325)]
	public virtual extern bool Show_Oflline_SMS([In] int index, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Time_stamp, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Discharge, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_PID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_DCS, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_VP, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_ST, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Reference, [In][Out] ref int All_Part, [In][Out] ref int Part_NO, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(326)]
	public virtual extern int Get_SMS_Buf_Send_Indx();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(321)]
	public virtual extern void Offline_SMS_Active([In] bool Active);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(322)]
	public virtual extern bool G_Offline_SMS_Active();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(327)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string CallIn_Number();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(328)]
	public virtual extern bool CallerID_Enable([In] bool CID_Active);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(329)]
	public virtual extern bool Select_SIM_Network();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(330)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string ATCommand([In][MarshalAs(UnmanagedType.BStr)] string Param, [In] int AT_Delay);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(331)]
	public virtual extern int Group_Send_NO_SMS();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(332)]
	public virtual extern bool Group_SMS_Finish();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(333)]
	public virtual extern int Get_SMS_Buf_Sended();
}
