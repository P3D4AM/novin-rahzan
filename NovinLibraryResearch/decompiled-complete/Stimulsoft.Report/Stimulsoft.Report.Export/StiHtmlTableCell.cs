using System.Collections;

namespace Stimulsoft.Report.Export;

public class StiHtmlTableCell
{
	public StiHtmlUnit Width;

	public StiHtmlUnit Height;

	public Hashtable Style;

	public int ColumnSpan;

	public int RowSpan;

	public string CssClass;

	public string Text;

	public string ToolTip;

	public ArrayList Controls;

	public string Id;

	public string Interaction;

	public string Collapsed;

	public string SortDirection;

	public string DataBandSort;

	public string PageGuid;

	public string PageIndex;

	public string ReportFile;

	public string ComponentIndex;

	public string Editable;

	public string DrillDownMode;

	public StiHtmlTableCell()
	{
		Style = new Hashtable();
		Controls = new ArrayList();
	}
}
