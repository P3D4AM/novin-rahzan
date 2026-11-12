using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiShiftModeEditor : UITypeEditor
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiShiftMode stiShiftMode = (StiShiftMode)value;
		Panel val = new Panel
		{
			Width = StiScale.XXI(224.0),
			Height = StiScale.YYI(96.0)
		};
		CheckBox val2 = new CheckBox
		{
			Checked = ((stiShiftMode & StiShiftMode.IncreasingSize) != 0),
			Text = StiLocalization.Get("PropertyEnum", "StiShiftModeIncreasingSize")
		};
		((Control)val2).SetBounds(StiScale.XXI(12.0), StiScale.YYI(12.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
		CheckBox val3 = new CheckBox
		{
			Checked = ((stiShiftMode & StiShiftMode.DecreasingSize) != 0),
			Text = StiLocalization.Get("PropertyEnum", "StiShiftModeDecreasingSize")
		};
		((Control)val3).SetBounds(StiScale.XXI(12.0), StiScale.YYI(40.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
		CheckBox val4 = new CheckBox
		{
			Checked = ((stiShiftMode & StiShiftMode.OnlyInWidthOfComponent) != 0),
			Text = StiLocalization.Get("PropertyEnum", "StiShiftModeOnlyInWidthOfComponent")
		};
		((Control)val4).SetBounds(StiScale.XXI(12.0), StiScale.YYI(68.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
		((Control)val).Controls.Add((Control)(object)val2);
		((Control)val).Controls.Add((Control)(object)val3);
		((Control)val).Controls.Add((Control)(object)val4);
		edSvc.DropDownControl((Control)(object)val);
		stiShiftMode = StiShiftMode.None;
		if (val3.Checked)
		{
			stiShiftMode |= StiShiftMode.DecreasingSize;
		}
		if (val2.Checked)
		{
			stiShiftMode |= StiShiftMode.IncreasingSize;
		}
		if (val4.Checked)
		{
			stiShiftMode |= StiShiftMode.OnlyInWidthOfComponent;
		}
		return stiShiftMode;
	}
}
