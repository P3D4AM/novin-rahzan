using System.Collections.Generic;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;

namespace Stimulsoft.BlocklyEditor.Wpf.Helper;

public static class StiComponentHelper
{
	public static List<string> LoadComponents(StiReport report)
	{
		List<string> list = new List<string>();
		foreach (StiComponent component in report.GetComponents())
		{
			list.Add(component.Name);
		}
		return list;
	}

	public static List<string> LoadContainerComponents(StiReport report)
	{
		List<string> list = new List<string>();
		foreach (StiComponent component in report.GetComponents())
		{
			if (component is StiPage || component is StiContainer)
			{
				list.Add(component.Name);
			}
		}
		return list;
	}
}
