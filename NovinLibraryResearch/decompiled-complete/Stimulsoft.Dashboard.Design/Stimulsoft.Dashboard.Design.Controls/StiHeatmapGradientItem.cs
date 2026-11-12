using System;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.OnlineMap;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiHeatmapGradientItem : ICloneable
{
	public StiHeatmapGradientRange Range { get; set; }

	public StiHeatmapGradientItem PrevItem
	{
		get
		{
			if (List == null)
			{
				return null;
			}
			int num = ((ListBox)List).Items.IndexOf((object)this);
			if (num < 1)
			{
				return null;
			}
			return ((ListBox)List).Items[num - 1] as StiHeatmapGradientItem;
		}
	}

	public StiHeatmapGradientItem NextItem
	{
		get
		{
			if (List == null)
			{
				return null;
			}
			int num = ((ListBox)List).Items.IndexOf((object)this);
			if (num < 0 || num >= ((ListBox)List).Items.Count - 1)
			{
				return null;
			}
			return ((ListBox)List).Items[num + 1] as StiHeatmapGradientItem;
		}
	}

	public StiHeatmapGradientListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiHeatmapGradientItem()
	{
	}

	public StiHeatmapGradientItem(StiHeatmapGradientRange range)
	{
		Range = range;
	}
}
