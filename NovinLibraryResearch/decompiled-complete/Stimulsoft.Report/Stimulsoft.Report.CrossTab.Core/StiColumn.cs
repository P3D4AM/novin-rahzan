using System.Collections.Generic;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiColumn
{
	public object HyperlinkValue { get; set; }

	public object TagValue { get; set; }

	public object ToolTipValue { get; set; }

	public Dictionary<string, object> DrillDownParameters { get; set; }

	public bool IsTotal { get; set; }

	public int Level { get; set; } = -1;


	public StiColumnCollection Cols { get; } = new StiColumnCollection();


	public object Value { get; set; }

	public object DisplayValue { get; set; }

	public string OthersText { get; set; }

	public StiColumn(object value, object displayValue)
	{
		Value = value;
		DisplayValue = displayValue;
	}
}
