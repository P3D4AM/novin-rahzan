using System.Drawing;

namespace Stimulsoft.Editor;

public class XmlHelper
{
	public static string SerializeColor(Color color)
	{
		if (color.IsNamedColor)
		{
			return color.Name;
		}
		if (color == Color.Empty)
		{
			return string.Empty;
		}
		return $"{color.A}:{color.R}:{color.G}:{color.B}";
	}

	public static Color DeserializeColor(string color)
	{
		if (color == string.Empty)
		{
			return Color.Empty;
		}
		string[] array = color.Split(new char[1] { ':' });
		if (array.Length == 1)
		{
			return Color.FromName(array[0]);
		}
		if (array.Length == 4)
		{
			byte alpha = byte.Parse(array[0]);
			byte red = byte.Parse(array[1]);
			byte green = byte.Parse(array[2]);
			byte blue = byte.Parse(array[3]);
			return Color.FromArgb(alpha, red, green, blue);
		}
		return Color.Empty;
	}
}
