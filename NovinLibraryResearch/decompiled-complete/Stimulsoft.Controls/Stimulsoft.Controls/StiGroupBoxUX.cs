using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiGroupBoxUX : GroupBox
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!((Control)this).Enabled)
			{
				return StiUX.InputForegroundDisabled;
			}
			return StiUX.Foreground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			return StiUX.Background;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		((Control)this).OnEnabledChanged(e);
		((Control)this).ForeColor = (((Control)this).Enabled ? StiUX.Foreground : StiUX.InputForegroundDisabled);
	}

	public StiGroupBoxUX()
	{
		if (StiUX.IsDark)
		{
			((Control)this).ForeColor = StiUX.Foreground;
		}
	}
}
