using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Equal.png")]
[StiGdiShapeTypePainter(typeof(StiEqualGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiEqualWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiEqualShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Equal");

	public override StiComponentId ComponentId => StiComponentId.StiEqualShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiEqualShapeType();
	}
}
