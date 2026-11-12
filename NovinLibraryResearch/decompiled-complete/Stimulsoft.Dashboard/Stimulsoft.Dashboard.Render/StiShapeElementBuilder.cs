using Stimulsoft.Dashboard.Components.Shape;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Render;

public class StiShapeElementBuilder
{
	public StiShape Render(StiShapeElement element)
	{
		return new StiShape
		{
			ShapeType = element.ShapeType,
			Brush = element.Fill,
			BorderColor = element.Stroke,
			Size = element.Size
		};
	}
}
