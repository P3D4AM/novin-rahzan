using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public abstract class StiShapeTypePainter
{
	public static StiShapeTypePainter GetPainter(Type componentType, StiGuiMode guiMode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return StiPainter.GetShapePainter(componentType, guiMode);
	}

	public abstract void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom);
}
