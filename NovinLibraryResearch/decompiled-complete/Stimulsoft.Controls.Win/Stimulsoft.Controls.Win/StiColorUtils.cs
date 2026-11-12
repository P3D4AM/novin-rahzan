using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win;

internal sealed class StiColorUtils
{
	public static Color GetSysColor(Win32.ColorType colorType)
	{
		return ColorTranslator.FromWin32(Win32.GetSysColor((int)colorType));
	}

	public static Color Light(Color baseColor, byte value)
	{
		byte r = baseColor.R;
		byte g = baseColor.G;
		byte b = baseColor.B;
		r = ((r + value <= 255) ? ((byte)(r + value)) : byte.MaxValue);
		g = ((g + value <= 255) ? ((byte)(g + value)) : byte.MaxValue);
		b = ((b + value <= 255) ? ((byte)(b + value)) : byte.MaxValue);
		return Color.FromArgb(r, g, b);
	}

	public static Color Dark(Color baseColor, byte value)
	{
		byte r = baseColor.R;
		byte g = baseColor.G;
		byte b = baseColor.B;
		r = (byte)((r - value >= 0) ? ((byte)(r - value)) : 0);
		g = (byte)((g - value >= 0) ? ((byte)(g - value)) : 0);
		b = (byte)((b - value >= 0) ? ((byte)(b - value)) : 0);
		return Color.FromArgb(r, g, b);
	}

	public static Color GetDisabledColor(Control control)
	{
		Color backColor = control.BackColor;
		if (backColor.A == 0)
		{
			Control parent = control.Parent;
			while (backColor.A == 0 || backColor.IsEmpty)
			{
				if (parent == null)
				{
					return SystemColors.Control;
				}
				backColor = parent.BackColor;
				parent = control.Parent;
			}
		}
		return backColor;
	}

	public static int GetColorRop(Color color, int darkROP, int lightROP)
	{
		if (color.GetBrightness() < 0.5f)
		{
			return darkROP;
		}
		return lightROP;
	}
}
