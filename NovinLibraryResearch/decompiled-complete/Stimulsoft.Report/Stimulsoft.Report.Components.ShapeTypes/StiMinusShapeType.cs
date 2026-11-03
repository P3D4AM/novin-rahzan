using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Minus.png")]
[StiGdiShapeTypePainter(typeof(StiMinusGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiMinusWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiMinusShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Minus");

	public override StiComponentId ComponentId => StiComponentId.StiMinusShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiMinusShapeType();
	}
}
