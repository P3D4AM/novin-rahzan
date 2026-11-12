using MSFlexGridLib;

namespace AxMSFlexGridLib;

public class DMSFlexGridEvents_OLESetDataEvent
{
	public DataObject data;

	public short dataFormat;

	public DMSFlexGridEvents_OLESetDataEvent(DataObject data, short dataFormat)
	{
		this.data = data;
		this.dataFormat = dataFormat;
	}
}
