using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Frame.png")]
[StiGdiShapeTypePainter(typeof(StiFrameGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiFrameWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiFrameShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Frame");

	public override StiComponentId ComponentId => StiComponentId.StiFrameShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiFrameShapeType();
	}
}
