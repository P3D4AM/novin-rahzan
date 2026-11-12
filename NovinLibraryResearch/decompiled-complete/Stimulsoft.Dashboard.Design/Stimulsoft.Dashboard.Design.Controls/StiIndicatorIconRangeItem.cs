using System;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Indicator;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiIndicatorIconRangeItem : ICloneable
{
	public StiIndicatorIconRange Range { get; set; }

	public StiIndicatorIconRangeItem PrevItem
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
			return ((ListBox)List).Items[num - 1] as StiIndicatorIconRangeItem;
		}
	}

	public StiIndicatorIconRangeItem NextItem
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
			return ((ListBox)List).Items[num + 1] as StiIndicatorIconRangeItem;
		}
	}

	public StiIndicatorIconRangeListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiIndicatorIconRangeItem()
		: this(null)
	{
	}

	public StiIndicatorIconRangeItem(StiIndicatorIconRange range)
	{
		Range = range;
	}
}
