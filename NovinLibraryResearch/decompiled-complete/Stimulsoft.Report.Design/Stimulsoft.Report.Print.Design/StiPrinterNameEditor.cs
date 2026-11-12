using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Report.Print.Design;

public class StiPrinterNameEditor : UITypeEditor
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

	internal void OnDoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		ListBox val = new ListBox
		{
			Height = 200,
			Width = 200,
			BorderStyle = (BorderStyle)0
		};
		int num = 1;
		int selectedIndex = 0;
		val.Items.Add((object)string.Empty);
		StringCollection installedPrinters = PrinterSettings.InstalledPrinters;
		foreach (string item in installedPrinters)
		{
			val.Items.Add((object)item);
			if (item == value as string)
			{
				selectedIndex = num;
			}
			num++;
		}
		((ListControl)val).SelectedIndex = selectedIndex;
		((Control)val).DoubleClick += OnDoubleClick;
		val.Click += OnDoubleClick;
		edSvc.DropDownControl((Control)(object)val);
		return val.SelectedItem as string;
	}
}
