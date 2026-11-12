using MSFlexGridLib;

namespace AxMSFlexGridLib;

public class DMSFlexGridEvents_OLEStartDragEvent
{
	public DataObject data;

	public int allowedEffects;

	public DMSFlexGridEvents_OLEStartDragEvent(DataObject data, int allowedEffects)
	{
		this.data = data;
		this.allowedEffects = allowedEffects;
	}
}
