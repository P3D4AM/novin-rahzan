using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Rectangle.png")]
[StiGdiShapeTypePainter(typeof(StiRectangleGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiRectangleWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRectangleShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Rectangle");

	public override StiComponentId ComponentId => StiComponentId.StiRectangleShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiRectangleShapeType();
	}
}
