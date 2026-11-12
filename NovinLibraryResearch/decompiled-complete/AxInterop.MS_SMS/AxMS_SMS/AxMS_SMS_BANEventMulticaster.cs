using System;
using System.Runtime.InteropServices;
using MS_SMS;

namespace AxMS_SMS;

[ClassInterface(ClassInterfaceType.None)]
public class AxMS_SMS_BANEventMulticaster : IMS_SMS_BANEvents
{
	private AxMS_SMS_BAN parent;

	public AxMS_SMS_BANEventMulticaster(AxMS_SMS_BAN parent)
	{
		this.parent = parent;
	}

	public virtual void OnActivate()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnActivate(parent, e);
	}

	public virtual void OnAfterMonitorDpiChanged(int oldDPI, int newDPI)
	{
		IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEvent e = new IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEvent(oldDPI, newDPI);
		parent.RaiseOnOnAfterMonitorDpiChanged(parent, e);
	}

	public virtual void OnBeforeMonitorDpiChanged(int oldDPI, int newDPI)
	{
		IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEvent e = new IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEvent(oldDPI, newDPI);
		parent.RaiseOnOnBeforeMonitorDpiChanged(parent, e);
	}

	public virtual void OnClick()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnClick(parent, e);
	}

	public virtual void OnCreate()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnCreate(parent, e);
	}

	public virtual void OnDblClick()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnDblClick(parent, e);
	}

	public virtual void OnDestroy()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnDestroy(parent, e);
	}

	public virtual void OnDeactivate()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnDeactivate(parent, e);
	}

	public virtual void OnKeyPress(ref short key)
	{
		IMS_SMS_BANEvents_OnKeyPressEvent iMS_SMS_BANEvents_OnKeyPressEvent = new IMS_SMS_BANEvents_OnKeyPressEvent(key);
		parent.RaiseOnOnKeyPress(parent, iMS_SMS_BANEvents_OnKeyPressEvent);
		key = iMS_SMS_BANEvents_OnKeyPressEvent.key;
	}

	public virtual void OnMouseEnter()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnMouseEnter(parent, e);
	}

	public virtual void OnMouseLeave()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnMouseLeave(parent, e);
	}

	public virtual void OnPaint()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnPaint(parent, e);
	}

	public virtual void OnSMS()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnSMS(parent, e);
	}

	public virtual void OnGSMEvent()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnGSMEvent(parent, e);
	}
}
