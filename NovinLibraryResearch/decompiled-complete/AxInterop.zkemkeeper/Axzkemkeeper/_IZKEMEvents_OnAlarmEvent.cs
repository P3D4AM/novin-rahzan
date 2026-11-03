namespace Axzkemkeeper;

public class _IZKEMEvents_OnAlarmEvent
{
	public int alarmType;

	public int enrollNumber;

	public int verified;

	public _IZKEMEvents_OnAlarmEvent(int alarmType, int enrollNumber, int verified)
	{
		this.alarmType = alarmType;
		this.enrollNumber = enrollNumber;
		this.verified = verified;
	}
}
