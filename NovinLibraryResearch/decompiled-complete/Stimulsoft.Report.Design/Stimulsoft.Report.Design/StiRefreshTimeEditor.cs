using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design;

public class StiRefreshTimeEditor : UITypeEditor
{
	private IWindowsFormsEditorService editorService;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		editorService.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			Height = StiScale.YYI(142.0)
		};
		Array values = Enum.GetValues(typeof(StiRefreshTimeValues));
		foreach (object item in values)
		{
			int num = (int)item;
			string text = item.ToString();
			if (StiPropertyGridOptions.Localizable)
			{
				text = ((num == 0) ? StiLocalization.Get("PropertyEnum", "BorderStyleNone") : text.Insert(text.IndexOfAny(new char[3] { 'S', 'M', 'H' }), " ").Replace("Refresh", ""));
			}
			val.Items.Add((object)text);
			if (num == (int)value)
			{
				((ListControl)val).SelectedIndex = val.Items.Count - 1;
			}
		}
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		editorService.DropDownControl((Control)(object)val);
		if (((ListControl)val).SelectedIndex < 0)
		{
			return value;
		}
		return values.GetValue(((ListControl)val).SelectedIndex);
	}
}
