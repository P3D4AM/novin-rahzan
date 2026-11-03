using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.DiagonalUpLine.png")]
[StiGdiShapeTypePainter(typeof(StiDiagonalUpLineGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiDiagonalUpLineWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDiagonalUpLineShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "DiagonalUpLine");

	public override StiComponentId ComponentId => StiComponentId.StiDiagonalUpLineShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiDiagonalUpLineShapeType();
	}
}
