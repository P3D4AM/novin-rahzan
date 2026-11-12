using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Design;

internal static class StiButtonSizeHelper
{
	public static void CheckSizeOfButton(Button button)
	{
		Size size = TextRenderer.MeasureText(((Control)button).Text, ((Control)button).Font);
		size.Width += StiScale.I10;
		if (((Control)button).Width < size.Width)
		{
			((Control)button).Width = size.Width;
		}
	}
}
