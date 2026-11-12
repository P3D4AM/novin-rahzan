using System;
using System.Windows.Forms;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiDataFilterItem : ICloneable
{
	public string Text { get; set; }

	public StiDataFilterRule Filter { get; set; }

	public StiDataFilterItemImage? Image { get; set; }

	public StiDataFilterItem PrevItem
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
			return ((ListBox)List).Items[num - 1] as StiDataFilterItem;
		}
	}

	public StiDataFilterItem NextItem
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
			return ((ListBox)List).Items[num + 1] as StiDataFilterItem;
		}
	}

	public StiDataFilterListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return Text;
	}

	public StiDataFilterItem()
		: this("", StiDataFilterItemImage.None)
	{
	}

	public StiDataFilterItem(string text, StiDataFilterItemImage? image = StiDataFilterItemImage.None)
		: this(text, null, image)
	{
	}

	public StiDataFilterItem(string text, StiDataFilterRule filter, StiDataFilterItemImage? image = StiDataFilterItemImage.None)
	{
		Image = image;
		Filter = filter;
		Text = text;
	}
}
