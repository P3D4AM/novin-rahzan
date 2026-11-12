using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiPaperSizeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool isDoubleClicked;

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
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		isDoubleClicked = false;
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		PaperKind val = (PaperKind)value;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		ListBox val2 = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			SelectedIndex = -1,
			ItemHeight = StiScale.YYI(20.0)
		};
		PaperSizeCollection val3 = StiOptions.Print.CustomPaperSizes;
		if (val3 == null)
		{
			PrinterSettings val4 = new PrinterSettings();
			val3 = val4.PaperSizes;
		}
		int num = 1;
		val2.Items.Add((object)StiLocalization.Get("Wizards", "Custom"));
		((ListControl)val2).SelectedIndex = 0;
		List<PaperSize> list = new List<PaperSize>();
		foreach (PaperSize item in val3)
		{
			PaperSize val5 = item;
			if ((int)val5.Kind != 0)
			{
				list.Add(val5);
				val2.Items.Add((object)val5.PaperName);
				if (val5.Kind == val)
				{
					((ListControl)val2).SelectedIndex = num;
				}
				num++;
			}
		}
		((Control)val2).DoubleClick += ListBox_DoubleClick;
		val2.Click += ListBox_DoubleClick;
		double y = StiScale.Y;
		((Control)val2).Height = val2.ItemHeight * val2.Items.Count + StiScale.I10;
		if (((Control)val2).Height > StiScale.YYI(300.0))
		{
			((Control)val2).Height = StiScale.YYI(300.0);
		}
		edSvc.DropDownControl((Control)(object)val2);
		if (!isDoubleClicked)
		{
			return (object)(PaperKind)value;
		}
		if (((ListControl)val2).SelectedIndex == 0)
		{
			return (object)(PaperKind)0;
		}
		PaperSize val6 = list[((ListControl)val2).SelectedIndex - 1];
		val = val6.Kind;
		if (context.Instance is StiPage stiPage && (int)val != 0)
		{
			SizeD paperSize = StiPageHelper.GetPaperSize(stiPage, val6);
			stiPage.PageWidth = paperSize.Width;
			stiPage.PageHeight = paperSize.Height;
		}
		return val;
	}
}
