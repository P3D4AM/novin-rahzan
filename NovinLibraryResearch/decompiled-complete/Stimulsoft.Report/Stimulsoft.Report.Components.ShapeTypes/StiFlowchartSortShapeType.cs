using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.FlowchartSort.png")]
[StiGdiShapeTypePainter(typeof(StiFlowchartSortGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiFlowchartSortWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiFlowchartSortShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "FlowchartSort");

	public override StiComponentId ComponentId => StiComponentId.StiFlowchartSortShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiFlowchartSortShapeType();
	}
}
