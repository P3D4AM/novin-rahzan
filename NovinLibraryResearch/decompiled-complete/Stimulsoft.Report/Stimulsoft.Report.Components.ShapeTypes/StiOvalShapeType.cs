using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Oval.png")]
[StiGdiShapeTypePainter(typeof(StiOvalGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiOvalWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiOvalShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Oval");

	public override StiComponentId ComponentId => StiComponentId.StiOvalShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiOvalShapeType();
	}
}
