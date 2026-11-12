namespace AxMSFlexGridLib;

public class DMSFlexGridEvents_CompareEvent
{
	public int row1;

	public int row2;

	public short cmp;

	public DMSFlexGridEvents_CompareEvent(int row1, int row2, short cmp)
	{
		this.row1 = row1;
		this.row2 = row2;
		this.cmp = cmp;
	}
}
