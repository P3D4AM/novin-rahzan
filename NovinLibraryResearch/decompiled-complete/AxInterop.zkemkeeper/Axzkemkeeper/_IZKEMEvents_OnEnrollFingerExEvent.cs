namespace Axzkemkeeper;

public class _IZKEMEvents_OnEnrollFingerExEvent
{
	public string enrollNumber;

	public int fingerIndex;

	public int actionResult;

	public int templateLength;

	public _IZKEMEvents_OnEnrollFingerExEvent(string enrollNumber, int fingerIndex, int actionResult, int templateLength)
	{
		this.enrollNumber = enrollNumber;
		this.fingerIndex = fingerIndex;
		this.actionResult = actionResult;
		this.templateLength = templateLength;
	}
}
