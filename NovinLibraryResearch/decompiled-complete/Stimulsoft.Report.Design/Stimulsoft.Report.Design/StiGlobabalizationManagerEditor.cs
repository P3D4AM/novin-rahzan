using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public class StiGlobabalizationManagerEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	internal void ListBoxItems_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		string text = value as string;
		StiComponent stiComponent = context.Instance as StiComponent;
		if (!(stiComponent.Report.GlobalizationManager is IStiGlobalizationManagerList stiGlobalizationManagerList))
		{
			return text;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		ListBox val = new ListBox();
		val.Items.Add((object)string.Empty);
		val.BorderStyle = (BorderStyle)0;
		((ListControl)val).SelectedIndex = 0;
		string[] array = ((stiComponent is StiImage) ? stiGlobalizationManagerList.GetImageGlobalizedNames() : stiGlobalizationManagerList.GetTextGlobalizedNames());
		ObjectCollection items = val.Items;
		object[] array2 = array;
		items.AddRange(array2);
		val.SelectedItem = text;
		((Control)val).DoubleClick += ListBoxItems_DoubleClick;
		val.Click += ListBoxItems_DoubleClick;
		((Control)val).Height = val.ItemHeight * val.Items.Count + 10;
		if (((Control)val).Height > 300)
		{
			((Control)val).Height = 300;
		}
		edSvc.DropDownControl((Control)(object)val);
		return val.SelectedItem as string;
	}
}
