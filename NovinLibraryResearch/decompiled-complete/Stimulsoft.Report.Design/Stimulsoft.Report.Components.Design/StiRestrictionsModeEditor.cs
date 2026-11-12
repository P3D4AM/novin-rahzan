using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiRestrictionsModeEditor : UITypeEditor
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

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiRestrictions stiRestrictions;
		if (context.Instance is Array)
		{
			Array array = context.Instance as Array;
			stiRestrictions = ((StiComponent)array.GetValue(0)).Restrictions;
		}
		else
		{
			stiRestrictions = (StiRestrictions)value;
		}
		Panel val = new Panel
		{
			Width = StiScale.XXI(224.0),
			Height = StiScale.YYI(152.0)
		};
		CheckBox val2 = new CheckBox
		{
			Checked = ((stiRestrictions & StiRestrictions.AllowChange) > StiRestrictions.None),
			Text = StiLocalization.Get("PropertyEnum", "StiRestrictionsAllowChange")
		};
		((Control)val2).SetBounds(StiScale.XXI(12.0), StiScale.YYI(12.0), StiScale.XXI(200.0), StiScale.YYI(20.0), (BoundsSpecified)15);
		CheckBox val3 = new CheckBox
		{
			Checked = ((stiRestrictions & StiRestrictions.AllowDelete) > StiRestrictions.None),
			Text = StiLocalization.Get("PropertyEnum", "StiRestrictionsAllowDelete")
		};
		((Control)val3).SetBounds(StiScale.XXI(12.0), StiScale.YYI(40.0), StiScale.XXI(200.0), StiScale.YYI(20.0), (BoundsSpecified)15);
		CheckBox val4 = new CheckBox
		{
			Checked = ((stiRestrictions & StiRestrictions.AllowMove) > StiRestrictions.None),
			Text = StiLocalization.Get("PropertyEnum", "StiRestrictionsAllowMove")
		};
		((Control)val4).SetBounds(StiScale.XXI(12.0), StiScale.YYI(68.0), StiScale.XXI(200.0), StiScale.YYI(20.0), (BoundsSpecified)15);
		CheckBox val5 = new CheckBox
		{
			Checked = ((stiRestrictions & StiRestrictions.AllowResize) > StiRestrictions.None),
			Text = StiLocalization.Get("PropertyEnum", "StiRestrictionsAllowResize")
		};
		((Control)val5).SetBounds(StiScale.XXI(12.0), StiScale.YYI(96.0), StiScale.XXI(200.0), StiScale.YYI(20.0), (BoundsSpecified)15);
		CheckBox val6 = new CheckBox
		{
			Checked = ((stiRestrictions & StiRestrictions.AllowSelect) > StiRestrictions.None),
			Text = StiLocalization.Get("PropertyEnum", "StiRestrictionsAllowSelect")
		};
		((Control)val6).SetBounds(StiScale.XXI(12.0), StiScale.YYI(124.0), StiScale.XXI(200.0), StiScale.YYI(20.0), (BoundsSpecified)15);
		((Control)val).Controls.Add((Control)(object)val2);
		((Control)val).Controls.Add((Control)(object)val3);
		((Control)val).Controls.Add((Control)(object)val4);
		((Control)val).Controls.Add((Control)(object)val5);
		((Control)val).Controls.Add((Control)(object)val6);
		edSvc.DropDownControl((Control)(object)val);
		stiRestrictions = StiRestrictions.None;
		if (val2.Checked)
		{
			stiRestrictions |= StiRestrictions.AllowChange;
		}
		if (val3.Checked)
		{
			stiRestrictions |= StiRestrictions.AllowDelete;
		}
		if (val4.Checked)
		{
			stiRestrictions |= StiRestrictions.AllowMove;
		}
		if (val5.Checked)
		{
			stiRestrictions |= StiRestrictions.AllowResize;
		}
		if (val6.Checked)
		{
			stiRestrictions |= StiRestrictions.AllowSelect;
		}
		return stiRestrictions;
	}
}
