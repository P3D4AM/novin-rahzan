using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design;

public class StiCalculationModeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiListBox stiListBox = new StiListBox();
		((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
		StiListBox stiListBox2 = stiListBox;
		string text = StiLocalization.Get("PropertyEnum", "StiCalculationModeCompilation");
		string text2 = StiLocalization.Get("PropertyEnum", "StiCalculationModeInterpretation");
		((ListBox)stiListBox2).Items.Add((object)text);
		((ListBox)stiListBox2).Items.Add((object)text2);
		((ListBox)stiListBox2).SelectedItem = value;
		((Control)stiListBox2).Height = (int)Math.Ceiling(40.0 * StiScale.Y);
		((Control)stiListBox2).DoubleClick += ListBox_DoubleClick;
		((ListBox)stiListBox2).Click += ListBox_DoubleClick;
		edSvc.DropDownControl((Control)(object)stiListBox2);
		if (((ListBox)stiListBox2).SelectedItem != null && ((ListBox)stiListBox2).SelectedItem.ToString() == text)
		{
			return StiCalculationMode.Compilation;
		}
		return StiCalculationMode.Interpretation;
	}
}
