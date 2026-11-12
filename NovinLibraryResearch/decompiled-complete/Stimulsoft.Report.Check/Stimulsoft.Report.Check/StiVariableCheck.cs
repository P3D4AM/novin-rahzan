using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Check;

public abstract class StiVariableCheck : StiCheck
{
	public override bool PreviewVisible => false;

	public override StiCheckObjectType ObjectType => StiCheckObjectType.Variable;

	public override string ElementName
	{
		get
		{
			if (base.Element != null)
			{
				return ((StiVariable)base.Element).Name;
			}
			return null;
		}
	}
}
