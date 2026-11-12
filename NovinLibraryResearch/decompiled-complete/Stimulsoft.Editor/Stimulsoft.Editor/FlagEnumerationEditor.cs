using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Editor;

public class FlagEnumerationEditor : UITypeEditor
{
	private IWindowsFormsEditorService service;

	public IWindowsFormsEditorService Service => service;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return value;
		}
		service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		if (service == null)
		{
			return value;
		}
		DlgFlagEnumeration dlgFlagEnumeration = new DlgFlagEnumeration(this);
		dlgFlagEnumeration.EditValue = value;
		service.DropDownControl((Control)(object)dlgFlagEnumeration.ListBox);
		value = dlgFlagEnumeration.EditValue;
		((Component)(object)dlgFlagEnumeration).Dispose();
		service = null;
		return value;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}
}
