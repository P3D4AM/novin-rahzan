using System.Collections.Generic;

namespace Stimulsoft.Controls;

public class StiDataGridViewPosition
{
	public int HorizontalScroll { get; set; } = -1;


	public int VerticalScroll { get; set; } = -1;


	public int RowIndex { get; set; } = -1;


	public int ColumnIndex { get; set; } = -1;


	public Dictionary<string, int> ColumnWidths { get; set; } = new Dictionary<string, int>();

}
