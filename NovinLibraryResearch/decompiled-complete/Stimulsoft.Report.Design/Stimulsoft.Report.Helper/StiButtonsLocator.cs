using System.Collections;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Helper;

public static class StiButtonsLocator
{
	public static void ArrangeButtons(Control control)
	{
		IOrderedEnumerable<Control> orderedEnumerable = from Control c in (IEnumerable)control.Controls
			where c is Button
			orderby c.Left descending
			select c;
		int num = control.Width - StiScale.I1 - 1;
		foreach (Control item in orderedEnumerable)
		{
			num = (item.Left = num - (item.Width - 1));
		}
	}
}
