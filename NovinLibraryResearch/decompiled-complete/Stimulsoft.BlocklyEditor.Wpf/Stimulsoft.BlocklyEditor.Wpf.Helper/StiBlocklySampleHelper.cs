using System.Collections.Generic;
using Stimulsoft.BlocklyEditor.Wpf.QuickStart.Samples;

namespace Stimulsoft.BlocklyEditor.Wpf.Helper;

public static class StiBlocklySampleHelper
{
	public static List<StiBlocklySample> GetSamples()
	{
		return new List<StiBlocklySample>
		{
			new StiBlocklySample
			{
				Name = "Change Brush",
				Source = "Brush"
			},
			new StiBlocklySample
			{
				Name = "Open Link",
				Source = "OpenLink"
			},
			new StiBlocklySample
			{
				Name = "Show Message",
				Source = "ShowMessage"
			}
		};
	}
}
