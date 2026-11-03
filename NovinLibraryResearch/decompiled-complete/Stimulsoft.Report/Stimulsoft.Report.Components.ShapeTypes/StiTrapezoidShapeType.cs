using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Trapezoid.png")]
[StiGdiShapeTypePainter(typeof(StiTrapezoidGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiTrapezoidWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiTrapezoidShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Trapezoid");

	public override StiComponentId ComponentId => StiComponentId.StiTrapezoidShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiTrapezoidShapeType();
	}
}
