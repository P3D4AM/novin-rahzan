using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.LeftAndRightLine.png")]
[StiGdiShapeTypePainter(typeof(StiLeftAndRightLineGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiLeftAndRightLineWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiLeftAndRightLineShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "LeftAndRightLine");

	public override StiComponentId ComponentId => StiComponentId.StiLeftAndRightLineShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiLeftAndRightLineShapeType();
	}
}
