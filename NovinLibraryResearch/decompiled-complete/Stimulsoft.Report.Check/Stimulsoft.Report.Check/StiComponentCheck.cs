using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Check;

public abstract class StiComponentCheck : StiCheck
{
	public override StiCheckObjectType ObjectType => StiCheckObjectType.Component;

	public override string ElementName
	{
		get
		{
			if (base.Element == null)
			{
				return null;
			}
			return ((StiComponent)base.Element).Name;
		}
	}
}
