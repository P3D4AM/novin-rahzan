using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using stdole;

namespace MS_SMS;

[ComImport]
[TypeLibType(4160)]
[Guid("9686C09B-763D-474F-AF5D-140EB90C59D0")]
public interface IMS_SMS_BAN
{
	[DispId(201)]
	bool Visible
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
	bool AutoScroll
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
	bool AutoSize
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
	TxActiveFormBorderStyle AxBorderStyle
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
	int BorderWidth
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
	string Caption
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
	uint Color
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
	IFontDisp Font
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

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(-512)]
	void let_Font([In][ComAliasName("stdole.IFontDisp")][MarshalAs(UnmanagedType.Interface)] IFontDisp Value);

	[DispId(206)]
	bool KeyPreview
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
	int PixelsPerInch
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
	TxPrintScale PrintScale
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
	bool Scaled
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
	bool Active
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(210)]
		get;
	}

	[DispId(211)]
	bool DropTarget
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(211)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(211)]
		[TypeLibFunc(64)]
		[param: In]
		set;
	}

	[DispId(212)]
	string HelpFile
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
	TxPopupMode PopupMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(213)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(213)]
		[param: In]
		set;
	}

	[DispId(214)]
	bool ScreenSnap
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(214)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(214)]
		[TypeLibFunc(64)]
		[param: In]
		set;
	}

	[DispId(215)]
	int SnapBuffer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(215)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(215)]
		[param: In]
		set;
	}

	[DispId(216)]
	bool DockSite
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(216)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(216)]
		[param: In]
		set;
	}

	[DispId(217)]
	bool DoubleBuffered
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(217)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(217)]
		[TypeLibFunc(64)]
		[param: In]
		set;
	}

	[DispId(218)]
	bool AlignDisabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(218)]
		get;
	}

	[DispId(219)]
	bool MouseInClient
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(219)]
		get;
	}

	[DispId(220)]
	int VisibleDockClientCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(220)]
		get;
	}

	[DispId(221)]
	bool ParentDoubleBuffered
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(221)]
		[TypeLibFunc(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(221)]
		[param: In]
		set;
	}

	[DispId(222)]
	bool UseDockManager
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(222)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(222)]
		[param: In]
		set;
	}

	[DispId(-514)]
	bool Enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(-514)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-514)]
		[TypeLibFunc(64)]
		[param: In]
		set;
	}

	[DispId(223)]
	int ExplicitLeft
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(223)]
		get;
	}

	[DispId(224)]
	int ExplicitTop
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(224)]
		get;
	}

	[DispId(225)]
	int ExplicitWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(225)]
		[TypeLibFunc(64)]
		get;
	}

	[DispId(226)]
	int ExplicitHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(226)]
		get;
	}

	[DispId(227)]
	bool AlignWithMargins
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
	bool ParentCustomHint
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(228)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(228)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(229)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string SMS_Ver();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(230)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string SMS_Select_Device([In] short Indx);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(231)]
	short SMS_Device_Count();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(232)]
	bool SMS_Set_Vol([In][MarshalAs(UnmanagedType.BStr)] string Volume_1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(233)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string SMS_Dev_INFO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(234)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string SMS_S_N();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(235)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string SMS_Soft_Ver();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(236)]
	bool GSM_Test_Connect();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(237)]
	bool SMS_PDU_Mode([In] bool PDU);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(238)]
	bool SMS_Char_Mode([In] short Char_Mode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(239)]
	bool Send_Text_SMS([In] bool PDU, [In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_TXT);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(240)]
	bool SMS_Delete([In] int Indx, [In] int Del_Type);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(241)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_Text_SMS([In] int Indx, [In] int SMS_State);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(242)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_PDU_SMS([In] int Indx);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(243)]
	int Get_SMS_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(244)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_Product_Name();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(245)]
	bool Set_Pulse_Dialing();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(246)]
	bool Set_Tone_Dialing();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(247)]
	bool Disconnect_Voice_Call();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(249)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_SIM_PIN();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(248)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string Set_SIM_PIN([In][MarshalAs(UnmanagedType.BStr)] string SIM_PIN);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(250)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_GSM_Date_Time();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(251)]
	bool Set_GSM_Date_Time([In] short GSM_Year, [In] short GSM_Month, [In] short GSM_Day, [In] short GSM_Hour, [In] short GSM_Minute, [In] short GSM_Second);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(252)]
	bool Get_Battery([In][Out][MarshalAs(UnmanagedType.BStr)] ref string voltage, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Percent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(253)]
	bool GSM_Reset_Default();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(254)]
	bool GSM_Reset_Factory();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(255)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_Product_SN();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(256)]
	bool GSM_Dial_NO([In][MarshalAs(UnmanagedType.BStr)] string CountryCode, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Res_MSG);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(257)]
	bool GSM_Answer_Call();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(258)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_Signal();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(259)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_Service_Center();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(260)]
	bool Set_Service_Center([In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Service_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(261)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_CCDID();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(262)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_CIMI();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(263)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_GSM_Ver();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(264)]
	bool Send_SMS_PDU([In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(265)]
	bool GSM_Emerg_Rest();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(266)]
	bool R_SMS([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Date_Time, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(267)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_MSG([In][Out] ref int CMD, [In][Out] ref int Len);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(268)]
	bool GSM_Ready();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(269)]
	bool SMS_Battery([In][Out] ref int Percent, [In][Out] ref int voltage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(270)]
	bool SMS_Battery_Calib([In][MarshalAs(UnmanagedType.BStr)] string Pass);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(271)]
	bool SMS_Bat_charging([In][Out] ref short Status);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(272)]
	bool GSM_T([In] int Indx);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(273)]
	bool GSM_Set_Baud([In] int Baud);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(274)]
	bool SMS_Set_Baud([In] int Baud);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(275)]
	bool GSM_ON();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(276)]
	bool GSM_OFF();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(277)]
	bool GSM_Set_IP([In] int IP1, [In] int IP2, [In] int IP3, [In] int IP4, [In] int Port);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(278)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_Send_AT([In][MarshalAs(UnmanagedType.BStr)] string AT, [In][Out] ref int Response_Len);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(279)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_Response([In] int R_Wait, [In] int Indx, [In][Out] ref int Response_Len);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(280)]
	bool GSM_Set_Device_NO([In] short D_NO);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(281)]
	short GSM_Get_Device_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(282)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_SMS2([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Date_Time, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(283)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_SMS3([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Time_stamp, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Discharge, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_PID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_DCS, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_VP, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_ST, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Reference);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(284)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string Send_SMS_PDU2([In][MarshalAs(UnmanagedType.BStr)] string Country_Code, [In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(285)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_GetUSSD();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(286)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GSM_Send_USSD([In][MarshalAs(UnmanagedType.BStr)] string S_USSD);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(287)]
	bool HID_CMD([In] short S_CMD, [In][MarshalAs(UnmanagedType.BStr)] string S_Data);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(288)]
	void DeVice_Type([In] bool H_Device);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(289)]
	bool S_Ready_Modem();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(290)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_SMS_Ref();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(291)]
	bool R_GSM_POW();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(292)]
	void Ad_Pass([In][MarshalAs(UnmanagedType.BStr)] string Pass);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(293)]
	void Clear_Buf();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(294)]
	short Last_Buf_Index();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(295)]
	bool Set_Chage([In] short On_Off);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(296)]
	bool Insert_PIN([In][MarshalAs(UnmanagedType.BStr)] string I_PIN);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(297)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string Selected_Network();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(298)]
	void Res_AT_Delay([In] int S_Delay);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(299)]
	bool Change_SMS_Center([In][MarshalAs(UnmanagedType.BStr)] string SMS_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(300)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_SMS_Center();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(301)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_IMEI();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(302)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_SIM_ID();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(303)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_Natttional_Mobile_ID();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(304)]
	bool Get_SMS_Space([In][Out] ref int Free_Space, [In][Out] ref int Total_Size);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(305)]
	void Get_AT_Res([In][Out][MarshalAs(UnmanagedType.BStr)] ref string AT_Res);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(306)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string R_SMS4([In] bool PDU, [In] int Indx, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Time_stamp, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Discharge, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_PID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_DCS, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_VP, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_ST, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Reference, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string All_Part, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Part_NO);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(307)]
	bool Active_New_SMS_Event([In] bool NS_Active);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(308)]
	bool Recieved_New_SMS([In][Out] ref int Indx_From, [In][Out] ref int Indx_To);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(309)]
	void S_SMS_Delivery([In] short Deliver);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(310)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string S_SMS3([In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Text, [In] bool Request_Delivery_Report, [In] short Send_Interval, [In] short Retry_Times, [In] short SMS_Validity, [In] short SMS_Reference, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(311)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string Send_SMS_Error_Code();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(312)]
	void Add_Group_SMS_Buffer([In][MarshalAs(UnmanagedType.BStr)] string Phone_NO, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Context);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(313)]
	void SMS_Buf_Start([In] bool Start);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(314)]
	void SMS_Buf_Clear([In] bool Stop);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(315)]
	int Get_SMS_Buf_Indx();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(316)]
	bool Get_SMS_Buf_Status([In] int index, [In][Out] ref bool Send_Done);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(317)]
	void Group_SMS_Setting([In] bool SMS_Delivery, [In] int SMS_Validity, [In] int SMS_NO_Change_Sim, [In] int Repeat_NO, [In] int Send_Interval, [In][MarshalAs(UnmanagedType.BStr)] string SMS_Center);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(318)]
	void SMS_Buf_Pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(319)]
	int Get_GSM_Message([In][Out][MarshalAs(UnmanagedType.BStr)] ref string Message_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(320)]
	void Set_Timer_Interval([In] int Pass, [In] int Interval);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(323)]
	int Total_Oflline_SMS_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(324)]
	bool Clear_Oflline_SMS_NO();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(325)]
	bool Show_Oflline_SMS([In] int index, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Center, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Type, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Tel_Number, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Time_stamp, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Discharge, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_PID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_DCS, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_VP, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TP_ST, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string S_Reference, [In][Out] ref int All_Part, [In][Out] ref int Part_NO, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SMS_Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(326)]
	int Get_SMS_Buf_Send_Indx();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(321)]
	void Offline_SMS_Active([In] bool Active);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(322)]
	bool G_Offline_SMS_Active();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(327)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string CallIn_Number();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(328)]
	bool CallerID_Enable([In] bool CID_Active);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(329)]
	bool Select_SIM_Network();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(330)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string ATCommand([In][MarshalAs(UnmanagedType.BStr)] string Param, [In] int AT_Delay);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(331)]
	int Group_Send_NO_SMS();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(332)]
	bool Group_SMS_Finish();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(333)]
	int Get_SMS_Buf_Sended();
}
