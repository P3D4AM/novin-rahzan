namespace Axzkemkeeper;

public class _IZKEMEvents_OnEnrollFingerEvent
{
	public int enrollNumber;

	public int fingerIndex;

	public int actionResult;

	public int templateLength;

	public _IZKEMEvents_OnEnrollFingerEvent(int enrollNumber, int fingerIndex, int actionResult, int templateLength)
	{
		this.enrollNumber = enrollNumber;
		this.fingerIndex = fingerIndex;
		this.actionResult = actionResult;
		this.templateLength = templateLength;
	}
}
