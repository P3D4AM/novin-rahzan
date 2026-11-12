using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class DataItemVisualEventArgs : EventArgs
{
	public object Visual;

	public readonly object DataItem;

	public DataItemVisualEventArgs(object visual, object dataItem)
	{
		Visual = visual;
		DataItem = dataItem;
	}
}
