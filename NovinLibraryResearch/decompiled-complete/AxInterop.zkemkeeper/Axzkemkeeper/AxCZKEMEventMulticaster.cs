using System;
using System.Runtime.InteropServices;
using zkemkeeper;

namespace Axzkemkeeper;

[ClassInterface(ClassInterfaceType.None)]
public class AxCZKEMEventMulticaster : _IZKEMEvents
{
	private AxCZKEM parent;

	public AxCZKEMEventMulticaster(AxCZKEM parent)
	{
		this.parent = parent;
	}

	public virtual void OnAttTransaction(int enrollNumber, int isInValid, int attState, int verifyMethod, int year, int month, int day, int hour, int minute, int second)
	{
		_IZKEMEvents_OnAttTransactionEvent e = new _IZKEMEvents_OnAttTransactionEvent(enrollNumber, isInValid, attState, verifyMethod, year, month, day, hour, minute, second);
		parent.RaiseOnOnAttTransaction(parent, e);
	}

	public virtual void OnKeyPress(int key)
	{
		_IZKEMEvents_OnKeyPressEvent e = new _IZKEMEvents_OnKeyPressEvent(key);
		parent.RaiseOnOnKeyPress(parent, e);
	}

	public virtual void OnEnrollFinger(int enrollNumber, int fingerIndex, int actionResult, int templateLength)
	{
		_IZKEMEvents_OnEnrollFingerEvent e = new _IZKEMEvents_OnEnrollFingerEvent(enrollNumber, fingerIndex, actionResult, templateLength);
		parent.RaiseOnOnEnrollFinger(parent, e);
	}

	public virtual void OnNewUser(int enrollNumber)
	{
		_IZKEMEvents_OnNewUserEvent e = new _IZKEMEvents_OnNewUserEvent(enrollNumber);
		parent.RaiseOnOnNewUser(parent, e);
	}

	public virtual void OnEMData(int dataType, int dataLen, ref sbyte dataBuffer)
	{
		_IZKEMEvents_OnEMDataEvent iZKEMEvents_OnEMDataEvent = new _IZKEMEvents_OnEMDataEvent(dataType, dataLen, dataBuffer);
		parent.RaiseOnOnEMData(parent, iZKEMEvents_OnEMDataEvent);
		dataBuffer = iZKEMEvents_OnEMDataEvent.dataBuffer;
	}

	public virtual void OnConnected()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnConnected(parent, e);
	}

	public virtual void OnDisConnected()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnDisConnected(parent, e);
	}

	public virtual void OnFinger()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnOnFinger(parent, e);
	}

	public virtual void OnVerify(int userID)
	{
		_IZKEMEvents_OnVerifyEvent e = new _IZKEMEvents_OnVerifyEvent(userID);
		parent.RaiseOnOnVerify(parent, e);
	}

	public virtual void OnFingerFeature(int score)
	{
		_IZKEMEvents_OnFingerFeatureEvent e = new _IZKEMEvents_OnFingerFeatureEvent(score);
		parent.RaiseOnOnFingerFeature(parent, e);
	}

	public virtual void OnHIDNum(int cardNumber)
	{
		_IZKEMEvents_OnHIDNumEvent e = new _IZKEMEvents_OnHIDNumEvent(cardNumber);
		parent.RaiseOnOnHIDNum(parent, e);
	}

	public virtual void OnDoor(int eventType)
	{
		_IZKEMEvents_OnDoorEvent e = new _IZKEMEvents_OnDoorEvent(eventType);
		parent.RaiseOnOnDoor(parent, e);
	}

	public virtual void OnAlarm(int alarmType, int enrollNumber, int verified)
	{
		_IZKEMEvents_OnAlarmEvent e = new _IZKEMEvents_OnAlarmEvent(alarmType, enrollNumber, verified);
		parent.RaiseOnOnAlarm(parent, e);
	}

	public virtual void OnWriteCard(int enrollNumber, int actionResult, int length)
	{
		_IZKEMEvents_OnWriteCardEvent e = new _IZKEMEvents_OnWriteCardEvent(enrollNumber, actionResult, length);
		parent.RaiseOnOnWriteCard(parent, e);
	}

	public virtual void OnEmptyCard(int actionResult)
	{
		_IZKEMEvents_OnEmptyCardEvent e = new _IZKEMEvents_OnEmptyCardEvent(actionResult);
		parent.RaiseOnOnEmptyCard(parent, e);
	}

	public virtual void OnDeleteTemplate(int enrollNumber, int fingerIndex)
	{
		_IZKEMEvents_OnDeleteTemplateEvent e = new _IZKEMEvents_OnDeleteTemplateEvent(enrollNumber, fingerIndex);
		parent.RaiseOnOnDeleteTemplate(parent, e);
	}

	public virtual void OnAttTransactionEx(string enrollNumber, int isInValid, int attState, int verifyMethod, int year, int month, int day, int hour, int minute, int second, int workCode)
	{
		_IZKEMEvents_OnAttTransactionExEvent e = new _IZKEMEvents_OnAttTransactionExEvent(enrollNumber, isInValid, attState, verifyMethod, year, month, day, hour, minute, second, workCode);
		parent.RaiseOnOnAttTransactionEx(parent, e);
	}

	public virtual void OnEnrollFingerEx(string enrollNumber, int fingerIndex, int actionResult, int templateLength)
	{
		_IZKEMEvents_OnEnrollFingerExEvent e = new _IZKEMEvents_OnEnrollFingerExEvent(enrollNumber, fingerIndex, actionResult, templateLength);
		parent.RaiseOnOnEnrollFingerEx(parent, e);
	}
}
