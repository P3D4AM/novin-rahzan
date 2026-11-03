using System.Collections;

namespace Stimulsoft.Report.Export;

public class StiHtmlTableRow
{
	public Hashtable Style;

	public ArrayList Cells;

	public StiHtmlUnit Height;

	public StiHtmlTableRow()
	{
		Style = new Hashtable();
		Cells = new ArrayList();
	}
}
