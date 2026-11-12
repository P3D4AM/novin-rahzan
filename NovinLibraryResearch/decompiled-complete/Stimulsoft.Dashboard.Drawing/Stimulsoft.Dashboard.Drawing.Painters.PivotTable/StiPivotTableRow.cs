using System.Collections.Generic;

namespace Stimulsoft.Dashboard.Drawing.Painters.PivotTable;

public class StiPivotTableRow
{
	internal List<StiPivotTableCell> Cells { get; set; } = new List<StiPivotTableCell>();


	internal StiPivotTableContainer Container { get; set; }

	public double Height { get; set; }

	public double Top { get; set; }

	public bool Visible { get; set; }

	public bool IsEven { get; set; }

	public StiPivotTableRow(StiPivotTableContainer container, double height)
	{
		Container = container;
		Height = height;
		Visible = true;
	}
}
