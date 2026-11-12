using System.ComponentModel;

namespace Stimulsoft.Base.Drawing;

public class RectangleDHelper : TypeConverter
{
	public static string ConvertRectangleDToString(RectangleD rectangle)
	{
		return $"{rectangle.X},{rectangle.Y},{rectangle.Width},{rectangle.Height}";
	}

	public static string ConvertRectangleDToString(RectangleD rectangle, char separator)
	{
		return $"{rectangle.X}{separator}{rectangle.Y}{separator}{rectangle.Width}{separator}{rectangle.Height}";
	}

	public static RectangleD ConvertStringToRectangleD(string str, char separator)
	{
		string[] array = str.Split(new char[1] { separator });
		return new RectangleD(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]), double.Parse(array[3]));
	}
}
