using System.Runtime.InteropServices;

namespace MS_SMS;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class IMS_SMS_BANEvents_SinkHelper : IMS_SMS_BANEvents
{
	public IMS_SMS_BANEvents_OnActivateEventHandler m_OnActivateDelegate;

	public IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEventHandler m_OnAfterMonitorDpiChangedDelegate;

	public IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEventHandler m_OnBeforeMonitorDpiChangedDelegate;

	public IMS_SMS_BANEvents_OnClickEventHandler m_OnClickDelegate;

	public IMS_SMS_BANEvents_OnCreateEventHandler m_OnCreateDelegate;

	public IMS_SMS_BANEvents_OnDblClickEventHandler m_OnDblClickDelegate;

	public IMS_SMS_BANEvents_OnDestroyEventHandler m_OnDestroyDelegate;

	public IMS_SMS_BANEvents_OnDeactivateEventHandler m_OnDeactivateDelegate;

	public IMS_SMS_BANEvents_OnKeyPressEventHandler m_OnKeyPressDelegate;

	public IMS_SMS_BANEvents_OnMouseEnterEventHandler m_OnMouseEnterDelegate;

	public IMS_SMS_BANEvents_OnMouseLeaveEventHandler m_OnMouseLeaveDelegate;

	public IMS_SMS_BANEvents_OnPaintEventHandler m_OnPaintDelegate;

	public IMS_SMS_BANEvents_OnSMSEventHandler m_OnSMSDelegate;

	public IMS_SMS_BANEvents_OnGSMEventEventHandler m_OnGSMEventDelegate;

	public int m_dwCookie;

	public void OnActivate()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnActivateDelegate != null)
		{
			m_OnActivateDelegate();
		}
	}

	public void OnAfterMonitorDpiChanged(int P_0, int P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnAfterMonitorDpiChangedDelegate != null)
		{
			m_OnAfterMonitorDpiChangedDelegate(P_0, P_1);
		}
	}

	public void OnBeforeMonitorDpiChanged(int P_0, int P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnBeforeMonitorDpiChangedDelegate != null)
		{
			m_OnBeforeMonitorDpiChangedDelegate(P_0, P_1);
		}
	}

	public void OnClick()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnClickDelegate != null)
		{
			m_OnClickDelegate();
		}
	}

	public void OnCreate()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnCreateDelegate != null)
		{
			m_OnCreateDelegate();
		}
	}

	public void OnDblClick()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnDblClickDelegate != null)
		{
			m_OnDblClickDelegate();
		}
	}

	public void OnDestroy()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnDestroyDelegate != null)
		{
			m_OnDestroyDelegate();
		}
	}

	public void OnDeactivate()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnDeactivateDelegate != null)
		{
			m_OnDeactivateDelegate();
		}
	}

	public void OnKeyPress(ref short P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnKeyPressDelegate != null)
		{
			m_OnKeyPressDelegate(ref P_0);
		}
	}

	public void OnMouseEnter()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnMouseEnterDelegate != null)
		{
			m_OnMouseEnterDelegate();
		}
	}

	public void OnMouseLeave()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnMouseLeaveDelegate != null)
		{
			m_OnMouseLeaveDelegate();
		}
	}

	public void OnPaint()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnPaintDelegate != null)
		{
			m_OnPaintDelegate();
		}
	}

	public void OnSMS()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnSMSDelegate != null)
		{
			m_OnSMSDelegate();
		}
	}

	public void OnGSMEvent()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnGSMEventDelegate != null)
		{
			m_OnGSMEventDelegate();
		}
	}

	internal IMS_SMS_BANEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_OnActivateDelegate = null;
		m_OnAfterMonitorDpiChangedDelegate = null;
		m_OnBeforeMonitorDpiChangedDelegate = null;
		m_OnClickDelegate = null;
		m_OnCreateDelegate = null;
		m_OnDblClickDelegate = null;
		m_OnDestroyDelegate = null;
		m_OnDeactivateDelegate = null;
		m_OnKeyPressDelegate = null;
		m_OnMouseEnterDelegate = null;
		m_OnMouseLeaveDelegate = null;
		m_OnPaintDelegate = null;
		m_OnSMSDelegate = null;
		m_OnGSMEventDelegate = null;
	}
}
