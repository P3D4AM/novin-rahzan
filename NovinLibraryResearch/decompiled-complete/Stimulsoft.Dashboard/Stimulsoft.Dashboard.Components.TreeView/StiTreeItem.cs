using System.Collections.Generic;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.TreeView;

public class StiTreeItem
{
	public object Key { get; set; }

	public IStiMeter Meter { get; set; }

	public List<StiTreeItem> Items { get; set; }

	public override string ToString()
	{
		if (Key != null)
		{
			return Key.ToString();
		}
		return string.Empty;
	}

	public StiTreeItem()
	{
	}

	public StiTreeItem(object key)
	{
		Key = key;
	}

	public StiTreeItem(object key, IStiMeter meter)
	{
		Key = key;
		Meter = meter;
	}
}
