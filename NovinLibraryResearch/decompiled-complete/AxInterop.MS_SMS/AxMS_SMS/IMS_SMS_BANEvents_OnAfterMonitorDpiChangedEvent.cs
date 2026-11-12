namespace AxMS_SMS;

public class IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEvent
{
	public int oldDPI;

	public int newDPI;

	public IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEvent(int oldDPI, int newDPI)
	{
		this.oldDPI = oldDPI;
		this.newDPI = newDPI;
	}
}
