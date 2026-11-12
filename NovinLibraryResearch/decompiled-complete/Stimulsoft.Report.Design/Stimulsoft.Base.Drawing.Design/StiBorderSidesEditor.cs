using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Base.Drawing.Design;

public class StiBorderSidesEditor : UITypeEditor
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

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiBorderSides borderSide = StiBorderSides.None;
		if (value is StiBorder)
		{
			borderSide = ((StiBorder)value).Side;
		}
		else if (value != null)
		{
			borderSide = (StiBorderSides)value;
		}
		StiBorderSideViewForm stiBorderSideViewForm = new StiBorderSideViewForm(borderSide);
		stiBorderSideViewForm.SetBorder();
		edSvc.DropDownControl((Control)(object)stiBorderSideViewForm.panel1);
		if (value is StiBorder)
		{
			return new StiBorder
			{
				Color = ((StiBorder)value).Color,
				ShadowBrush = ((StiBorder)value).ShadowBrush,
				ShadowSize = ((StiBorder)value).ShadowSize,
				Size = ((StiBorder)value).Size,
				Style = ((StiBorder)value).Style,
				DropShadow = ((StiBorder)value).DropShadow,
				Side = stiBorderSideViewForm.BorderSide
			};
		}
		return stiBorderSideViewForm.BorderSide;
	}
}
