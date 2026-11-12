using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Inplace;

public static class StiInplaceEvents
{
	public static event EventHandler Changed;

	public static void InvokeChanged(StiComponent component)
	{
		StiInplaceEvents.Changed?.Invoke(component, EventArgs.Empty);
	}
}
