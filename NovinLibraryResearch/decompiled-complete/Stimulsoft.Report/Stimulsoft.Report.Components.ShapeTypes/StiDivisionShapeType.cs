using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Division.png")]
[StiGdiShapeTypePainter(typeof(StiDivisionGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiDivisionWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDivisionShapeType : StiShapeTypeService
{
	public override string ServiceName => StiLocalization.Get("Shapes", "Division");

	public override StiComponentId ComponentId => StiComponentId.StiDivisionShapeType;

	public override StiShapeTypeService CreateNew()
	{
		return new StiDivisionShapeType();
	}
}
