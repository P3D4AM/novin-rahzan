using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.SnipSameSideCornerRectangle.png")]
[StiGdiShapeTypePainter(typeof(StiSnipSameSideCornerRectangleGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiSnipSameSideCornerRectangleWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiSnipSameSideCornerRectangleShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "SnipSameSideCornerRectangle");

	public override StiComponentId ComponentId => StiComponentId.StiSnipSameSideCornerRectangleShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiSnipSameSideCornerRectangleShapeType();
	}
}
