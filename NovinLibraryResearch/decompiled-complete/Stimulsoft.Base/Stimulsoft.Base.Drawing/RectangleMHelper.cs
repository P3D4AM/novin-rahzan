using System.ComponentModel;

namespace Stimulsoft.Base.Drawing;

public class RectangleMHelper : TypeConverter
{
	public static string ConvertRectangleMToString(RectangleM rectangle)
	{
		return $"{rectangle.X},{rectangle.Y},{rectangle.Width},{rectangle.Height}";
	}

	public static string ConvertRectangleMToString(RectangleM rectangle, char separator)
	{
		return $"{rectangle.X}{separator}{rectangle.Y}{separator}{rectangle.Width}{separator}{rectangle.Height}";
	}

	public static RectangleM ConvertStringToRectangleM(string str, char separator)
	{
		string[] array = str.Split(new char[1] { separator });
		return new RectangleM(decimal.Parse(array[0]), decimal.Parse(array[1]), decimal.Parse(array[2]), decimal.Parse(array[3]));
	}
}
