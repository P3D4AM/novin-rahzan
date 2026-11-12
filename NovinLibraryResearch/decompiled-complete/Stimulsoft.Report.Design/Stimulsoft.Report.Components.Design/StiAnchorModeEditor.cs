using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiAnchorModeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private CheckBox checkBoxTop;

	private CheckBox checkBoxBottom;

	private CheckBox checkBoxLeft;

	private CheckBox checkBoxRight;

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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			StiAnchorMode stiAnchorMode = (StiAnchorMode)value;
			Panel val = new Panel
			{
				Width = StiScale.XXI(124.0),
				Height = StiScale.YYI(124.0)
			};
			checkBoxTop = new CheckBox
			{
				Checked = ((stiAnchorMode & StiAnchorMode.Top) != 0),
				Text = StiLocalization.Get("PropertyMain", "Top")
			};
			checkBoxTop.CheckedChanged += delegate
			{
				CheckStates();
			};
			((Control)checkBoxTop).SetBounds(StiScale.XXI(12.0), StiScale.YYI(12.0), StiScale.XXI(100.0), StiScale.YYI(16.0), (BoundsSpecified)15);
			checkBoxBottom = new CheckBox
			{
				Checked = ((stiAnchorMode & StiAnchorMode.Bottom) != 0),
				Text = StiLocalization.Get("PropertyMain", "Bottom")
			};
			checkBoxBottom.CheckedChanged += delegate
			{
				CheckStates();
			};
			((Control)checkBoxBottom).SetBounds(StiScale.XXI(12.0), StiScale.YYI(40.0), StiScale.XXI(100.0), StiScale.YYI(16.0), (BoundsSpecified)15);
			checkBoxLeft = new CheckBox
			{
				Checked = ((stiAnchorMode & StiAnchorMode.Left) != 0),
				Text = StiLocalization.Get("PropertyMain", "Left")
			};
			checkBoxLeft.CheckedChanged += delegate
			{
				CheckStates();
			};
			((Control)checkBoxLeft).SetBounds(StiScale.XXI(12.0), StiScale.YYI(68.0), StiScale.XXI(100.0), StiScale.YYI(16.0), (BoundsSpecified)15);
			checkBoxRight = new CheckBox
			{
				Checked = ((stiAnchorMode & StiAnchorMode.Right) != 0),
				Text = StiLocalization.Get("PropertyMain", "Right")
			};
			checkBoxRight.CheckedChanged += delegate
			{
				CheckStates();
			};
			((Control)checkBoxRight).SetBounds(StiScale.XXI(12.0), StiScale.YYI(96.0), StiScale.XXI(100.0), StiScale.YYI(16.0), (BoundsSpecified)15);
			((Control)val).Controls.Add((Control)(object)checkBoxTop);
			((Control)val).Controls.Add((Control)(object)checkBoxBottom);
			((Control)val).Controls.Add((Control)(object)checkBoxLeft);
			((Control)val).Controls.Add((Control)(object)checkBoxRight);
			CheckStates();
			edSvc.DropDownControl((Control)(object)val);
			stiAnchorMode = (StiAnchorMode)0;
			if (checkBoxTop.Checked)
			{
				stiAnchorMode |= StiAnchorMode.Top;
			}
			if (checkBoxBottom.Checked)
			{
				stiAnchorMode |= StiAnchorMode.Bottom;
			}
			if (checkBoxLeft.Checked)
			{
				stiAnchorMode |= StiAnchorMode.Left;
			}
			if (checkBoxRight.Checked)
			{
				stiAnchorMode |= StiAnchorMode.Right;
			}
			return stiAnchorMode;
		}
		return null;
	}

	private void CheckStates()
	{
		if (!checkBoxBottom.Checked)
		{
			checkBoxTop.Checked = true;
		}
		if (!checkBoxRight.Checked)
		{
			checkBoxLeft.Checked = true;
		}
		((Control)checkBoxTop).Enabled = checkBoxBottom.Checked;
		((Control)checkBoxLeft).Enabled = checkBoxRight.Checked;
	}
}
