namespace Axzkemkeeper;

public class _IZKEMEvents_OnWriteCardEvent
{
	public int enrollNumber;

	public int actionResult;

	public int length;

	public _IZKEMEvents_OnWriteCardEvent(int enrollNumber, int actionResult, int length)
	{
		this.enrollNumber = enrollNumber;
		this.actionResult = actionResult;
		this.length = length;
	}
}
