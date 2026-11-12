using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer;

internal static class StiRectangleHelper
{
	internal static int Round(double value)
	{
		return (int)Math.Round(value, MidpointRounding.AwayFromZero);
	}

	internal static Rectangle Round(RectangleD rect)
	{
		return new Rectangle(Round(rect.Left), Round(rect.Top), Round(rect.Width), Round(rect.Height));
	}
}
