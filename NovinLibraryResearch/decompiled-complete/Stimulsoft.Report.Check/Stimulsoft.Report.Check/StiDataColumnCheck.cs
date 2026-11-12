using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Check;

public abstract class StiDataColumnCheck : StiCheck
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	public override bool PreviewVisible => false;

	public override StiCheckObjectType ObjectType => StiCheckObjectType.DataColumn;

	public override string ElementName
	{
		get
		{
			if (base.Element == null)
			{
				return null;
			}
			return ((StiDataColumn)base.Element).Name;
		}
	}
}
