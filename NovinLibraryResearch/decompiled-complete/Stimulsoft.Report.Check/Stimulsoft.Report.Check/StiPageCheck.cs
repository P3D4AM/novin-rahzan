using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Check;

public abstract class StiPageCheck : StiCheck
{
	public override bool PreviewVisible => true;

	public override StiCheckObjectType ObjectType => StiCheckObjectType.Page;

	public override string ElementName
	{
		get
		{
			if (base.Element == null)
			{
				return null;
			}
			return ((StiPage)base.Element).Name;
		}
	}
}
