using System.Drawing;

namespace Stimulsoft.Report.Helpers;

internal static class StiColorFXHelper
{
	internal static string HexConverter(Color c)
	{
		if (c.A == byte.MaxValue)
		{
			return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
		}
		return "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
	}
}
