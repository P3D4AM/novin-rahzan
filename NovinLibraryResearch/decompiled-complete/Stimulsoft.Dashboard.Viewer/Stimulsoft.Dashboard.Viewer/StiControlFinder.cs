using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stimulsoft.Dashboard.Viewer;

internal class StiControlFinder
{
	public static IEnumerable<T> FindControls<T>(Control control) where T : Control
	{
		IEnumerable<Control> enumerable = ((IEnumerable)control.Controls).Cast<Control>();
		return (from c in ((IEnumerable<Control>)enumerable.SelectMany(FindControls<T>)).Concat(enumerable)
			where ((object)c).GetType() == typeof(T)
			select c).Cast<T>();
	}
}
