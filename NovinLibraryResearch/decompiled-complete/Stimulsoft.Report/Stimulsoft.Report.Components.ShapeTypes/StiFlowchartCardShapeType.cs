using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.FlowchartCard.png")]
[StiGdiShapeTypePainter(typeof(StiFlowchartCardGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiFlowchartCardWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiFlowchartCardShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "FlowchartCard");

	public override StiComponentId ComponentId => StiComponentId.StiFlowchartCardShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiFlowchartCardShapeType();
	}
}
