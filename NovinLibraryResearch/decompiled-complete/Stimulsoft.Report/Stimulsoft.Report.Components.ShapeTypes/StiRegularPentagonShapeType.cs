using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.RegularPentagon.png")]
[StiGdiShapeTypePainter(typeof(StiRegularPentagonGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiRegularPentagonWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRegularPentagonShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "RegularPentagon");

	public override StiComponentId ComponentId => StiComponentId.StiRegularPentagonShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiRegularPentagonShapeType();
	}
}
