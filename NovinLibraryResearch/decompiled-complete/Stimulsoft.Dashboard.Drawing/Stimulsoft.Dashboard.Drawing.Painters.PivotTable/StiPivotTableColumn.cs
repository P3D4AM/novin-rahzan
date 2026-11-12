namespace Stimulsoft.Dashboard.Drawing.Painters.PivotTable;

public class StiPivotTableColumn
{
	public double Width { get; set; }

	public double Left { get; set; }

	public bool Visible { get; set; }

	public StiPivotTableColumn(double width)
	{
		Width = width;
		Visible = true;
	}
}
