using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.SnipDiagonalSideCornerRectangle.png")]
[StiGdiShapeTypePainter(typeof(StiSnipDiagonalSideCornerRectangleGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiSnipDiagonalSideCornerRectangleWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiSnipDiagonalSideCornerRectangleShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "SnipDiagonalSideCornerRectangle");

	public override StiComponentId ComponentId => StiComponentId.StiSnipDiagonalSideCornerRectangleShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiSnipDiagonalSideCornerRectangleShapeType();
	}
}
