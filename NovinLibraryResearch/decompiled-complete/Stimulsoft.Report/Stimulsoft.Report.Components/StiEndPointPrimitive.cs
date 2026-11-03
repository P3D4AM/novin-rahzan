using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
public class StiEndPointPrimitive : StiPointPrimitive
{
	public override StiComponentId ComponentId => StiComponentId.StiEndPointPrimitive;

	public override string LocalizedName => "End Point Primitive";

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override StiComponent CreateNew()
	{
		return new StiEndPointPrimitive();
	}

	public StiEndPointPrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiEndPointPrimitive(RectangleD rect)
		: base(rect)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)

}
