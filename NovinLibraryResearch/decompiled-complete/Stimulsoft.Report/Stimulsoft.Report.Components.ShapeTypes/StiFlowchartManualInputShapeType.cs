using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.FlowchartManualInput.png")]
[StiGdiShapeTypePainter(typeof(StiFlowchartManualInputGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiFlowchartManualInputWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiFlowchartManualInputShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "FlowchartManualInput");

	public override StiComponentId ComponentId => StiComponentId.StiFlowchartManualInputShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiFlowchartManualInputShapeType();
	}
}
