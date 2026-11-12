using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Components.Design;

public class StiCheckValuesEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool selected;

	private string[] values = new string[4] { "true/false", "yes/no", "on/off", "1/0" };

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	private int GetMaxWidth(Graphics g, Font font)
	{
		int num = 0;
		string[] array = values;
		foreach (string text in array)
		{
			int val = (int)g.MeasureString(text, font).Width;
			num = Math.Max(num, val);
		}
		return num;
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
		selected = true;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		StiComponent stiComponent;
		if (context.Instance is Array)
		{
			Array array = context.Instance as Array;
			stiComponent = array.GetValue(0) as StiComponent;
		}
		else
		{
			stiComponent = context.Instance as StiComponent;
		}
		if (stiComponent == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiListBox stiListBox = new StiListBox();
		try
		{
			((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
			double y = StiScale.Y;
			((ListBox)stiListBox).ItemHeight = StiScale.YYI(19.0);
			((Control)stiListBox).Height = ((ListBox)stiListBox).ItemHeight * values.Length;
			if (((Control)stiListBox).Height > StiScale.YYI(300.0))
			{
				((Control)stiListBox).Height = StiScale.YYI(300.0);
			}
			ObjectCollection items = ((ListBox)stiListBox).Items;
			object[] array2 = values;
			items.AddRange(array2);
			Graphics val = Graphics.FromHwnd(((Control)stiListBox).Handle);
			try
			{
				Font val2 = new Font("Arial", 8f);
				try
				{
					((Control)stiListBox).Width = GetMaxWidth(val, val2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			((ListBox)stiListBox).SelectedItem = ((StiCheckBox)stiComponent).Values;
			((Control)stiListBox).DoubleClick += ListBox_DoubleClick;
			((ListBox)stiListBox).Click += ListBox_DoubleClick;
			edSvc.DropDownControl((Control)(object)stiListBox);
			if (selected)
			{
				return ((ListBox)stiListBox).SelectedItem as string;
			}
			return ((StiCheckBox)context.Instance).Values;
		}
		finally
		{
			((IDisposable)stiListBox)?.Dispose();
		}
	}
}
