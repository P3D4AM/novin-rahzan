using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public sealed class StiRectangleUtils
{
	public static Rectangle Normalize(Rectangle rect)
	{
		if (rect.Width < 0)
		{
			rect.X += rect.Width;
			rect.Width = -rect.Width;
		}
		if (rect.Height < 0)
		{
			rect.Y += rect.Height;
			rect.Height = -rect.Height;
		}
		return rect;
	}

	public static Rectangle FitToRectangle(Rectangle rect1, Rectangle rect2)
	{
		if (rect1.IsEmpty)
		{
			return rect2;
		}
		if (rect2.IsEmpty)
		{
			return rect1;
		}
		Rectangle result = rect1;
		if (result.Left > rect2.Left)
		{
			result.Width += result.Left - rect2.Left;
			result.X = rect2.Left;
		}
		if (result.Top > rect2.Top)
		{
			result.Height += result.Top - rect2.Top;
			result.Y = rect2.Top;
		}
		if (result.Right < rect2.Right)
		{
			result.Width += rect2.Right - result.Right;
		}
		if (result.Bottom < rect2.Bottom)
		{
			result.Height += rect2.Bottom - result.Bottom;
		}
		return result;
	}

	public static RectangleF Convert(Rectangle rect)
	{
		return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
	}

	public static RectangleD AlignSizeInRect(RectangleD rect, SizeD size, ContentAlignment alignment)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Invalid comparison between Unknown and I4
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		double x = 0.0;
		double y = 0.0;
		if ((int)alignment == 256 || (int)alignment == 16 || (int)alignment == 1)
		{
			x = rect.X;
		}
		if ((int)alignment == 512 || (int)alignment == 32 || (int)alignment == 2)
		{
			x = rect.X + (rect.Width - size.Width) / 2.0;
		}
		if ((int)alignment == 1024 || (int)alignment == 64 || (int)alignment == 4)
		{
			x = rect.Right - size.Width;
		}
		if ((int)alignment == 1 || (int)alignment == 2 || (int)alignment == 4)
		{
			y = rect.Y;
		}
		if ((int)alignment == 16 || (int)alignment == 32 || (int)alignment == 64)
		{
			y = rect.Y + (rect.Height - size.Height) / 2.0;
		}
		if ((int)alignment == 256 || (int)alignment == 512 || (int)alignment == 1024)
		{
			y = rect.Bottom - size.Height;
		}
		return new RectangleD(x, y, size.Width, size.Height);
	}

	public static RectangleD AlignSizeInRect(RectangleD rect, SizeD size, StiHorAlignment horAlignment, StiVertAlignment vertAlignment)
	{
		double x = 0.0;
		double y = 0.0;
		if (horAlignment == StiHorAlignment.Left)
		{
			x = rect.X;
		}
		if (horAlignment == StiHorAlignment.Center)
		{
			x = rect.X + (rect.Width - size.Width) / 2.0;
		}
		if (horAlignment == StiHorAlignment.Right)
		{
			x = rect.Right - size.Width;
		}
		if (vertAlignment == StiVertAlignment.Top)
		{
			y = rect.Y;
		}
		if (vertAlignment == StiVertAlignment.Center)
		{
			y = rect.Y + (rect.Height - size.Height) / 2.0;
		}
		if (vertAlignment == StiVertAlignment.Bottom)
		{
			y = rect.Bottom - size.Height;
		}
		return new RectangleD(x, y, size.Width, size.Height);
	}
}
