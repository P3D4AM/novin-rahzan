using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Components.Design;

public class StiPaperSourceEditor : UITypeEditor
{
	private bool isDoubleClicked;

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

	internal void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
		isDoubleClicked = true;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		isDoubleClicked = false;
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		string text = value as string;
		if (text == null)
		{
			text = string.Empty;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			SelectedIndex = -1,
			ItemHeight = StiScale.YYI(20.0)
		};
		PaperSourceCollection val2 = StiOptions.Print.CustomPaperSources;
		if (val2 == null)
		{
			PrinterSettings val3 = new PrinterSettings();
			val2 = val3.PaperSources;
		}
		int num = 0;
		((ListControl)val).SelectedIndex = -1;
		foreach (PaperSource item in val2)
		{
			PaperSource val4 = item;
			val.Items.Add((object)val4.SourceName);
			if (val4.SourceName == text)
			{
				((ListControl)val).SelectedIndex = num;
			}
			num++;
		}
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		((Control)val).Height = val.ItemHeight * val.Items.Count + StiScale.I10;
		if (((Control)val).Height > StiScale.YYI(300.0))
		{
			((Control)val).Height = StiScale.YYI(300.0);
		}
		edSvc.DropDownControl((Control)(object)val);
		if (!isDoubleClicked)
		{
			return value as string;
		}
		return val.SelectedItem as string;
	}
}
