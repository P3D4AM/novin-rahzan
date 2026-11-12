using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiExceedMarginsEditor : UITypeEditor
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
		//IL_0082: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiExceedMargins stiExceedMargins = (StiExceedMargins)value;
		Panel val = new Panel
		{
			Width = StiScale.XXI(224.0),
			Height = StiScale.YYI(130.0)
		};
		CheckBox val2 = new CheckBox
		{
			Checked = ((stiExceedMargins & StiExceedMargins.Left) != 0),
			Text = Loc.GetEnum("StiBorderSidesLeft")
		};
		((Control)val2).SetBounds(StiScale.XXI(12.0), StiScale.YYI(12.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
		CheckBox val3 = new CheckBox
		{
			Checked = ((stiExceedMargins & StiExceedMargins.Right) != 0),
			Text = Loc.GetEnum("StiBorderSidesRight")
		};
		((Control)val3).SetBounds(StiScale.XXI(12.0), StiScale.YYI(40.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
		CheckBox val4 = new CheckBox
		{
			Checked = ((stiExceedMargins & StiExceedMargins.Top) != 0),
			Text = Loc.GetEnum("StiBorderSidesTop")
		};
		((Control)val4).SetBounds(StiScale.XXI(12.0), StiScale.YYI(68.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
		CheckBox val5 = new CheckBox
		{
			Checked = ((stiExceedMargins & StiExceedMargins.Bottom) != 0),
			Text = Loc.GetEnum("StiBorderSidesBottom")
		};
		((Control)val5).SetBounds(StiScale.XXI(12.0), StiScale.YYI(96.0), StiScale.XXI(200.0), StiScale.YYI(16.0), (BoundsSpecified)15);
		((Control)val).Controls.Add((Control)(object)val2);
		((Control)val).Controls.Add((Control)(object)val3);
		((Control)val).Controls.Add((Control)(object)val4);
		((Control)val).Controls.Add((Control)(object)val5);
		edSvc.DropDownControl((Control)(object)val);
		stiExceedMargins = StiExceedMargins.None;
		if (val2.Checked)
		{
			stiExceedMargins |= StiExceedMargins.Left;
		}
		if (val3.Checked)
		{
			stiExceedMargins |= StiExceedMargins.Right;
		}
		if (val4.Checked)
		{
			stiExceedMargins |= StiExceedMargins.Top;
		}
		if (val5.Checked)
		{
			stiExceedMargins |= StiExceedMargins.Bottom;
		}
		return stiExceedMargins;
	}
}
