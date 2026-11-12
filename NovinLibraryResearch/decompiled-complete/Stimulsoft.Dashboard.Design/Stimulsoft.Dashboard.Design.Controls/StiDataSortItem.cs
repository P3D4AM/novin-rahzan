using System;
using System.Windows.Forms;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiDataSortItem : ICloneable
{
	public string Text { get; set; }

	public StiDataSortRule Sort { get; set; }

	public StiDataSortItem PrevItem
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
			return ((ListBox)List).Items[num - 1] as StiDataSortItem;
		}
	}

	public StiDataSortItem NextItem
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
			return ((ListBox)List).Items[num + 1] as StiDataSortItem;
		}
	}

	public StiDataSortListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return Text;
	}

	public StiDataSortItem()
		: this("")
	{
	}

	public StiDataSortItem(string text)
		: this(text, null)
	{
	}

	public StiDataSortItem(string text, StiDataSortRule sort)
	{
		Sort = sort;
		Text = text;
	}
}
