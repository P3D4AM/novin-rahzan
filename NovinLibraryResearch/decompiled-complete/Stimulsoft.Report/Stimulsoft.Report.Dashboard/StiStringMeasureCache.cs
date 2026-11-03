using System.Collections;
using System.Drawing;

namespace Stimulsoft.Report.Dashboard;

public static class StiStringMeasureCache
{
	private static Hashtable stringToSize = new Hashtable();

	public static SizeF? GetSize(Font font, string str)
	{
		if (str == null)
		{
			return null;
		}
		string hashCode = GetHashCode(font, str);
		if (stringToSize[hashCode] != null)
		{
			return (SizeF)stringToSize[hashCode];
		}
		return null;
	}

	public static void PutSize(Font font, string str, SizeF size)
	{
		if (str != null)
		{
			string hashCode = GetHashCode(font, str);
			stringToSize[hashCode] = size;
		}
	}

	private static string GetHashCode(Font font, string str)
	{
		return ((object)font).GetHashCode() + str;
	}
}
