using System.Runtime.InteropServices;

namespace zkemkeeper;

[ComVisible(false)]
[ComEventInterface(typeof(_IZKEMEvents_0000), typeof(_IZKEMEvents_EventProvider_0000))]
[TypeLibType(16)]
public interface _IZKEMEvents_Event
{
	event _IZKEMEvents_OnAttTransactionEventHandler OnAttTransaction;

	event _IZKEMEvents_OnKeyPressEventHandler OnKeyPress;

	event _IZKEMEvents_OnEnrollFingerEventHandler OnEnrollFinger;

	event _IZKEMEvents_OnNewUserEventHandler OnNewUser;

	event _IZKEMEvents_OnEMDataEventHandler OnEMData;

	event _IZKEMEvents_OnConnectedEventHandler OnConnected;

	event _IZKEMEvents_OnDisConnectedEventHandler OnDisConnected;

	event _IZKEMEvents_OnFingerEventHandler OnFinger;

	event _IZKEMEvents_OnVerifyEventHandler OnVerify;

	event _IZKEMEvents_OnFingerFeatureEventHandler OnFingerFeature;

	event _IZKEMEvents_OnHIDNumEventHandler OnHIDNum;

	event _IZKEMEvents_OnDoorEventHandler OnDoor;

	event _IZKEMEvents_OnAlarmEventHandler OnAlarm;

	event _IZKEMEvents_OnWriteCardEventHandler OnWriteCard;

	event _IZKEMEvents_OnEmptyCardEventHandler OnEmptyCard;

	event _IZKEMEvents_OnDeleteTemplateEventHandler OnDeleteTemplate;

	event _IZKEMEvents_OnAttTransactionExEventHandler OnAttTransactionEx;

	event _IZKEMEvents_OnEnrollFingerExEventHandler OnEnrollFingerEx;
}
