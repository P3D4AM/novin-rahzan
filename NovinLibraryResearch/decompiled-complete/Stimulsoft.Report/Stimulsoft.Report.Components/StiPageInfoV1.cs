using System.Collections;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiPageInfoV1 : StiComponentInfo
{
	internal StiComponentsCollection Overlays;

	internal bool Processed;

	public Hashtable DelimiterComponentsLeft = new Hashtable();

	public bool IsFirstDataBandOnPage = true;

	public int PageNumber = 1;

	public int TotalPageCount = -1;

	public double TopPageContentPosition;

	public double BottomPageContentPosition;

	public bool IsPrevPage;

	public bool RestoreCurrentDetailComponent = true;

	public int NumberOfCopiesToPrint = 1;
}
