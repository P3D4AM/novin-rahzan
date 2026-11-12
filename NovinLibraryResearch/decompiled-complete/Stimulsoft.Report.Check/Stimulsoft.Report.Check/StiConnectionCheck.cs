using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Check;

public abstract class StiConnectionCheck : StiCheck
{
	public override StiCheckObjectType ObjectType => StiCheckObjectType.Database;

	public override string ElementName
	{
		get
		{
			if (base.Element == null)
			{
				return null;
			}
			return ((StiDatabase)base.Element).Name;
		}
	}
}
