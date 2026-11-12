using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Dashboard.Viewer.Extensions;

public static class ControlCollectionExt
{
	public static IEnumerable<T> GetControlsOfType<T>(this ControlCollection collection) where T : Control
	{
		foreach (Control item in (ArrangedElementCollection)collection)
		{
			Control c = item;
			T val = (T)(object)((c is T) ? c : null);
			if (val != null)
			{
				yield return val;
			}
			foreach (T item2 in c.Controls.GetControlsOfType<T>())
			{
				yield return item2;
			}
		}
	}
}
