using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Controls;

namespace Stimulsoft.Base.Design;

public class StiTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	internal void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiTypeWrapper[] typeWrappers = StiTypeWrapper.GetTypeWrappers();
		StiListBox stiListBox = new StiListBox();
		((Control)stiListBox).Height = (int)Math.Ceiling(100.0 * StiScale.X);
		((Control)stiListBox).Width = (int)Math.Ceiling(100.0 * StiScale.Y);
		((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
		StiListBox stiListBox2 = stiListBox;
		int num = 0;
		int selectedIndex = 0;
		StiTypeWrapper[] array = typeWrappers;
		foreach (StiTypeWrapper stiTypeWrapper in array)
		{
			if (stiTypeWrapper.Type == (Type)value)
			{
				selectedIndex = num;
				break;
			}
			num++;
		}
		ObjectCollection items = ((ListBox)stiListBox2).Items;
		object[] array2 = typeWrappers;
		items.AddRange(array2);
		((ListControl)stiListBox2).SelectedIndex = selectedIndex;
		((Control)stiListBox2).DoubleClick += ListBox_DoubleClick;
		((ListBox)stiListBox2).Click += ListBox_DoubleClick;
		edSvc.DropDownControl((Control)(object)stiListBox2);
		return ((StiTypeWrapper)((ListBox)stiListBox2).SelectedItem).Type;
	}
}
