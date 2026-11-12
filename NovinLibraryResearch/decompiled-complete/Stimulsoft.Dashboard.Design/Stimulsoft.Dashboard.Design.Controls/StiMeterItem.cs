using System;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiMeterItem : ICloneable
{
	public string Text { get; set; }

	public StiMeter Meter { get; set; }

	public StiMeterItemImage? Image { get; set; }

	public StiMeterItem PrevItem
	{
		get
		{
			if (List == null)
			{
				return null;
			}
			int num = List.Items.IndexOf((object)this);
			if (num < 1)
			{
				return null;
			}
			return List.Items[num - 1] as StiMeterItem;
		}
	}

	public StiMeterItem NextItem
	{
		get
		{
			if (List == null)
			{
				return null;
			}
			int num = List.Items.IndexOf((object)this);
			if (num < 0 || num >= List.Items.Count - 1)
			{
				return null;
			}
			return List.Items[num + 1] as StiMeterItem;
		}
	}

	public ListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return Text;
	}

	public StiMeterItem()
		: this("", StiMeterItemImage.Measure)
	{
	}

	public StiMeterItem(string text, StiMeterItemImage? image = StiMeterItemImage.Measure)
		: this(text, null, image)
	{
	}

	public StiMeterItem(string text, StiMeter meter, StiMeterItemImage? image = StiMeterItemImage.Measure)
	{
		Image = image;
		Meter = meter;
		Text = text;
	}
}
