using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.DiagonalDownLine.png")]
[StiGdiShapeTypePainter(typeof(StiDiagonalDownLineGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiDiagonalDownLineWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDiagonalDownLineShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "DiagonalDownLine");

	public override StiComponentId ComponentId => StiComponentId.StiDiagonalDownLineShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiDiagonalDownLineShapeType();
	}
}
