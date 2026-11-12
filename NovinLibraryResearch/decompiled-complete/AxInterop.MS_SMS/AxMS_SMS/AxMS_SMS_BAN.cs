using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MS_SMS;
using stdole;

namespace AxMS_SMS;

[DesignTimeVisible(true)]
[Clsid("{f69fc669-2e6d-4d10-b14f-65b5fbcd8481}")]
public class AxMS_SMS_BAN : AxHost
{
	private IMS_SMS_BAN ocx;

	private AxMS_SMS_BANEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DispId(201)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool CtlVisible
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CtlVisible", (ActiveXInvokeKind)1);
			}
			return ocx.Visible;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CtlVisible", (ActiveXInvokeKind)2);
			}
			ocx.Visible = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(202)]
	public virtual bool AutoScroll
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AutoScroll", (ActiveXInvokeKind)1);
			}
			return ocx.AutoScroll;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AutoScroll", (ActiveXInvokeKind)2);
			}
			ocx.AutoScroll = value;
		}
	}

	[DispId(203)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool CtlAutoSize
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CtlAutoSize", (ActiveXInvokeKind)1);
			}
			return ocx.AutoSize;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CtlAutoSize", (ActiveXInvokeKind)2);
			}
			ocx.AutoSize = value;
		}
	}

	[DispId(204)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual TxActiveFormBorderStyle AxBorderStyle
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AxBorderStyle", (ActiveXInvokeKind)1);
			}
			return ocx.AxBorderStyle;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AxBorderStyle", (ActiveXInvokeKind)2);
			}
			ocx.AxBorderStyle = value;
		}
	}

	[DispId(205)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int BorderWidth
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BorderWidth", (ActiveXInvokeKind)1);
			}
			return ocx.BorderWidth;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("BorderWidth", (ActiveXInvokeKind)2);
			}
			ocx.BorderWidth = value;
		}
	}

	[DispId(-518)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string Caption
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Caption", (ActiveXInvokeKind)1);
			}
			return ocx.Caption;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Caption", (ActiveXInvokeKind)2);
			}
			ocx.Caption = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[ComAliasName("System.UInt32")]
	[DispId(-501)]
	public virtual Color Color
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Color", (ActiveXInvokeKind)1);
			}
			return AxHost.GetColorFromOleColor(ocx.Color);
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Color", (ActiveXInvokeKind)2);
			}
			ocx.Color = AxHost.GetOleColorFromColor(value);
		}
	}

	[ComAliasName("stdole.IFontDisp")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(-512)]
	[Browsable(true)]
	public override Font Font
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return AxHost.GetFontFromIFont((object)ocx.Font);
			}
			return ((AxHost)this).Font;
		}
		set
		{
			IFontDisp value2 = (IFontDisp)AxHost.GetIFontFromFont(value);
			((AxHost)this).Font = value;
			if (ocx != null)
			{
				ocx.let_Font(value2);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(206)]
	public virtual bool KeyPreview
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("KeyPreview", (ActiveXInvokeKind)1);
			}
			return ocx.KeyPreview;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("KeyPreview", (ActiveXInvokeKind)2);
			}
			ocx.KeyPreview = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(207)]
	public virtual int PixelsPerInch
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PixelsPerInch", (ActiveXInvokeKind)1);
			}
			return ocx.PixelsPerInch;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PixelsPerInch", (ActiveXInvokeKind)2);
			}
			ocx.PixelsPerInch = value;
		}
	}

	[DispId(208)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual TxPrintScale PrintScale
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PrintScale", (ActiveXInvokeKind)1);
			}
			return ocx.PrintScale;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PrintScale", (ActiveXInvokeKind)2);
			}
			ocx.PrintScale = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(209)]
	public virtual bool Scaled
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Scaled", (ActiveXInvokeKind)1);
			}
			return ocx.Scaled;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Scaled", (ActiveXInvokeKind)2);
			}
			ocx.Scaled = value;
		}
	}

	[DispId(210)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool Active
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Active", (ActiveXInvokeKind)1);
			}
			return ocx.Active;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(211)]
	public virtual bool DropTarget
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DropTarget", (ActiveXInvokeKind)1);
			}
			return ocx.DropTarget;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DropTarget", (ActiveXInvokeKind)2);
			}
			ocx.DropTarget = value;
		}
	}

	[DispId(212)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual string HelpFile
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("HelpFile", (ActiveXInvokeKind)1);
			}
			return ocx.HelpFile;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("HelpFile", (ActiveXInvokeKind)2);
			}
			ocx.HelpFile = value;
		}
	}

	[DispId(213)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual TxPopupMode PopupMode
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PopupMode", (ActiveXInvokeKind)1);
			}
			return ocx.PopupMode;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("PopupMode", (ActiveXInvokeKind)2);
			}
			ocx.PopupMode = value;
		}
	}

	[DispId(214)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool ScreenSnap
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ScreenSnap", (ActiveXInvokeKind)1);
			}
			return ocx.ScreenSnap;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ScreenSnap", (ActiveXInvokeKind)2);
			}
			ocx.ScreenSnap = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(215)]
	public virtual int SnapBuffer
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("SnapBuffer", (ActiveXInvokeKind)1);
			}
			return ocx.SnapBuffer;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("SnapBuffer", (ActiveXInvokeKind)2);
			}
			ocx.SnapBuffer = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(216)]
	public virtual bool DockSite
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DockSite", (ActiveXInvokeKind)1);
			}
			return ocx.DockSite;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DockSite", (ActiveXInvokeKind)2);
			}
			ocx.DockSite = value;
		}
	}

	[DispId(217)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool DoubleBuffered
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DoubleBuffered", (ActiveXInvokeKind)1);
			}
			return ocx.DoubleBuffered;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("DoubleBuffered", (ActiveXInvokeKind)2);
			}
			ocx.DoubleBuffered = value;
		}
	}

	[DispId(218)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AlignDisabled
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AlignDisabled", (ActiveXInvokeKind)1);
			}
			return ocx.AlignDisabled;
		}
	}

	[DispId(219)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool MouseInClient
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MouseInClient", (ActiveXInvokeKind)1);
			}
			return ocx.MouseInClient;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(220)]
	public virtual int VisibleDockClientCount
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("VisibleDockClientCount", (ActiveXInvokeKind)1);
			}
			return ocx.VisibleDockClientCount;
		}
	}

	[DispId(221)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool ParentDoubleBuffered
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ParentDoubleBuffered", (ActiveXInvokeKind)1);
			}
			return ocx.ParentDoubleBuffered;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ParentDoubleBuffered", (ActiveXInvokeKind)2);
			}
			ocx.ParentDoubleBuffered = value;
		}
	}

	[DispId(222)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool UseDockManager
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("UseDockManager", (ActiveXInvokeKind)1);
			}
			return ocx.UseDockManager;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("UseDockManager", (ActiveXInvokeKind)2);
			}
			ocx.UseDockManager = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(-514)]
	public override bool Enabled
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return ocx.Enabled;
			}
			return ((AxHost)this).Enabled;
		}
		set
		{
			((AxHost)this).Enabled = value;
			if (ocx != null)
			{
				ocx.Enabled = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(223)]
	[Browsable(false)]
	public virtual int ExplicitLeft
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ExplicitLeft", (ActiveXInvokeKind)1);
			}
			return ocx.ExplicitLeft;
		}
	}

	[DispId(224)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual int ExplicitTop
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ExplicitTop", (ActiveXInvokeKind)1);
			}
			return ocx.ExplicitTop;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(225)]
	public virtual int ExplicitWidth
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ExplicitWidth", (ActiveXInvokeKind)1);
			}
			return ocx.ExplicitWidth;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(226)]
	public virtual int ExplicitHeight
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ExplicitHeight", (ActiveXInvokeKind)1);
			}
			return ocx.ExplicitHeight;
		}
	}

	[DispId(227)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AlignWithMargins
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AlignWithMargins", (ActiveXInvokeKind)1);
			}
			return ocx.AlignWithMargins;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("AlignWithMargins", (ActiveXInvokeKind)2);
			}
			ocx.AlignWithMargins = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(228)]
	public virtual bool ParentCustomHint
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ParentCustomHint", (ActiveXInvokeKind)1);
			}
			return ocx.ParentCustomHint;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ParentCustomHint", (ActiveXInvokeKind)2);
			}
			ocx.ParentCustomHint = value;
		}
	}

	public event EventHandler OnActivate;

	public event IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEventHandler OnAfterMonitorDpiChanged;

	public event IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEventHandler OnBeforeMonitorDpiChanged;

	public event EventHandler OnClick;

	public event EventHandler OnCreate;

	public event EventHandler OnDblClick;

	public event EventHandler OnDestroy;

	public event EventHandler OnDeactivate;

	public event IMS_SMS_BANEvents_OnKeyPressEventHandler OnKeyPress;

	public event EventHandler OnMouseEnter;

	public event EventHandler OnMouseLeave;

	public event EventHandler OnPaint;

	public event EventHandler OnSMS;

	public event EventHandler OnGSMEvent;

	public AxMS_SMS_BAN()
		: base("f69fc669-2e6d-4d10-b14f-65b5fbcd8481")
	{
	}

	public virtual bool GSM_Answer_Call()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Answer_Call", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Answer_Call();
	}

	public virtual string GSM_Signal()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Signal", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Signal();
	}

	public virtual string R_Service_Center()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_Service_Center", (ActiveXInvokeKind)0);
		}
		return ocx.R_Service_Center();
	}

	public virtual bool Set_Service_Center(string country_Code, string service_Center)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Set_Service_Center", (ActiveXInvokeKind)0);
		}
		return ocx.Set_Service_Center(country_Code, service_Center);
	}

	public virtual string GSM_CCDID()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_CCDID", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_CCDID();
	}

	public virtual string GSM_CIMI()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_CIMI", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_CIMI();
	}

	public virtual string R_GSM_Ver()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_GSM_Ver", (ActiveXInvokeKind)0);
		}
		return ocx.R_GSM_Ver();
	}

	public virtual bool Send_SMS_PDU(string country_Code, string phone_NO, string sMS_Text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Send_SMS_PDU", (ActiveXInvokeKind)0);
		}
		return ocx.Send_SMS_PDU(country_Code, phone_NO, sMS_Text);
	}

	public virtual bool GSM_Emerg_Rest()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Emerg_Rest", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Emerg_Rest();
	}

	public virtual bool R_SMS(bool pDU, int indx, ref string sMS_Center, ref string sMS_Type, ref string tel_Number, ref string sMS_Date_Time, ref string sMS_Text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SMS", (ActiveXInvokeKind)0);
		}
		return ocx.R_SMS(pDU, indx, ref sMS_Center, ref sMS_Type, ref tel_Number, ref sMS_Date_Time, ref sMS_Text);
	}

	public virtual string GSM_MSG(ref int cMD, ref int len)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_MSG", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_MSG(ref cMD, ref len);
	}

	public virtual bool GSM_Ready()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Ready", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Ready();
	}

	public virtual bool SMS_Battery(ref int percent, ref int voltage)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Battery", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Battery(ref percent, ref voltage);
	}

	public virtual bool SMS_Battery_Calib(string pass)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Battery_Calib", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Battery_Calib(pass);
	}

	public virtual bool SMS_Bat_charging(ref short status)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Bat_charging", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Bat_charging(ref status);
	}

	public virtual bool GSM_T(int indx)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_T", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_T(indx);
	}

	public virtual bool GSM_Set_Baud(int baud)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Set_Baud", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Set_Baud(baud);
	}

	public virtual bool SMS_Set_Baud(int baud)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Set_Baud", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Set_Baud(baud);
	}

	public virtual bool GSM_ON()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_ON", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_ON();
	}

	public virtual bool GSM_OFF()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_OFF", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_OFF();
	}

	public virtual bool GSM_Set_IP(int iP1, int iP2, int iP3, int iP4, int port)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Set_IP", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Set_IP(iP1, iP2, iP3, iP4, port);
	}

	public virtual string GSM_Send_AT(string aT, ref int response_Len)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Send_AT", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Send_AT(aT, ref response_Len);
	}

	public virtual string GSM_Response(int r_Wait, int indx, ref int response_Len)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Response", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Response(r_Wait, indx, ref response_Len);
	}

	public virtual bool GSM_Set_Device_NO(short d_NO)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Set_Device_NO", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Set_Device_NO(d_NO);
	}

	public virtual short GSM_Get_Device_NO()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Get_Device_NO", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Get_Device_NO();
	}

	public virtual string R_SMS2(bool pDU, int indx, ref string sMS_Center, ref string sMS_Type, ref string tel_Number, ref string sMS_Date_Time, ref string sMS_Text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SMS2", (ActiveXInvokeKind)0);
		}
		return ocx.R_SMS2(pDU, indx, ref sMS_Center, ref sMS_Type, ref tel_Number, ref sMS_Date_Time, ref sMS_Text);
	}

	public virtual string R_SMS3(bool pDU, int indx, ref string sMS_Center, ref string sMS_Type, ref string tel_Number, ref string time_stamp, ref string s_Discharge, ref string sMS_Text, ref string tP_PID, ref string tP_DCS, ref string tP_VP, ref string tP_ST, ref string s_Reference)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SMS3", (ActiveXInvokeKind)0);
		}
		return ocx.R_SMS3(pDU, indx, ref sMS_Center, ref sMS_Type, ref tel_Number, ref time_stamp, ref s_Discharge, ref sMS_Text, ref tP_PID, ref tP_DCS, ref tP_VP, ref tP_ST, ref s_Reference);
	}

	public virtual string Send_SMS_PDU2(string country_Code, string phone_NO, string sMS_Text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Send_SMS_PDU2", (ActiveXInvokeKind)0);
		}
		return ocx.Send_SMS_PDU2(country_Code, phone_NO, sMS_Text);
	}

	public virtual string GSM_GetUSSD()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_GetUSSD", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_GetUSSD();
	}

	public virtual string GSM_Send_USSD(string s_USSD)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Send_USSD", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Send_USSD(s_USSD);
	}

	public virtual bool HID_CMD(short s_CMD, string s_Data)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("HID_CMD", (ActiveXInvokeKind)0);
		}
		return ocx.HID_CMD(s_CMD, s_Data);
	}

	public virtual void DeVice_Type(bool h_Device)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeVice_Type", (ActiveXInvokeKind)0);
		}
		ocx.DeVice_Type(h_Device);
	}

	public virtual bool S_Ready_Modem()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("S_Ready_Modem", (ActiveXInvokeKind)0);
		}
		return ocx.S_Ready_Modem();
	}

	public virtual string R_SMS_Ref()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SMS_Ref", (ActiveXInvokeKind)0);
		}
		return ocx.R_SMS_Ref();
	}

	public virtual bool R_GSM_POW()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_GSM_POW", (ActiveXInvokeKind)0);
		}
		return ocx.R_GSM_POW();
	}

	public virtual void Ad_Pass(string pass)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Ad_Pass", (ActiveXInvokeKind)0);
		}
		ocx.Ad_Pass(pass);
	}

	public virtual void Clear_Buf()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Clear_Buf", (ActiveXInvokeKind)0);
		}
		ocx.Clear_Buf();
	}

	public virtual short Last_Buf_Index()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Last_Buf_Index", (ActiveXInvokeKind)0);
		}
		return ocx.Last_Buf_Index();
	}

	public virtual bool Set_Chage(short on_Off)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Set_Chage", (ActiveXInvokeKind)0);
		}
		return ocx.Set_Chage(on_Off);
	}

	public virtual bool Insert_PIN(string i_PIN)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Insert_PIN", (ActiveXInvokeKind)0);
		}
		return ocx.Insert_PIN(i_PIN);
	}

	public virtual string Selected_Network()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Selected_Network", (ActiveXInvokeKind)0);
		}
		return ocx.Selected_Network();
	}

	public virtual void Res_AT_Delay(int s_Delay)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Res_AT_Delay", (ActiveXInvokeKind)0);
		}
		ocx.Res_AT_Delay(s_Delay);
	}

	public virtual bool Change_SMS_Center(string sMS_Center)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Change_SMS_Center", (ActiveXInvokeKind)0);
		}
		return ocx.Change_SMS_Center(sMS_Center);
	}

	public virtual string R_SMS_Center()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SMS_Center", (ActiveXInvokeKind)0);
		}
		return ocx.R_SMS_Center();
	}

	public virtual string R_IMEI()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_IMEI", (ActiveXInvokeKind)0);
		}
		return ocx.R_IMEI();
	}

	public virtual string R_SIM_ID()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SIM_ID", (ActiveXInvokeKind)0);
		}
		return ocx.R_SIM_ID();
	}

	public virtual string R_Natttional_Mobile_ID()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_Natttional_Mobile_ID", (ActiveXInvokeKind)0);
		}
		return ocx.R_Natttional_Mobile_ID();
	}

	public virtual bool Get_SMS_Space(ref int free_Space, ref int total_Size)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_SMS_Space", (ActiveXInvokeKind)0);
		}
		return ocx.Get_SMS_Space(ref free_Space, ref total_Size);
	}

	public virtual void Get_AT_Res(ref string aT_Res)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_AT_Res", (ActiveXInvokeKind)0);
		}
		ocx.Get_AT_Res(ref aT_Res);
	}

	public virtual string R_SMS4(bool pDU, int indx, ref string sMS_Center, ref string sMS_Type, ref string tel_Number, ref string time_stamp, ref string s_Discharge, ref string sMS_Text, ref string tP_PID, ref string tP_DCS, ref string tP_VP, ref string tP_ST, ref string s_Reference, ref string all_Part, ref string part_NO)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SMS4", (ActiveXInvokeKind)0);
		}
		return ocx.R_SMS4(pDU, indx, ref sMS_Center, ref sMS_Type, ref tel_Number, ref time_stamp, ref s_Discharge, ref sMS_Text, ref tP_PID, ref tP_DCS, ref tP_VP, ref tP_ST, ref s_Reference, ref all_Part, ref part_NO);
	}

	public virtual bool Active_New_SMS_Event(bool nS_Active)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Active_New_SMS_Event", (ActiveXInvokeKind)0);
		}
		return ocx.Active_New_SMS_Event(nS_Active);
	}

	public virtual bool Recieved_New_SMS(ref int indx_From, ref int indx_To)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Recieved_New_SMS", (ActiveXInvokeKind)0);
		}
		return ocx.Recieved_New_SMS(ref indx_From, ref indx_To);
	}

	public virtual void S_SMS_Delivery(short deliver)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("S_SMS_Delivery", (ActiveXInvokeKind)0);
		}
		ocx.S_SMS_Delivery(deliver);
	}

	public virtual string S_SMS3(string phone_NO, string sMS_Text, bool request_Delivery_Report, short send_Interval, short retry_Times, short sMS_Validity, short sMS_Reference, string sMS_Center)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("S_SMS3", (ActiveXInvokeKind)0);
		}
		return ocx.S_SMS3(phone_NO, sMS_Text, request_Delivery_Report, send_Interval, retry_Times, sMS_Validity, sMS_Reference, sMS_Center);
	}

	public virtual string Send_SMS_Error_Code()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Send_SMS_Error_Code", (ActiveXInvokeKind)0);
		}
		return ocx.Send_SMS_Error_Code();
	}

	public virtual void Add_Group_SMS_Buffer(string phone_NO, string sMS_Context)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Add_Group_SMS_Buffer", (ActiveXInvokeKind)0);
		}
		ocx.Add_Group_SMS_Buffer(phone_NO, sMS_Context);
	}

	public virtual void SMS_Buf_Start(bool start)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Buf_Start", (ActiveXInvokeKind)0);
		}
		ocx.SMS_Buf_Start(start);
	}

	public virtual void SMS_Buf_Clear(bool stop)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Buf_Clear", (ActiveXInvokeKind)0);
		}
		ocx.SMS_Buf_Clear(stop);
	}

	public virtual int Get_SMS_Buf_Indx()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_SMS_Buf_Indx", (ActiveXInvokeKind)0);
		}
		return ocx.Get_SMS_Buf_Indx();
	}

	public virtual bool Get_SMS_Buf_Status(int index, ref bool send_Done)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_SMS_Buf_Status", (ActiveXInvokeKind)0);
		}
		return ocx.Get_SMS_Buf_Status(index, ref send_Done);
	}

	public virtual void Group_SMS_Setting(bool sMS_Delivery, int sMS_Validity, int sMS_NO_Change_Sim, int repeat_NO, int send_Interval, string sMS_Center)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Group_SMS_Setting", (ActiveXInvokeKind)0);
		}
		ocx.Group_SMS_Setting(sMS_Delivery, sMS_Validity, sMS_NO_Change_Sim, repeat_NO, send_Interval, sMS_Center);
	}

	public virtual void SMS_Buf_Pause()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Buf_Pause", (ActiveXInvokeKind)0);
		}
		ocx.SMS_Buf_Pause();
	}

	public virtual int Get_GSM_Message(ref string message_Text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_GSM_Message", (ActiveXInvokeKind)0);
		}
		return ocx.Get_GSM_Message(ref message_Text);
	}

	public virtual void Set_Timer_Interval(int pass, int interval)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Set_Timer_Interval", (ActiveXInvokeKind)0);
		}
		ocx.Set_Timer_Interval(pass, interval);
	}

	public virtual int Total_Oflline_SMS_NO()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Total_Oflline_SMS_NO", (ActiveXInvokeKind)0);
		}
		return ocx.Total_Oflline_SMS_NO();
	}

	public virtual bool Clear_Oflline_SMS_NO()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Clear_Oflline_SMS_NO", (ActiveXInvokeKind)0);
		}
		return ocx.Clear_Oflline_SMS_NO();
	}

	public virtual bool Show_Oflline_SMS(int index, ref string sMS_Center, ref string sMS_Type, ref string tel_Number, ref string time_stamp, ref string s_Discharge, ref string tP_PID, ref string tP_DCS, ref string tP_VP, ref string tP_ST, ref string s_Reference, ref int all_Part, ref int part_NO, ref string sMS_Text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Show_Oflline_SMS", (ActiveXInvokeKind)0);
		}
		return ocx.Show_Oflline_SMS(index, ref sMS_Center, ref sMS_Type, ref tel_Number, ref time_stamp, ref s_Discharge, ref tP_PID, ref tP_DCS, ref tP_VP, ref tP_ST, ref s_Reference, ref all_Part, ref part_NO, ref sMS_Text);
	}

	public virtual int Get_SMS_Buf_Send_Indx()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_SMS_Buf_Send_Indx", (ActiveXInvokeKind)0);
		}
		return ocx.Get_SMS_Buf_Send_Indx();
	}

	public virtual void Offline_SMS_Active(bool active)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Offline_SMS_Active", (ActiveXInvokeKind)0);
		}
		ocx.Offline_SMS_Active(active);
	}

	public virtual bool G_Offline_SMS_Active()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("G_Offline_SMS_Active", (ActiveXInvokeKind)0);
		}
		return ocx.G_Offline_SMS_Active();
	}

	public virtual string CallIn_Number()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CallIn_Number", (ActiveXInvokeKind)0);
		}
		return ocx.CallIn_Number();
	}

	public virtual bool CallerID_Enable(bool cID_Active)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CallerID_Enable", (ActiveXInvokeKind)0);
		}
		return ocx.CallerID_Enable(cID_Active);
	}

	public virtual bool Select_SIM_Network()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Select_SIM_Network", (ActiveXInvokeKind)0);
		}
		return ocx.Select_SIM_Network();
	}

	public virtual string ATCommand(string param, int aT_Delay)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ATCommand", (ActiveXInvokeKind)0);
		}
		return ocx.ATCommand(param, aT_Delay);
	}

	public virtual int Group_Send_NO_SMS()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Group_Send_NO_SMS", (ActiveXInvokeKind)0);
		}
		return ocx.Group_Send_NO_SMS();
	}

	public virtual bool Group_SMS_Finish()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Group_SMS_Finish", (ActiveXInvokeKind)0);
		}
		return ocx.Group_SMS_Finish();
	}

	public virtual int Get_SMS_Buf_Sended()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_SMS_Buf_Sended", (ActiveXInvokeKind)0);
		}
		return ocx.Get_SMS_Buf_Sended();
	}

	public virtual string SMS_Ver()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Ver", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Ver();
	}

	public virtual string SMS_Select_Device(short indx)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Select_Device", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Select_Device(indx);
	}

	public virtual short SMS_Device_Count()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Device_Count", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Device_Count();
	}

	public virtual bool SMS_Set_Vol(string volume_1)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Set_Vol", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Set_Vol(volume_1);
	}

	public virtual string SMS_Dev_INFO()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Dev_INFO", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Dev_INFO();
	}

	public virtual string SMS_S_N()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_S_N", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_S_N();
	}

	public virtual string SMS_Soft_Ver()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Soft_Ver", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Soft_Ver();
	}

	public virtual bool GSM_Test_Connect()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Test_Connect", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Test_Connect();
	}

	public virtual bool SMS_PDU_Mode(bool pDU)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_PDU_Mode", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_PDU_Mode(pDU);
	}

	public virtual bool SMS_Char_Mode(short char_Mode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Char_Mode", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Char_Mode(char_Mode);
	}

	public virtual bool Send_Text_SMS(bool pDU, string country_Code, string phone_NO, string sMS_TXT)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Send_Text_SMS", (ActiveXInvokeKind)0);
		}
		return ocx.Send_Text_SMS(pDU, country_Code, phone_NO, sMS_TXT);
	}

	public virtual bool SMS_Delete(int indx, int del_Type)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SMS_Delete", (ActiveXInvokeKind)0);
		}
		return ocx.SMS_Delete(indx, del_Type);
	}

	public virtual string R_Text_SMS(int indx, int sMS_State)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_Text_SMS", (ActiveXInvokeKind)0);
		}
		return ocx.R_Text_SMS(indx, sMS_State);
	}

	public virtual string R_PDU_SMS(int indx)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_PDU_SMS", (ActiveXInvokeKind)0);
		}
		return ocx.R_PDU_SMS(indx);
	}

	public virtual int Get_SMS_NO()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_SMS_NO", (ActiveXInvokeKind)0);
		}
		return ocx.Get_SMS_NO();
	}

	public virtual string R_Product_Name()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_Product_Name", (ActiveXInvokeKind)0);
		}
		return ocx.R_Product_Name();
	}

	public virtual bool Set_Pulse_Dialing()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Set_Pulse_Dialing", (ActiveXInvokeKind)0);
		}
		return ocx.Set_Pulse_Dialing();
	}

	public virtual bool Set_Tone_Dialing()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Set_Tone_Dialing", (ActiveXInvokeKind)0);
		}
		return ocx.Set_Tone_Dialing();
	}

	public virtual bool Disconnect_Voice_Call()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Disconnect_Voice_Call", (ActiveXInvokeKind)0);
		}
		return ocx.Disconnect_Voice_Call();
	}

	public virtual string R_SIM_PIN()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_SIM_PIN", (ActiveXInvokeKind)0);
		}
		return ocx.R_SIM_PIN();
	}

	public virtual string Set_SIM_PIN(string sIM_PIN)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Set_SIM_PIN", (ActiveXInvokeKind)0);
		}
		return ocx.Set_SIM_PIN(sIM_PIN);
	}

	public virtual string R_GSM_Date_Time()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("R_GSM_Date_Time", (ActiveXInvokeKind)0);
		}
		return ocx.R_GSM_Date_Time();
	}

	public virtual bool Set_GSM_Date_Time(short gSM_Year, short gSM_Month, short gSM_Day, short gSM_Hour, short gSM_Minute, short gSM_Second)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Set_GSM_Date_Time", (ActiveXInvokeKind)0);
		}
		return ocx.Set_GSM_Date_Time(gSM_Year, gSM_Month, gSM_Day, gSM_Hour, gSM_Minute, gSM_Second);
	}

	public virtual bool Get_Battery(ref string voltage, ref string percent)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Get_Battery", (ActiveXInvokeKind)0);
		}
		return ocx.Get_Battery(ref voltage, ref percent);
	}

	public virtual bool GSM_Reset_Default()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Reset_Default", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Reset_Default();
	}

	public virtual bool GSM_Reset_Factory()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Reset_Factory", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Reset_Factory();
	}

	public virtual string GSM_Product_SN()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Product_SN", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Product_SN();
	}

	public virtual bool GSM_Dial_NO(string countryCode, string phone_NO, ref string res_MSG)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GSM_Dial_NO", (ActiveXInvokeKind)0);
		}
		return ocx.GSM_Dial_NO(countryCode, phone_NO, ref res_MSG);
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxMS_SMS_BANEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(IMS_SMS_BANEvents));
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
			ocx = (IMS_SMS_BAN)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnOnActivate(object sender, EventArgs e)
	{
		if (this.OnActivate != null)
		{
			this.OnActivate(sender, e);
		}
	}

	internal void RaiseOnOnAfterMonitorDpiChanged(object sender, IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEvent e)
	{
		if (this.OnAfterMonitorDpiChanged != null)
		{
			this.OnAfterMonitorDpiChanged(sender, e);
		}
	}

	internal void RaiseOnOnBeforeMonitorDpiChanged(object sender, IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEvent e)
	{
		if (this.OnBeforeMonitorDpiChanged != null)
		{
			this.OnBeforeMonitorDpiChanged(sender, e);
		}
	}

	internal void RaiseOnOnClick(object sender, EventArgs e)
	{
		if (this.OnClick != null)
		{
			this.OnClick(sender, e);
		}
	}

	internal void RaiseOnOnCreate(object sender, EventArgs e)
	{
		if (this.OnCreate != null)
		{
			this.OnCreate(sender, e);
		}
	}

	internal void RaiseOnOnDblClick(object sender, EventArgs e)
	{
		if (this.OnDblClick != null)
		{
			this.OnDblClick(sender, e);
		}
	}

	internal void RaiseOnOnDestroy(object sender, EventArgs e)
	{
		if (this.OnDestroy != null)
		{
			this.OnDestroy(sender, e);
		}
	}

	internal void RaiseOnOnDeactivate(object sender, EventArgs e)
	{
		if (this.OnDeactivate != null)
		{
			this.OnDeactivate(sender, e);
		}
	}

	internal void RaiseOnOnKeyPress(object sender, IMS_SMS_BANEvents_OnKeyPressEvent e)
	{
		if (this.OnKeyPress != null)
		{
			this.OnKeyPress(sender, e);
		}
	}

	internal void RaiseOnOnMouseEnter(object sender, EventArgs e)
	{
		if (this.OnMouseEnter != null)
		{
			this.OnMouseEnter(sender, e);
		}
	}

	internal void RaiseOnOnMouseLeave(object sender, EventArgs e)
	{
		if (this.OnMouseLeave != null)
		{
			this.OnMouseLeave(sender, e);
		}
	}

	internal void RaiseOnOnPaint(object sender, EventArgs e)
	{
		if (this.OnPaint != null)
		{
			this.OnPaint(sender, e);
		}
	}

	internal void RaiseOnOnSMS(object sender, EventArgs e)
	{
		if (this.OnSMS != null)
		{
			this.OnSMS(sender, e);
		}
	}

	internal void RaiseOnOnGSMEvent(object sender, EventArgs e)
	{
		if (this.OnGSMEvent != null)
		{
			this.OnGSMEvent(sender, e);
		}
	}
}
