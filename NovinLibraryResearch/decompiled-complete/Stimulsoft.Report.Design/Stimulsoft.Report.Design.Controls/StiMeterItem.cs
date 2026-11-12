using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public sealed class StiMeterItem : ICloneable
{
	public string Text { get; set; }

	public StiMeterItem PrevItem
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
			return ((ListBox)List).Items[num - 1] as StiMeterItem;
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
			int num = ((ListBox)List).Items.IndexOf((object)this);
			if (num < 0 || num >= ((ListBox)List).Items.Count - 1)
			{
				return null;
			}
			return ((ListBox)List).Items[num + 1] as StiMeterItem;
		}
	}

	public StiMeterListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return Text;
	}

	public StiMeterItem(string text)
	{
		Text = text;
	}
}
