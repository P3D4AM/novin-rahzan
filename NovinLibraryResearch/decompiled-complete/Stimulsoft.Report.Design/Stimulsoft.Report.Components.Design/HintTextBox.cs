using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class HintTextBox : TextBox
{
	private string hint;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((TextBoxBase)this).ForeColor;
			}
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((TextBoxBase)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((TextBoxBase)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((TextBoxBase)this).BackColor = value;
			}
		}
	}

	public string Hint
	{
		get
		{
			return hint;
		}
		set
		{
			hint = value;
			((Control)this).Invalidate();
		}
	}

	protected override void WndProc(ref Message m)
	{
		((TextBox)this).WndProc(ref m);
		if (((Message)(ref m)).Msg == 15 && string.IsNullOrEmpty(((Control)this).Text) && !string.IsNullOrEmpty(Hint))
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				TextRenderer.DrawText((IDeviceContext)(object)val, Hint, ((Control)this).Font, ((Control)this).ClientRectangle, StiUX.InputForegroundHint, ((Control)this).BackColor, (TextFormatFlags)4);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public HintTextBox()
	{
		((TextBoxBase)this).BackColor = StiUX.InputBackground;
	}
}
