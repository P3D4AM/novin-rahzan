using System;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Gauge;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiGaugeRangeItem : ICloneable
{
	public StiGaugeRange Range { get; set; }

	public StiGaugeRangeItemImage? Image { get; set; }

	public StiGaugeRangeItem PrevItem
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
			return ((ListBox)List).Items[num - 1] as StiGaugeRangeItem;
		}
	}

	public StiGaugeRangeItem NextItem
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
			return ((ListBox)List).Items[num + 1] as StiGaugeRangeItem;
		}
	}

	public StiGaugeRangeListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiGaugeRangeItem()
	{
	}

	public StiGaugeRangeItem(StiGaugeRangeItemImage? image = StiGaugeRangeItemImage.None)
		: this(null, image)
	{
	}

	public StiGaugeRangeItem(StiGaugeRange range, StiGaugeRangeItemImage? image = StiGaugeRangeItemImage.None)
	{
		Image = image;
		Range = range;
	}
}
