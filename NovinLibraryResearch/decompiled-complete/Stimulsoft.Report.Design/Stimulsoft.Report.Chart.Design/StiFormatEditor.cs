using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Chart.Design;

public class StiFormatEditor : UITypeEditor
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
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiListBox stiListBox = new StiListBox();
		((ListBox)stiListBox).ItemHeight = (int)Math.Ceiling(19.0 * StiScale.Y);
		((Control)stiListBox).Width = (int)Math.Ceiling(100.0 * StiScale.X);
		((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
		((ListBox)stiListBox).Items.Add((object)"");
		((ListBox)stiListBox).Items.Add((object)"N");
		((ListBox)stiListBox).Items.Add((object)"N2");
		((ListBox)stiListBox).Items.Add((object)"C");
		((ListBox)stiListBox).Items.Add((object)"C2");
		((ListBox)stiListBox).Items.Add((object)"P");
		((ListBox)stiListBox).Items.Add((object)"P2");
		((ListBox)stiListBox).Items.Add((object)"E");
		((ListBox)stiListBox).Items.Add((object)"E2");
		((ListBox)stiListBox).Items.Add((object)"F");
		((ListBox)stiListBox).Items.Add((object)"F2");
		((ListBox)stiListBox).Items.Add((object)"MM/dd/yyyy");
		((ListBox)stiListBox).Items.Add((object)"MMMM dd");
		((ListBox)stiListBox).Items.Add((object)"yyyy MMMM");
		((ListBox)stiListBox).Items.Add((object)"F2");
		((Control)stiListBox).Height = ((ListBox)stiListBox).ItemHeight * ((ListBox)stiListBox).Items.Count + (int)Math.Ceiling(5.0 * StiScale.Y);
		if (((Control)stiListBox).Height > (int)Math.Ceiling(300.0 * StiScale.Y))
		{
			((Control)stiListBox).Height = (int)Math.Ceiling(300.0 * StiScale.Y);
		}
		((Control)stiListBox).DoubleClick += OnDoubleClick;
		((ListBox)stiListBox).Click += OnDoubleClick;
		edSvc.DropDownControl((Control)(object)stiListBox);
		return ((ListBox)stiListBox).SelectedItem;
	}
}
