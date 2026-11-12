using System;
using System.Windows.Forms;

namespace Stimulsoft.Report.Chart.Design;

public sealed class StiChartItem : ICloneable
{
	public string Text { get; set; }

	public StiDataColumnMeter Meter { get; set; }

	public StiMeterItemImage? Image { get; set; }

	public StiChartItem PrevItem
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
			return List.Items[num - 1] as StiChartItem;
		}
	}

	public StiChartItem NextItem
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
			return List.Items[num + 1] as StiChartItem;
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

	public StiChartItem()
		: this("", StiMeterItemImage.Measure)
	{
	}

	public StiChartItem(string text, StiMeterItemImage? image = StiMeterItemImage.Measure)
		: this(text, null, image)
	{
	}

	public StiChartItem(string text, StiDataColumnMeter meter, StiMeterItemImage? image = StiMeterItemImage.Measure)
	{
		Image = image;
		Meter = meter;
		Text = text;
	}
}
