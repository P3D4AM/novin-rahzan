using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stimulsoft.Base.Drawing;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct StiAlignHelper
{
	public static double AlignToGrid(double value, double gridSize, bool aligningToGrid)
	{
		if (aligningToGrid)
		{
			return (double)(int)Math.Round(value / gridSize) * gridSize;
		}
		return value;
	}

	public static Rectangle AlignToGrid(Rectangle rect, int gridSize, bool aligningToGrid)
	{
		if (aligningToGrid)
		{
			return new Rectangle((int)Math.Round((float)rect.X / (float)gridSize) * gridSize, (int)Math.Round((float)rect.Y / (float)gridSize) * gridSize, (int)Math.Round((float)rect.Width / (float)gridSize) * gridSize, (int)Math.Round((float)rect.Height / (float)gridSize) * gridSize);
		}
		return rect;
	}

	public static RectangleD AlignToGrid(RectangleD rect, double gridSize, bool aligningToGrid)
	{
		if (aligningToGrid)
		{
			return new RectangleD((double)(int)Math.Round(rect.X / gridSize) * gridSize, (double)(int)Math.Round(rect.Y / gridSize) * gridSize, (double)(int)Math.Round(rect.Width / gridSize) * gridSize, (double)(int)Math.Round(rect.Height / gridSize) * gridSize);
		}
		return rect;
	}
}
