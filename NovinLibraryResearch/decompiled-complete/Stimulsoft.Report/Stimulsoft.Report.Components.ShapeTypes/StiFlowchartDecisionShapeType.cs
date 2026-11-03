using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.FlowchartDecision.png")]
[StiGdiShapeTypePainter(typeof(StiFlowchartDecisionGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiFlowchartDecisionWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiFlowchartDecisionShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "FlowchartDecision");

	public override StiComponentId ComponentId => StiComponentId.StiFlowchartDecisionShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiFlowchartDecisionShapeType();
	}
}
