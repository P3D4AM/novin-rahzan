using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Multiply.png")]
[StiGdiShapeTypePainter(typeof(StiMultiplyGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiMultiplyWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiMultiplyShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Multiply");

	public override StiComponentId ComponentId => StiComponentId.StiMultiplyShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiMultiplyShapeType();
	}
}
