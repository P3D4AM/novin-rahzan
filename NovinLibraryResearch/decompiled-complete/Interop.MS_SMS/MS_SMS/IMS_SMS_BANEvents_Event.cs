using System.Runtime.InteropServices;

namespace MS_SMS;

[TypeLibType(16)]
[ComEventInterface(typeof(IMS_SMS_BANEvents_0000), typeof(IMS_SMS_BANEvents_EventProvider_0000))]
[ComVisible(false)]
public interface IMS_SMS_BANEvents_Event
{
	event IMS_SMS_BANEvents_OnActivateEventHandler OnActivate;

	event IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEventHandler OnAfterMonitorDpiChanged;

	event IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEventHandler OnBeforeMonitorDpiChanged;

	event IMS_SMS_BANEvents_OnClickEventHandler OnClick;

	event IMS_SMS_BANEvents_OnCreateEventHandler OnCreate;

	event IMS_SMS_BANEvents_OnDblClickEventHandler OnDblClick;

	event IMS_SMS_BANEvents_OnDestroyEventHandler OnDestroy;

	event IMS_SMS_BANEvents_OnDeactivateEventHandler OnDeactivate;

	event IMS_SMS_BANEvents_OnKeyPressEventHandler OnKeyPress;

	event IMS_SMS_BANEvents_OnMouseEnterEventHandler OnMouseEnter;

	event IMS_SMS_BANEvents_OnMouseLeaveEventHandler OnMouseLeave;

	event IMS_SMS_BANEvents_OnPaintEventHandler OnPaint;

	event IMS_SMS_BANEvents_OnSMSEventHandler OnSMS;

	event IMS_SMS_BANEvents_OnGSMEventEventHandler OnGSMEvent;
}
