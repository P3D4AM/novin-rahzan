using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Helpers;

internal static class StiStringHelper
{
	internal static string GetMeasureText(string text, Font font, double maxWidth)
	{
		for (int i = 0; text.Length > i; i++)
		{
			string text2 = text.Substring(0, i);
			if (maxWidth < (double)TextRenderer.MeasureText(text2, font).Width)
			{
				return text2 + "...";
			}
		}
		return text;
	}
}
