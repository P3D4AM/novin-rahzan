using MSFlexGridLib;

namespace AxMSFlexGridLib;

public class DMSFlexGridEvents_OLEDragDropEvent
{
	public DataObject data;

	public int effect;

	public short button;

	public short shift;

	public float x;

	public float y;

	public DMSFlexGridEvents_OLEDragDropEvent(DataObject data, int effect, short button, short shift, float x, float y)
	{
		this.data = data;
		this.effect = effect;
		this.button = button;
		this.shift = shift;
		this.x = x;
		this.y = y;
	}
}
