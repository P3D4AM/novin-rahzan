using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.TopAndBottomLine.png")]
[StiGdiShapeTypePainter(typeof(StiTopAndBottomLineGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiTopAndBottomLineWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiTopAndBottomLineShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "TopAndBottomLine");

	public override StiComponentId ComponentId => StiComponentId.StiTopAndBottomLineShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiTopAndBottomLineShapeType();
	}
}
