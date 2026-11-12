namespace Stimulsoft.Report.Check;

public abstract class StiReportCheck : StiCheck
{
	public override StiCheckObjectType ObjectType => StiCheckObjectType.Report;

	public override string ElementName
	{
		get
		{
			if (base.Element == null)
			{
				return null;
			}
			return ((StiReport)base.Element).ReportName;
		}
	}
}
