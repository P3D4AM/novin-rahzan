using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
[StiGdiPainter(typeof(StiUndefinedComponentGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiUndefinedComponentWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiUndefinedComponent : StiComponent
{
	public override StiComponentId ComponentId => StiComponentId.StiUndefinedComponent;

	public override int ToolboxPosition => 101;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => "Undefined Component";

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public StiUndefinedComponent()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiUndefinedComponent(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
