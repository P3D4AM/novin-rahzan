using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.HorizontalLine.png")]
[StiGdiShapeTypePainter(typeof(StiHorizontalLineGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiHorizontalLineWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiHorizontalLineShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "HorizontalLine");

	public override StiComponentId ComponentId => StiComponentId.StiHorizontalLineShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiHorizontalLineShapeType();
	}
}
