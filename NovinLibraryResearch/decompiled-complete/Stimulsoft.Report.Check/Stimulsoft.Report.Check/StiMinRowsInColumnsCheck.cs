using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiMinRowsInColumnsCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiMinRowsInColumnsCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiMinRowsInColumnsCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiDataBand { MinRowsInColumn: >0, Columns: 0 })
		{
			return true;
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (Check())
			{
				StiMinRowsInColumnsCheck stiMinRowsInColumnsCheck = new StiMinRowsInColumnsCheck();
				stiMinRowsInColumnsCheck.Element = obj;
				stiMinRowsInColumnsCheck.Actions.Add(new StiMinRowsInColumnsAction());
				return stiMinRowsInColumnsCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
