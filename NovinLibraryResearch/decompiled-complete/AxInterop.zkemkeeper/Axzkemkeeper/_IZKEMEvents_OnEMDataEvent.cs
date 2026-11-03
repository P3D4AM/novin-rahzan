namespace Axzkemkeeper;

public class _IZKEMEvents_OnEMDataEvent
{
	public int dataType;

	public int dataLen;

	public sbyte dataBuffer;

	public _IZKEMEvents_OnEMDataEvent(int dataType, int dataLen, sbyte dataBuffer)
	{
		this.dataType = dataType;
		this.dataLen = dataLen;
		this.dataBuffer = dataBuffer;
	}
}
