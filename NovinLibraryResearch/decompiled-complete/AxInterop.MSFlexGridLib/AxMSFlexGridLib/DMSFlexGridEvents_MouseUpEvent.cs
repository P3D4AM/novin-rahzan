namespace AxMSFlexGridLib;

public class DMSFlexGridEvents_MouseUpEvent
{
	public short button;

	public short shift;

	public int x;

	public int y;

	public DMSFlexGridEvents_MouseUpEvent(short button, short shift, int x, int y)
	{
		this.button = button;
		this.shift = shift;
		this.x = x;
		this.y = y;
	}
}
