using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiChartSeriesLabelsValueTypeEditor : UITypeEditor
{
	private class Item
	{
		public StiSeriesLabelsValueType? Type { get; }

		public override string ToString()
		{
			if (Type.HasValue)
			{
				return Loc.GetEnum("StiSeriesLabelsValueType" + Type.Value);
			}
			return Loc.GetMain("Auto");
		}

		public Item(StiSeriesLabelsValueType? type)
		{
			Type = type;
		}
	}

	private IWindowsFormsEditorService edSvc;

	internal void OnDoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		StiSeriesLabelsValueType? stiSeriesLabelsValueType = (StiSeriesLabelsValueType?)value;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		Item[] array = (from c in Enum.GetValues(typeof(StiSeriesLabelsValueType)).Cast<StiSeriesLabelsValueType>().ToList()
			where c != StiSeriesLabelsValueType.Weight && c != StiSeriesLabelsValueType.Tag
			select new Item(c) into c
			orderby c.ToString()
			select c).ToArray();
		StiListBox stiListBox = new StiListBox();
		((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
		((ListBox)stiListBox).ItemHeight = StiScale.YYI(19.0);
		((Control)stiListBox).Width = StiScale.XXI(200.0);
		StiListBox stiListBox2 = stiListBox;
		((ListBox)stiListBox2).Items.Add((object)new Item(null));
		ObjectCollection items = ((ListBox)stiListBox2).Items;
		object[] array2 = array;
		items.AddRange(array2);
		((Control)stiListBox2).Height = ((ListBox)stiListBox2).ItemHeight * array.Count() + 20;
		if (((Control)stiListBox2).Height > StiScale.YYI(300.0))
		{
			((Control)stiListBox2).Height = StiScale.YYI(300.0);
		}
		int num = 0;
		Item[] array3 = array;
		foreach (Item item in array3)
		{
			if (item.Type == stiSeriesLabelsValueType)
			{
				((ListControl)stiListBox2).SelectedIndex = num;
				break;
			}
			num++;
		}
		((Control)stiListBox2).DoubleClick += OnDoubleClick;
		((ListBox)stiListBox2).Click += OnDoubleClick;
		edSvc.DropDownControl((Control)(object)stiListBox2);
		return (((ListBox)stiListBox2).SelectedItem as Item)?.Type;
	}
}
