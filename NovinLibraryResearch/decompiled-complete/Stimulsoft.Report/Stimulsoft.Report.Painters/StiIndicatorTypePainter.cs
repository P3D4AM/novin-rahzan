using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public abstract class StiIndicatorTypePainter
{
	public static StiIndicatorTypePainter GetPainter(Type indicatorType, StiGuiMode guiMode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return StiPainter.GetIndicatorPainter(indicatorType, guiMode);
	}

	public abstract RectangleD Paint(object context, StiComponent component, RectangleD rect);
}
