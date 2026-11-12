namespace AxMSFlexGridLib;

public class DMSFlexGridEvents_KeyDownEvent
{
	public short keyCode;

	public short shift;

	public DMSFlexGridEvents_KeyDownEvent(short keyCode, short shift)
	{
		this.keyCode = keyCode;
		this.shift = shift;
	}
}
