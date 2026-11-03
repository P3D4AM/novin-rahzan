using System.Runtime.InteropServices;

namespace zkemkeeper;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class _IZKEMEvents_SinkHelper : _IZKEMEvents
{
	public _IZKEMEvents_OnAttTransactionEventHandler m_OnAttTransactionDelegate;

	public _IZKEMEvents_OnKeyPressEventHandler m_OnKeyPressDelegate;

	public _IZKEMEvents_OnEnrollFingerEventHandler m_OnEnrollFingerDelegate;

	public _IZKEMEvents_OnNewUserEventHandler m_OnNewUserDelegate;

	public _IZKEMEvents_OnEMDataEventHandler m_OnEMDataDelegate;

	public _IZKEMEvents_OnConnectedEventHandler m_OnConnectedDelegate;

	public _IZKEMEvents_OnDisConnectedEventHandler m_OnDisConnectedDelegate;

	public _IZKEMEvents_OnFingerEventHandler m_OnFingerDelegate;

	public _IZKEMEvents_OnVerifyEventHandler m_OnVerifyDelegate;

	public _IZKEMEvents_OnFingerFeatureEventHandler m_OnFingerFeatureDelegate;

	public _IZKEMEvents_OnHIDNumEventHandler m_OnHIDNumDelegate;

	public _IZKEMEvents_OnDoorEventHandler m_OnDoorDelegate;

	public _IZKEMEvents_OnAlarmEventHandler m_OnAlarmDelegate;

	public _IZKEMEvents_OnWriteCardEventHandler m_OnWriteCardDelegate;

	public _IZKEMEvents_OnEmptyCardEventHandler m_OnEmptyCardDelegate;

	public _IZKEMEvents_OnDeleteTemplateEventHandler m_OnDeleteTemplateDelegate;

	public _IZKEMEvents_OnAttTransactionExEventHandler m_OnAttTransactionExDelegate;

	public _IZKEMEvents_OnEnrollFingerExEventHandler m_OnEnrollFingerExDelegate;

	public int m_dwCookie;

	public void OnAttTransaction(int P_0, int P_1, int P_2, int P_3, int P_4, int P_5, int P_6, int P_7, int P_8, int P_9)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnAttTransactionDelegate != null)
		{
			m_OnAttTransactionDelegate(P_0, P_1, P_2, P_3, P_4, P_5, P_6, P_7, P_8, P_9);
		}
	}

	public void OnKeyPress(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnKeyPressDelegate != null)
		{
			m_OnKeyPressDelegate(P_0);
		}
	}

	public void OnEnrollFinger(int P_0, int P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnEnrollFingerDelegate != null)
		{
			m_OnEnrollFingerDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void OnNewUser(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnNewUserDelegate != null)
		{
			m_OnNewUserDelegate(P_0);
		}
	}

	public void OnEMData(int P_0, int P_1, ref sbyte P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnEMDataDelegate != null)
		{
			m_OnEMDataDelegate(P_0, P_1, ref P_2);
		}
	}

	public void OnConnected()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnConnectedDelegate != null)
		{
			m_OnConnectedDelegate();
		}
	}

	public void OnDisConnected()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnDisConnectedDelegate != null)
		{
			m_OnDisConnectedDelegate();
		}
	}

	public void OnFinger()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnFingerDelegate != null)
		{
			m_OnFingerDelegate();
		}
	}

	public void OnVerify(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnVerifyDelegate != null)
		{
			m_OnVerifyDelegate(P_0);
		}
	}

	public void OnFingerFeature(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnFingerFeatureDelegate != null)
		{
			m_OnFingerFeatureDelegate(P_0);
		}
	}

	public void OnHIDNum(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnHIDNumDelegate != null)
		{
			m_OnHIDNumDelegate(P_0);
		}
	}

	public void OnDoor(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnDoorDelegate != null)
		{
			m_OnDoorDelegate(P_0);
		}
	}

	public void OnAlarm(int P_0, int P_1, int P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnAlarmDelegate != null)
		{
			m_OnAlarmDelegate(P_0, P_1, P_2);
		}
	}

	public void OnWriteCard(int P_0, int P_1, int P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnWriteCardDelegate != null)
		{
			m_OnWriteCardDelegate(P_0, P_1, P_2);
		}
	}

	public void OnEmptyCard(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnEmptyCardDelegate != null)
		{
			m_OnEmptyCardDelegate(P_0);
		}
	}

	public void OnDeleteTemplate(int P_0, int P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnDeleteTemplateDelegate != null)
		{
			m_OnDeleteTemplateDelegate(P_0, P_1);
		}
	}

	public void OnAttTransactionEx(string P_0, int P_1, int P_2, int P_3, int P_4, int P_5, int P_6, int P_7, int P_8, int P_9, int P_10)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnAttTransactionExDelegate != null)
		{
			m_OnAttTransactionExDelegate(P_0, P_1, P_2, P_3, P_4, P_5, P_6, P_7, P_8, P_9, P_10);
		}
	}

	public void OnEnrollFingerEx(string P_0, int P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnEnrollFingerExDelegate != null)
		{
			m_OnEnrollFingerExDelegate(P_0, P_1, P_2, P_3);
		}
	}

	internal _IZKEMEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_OnAttTransactionDelegate = null;
		m_OnKeyPressDelegate = null;
		m_OnEnrollFingerDelegate = null;
		m_OnNewUserDelegate = null;
		m_OnEMDataDelegate = null;
		m_OnConnectedDelegate = null;
		m_OnDisConnectedDelegate = null;
		m_OnFingerDelegate = null;
		m_OnVerifyDelegate = null;
		m_OnFingerFeatureDelegate = null;
		m_OnHIDNumDelegate = null;
		m_OnDoorDelegate = null;
		m_OnAlarmDelegate = null;
		m_OnWriteCardDelegate = null;
		m_OnEmptyCardDelegate = null;
		m_OnDeleteTemplateDelegate = null;
		m_OnAttTransactionExDelegate = null;
		m_OnEnrollFingerExDelegate = null;
	}
}
