using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Editor;

public class SyntaxBuilderEditor : UITypeEditor
{
	private IWindowsFormsEditorService service;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		if (context == null || context.Instance == null || provider == null)
		{
			return value;
		}
		service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		if (service == null)
		{
			return value;
		}
		DlgSyntaxBuilder dlgSyntaxBuilder = new DlgSyntaxBuilder(this);
		try
		{
			dlgSyntaxBuilder.Scheme = (LexScheme)value;
		}
		catch
		{
			MessageBox.Show("Not implemented: Error loading scheme");
			((Component)(object)dlgSyntaxBuilder).Dispose();
			service = null;
			return value;
		}
		DialogResult val = service.ShowDialog((Form)(object)dlgSyntaxBuilder);
		if ((int)val == 1)
		{
			value = dlgSyntaxBuilder.Scheme;
		}
		((Component)(object)dlgSyntaxBuilder).Dispose();
		service = null;
		return value;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}
}
