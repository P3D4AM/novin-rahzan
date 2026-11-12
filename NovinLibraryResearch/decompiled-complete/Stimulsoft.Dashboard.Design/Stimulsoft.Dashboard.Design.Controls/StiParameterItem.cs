using System;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Interactions;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiParameterItem : ICloneable
{
	public StiDashboardDrillDownParameter Parameter { get; set; }

	public StiParameterItemImage? Image { get; set; }

	public StiParameterItem PrevItem
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
			return ((ListBox)List).Items[num - 1] as StiParameterItem;
		}
	}

	public StiParameterItem NextItem
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
			return ((ListBox)List).Items[num + 1] as StiParameterItem;
		}
	}

	public StiParameterListBox List { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiParameterItem()
	{
	}

	public StiParameterItem(StiParameterItemImage? image = StiParameterItemImage.None)
		: this(null, image)
	{
	}

	public StiParameterItem(StiDashboardDrillDownParameter parameter, StiParameterItemImage? image = StiParameterItemImage.None)
	{
		Image = image;
		Parameter = parameter;
	}
}
