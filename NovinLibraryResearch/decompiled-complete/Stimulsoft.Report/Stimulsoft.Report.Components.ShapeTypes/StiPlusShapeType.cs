using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Plus.png")]
[StiGdiShapeTypePainter(typeof(StiPlusGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiPlusWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiPlusShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Plus");

	public override StiComponentId ComponentId => StiComponentId.StiPlusShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiPlusShapeType();
	}
}
