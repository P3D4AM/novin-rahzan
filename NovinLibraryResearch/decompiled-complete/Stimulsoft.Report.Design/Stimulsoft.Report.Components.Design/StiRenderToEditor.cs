using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Components.Design;

public class StiRenderToEditor : UITypeEditor
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

	internal void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	private bool InstanceContensComponent(object obj, StiComponent component)
	{
		if (obj is Array)
		{
			Array array = obj as Array;
			foreach (object item in array)
			{
				StiComponent stiComponent = item as StiComponent;
				if (stiComponent == component)
				{
					return true;
				}
			}
		}
		else
		{
			StiComponent stiComponent2 = obj as StiComponent;
			if (stiComponent2 == component)
			{
				return true;
			}
		}
		return false;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		string text = value as string;
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
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			ItemHeight = StiScale.YYI(20.0)
		};
		val.Items.Add((object)string.Empty);
		((ListControl)val).SelectedIndex = 0;
		foreach (StiComponent component in stiComponent.Parent.Components)
		{
			if (component is StiText && !InstanceContensComponent(context.Instance, component))
			{
				val.Items.Add((object)component.Name);
				if (text == component.Name)
				{
					((ListControl)val).SelectedIndex = val.Items.Count - 1;
				}
			}
		}
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		((Control)val).Height = val.ItemHeight * val.Items.Count + StiScale.I10;
		if (((Control)val).Height > StiScale.YYI(300.0))
		{
			((Control)val).Height = StiScale.YYI(300.0);
		}
		edSvc.DropDownControl((Control)(object)val);
		return val.SelectedItem as string;
	}
}
