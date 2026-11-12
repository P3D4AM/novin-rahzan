using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design.Components;

public class StiLegendColumnsEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return false;
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		ListBox val = new ListBox
		{
			ItemHeight = StiScale.YYI(19.0),
			Width = StiScale.XXI(100.0),
			BorderStyle = (BorderStyle)0
		};
		((Control)val).Height = val.ItemHeight * 6;
		val.Items.Add((object)Loc.Get("PropertyMain", "Auto"));
		val.Items.Add((object)"1");
		val.Items.Add((object)"2");
		val.Items.Add((object)"3");
		val.Items.Add((object)"4");
		val.Items.Add((object)"5");
		if ((int)value == 0)
		{
			((ListControl)val).SelectedIndex = 0;
		}
		else if ((int)value == 1)
		{
			((ListControl)val).SelectedIndex = 1;
		}
		else if ((int)value == 2)
		{
			((ListControl)val).SelectedIndex = 2;
		}
		else if ((int)value == 3)
		{
			((ListControl)val).SelectedIndex = 3;
		}
		else if ((int)value == 4)
		{
			((ListControl)val).SelectedIndex = 4;
		}
		else if ((int)value == 5)
		{
			((ListControl)val).SelectedIndex = 5;
		}
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		edSvc.DropDownControl((Control)(object)val);
		return ((ListControl)val).SelectedIndex;
	}

	private void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}
}
