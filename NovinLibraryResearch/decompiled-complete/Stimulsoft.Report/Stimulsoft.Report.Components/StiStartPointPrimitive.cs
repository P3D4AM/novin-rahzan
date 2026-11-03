using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
public class StiStartPointPrimitive : StiPointPrimitive
{
	public override StiComponentId ComponentId => StiComponentId.StiStartPointPrimitive;

	public override string LocalizedName => "Start Point Primitive";

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
		return new StiStartPointPrimitive();
	}

	public StiStartPointPrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiStartPointPrimitive(RectangleD rect)
		: base(rect)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)

}
