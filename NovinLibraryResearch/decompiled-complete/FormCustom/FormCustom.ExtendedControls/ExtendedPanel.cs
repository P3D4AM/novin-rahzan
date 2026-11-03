using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormCustom.ExtendedControls;

public class ExtendedPanel : Panel
{
	private const int WS_EX_TRANSPARENT = 32;

	private int opacity;

	[DefaultValue(0)]
	public int Opacity
	{
		get
		{
			return opacity;
		}
		set
		{
			if (value < 0 || value > 100)
			{
				throw new ArgumentException("value must be between 0 and 100");
			}
			opacity = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Panel)this).CreateParams;
			createParams.ExStyle |= 0x20;
			return createParams;
		}
	}

	public ExtendedPanel()
	{
		((Control)this).BringToFront();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(Color.FromArgb(opacity * 255 / 100, ((Control)this).BackColor));
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((Control)this).OnPaint(e);
	}
}
