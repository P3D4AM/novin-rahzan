using MSFlexGridLib;

namespace AxMSFlexGridLib;

public class DMSFlexGridEvents_OLEDragOverEvent
{
	public DataObject data;

	public int effect;

	public short button;

	public short shift;

	public float x;

	public float y;

	public short state;

	public DMSFlexGridEvents_OLEDragOverEvent(DataObject data, int effect, short button, short shift, float x, float y, short state)
	{
		this.data = data;
		this.effect = effect;
		this.button = button;
		this.shift = shift;
		this.x = x;
		this.y = y;
		this.state = state;
	}
}
